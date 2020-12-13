using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Text;
using pornWs.Models;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace pornWs.Scraper
{
    public class scraper
    {
        VideosEntities db = new VideosEntities();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<Videos> getVideosX3xtube(int numpaginas, int startFromPage)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var categorias = new List<string>();
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();

            try
            {

                var location = "http://www.x3xtube.com/";
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb web = new HtmlWeb();
                Guid id = Guid.NewGuid();
                Videos video = new Videos();
                List<string> urlList = new List<string>();
                for (int i = startFromPage; i <= numpaginas; i++)
                {
                    //var url = location + "/?page=" + i;
                    var url = location;
                    doc = web.Load(url);

                    try
                    {
                        foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//td[@id='thumbs']")
)
                        {
                            try
                            {
                                video = new Videos();
                                video.id = Guid.NewGuid();
                                //video.Title = link.SelectSingleNode("//p[@class='videoTitle']").Attributes["title"].Value;
                                video.Title = link.SelectSingleNode("a//img").Attributes["alt"].Value;

                                var Tduplicate = db.Videos.Where(m => m.Title == video.Title).Count();
                                if (Tduplicate == 0)
                                {
                                    video.Img = link.SelectSingleNode("a//img").Attributes["src"].Value;
                                    var urlvideo = link.SelectSingleNode("a").Attributes["href"].Value;
                                    var posHttp = urlvideo.LastIndexOf("-");
                                    var idVideo = urlvideo.Substring(posHttp + 1);
                                    idVideo = idVideo.Remove(idVideo.Length - 1);
                                    // video.Views = link.SelectSingleNode("div//div//span[@class='views']").InnerText;
                                    // video.PublishDate = link.SelectSingleNode("div//div//var[@class='added']").InnerText;
                                    urlvideo = location + "iframe.php?vid=" + idVideo;
                                    //urlvideo = urlvideo.Replace("watch", "embed");
                                    //doc2 = web.Load(urlvideo);
                                    video.Url = urlvideo;
                                    //getting the correct embed link
                                    /*var indexurl = video.Url.IndexOf("http", 0);
                                    video.Url = video.Url.Substring(indexurl, 43);
                                    var indexC = video.Url.IndexOf("&", 0);
                                    video.Url = video.Url.Substring(0, indexC);*/
                                    video.Creation = DateTime.Today;
                                    try
                                    {
                                        db.Videos.Add(video);
                                        db.SaveChanges();
                                        videos.Add(video);
                                        logger.Info("done adding video " + video.Title + " provider Xextube at " + getLocalServerDate());

                                    }
                                    catch (Exception ex)
                                    {
                                        var error = ToMessageAndCompleteStacktrace(ex);
                                        logger.Error(error + " at " + getLocalServerDate());
                                    }
                                }
                                else
                                {
                                    logger.Info(Tduplicate + " duplicates video of " + video.Title + " provider Xextube at " + getLocalServerDate());

                                }

                            }
                            catch (Exception ex)
                            {
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error(error + " at " + getLocalServerDate());
                            }
                            link.RemoveAll();

                        }
                        logger.Info("success fetching data from page " + i + " provider Xextube at " + getLocalServerDate());


                    }
                    catch (Exception ex)
                    {
                        var error = ToMessageAndCompleteStacktrace(ex);
                        logger.Error("Error on loop " + error + " at " + getLocalServerDate() + " provider Xextube");
                    }



                }

            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " at " + fecha + " provider Xextube");
                throw ex;
            }
            return videos;

        }

        public List<Videos> getVideosByCatYouJizz(int numpaginas, int startFromPage, int startFromCat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            numpaginas = numpaginas + startFromPage;
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();

            var location = "http://www.youjizz.com";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb web = new HtmlWeb();
            var category = string.Empty;
            int counter = 1;
            try
            {
                doc = web.Load(location);
                foreach (HtmlNode cat in doc.DocumentNode.SelectNodes("//div[@id='categories2']//ul//li"))
                {
                    category = cat.SelectSingleNode("a").InnerText;
                    var stateCat = category.Length > 40 ? false : true;
                    var urlCat = cat.SelectSingleNode("a").Attributes["href"].Value;
                    
                    if (counter >= startFromCat && stateCat)
                    {
                        try
                        {
                            logger.Info("***Starting  fetching data from provider=YouJizz category=" + category + "[" + counter + "] at " + getLocalServerDate());

                            try
                            {
                                for (int i = startFromPage; i <= numpaginas; i++)
                                {

                                    var url = location + "/categories/"+category+"-"+i+".html";
                                    doc2 = web.Load(url);
                                    foreach (HtmlNode link in doc2.DocumentNode.SelectNodes("//tr//td//span[@id='miniatura']"))
                                    {
                                        try
                                        {
                                            Videos video = new Videos();
                                            video.id = Guid.NewGuid();
                                            video.Category = category;
                                            video.Title = link.SelectSingleNode("span[@id='title1']").InnerText;

                                            var Tduplicate = db.Videos.Where(m => m.Title == video.Title).ToList();
                                            var countduplicates = Tduplicate.Count();
                                            if (countduplicates == 0)
                                            {
                                                //for this provider gives an array of images 
                                                //see option to store array of images
                                                video.Img = link.SelectSingleNode("span[@id='min']//img[@class='lazy']").Attributes["data-original"].Value;
                                                video.Views = link.SelectSingleNode("span//span[@class='thumbviews']//span").InnerText;
                                                var urlvideo = link.SelectSingleNode("span//a").Attributes["href"].Value;
                                                var posVideo = urlvideo.LastIndexOf("-");
                                                var idVideo = urlvideo.Substring(posVideo+1);
                                                idVideo = Regex.Match(idVideo, @"\d+").Value;
                                                //building the embed url
                                                urlvideo = location + "/videos/embed/"+idVideo;
                                                video.Url = urlvideo;
                                                video.Creation = DateTime.Today;
                                                try
                                                {
                                                    db.Videos.Add(video);
                                                    db.SaveChanges();
                                                    videos.Add(video);
                                                    logger.Info("done adding video " + video.Title + "... provider=YouJizz" + " category=" + category + "[" + counter + "] page=" + i + " at " + getLocalServerDate());

                                                }
                                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                                {
                                                    Exception raise = dbEx;
                                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                    {
                                                        foreach (var validationError in validationErrors.ValidationErrors)
                                                        {
                                                            string message = string.Format("{0}:{1}",
                                                                validationErrors.Entry.Entity.ToString(),
                                                                validationError.ErrorMessage);
                                                            // raise a new exception nesting
                                                            // the current instance as InnerException
                                                            raise = new InvalidOperationException(message, raise);
                                                        }
                                                    }
                                                    logger.Error(raise);
                                                   logger.Error("category" + category + "[" + counter + "] at " + fecha);
                                                }
                                            }
                                            else
                                            {
                                                var state = false;
                                                try
                                                {
                                                    foreach (var elem in Tduplicate)
                                                    {

                                                        if (!string.IsNullOrEmpty(elem.Category) && !elem.Category.Contains(category))
                                                        {

                                                            elem.Category = elem.Category == null ? category : elem.Category + ',' + category;
                                                            db.Videos.Attach(elem);
                                                            db.Entry(elem).State = EntityState.Modified;
                                                            logger.Info("Category " + category + "[" + counter + "] updated for title " + elem.Title + "... category=" + category + "[" + counter + "] provider=YouJizz at " + getLocalServerDate());
                                                            state = true;
                                                        }
                                                    }
                                                    if (state)
                                                    {
                                                        db.SaveChanges();
                                                    }
               
                                                }
                                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                                {
                                                    Exception raise = dbEx;
                                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                    {
                                                        foreach (var validationError in validationErrors.ValidationErrors)
                                                        {
                                                            string message = string.Format("{0}:{1}",
                                                                validationErrors.Entry.Entity.ToString(),
                                                                validationError.ErrorMessage);
                                                            // raise a new exception nesting
                                                            // the current instance as InnerException
                                                            raise = new InvalidOperationException(message, raise);
                                                        }
                                                    }
                                                    var error = ToMessageAndCompleteStacktrace(raise);
                                                    logger.Error(raise);
                                                    logger.Error("Category " + category + "[" + counter + "] at " + fecha);
                                                }



                                                logger.Info(Tduplicate.Count() + " duplicates video of " + video.Title + "... provider=YouJizz category=" + category + "[" + counter + "] at " + getLocalServerDate());

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            var error = ToMessageAndCompleteStacktrace(ex);
                                            logger.Error(error + " at " + getLocalServerDate());
                                        }
                                    }
                                    logger.Info("success fetching data from page " + i + "... provider=YouJizz Category " + category + "[" + counter + "]  at " + getLocalServerDate());
                                }

                            }
                            catch (Exception ex)
                            {
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error("Error on loop Category " + category + "[" + counter + "]" + error + " at " + getLocalServerDate());
                            }

                        }
                        catch (Exception ex)
                        {
                            var error = ToMessageAndCompleteStacktrace(ex);
                            logger.Error(error + " Category " + category + "[" + counter + "] at " + getLocalServerDate());
                        }
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " Category " + category + "[" + counter + "]  at " + getLocalServerDate());
            }
            return videos;
        }


        
        public List<Videos> getVideosByCatKeezmovies(int numpaginas, int startFromPage, int startFromCat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            numpaginas = numpaginas + startFromPage;
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();

            var location = "http://www.keezmovies.com";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb web = new HtmlWeb();
            
            var category = string.Empty;
            int counter = 1;
            try
            {
                doc = web.Load(location);
                foreach (HtmlNode cat in doc.DocumentNode.SelectNodes("//div[@class='catmenu_holder']//div//a"))
                {
                    category = cat.InnerText;
                    var stateCat = category.Length > 40 ? false : true;
                    var aux = cat.Attributes["href"].Value;
                    if (counter >= startFromCat && stateCat)
                    {
                        try
                        {

                            logger.Info("***Starting  fetching data from provider=KeezMovies category=" + category + "[" + counter + "] at " + getLocalServerDate());

                            try
                            {
                                for (int i = startFromPage; i <= numpaginas; i++)
                                {

                                    var url = location + aux + "?page=" + i;
                                    doc2 = web.Load(url);
                                    foreach (HtmlNode link in doc2.DocumentNode.SelectNodes("//div//ul[@class='ul_video_block']//li"))
                                    {
                                        try
                                        {
                                            Videos video = new Videos();
                                            video.id = Guid.NewGuid();
                                            video.Category = category;

                                            video.Title = link.SelectSingleNode("div[@class='hoverab']//a").Attributes["title"].Value == null ? string.Empty : link.SelectSingleNode("div[@class='hoverab']//a").Attributes["title"].Value;

                                            var Tduplicate = db.Videos.Where(m => m.Title == video.Title).ToList();
                                            var countduplicates = Tduplicate.Count();
                                            if (countduplicates == 0)
                                            {
                                                video.Img = link.SelectSingleNode("div//a//img").Attributes["data-srcdata"].Value;
                                                video.Views = link.SelectSingleNode("div[@class='video_extra']//span[@class='views']").InnerText;
                                                var urlvideo = link.SelectSingleNode("div[@class='hoverab']//a").Attributes["href"].Value;
                                                urlvideo = location + urlvideo;
                                                //getting the correct embed link
                                                urlvideo = urlvideo.Replace("video", "embed");
                                                video.Url = urlvideo;

                                                video.Creation = DateTime.Today;
                                                try
                                                {
                                                    db.Videos.Add(video);
                                                    db.SaveChanges();
                                                    videos.Add(video);
                                                    logger.Info("done adding video " + video.Title + "... provider=KeezMovies" + " category=" + category + "[" + counter + "] page=" + i + " at " + getLocalServerDate());

                                                }
                                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                                {
                                                    Exception raise = dbEx;
                                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                    {
                                                        foreach (var validationError in validationErrors.ValidationErrors)
                                                        {
                                                            string message = string.Format("{0}:{1}",
                                                                validationErrors.Entry.Entity.ToString(),
                                                                validationError.ErrorMessage);
                                                            // raise a new exception nesting
                                                            // the current instance as InnerException
                                                            raise = new InvalidOperationException(message, raise);
                                                        }
                                                    }
                                                    var error = ToMessageAndCompleteStacktrace(raise);
                                                    logger.Error(raise);
                                                    logger.Error(error + "category" + category + "[" + counter + "] at " + fecha);
                                                    //throw ex;
                                                }
                                                //catch (Exception ex)
                                                //{
                                                //    var error = ToMessageAndCompleteStacktrace(ex);
                                                //    logger.Error(error + " at " + getLocalServerDate() + "... provider=KeezMovies" + " category=" + category+ "["+counter+"]");
                                                //}
                                            }
                                            else
                                            {
                                                var state = false;
                                                try
                                                {
                                                    foreach (var elem in Tduplicate)
                                                    {

                                                        if (!string.IsNullOrEmpty(elem.Category) && !elem.Category.Contains(category))
                                                        {

                                                            elem.Category = elem.Category == null ? category : elem.Category + ',' + category;
                                                            db.Videos.Attach(elem);
                                                            db.Entry(elem).State = EntityState.Modified;
                                                            logger.Info("Category "+category+"["+counter+"] updated for title " + elem.Title + "... category=" + category + "[" + counter + "] provider=KeezMovies at " + getLocalServerDate());
                                                            state = true;
                                                        }
                                                    }
                                                    if (state)
                                                    {
                                                        db.SaveChanges();
                                                    }
                                                    //catch (Exception dbEx)
                                                    //{
                                                    //    Exception raise = dbEx;
                                                    //    var error = ToMessageAndCompleteStacktrace(raise);
                                                    //    logger.Error(error + " at " + getLocalServerDate());
                                                    //}
                                                }
                                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                                {
                                                    Exception raise = dbEx;
                                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                    {
                                                        foreach (var validationError in validationErrors.ValidationErrors)
                                                        {
                                                            string message = string.Format("{0}:{1}",
                                                                validationErrors.Entry.Entity.ToString(),
                                                                validationError.ErrorMessage);
                                                            // raise a new exception nesting
                                                            // the current instance as InnerException
                                                            raise = new InvalidOperationException(message, raise);
                                                        }
                                                    }
                                                    var error = ToMessageAndCompleteStacktrace(raise);
                                                    logger.Error(raise);
                                                    logger.Error(error +"Category "+category+"["+counter+"] at " + fecha);
                                                    //throw ex;
                                                }



                                                logger.Info(Tduplicate.Count() + " duplicates video of " + video.Title + "... provider=KeezMovies category=" + category + "[" + counter + "] at " + getLocalServerDate());

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            var error = ToMessageAndCompleteStacktrace(ex);
                                            logger.Error(error + " at " + getLocalServerDate());
                                        }
                                    }
                                    logger.Info("success fetching data from page " + i + "... provider=KeezMovies Category "+category+"["+counter+"]  at " + getLocalServerDate());
                                }

                            }
                            catch (Exception ex)
                            {
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error("Error on loop Category " + category + "[" + counter + "]"  + error + " at " + getLocalServerDate());
                            }

                        }
                        catch (Exception ex)
                        {
                            var error = ToMessageAndCompleteStacktrace(ex);
                            logger.Error(error +" Category "+category+"["+counter+"] at " + getLocalServerDate());
                        }
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " Category " + category + "[" + counter + "]  at " + getLocalServerDate());
            }
            return videos;
        }

        public List<Videos> getVideosYouPorn(int numpaginas, int startFromPage)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var categorias = new List<string>();
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();

            try
            {

                var location = "http://www.youporn.com";
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb web = new HtmlWeb();
                Guid id = Guid.NewGuid();
                Videos video = new Videos();
                List<string> urlList = new List<string>();
                for (int i = startFromPage; i <= numpaginas; i++)
                {
                    var url = location + "/?page=" + i;
                    doc = web.Load(url);

                    try
                    {
                        foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div//ul//li[@class='videoBox grid_3']"))
                        {
                            try
                            {
                                video = new Videos();
                                video.id = Guid.NewGuid();
                                //video.Title = link.SelectSingleNode("//p[@class='videoTitle']").Attributes["title"].Value;
                                video.Title = link.SelectSingleNode("div//a//p").Attributes["title"].Value;

                                var Tduplicate = db.Videos.Where(m => m.Title == video.Title).Count();
                                if (Tduplicate == 0)
                                {
                                    video.Img = link.SelectSingleNode("div//a//img").Attributes["data-thumbnail"].Value;
                                    var urlvideo = link.SelectSingleNode("div//a").Attributes["href"].Value;
                                    // video.Views = link.SelectSingleNode("div//div//span[@class='views']").InnerText;
                                    // video.PublishDate = link.SelectSingleNode("div//div//var[@class='added']").InnerText;
                                    urlvideo = location + urlvideo;
                                    urlvideo = urlvideo.Replace("watch", "embed");
                                    //doc2 = web.Load(urlvideo);
                                    video.Url = urlvideo;
                                    //getting the correct embed link
                                    /*var indexurl = video.Url.IndexOf("http", 0);
                                    video.Url = video.Url.Substring(indexurl, 43);
                                    var indexC = video.Url.IndexOf("&", 0);
                                    video.Url = video.Url.Substring(0, indexC);*/
                                    video.Creation = DateTime.Today;
                                    try
                                    {
                                        db.Videos.Add(video);
                                        db.SaveChanges();
                                        videos.Add(video);
                                        logger.Info("done adding video " + video.Title);

                                    }
                                    catch (Exception ex)
                                    {
                                        var error = ToMessageAndCompleteStacktrace(ex);
                                        logger.Error(error + " at " + fecha);
                                    }
                                }
                                else
                                {
                                    logger.Info(Tduplicate + " duplicates video of " + video.Title);

                                }

                            }
                            catch (Exception ex)
                            {
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error(error + " at " + fecha);
                            }
                            link.RemoveAll();

                        }
                        logger.Info("success fetching data from page " + i);


                    }
                    catch (Exception ex)
                    {
                        var error = ToMessageAndCompleteStacktrace(ex);
                        logger.Error("Error on loop " + error + " at " + fecha);
                    }



                }

            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " at " + fecha);
                throw ex;
            }
            return videos;

        }

        public List<Videos> getVideosByCatYouPorn(int numpaginas, int startFromPage, int startFromCat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            numpaginas = numpaginas + startFromPage;
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();

            var location = "http://www.youporn.com";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb web = new HtmlWeb();
            doc = web.Load(location + "/categories/");
            var category = string.Empty;
            int counter = 1;
            try
            {
                foreach (HtmlNode cat in doc.DocumentNode.SelectNodes("//ul[@class='categories-list']//li"))
                {
                    category = cat.SelectSingleNode("div//div//div//a//span[@class='catTitle']//strong").Attributes["data-content"].Value;
                    var stateCat = category.Length > 40 ? false : true;

                    if (counter >= startFromCat && stateCat)
                    {
                        try
                        {

                            var aux = cat.SelectSingleNode("div//div//div//a").Attributes["href"].Value;
                            logger.Info("***Starting  fetching data from provider=Youporn category=" + category + "[" + counter + "] at " + getLocalServerDate());
                            try
                            {
                                for (int i = startFromPage; i <= numpaginas; i++)
                                {

                                    var url = location + aux + "?page=" + i;
                                    doc2 = web.Load(url);
                                    foreach (HtmlNode link in doc2.DocumentNode.SelectNodes("//div//ul//li[@class='videoBox grid_3']"))
                                    {
                                        try
                                        {
                                            Videos video = new Videos();
                                            video.id = Guid.NewGuid();
                                            video.Category = category;
                                            //video.Title = link.SelectSingleNode("//p[@class='videoTitle']").Attributes["title"].Value;
                                            video.Title = link.SelectSingleNode("div//a//p[@class='videoTitle']").Attributes["title"].Value;
                                            var Tduplicate = db.Videos.Where(m => m.Title == video.Title).ToList();
                                            var countduplicates = Tduplicate.Count();
                                            if (countduplicates == 0)
                                            {
                                                video.Img = link.SelectSingleNode("div//a//img").Attributes["data-thumbnail"].Value;
                                                var urlvideo = link.SelectSingleNode("div//a").Attributes["href"].Value;
                                                // video.Views = link.SelectSingleNode("div//div//span[@class='views']").InnerText;
                                                // video.PublishDate = link.SelectSingleNode("div//div//var[@class='added']").InnerText;
                                                urlvideo = location + urlvideo;
                                                urlvideo = urlvideo.Replace("watch", "embed");
                                                //doc2 = web.Load(urlvideo);
                                                video.Url = urlvideo;
                                                //getting the correct embed link
                                                /*var indexurl = video.Url.IndexOf("http", 0);
                                                video.Url = video.Url.Substring(indexurl, 43);
                                                var indexC = video.Url.IndexOf("&", 0);
                                                video.Url = video.Url.Substring(0, indexC);*/
                                                video.Creation = DateTime.Today;
                                                try
                                                {
                                                    db.Videos.Add(video);
                                                    db.SaveChanges();
                                                    videos.Add(video);
                                                    logger.Info("done adding video " + video.Title + " provider=Youporn" + " category=" + category + "[" + counter + "] page=" + i + " at " + getLocalServerDate());

                                                }
                                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                                {
                                                    Exception raise = dbEx;
                                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                    {
                                                        foreach (var validationError in validationErrors.ValidationErrors)
                                                        {
                                                            string message = string.Format("{0}:{1}",
                                                                validationErrors.Entry.Entity.ToString(),
                                                                validationError.ErrorMessage);
                                                            // raise a new exception nesting
                                                            // the current instance as InnerException
                                                            raise = new InvalidOperationException(message, raise);
                                                        }
                                                    }
                                                    var error = ToMessageAndCompleteStacktrace(raise);
                                                    logger.Error(raise);
                                                    logger.Error(error + " Category " + category + "[" + counter + "]  at " + fecha);
                                                }
                                                //catch (Exception ex)
                                                //{
                                                //    var error = ToMessageAndCompleteStacktrace(ex);
                                                //    logger.Error(error + " at " + getLocalServerDate() + " provider=Youporn" + " category=" + category+ "["+counter+"]");
                                                //}
                                            }
                                            else
                                            {
                                                var state = false;
                                                try
                                                {
                                                    foreach (var elem in Tduplicate)
                                                    {

                                                        if (!string.IsNullOrEmpty(elem.Category) && !elem.Category.Contains(category))
                                                        {

                                                            elem.Category = elem.Category == null ? category : elem.Category + ',' + category;
                                                            db.Videos.Attach(elem);
                                                            db.Entry(elem).State = EntityState.Modified;
                                                            logger.Info("Category " + category + "[" + counter + "] updated for title " + elem.Title + " category=" + category + "[" + counter + "] provider=Youporn at " + getLocalServerDate());
                                                            state = true;
                                                        }
                                                    }
                                                    if (state)
                                                    {
                                                        db.SaveChanges();
                                                    }
                                                    logger.Info(Tduplicate.Count() + " duplicates video of " + video.Title + " provider=Youporn category=" + category + "[" + counter + "] at " + getLocalServerDate());

                                                }
                                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                                {
                                                    Exception raise = dbEx;
                                                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                                                    {
                                                        foreach (var validationError in validationErrors.ValidationErrors)
                                                        {
                                                            string message = string.Format("{0}:{1}",
                                                                validationErrors.Entry.Entity.ToString(),
                                                                validationError.ErrorMessage);
                                                            // raise a new exception nesting
                                                            // the current instance as InnerException
                                                            raise = new InvalidOperationException(message, raise);
                                                        }
                                                    }
                                                    var error = ToMessageAndCompleteStacktrace(raise);
                                                    logger.Error(raise);
                                                    logger.Error(error + "category "+category+"["+counter+"] at " + fecha);
                                                }

                                                //catch (Exception dbEx)
                                                //{
                                                //    Exception raise = dbEx;
                                                //    var error = ToMessageAndCompleteStacktrace(raise);
                                                //    logger.Error(error + " at " + getLocalServerDate());
                                                //}
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            var error = ToMessageAndCompleteStacktrace(ex);
                                            logger.Error(error + "category "+category+"["+counter+"] at " + getLocalServerDate());
                                        }
                                    }
                                    logger.Info("success fetching data from page " + i + " provider=Youporn category=" + category + "[" + counter + "] at " + getLocalServerDate());
                                }

                            }
                            catch (Exception ex)
                            {
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error("Error on loop " + error + "category " + category + "[" + counter + "] at " + getLocalServerDate());
                            }

                        }
                        catch (Exception ex)
                        {
                            var error = ToMessageAndCompleteStacktrace(ex);
                            logger.Error(error + " Category " + category + "[" + counter + "]  at " + getLocalServerDate());
                        }
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " Category " + category + "[" + counter + "]  at " + getLocalServerDate());
            }
            return videos;
        }

        public List<string> CategoriesPornHub(int maxCategorie)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var categorias = new List<string>();
            var urlb = "http://www.pornhub.com/video?c=" + 1 + "&page=" + 1;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlWeb().Load(urlb);
            HtmlWeb web = new HtmlWeb();
            var num = 0;
            for (int s = 2; s <= maxCategorie; s++)
            {

                try
                {
                    var categoria = doc.DocumentNode.SelectSingleNode("//div[@id='categoriesStraight']//span").InnerText;
                    if (categoria != null)
                    {
                        num = categorias.Where(m => m == categoria).Count();
                        if (num == 0)
                        {
                            categorias.Add(categoria);

                        }
                    }
                    //                    logger.Info("scraping category " + categoria);
                    urlb = "http://www.pornhub.com/video?c=" + s + "&page=" + 1;
                    doc = web.Load(urlb);
                }
                catch (Exception ex)
                {
                    var error = ToMessageAndCompleteStacktrace(ex);

                    logger.Error(error);
                }
            }
            return categorias;

        }

        public List<Videos> getVideosByCatFromPornHub(int? numberPages, int? startFromPage, int startFromCategory)
        {
            db.Configuration.ProxyCreationEnabled = false;
            startFromPage = startFromPage == null ? 1 : startFromPage;
            int npaginas = numberPages == null ? 5 : numberPages.Value;
            npaginas = npaginas + startFromPage.Value;
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();
            int i = startFromPage.Value;
            try
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument doc3 = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb web = new HtmlWeb();
                List<Videos> videoList = new List<Videos>();
                Guid id = Guid.NewGuid();
                Videos video = new Videos();
                List<string> urlList = new List<string>();
                var categoria = string.Empty;


                //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//li[@class='cat_pic']"))
                try
                {
                    for (int s = startFromCategory; s <= 120; s++)
                    {
                        try
                        {
                            i = startFromPage.Value;
                            var urlb = "http://www.pornhub.com/video?c=" + s + "&page=" + i;
                            logger.Info("starting scraping to category " + categoria + " provider PornHub at" + getLocalServerDate());
                            doc = web.Load(urlb);
                            categoria = doc.DocumentNode.SelectSingleNode("//div[@id='categoriesStraight']//span").InnerText;
                            logger.Info("scraping category " + categoria);
                            while (i <= npaginas)
                            {
                                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//li[@class='videoblock']"))
                                {
                                    try
                                    {

                                        video = new Videos();
                                        video.Category = categoria;
                                        video.id = Guid.NewGuid();
                                        video.Title = node.SelectSingleNode("div//div//a").Attributes["title"].Value;
                                        var Tduplicate = db.Videos.Where(m => m.Title == video.Title);
                                        if (Tduplicate.Count() == 0)
                                        {
                                            video.Img = node.SelectSingleNode("div//div//a//img").Attributes["data-mediumthumb"].Value;
                                            var urlvideo = node.SelectSingleNode("div//div//a").Attributes["href"].Value;
                                            video.Views = node.SelectSingleNode("div//div//span[@class='views']").InnerText;
                                            video.PublishDate = node.SelectSingleNode("div//div//var[@class='added']").InnerText;
                                            urlvideo = "http://www.pornhub.com" + urlvideo;
                                            doc3 = web.Load(urlvideo);
                                            video.Url = doc3.DocumentNode.SelectSingleNode("//div[@id='embed']//div//textarea").InnerText;
                                            //getting the correct embed link
                                            var indexurl = video.Url.IndexOf("http", 0);
                                            video.Url = video.Url.Substring(indexurl, 43);
                                            var indexC = video.Url.IndexOf("&", 0);
                                            video.Url = video.Url.Substring(0, indexC);

                                            video.Creation = DateTime.Today;
                                            try
                                            {
                                                //if there is a duplicate video...skip
                                                //var duplicate = db.Videos.Where(m => m.Url == video.Url).Count();
                                                //if (duplicate == 0)
                                                //{
                                                db.Videos.Add(video);
                                                db.SaveChanges();
                                                videos.Add(video);
                                                logger.Info("done adding video:" + video.Title + " cat:" + categoria + "[" + s + "] provider PornHub at" + getLocalServerDate());

                                                //}
                                            }
                                            catch (Exception dbEx)
                                            {
                                                Exception raise = dbEx;
                                                var error = ToMessageAndCompleteStacktrace(raise);
                                                logger.Error(error + " at " + fecha + " cat[" + s + "] provider PornHub at" + getLocalServerDate());
                                            }
                                        }
                                        else
                                        {
                                            foreach (var elem in Tduplicate)
                                            {
                                                if (!string.IsNullOrEmpty(elem.Category) && !elem.Category.Contains(categoria))
                                                {
                                                    try
                                                    {
                                                        elem.Category = elem.Category == null ? categoria : elem.Category + ',' + categoria;
                                                        db.Videos.Attach(elem);
                                                        db.Entry(elem).State = EntityState.Modified;
                                                        logger.Info("Catergory " + categoria + " updated for title " + elem.Title + " provider PornHub at " + getLocalServerDate());

                                                    }
                                                    catch (Exception dbEx)
                                                    {
                                                        Exception raise = dbEx;
                                                        var error = ToMessageAndCompleteStacktrace(raise);
                                                        logger.Error(error + " at " + fecha + " cat[" + s + "] provider PornHub at " + getLocalServerDate());
                                                    }
                                                }
                                            }
                                            db.SaveChanges();

                                            logger.Info(+Tduplicate.Count() + " duplicate Video " + video.Title + "cat[" + s + "]");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        var error = ToMessageAndCompleteStacktrace(ex);
                                        logger.Error("Error on loop with the cat=" + categoria + "[" + s + "] and title= " + video.Title + " S[" + s + "] provider PornHub at " + getLocalServerDate());
                                        logger.Error(error + " at " + fecha);
                                    }
                                }
                                i++;
                                var url = "http://www.pornhub.com/video?c=" + s + "&page=" + i;
                                logger.Info("**done scraping page " + i + " for category " + categoria + "[" + s + "] provider PornHub at " + getLocalServerDate() + "**");
                                doc = web.Load(url);
                            }
                        }
                        catch (Exception ex)
                        {
                            i++;
                            var error = ToMessageAndCompleteStacktrace(ex);
                            logger.Error(error + " at " + getLocalServerDate() + " cat[" + s + "]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var error = ToMessageAndCompleteStacktrace(ex);
                    logger.Error("Error on loop " + error + " at " + getLocalServerDate());
                }
            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " at " + getLocalServerDate() + " at page" + i);
                throw ex;
            }
            return videos;
        }

        public List<Videos> getVideosByCatFromPornRabbit(int? numberPages, int? startFromPage, int startFromCat)
        {
            db.Configuration.ProxyCreationEnabled = false;
            startFromPage = startFromPage == null ? 1 : startFromPage;
            int npaginas = numberPages == null ? 5 : numberPages.Value;
            npaginas = npaginas + startFromPage.Value;
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();
            int i = startFromPage.Value;
            try
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlDocument doc3 = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb web = new HtmlWeb();
                List<Videos> videoList = new List<Videos>();
                Guid id = Guid.NewGuid();
                Videos video = new Videos();
                List<string> urlList = new List<string>();
                var categoria = string.Empty;
                int counter = 1;
                try
                {
                    i = startFromPage.Value;
                    var urlb = "http://www.pornrabbit.com/page/categories/";
                    logger.Info("starting scraping to category " + categoria);
                    doc = web.Load(urlb);

                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='cat']"))
                    {
                        if (counter >= startFromCat)
                        {

                            try
                            {
                                categoria = node.SelectSingleNode("a//h2").InnerHtml;
                                var posCat = categoria.IndexOf("<small>");
                                categoria = categoria.Substring(0, posCat);
                                logger.Info("scraping category " + video.Category + "[" + counter + "]");


                                var urlCategory = node.SelectSingleNode("a").Attributes["href"].Value;
                                urlCategory = "http://www.pornrabbit.com/" + urlCategory + "/" + startFromPage;
                                doc2 = web.Load(urlCategory);

                                for (int s = startFromPage.Value + 1; s <= npaginas; s++)
                                {
                                    try
                                    {

                                        foreach (HtmlNode link in doc2.DocumentNode.SelectNodes("//div[@class='contenttouch']//div[@class='video']"))
                                        {
                                            try
                                            {
                                                video = new Videos();
                                                //get all video info
                                                video = new Videos();
                                                video.id = Guid.NewGuid();
                                                video.Title = link.SelectSingleNode("a//h2").InnerText;
                                                video.Category = categoria;
                                                var Tduplicate = db.Videos.Where(m => m.Title == video.Title);
                                                if (Tduplicate.Count() == 0)
                                                {
                                                    video.PublishDate = DateTime.Now.ToString();
                                                    video.Img = link.SelectSingleNode("a//span//img").Attributes["src"].Value; ;
                                                    video.Views = link.SelectSingleNode("div//span//b").InnerText;
                                                    video.Creation = DateTime.Today;
                                                    var urlvideo = link.SelectSingleNode("a").Attributes["href"].Value; ;
                                                    urlvideo = "http://www.pornrabbit.com" + urlvideo;
                                                    doc2 = web.Load(urlvideo);
                                                    //load embed url
                                                    //video.Url = doc2.DocumentNode.SelectSingleNode("//div[@class='textshare']//input").Attributes["value"].Value;

                                                    foreach (HtmlNode elem in doc2.DocumentNode.SelectNodes("//div[@id='video-details']//div[@class='textshare']"))
                                                    {
                                                        var texto = elem.SelectSingleNode("input").Attributes["value"].Value;
                                                        if (texto.Contains("iframe"))
                                                        {
                                                            var index = texto.IndexOf("http");
                                                            // var pos=texto.IndexOf("width");
                                                            video.Url = texto.Substring(index, 40);
                                                        }
                                                    }
                                                    //avoid spam videos
                                                    if (!video.Url.Contains("http://syndication.traffichaus.com"))
                                                    {
                                                        try
                                                        {

                                                            videos.Add(video);
                                                            //search for duplicates
                                                            db.Videos.Add(video);
                                                            db.SaveChanges();
                                                            videos.Add(video);

                                                            logger.Info("added video=" + video.Title + " cat=" + categoria + "[" + counter + "]  page=" + s + " at" + getLocalServerDate());

                                                        }
                                                        catch (Exception raise)
                                                        {

                                                            var error = ToMessageAndCompleteStacktrace(raise);
                                                            logger.Error(error + " at " + getLocalServerDate());
                                                            //throw ex;
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    var state = false;
                                                    foreach (var elem in Tduplicate)
                                                    {
                                                        try
                                                        {
                                                            if (!string.IsNullOrEmpty(elem.Category) && !elem.Category.Contains(categoria))
                                                            {

                                                                elem.Category = elem.Category == null ? categoria : elem.Category + ',' + categoria;
                                                                db.Videos.Attach(elem);
                                                                db.Entry(elem).State = EntityState.Modified;
                                                                logger.Info("Catergory " + categoria + "[" + counter + "] updated for title " + elem.Title);
                                                                state = true;
                                                            }

                                                        }
                                                        catch (Exception dbEx)
                                                        {
                                                            Exception raise = dbEx;
                                                            var error = ToMessageAndCompleteStacktrace(raise);
                                                            logger.Error(error + " at " + getLocalServerDate());
                                                        }
                                                    }
                                                    if (state)
                                                    {
                                                        db.SaveChanges();
                                                    }


                                                    logger.Info(+Tduplicate.Count() + " duplicate Video " + video.Title);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                var error = ToMessageAndCompleteStacktrace(ex);
                                                logger.Error(error);
                                                logger.Error("Error on movie " + video.Title + " cat=" + categoria + "[" + counter + "]");
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        var error = ToMessageAndCompleteStacktrace(ex);
                                        logger.Error("Error on loop with the cat=" + categoria + "[" + s + "] and title= " + video.Title + " S[" + s + "]");
                                        logger.Error(error + " at " + getLocalServerDate());
                                    }
                                    logger.Info("done scraping PAGE " + s + " for category " + categoria + "[" + counter + "]");

                                    var url = "http://www.pornrabbit.com/videos/" + categoria + "/" + s;
                                    doc2 = web.Load(url);
                                }

                            }
                            catch (Exception ex)
                            {
                                i++;
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error(error + " at " + getLocalServerDate());
                            }
                        }
                        counter++;
                    }
                }
                catch (Exception ex)
                {
                    var error = ToMessageAndCompleteStacktrace(ex);
                    logger.Error("Error on loop " + error + " at " + getLocalServerDate());
                }
            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " at " + getLocalServerDate() + " at page" + i);
                throw ex;
            }
            return videos;
        }

        public List<Videos> getVideosFromPornHub(int? numberPages, int? startFromPage)
        {
            db.Configuration.ProxyCreationEnabled = false;
            int npaginas = numberPages == null ? 5 : numberPages.Value;
            startFromPage = startFromPage == null ? 1 : startFromPage.Value;
            npaginas = npaginas + startFromPage.Value;
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();
            int i = startFromPage.Value;
            try
            {
                String location = string.Empty;
                if (startFromPage != null)
                {
                    location = "http://www.pornhub.com/video?page=" + startFromPage;
                }
                else
                {
                    location = "http://www.pornhub.com/";
                }

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlWeb().Load(location);
                HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb web = new HtmlWeb();
                List<Videos> videoList = new List<Videos>();
                Guid id = Guid.NewGuid();
                Videos video = new Videos();
                List<string> urlList = new List<string>();
                //for (int i = 1; i <= npaginas; i++)
                while (i <= npaginas)
                {
                    try
                    {
                        foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//li[@class='videoblock']"))
                        {
                            try
                            {
                                video = new Videos();
                                video.id = Guid.NewGuid();
                                video.Title = link.SelectSingleNode("div//div//a").Attributes["title"].Value;
                                var Tduplicate = db.Videos.Where(m => m.Title == video.Title).Count();
                                if (Tduplicate == 0)
                                {
                                    video.Img = link.SelectSingleNode("div//div//a//img").Attributes["data-mediumthumb"].Value;
                                    var urlvideo = link.SelectSingleNode("div//div//a").Attributes["href"].Value;
                                    video.Views = link.SelectSingleNode("div//div//span[@class='views']").InnerText;
                                    video.PublishDate = link.SelectSingleNode("div//div//var[@class='added']").InnerText;
                                    urlvideo = "http://www.pornhub.com" + urlvideo;
                                    doc2 = web.Load(urlvideo);
                                    video.Url = doc2.DocumentNode.SelectSingleNode("//div[@id='embed']//div//textarea").InnerText;
                                    //getting the correct embed link
                                    var indexurl = video.Url.IndexOf("http", 0);
                                    video.Url = video.Url.Substring(indexurl, 43);
                                    var indexC = video.Url.IndexOf("&", 0);
                                    video.Url = video.Url.Substring(0, indexC);
                                    video.Creation = DateTime.Today;
                                    try
                                    {
                                        var duplicate = db.Videos.Where(m => m.Url == video.Url).Count();
                                        if (duplicate == 0)
                                        {
                                            db.Videos.Add(video);
                                            db.SaveChanges();
                                            videos.Add(video);
                                            logger.Info("done adding video " + video.Title);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        var error = ToMessageAndCompleteStacktrace(ex);
                                        logger.Error(error + " at " + getLocalServerDate());
                                        //throw ex;
                                    }
                                }
                                else
                                {
                                    logger.Info(Tduplicate + " duplicates video of " + video.Title);
                                }

                            }
                            catch (Exception ex)
                            {
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error(error + " at " + getLocalServerDate());
                                //                            throw ex;
                            }

                        }
                        logger.Info("success fetching data from page " + i);
                        i++;
                        var url = "http://www.pornhub.com" + "/video?page=" + i;
                        doc = web.Load(url);

                    }
                    catch (Exception ex)
                    {
                        var error = ToMessageAndCompleteStacktrace(ex);
                        logger.Error("Error on loop " + error + " at " + getLocalServerDate());
                    }



                }

            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " at " + fecha + " at page" + i);
                throw ex;
            }
            return videos;
        }

        public List<Videos> getVideosFromPornRabbit(int numberPages, int? startFromPage)
        {
            db.Configuration.ProxyCreationEnabled = false;
            startFromPage = startFromPage == null ? 1 : startFromPage.Value;
            numberPages = numberPages + startFromPage.Value;
            var fecha = getLocalServerDate();
            List<Videos> videos = new List<Videos>();
            try
            {

                String location = string.Empty;
                if (startFromPage != null)
                {
                    location = "http://www.pornrabbit.com/videos/" + startFromPage + "/";
                }
                else
                {
                    location = "http://www.pornrabbit.com";
                }

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlWeb().Load(location);
                HtmlAgilityPack.HtmlDocument doc2 = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb web = new HtmlWeb();

                Guid id = Guid.NewGuid();
                List<string> urlList = new List<string>();

                Videos video = new Videos();
                for (int i = startFromPage.Value; i <= numberPages; i++)
                {
                    try
                    {
                        foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@class='contenttouch']//div[@class='video']"))
                        {
                            try
                            {
                                //get all video info
                                video = new Videos();
                                video.id = Guid.NewGuid();
                                video.Title = link.SelectSingleNode("a//h2").InnerText;
                                var Tduplicate = db.Videos.Where(m => m.Title == video.Title).Count();
                                if (Tduplicate == 0)
                                {
                                    video.PublishDate = DateTime.Now.ToString();
                                    video.Img = link.SelectSingleNode("a//span//img").Attributes["src"].Value; ;
                                    video.Views = link.SelectSingleNode("div//span//b").InnerText;
                                    video.Creation = DateTime.Today;
                                    var urlvideo = link.SelectSingleNode("a").Attributes["href"].Value; ;
                                    urlvideo = "http://www.pornrabbit.com" + urlvideo;
                                    doc2 = web.Load(urlvideo);
                                    //load embed url
                                    //video.Url = doc2.DocumentNode.SelectSingleNode("//div[@class='textshare']//input").Attributes["value"].Value;

                                    foreach (HtmlNode node in doc2.DocumentNode.SelectNodes("//div[@id='video-details']//div[@class='textshare']"))
                                    {
                                        var texto = node.SelectSingleNode("input").Attributes["value"].Value;
                                        if (texto.Contains("iframe"))
                                        {
                                            var index = texto.IndexOf("http");
                                            // var pos=texto.IndexOf("width");
                                            video.Url = texto.Substring(index, 40);
                                        }
                                    }
                                    //avoid spam videos
                                    if (!video.Url.Contains("http://syndication.traffichaus.com"))
                                    {
                                        try
                                        {
                                            var duplicate = db.Videos.Where(m => m.Url == video.Url).Count();
                                            if (duplicate == 0)
                                            {
                                                videos.Add(video);
                                                //search for duplicates
                                                db.Videos.Add(video);
                                                db.SaveChanges();
                                                logger.Info("done adding video " + video.Title + " provider PornRabbit at " + getLocalServerDate());
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                            var error = ToMessageAndCompleteStacktrace(ex);
                                            logger.Error(error + " provider PornRabbit at " + getLocalServerDate());
                                            //throw ex;
                                        }
                                    }

                                }
                                else
                                {
                                    logger.Info(+Tduplicate + " duplicates with video " + video.Title + " provider PornRabbit at " + getLocalServerDate());
                                }
                            }
                            catch (Exception ex)
                            {
                                var error = ToMessageAndCompleteStacktrace(ex);
                                logger.Error(error);
                                logger.Error("Error on movie " + video.Title + " provider PornRabbit at " + getLocalServerDate());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = ToMessageAndCompleteStacktrace(ex);
                        logger.Error("Error on loop " + error + " provider PornRabbit at " + getLocalServerDate());
                    }
                    logger.Info("done scraping PAGE " + i + " provider PornRabbit at " + getLocalServerDate());
                    i++;
                    var url = "http://www.pornrabbit.com/videos/" + i + "/";
                    doc = web.Load(url);


                }
            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                logger.Error(error + " provider PornRabbit at " + getLocalServerDate());
            }
            return videos;

        }

        public string ToMessageAndCompleteStacktrace(Exception exception)
        {
            Exception e = exception;
            StringBuilder s = new StringBuilder();
            while (e != null)
            {
                s.AppendLine("Exception type: " + e.GetType().FullName);
                s.AppendLine("Message       : " + e.Message);
                s.AppendLine("Stacktrace:" + e.StackTrace);
                s.AppendLine();
                e = e.InnerException;
            }
            return s.ToString();
        }

        public string getLocalServerDate()
        {

            var info = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            //local time
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            //users time
            DateTimeOffset usersTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            //utc Time
            DateTimeOffset utc = localServerTime.ToUniversalTime();
            return usersTime.ToString();
        }

        /// <summary>
        /// Method that write and save JsonFile with 
        /// the Videos object
        /// </summary>
        /// <param name="videos"></param>
        /// <param name="nombre"></param>
        public string writeJsonFile(List<Videos> videos, string name)
        {
            var date = getLocalServerDate();
            try
            {
                //parse Movies object to Json
                string json = JsonConvert.SerializeObject(videos, Formatting.Indented);
                //write and save file

                File.WriteAllText(HttpContext.Current.Server.MapPath("~\\Data\\" + name + ".json"), json);
                var result = "Success building the json file for the provider " + name + " at " + date;
                logger.Info(result);
                return result;
            }
            catch (Exception ex)
            {
                var error = ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error);
                return error;

            }


        }
    }
}