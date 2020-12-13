using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xxx.Data;
using xxx.Models;
using PagedList;
using System.Web.UI.WebControls;
using AttributeRouting.Web.Mvc;
using System.Data.Entity;

namespace xxx.Controllers
{

    public class HomeController : Controller
    {

        public servicioXXX servicio;
        pornmaleonEntities db = new pornmaleonEntities();

        [GET("Videos")]
        public ActionResult Videos()
        {

            return View();
        }

        public static System.Collections.Generic.Dictionary<string, string> Categories = new System.Collections.Generic.Dictionary<string, string>()
        {
            {"PornMaleon Videos","PornMaleon Videos"},
            {"Asian","Asian"},
            {"Orgy","Orgy"},
			{"Amateur","Amateur"},
			{"Big Ass","Big Ass"},
			{"Babe","Babe"},
			{"BBW","BBW"},
			{"Big Dick"	,"Big Dick"},
			{"Big Tits"	,"Big Tits"},
			{"Blonde"	,"Blonde"	},
			{"Bondage"	,"Bondage"	},
			{"Brunette"	,"Brunette"	},
			{"Celebrity","Celebrity"},
            {"Blowjob","Blowjob"},
            {"Bukkake","Bukkake"	},
            
            {"Creampie"	,"Creampie"	},
			{"Cumshots"	,"Cumshots"	},
            {"Ebony","Ebony"},
			{"Fetish","Fetish"},
			{"Fisting","Fisting"},
			{"Handjob","Handjob"},
			{"Hardcore","Hardcore"},
			{"Masturbation","Masturbation"	},
			{"Toys"	,"Toys"	},
            {"Public","Public"},
			{"Interracial","Interracial"	},
            {"Latina","Latina"},
			{"Lesbian","Lesbian"},
            {"Mature","Mature"},
            {"MILF","MILF"	},
            {"Pornstar"	,"Pornstar"	},
            {"Reality"	,"Reality"	},
            {"Funny"	,"Funny"	},
            {"Striptease"	,"Striptease"	},
			{"Anal"	,"Anal"	},
			{"Hentai"	,"Hentai"	},
            {"Teen"	,"Teen"	},
            {"HD Porn"	,"HD Porn"	},
			{"POV"	,"POV"	},
			{"Red Head"	,"Red Head"	},
			{"Vintage"	,"Vintage"	},
            {"Party"	,"Party"	},
			{"Euro"	,"Euro"	},
			{"Compilation"	,"Compilation"	},
			{"Small Tits"	,"Small Tits"	},
            {"Webcam"	,"Webcam"	}

        };
        public static System.Collections.Generic.Dictionary<string, string> ProvidersD = new System.Collections.Generic.Dictionary<string, string>()
        {
            {"PornHub","PornHub"},
            {"YouPorn","YouPorn"},
            {"YouJizz","YouJizz"},
			{"PornRabbit","PornRabbit"},
			{"KeezMovies","KeezMovies"}

        };
        public SelectList Providers
        {

            get
            {
                return new SelectList(ProvidersD, "Key", "Value");

            }
        }

        public SelectList Categorias
        {

            get
            {
                return new SelectList(Categories, "Key", "Value");

            }
        }

        [GET("Videos/List/{filter}/{provider}/{page}")]
        public ActionResult List(string filter, string provider, int page = 1)
        {
            var videos = new List<Videos>();
            try
            {

                //Create a list of select list items - this will be returned as your select list
                var i = 0;

                //videos = db.Videos.ToList();
                //  var categorias = videos.Select(m => m.Category).Distinct();
                //procedure to check titles with a category name and then insert that "name" as a category
                /*foreach (var item in categorias)
                {

                    var anyvideos = new List<Videos>();
                    if (item != null)
                    {
                        //var countVideos = 1;
                        //anyvideos = db.Videos.Where(m => m.Title.Contains(item) && !m.Category.Contains(item)).ToList();
                        //if (anyvideos != null && anyvideos.Count() > 0)
                        //{
                        //    foreach (var elem in anyvideos)
                        //    {
                        //        elem.Category = elem.Category + "," + item;
                        //        db.Videos.Attach(elem);
                        //        db.Entry(elem).State = EntityState.Modified;
                        //        db.SaveChanges();
                        //        Logs.logger.Info("Added video "+countVideos+" to  cat="+item);
                        //        countVideos++;
                        //    }
                        //    Logs.logger.Info("Added " + countVideos + " VIDEOS");

                        //}
                        SelectListItem selListItem = new SelectListItem() { Value = i.ToString(), Text = item };

                        newList.Add(selListItem);
                    }
                }*/
                if (provider != null && provider != "null")
                {
                    provider = provider.ToLower();
                    videos = db.Videos.Where(m => m.Url.Contains(provider)).Take(500).ToList();
                }
                else if (filter != null && filter != "null")
                {
                    var estado = filter.Contains("PornMaleon Videos");
                    filter = estado == true ? null : filter;
                    //get just videos by cat
                    ViewBag.filter = filter;
                    if (filter == null)
                    {
                        videos = db.Videos.Where(m => m.Category == null).Take(500).ToList();
                    }
                    else
                    {
                        videos = db.Videos.Where(m => m.Category.Contains(filter)).Take(500).ToList();
                    }
                }
                else
                {
                    //show random videos without categories
                    //videos = db.Videos.Where(m => m.Category == null).Take(300).OrderBy(a => Guid.NewGuid()).ToList();
                    //  videos = db.Videos.Where(m => m.Category == null).OrderBy(a => Guid.NewGuid()).Take(300).ToList();
                    videos = db.Videos.Where(m => m.Category == null).Take(500).OrderBy(a => Guid.NewGuid()).ToList();
                    //videos = db.Videos.Where(m => m.Url.Contains("x3xtube")).ToList();
                }
                ViewBag.categorias = new SelectList(Categorias, "Value", "Text", null);
                ViewBag.providers = new SelectList(Providers, "Value", "Text", null);
                //check for duplicates values
                videos = videos.Distinct(new VideosComparer()).ToList();
            }
            catch (Exception ex)
            {
                Logs.logger.Error(Logs.ToMessageAndCompleteStacktrace(ex));
                Response.StatusCode = 400;
            }


            return PartialView("_List", videos.ToPagedList(page, 18));
        }

        [GET("Videos/Video/{video}")]
        public ActionResult Video(Guid video)
        {
            try
            {

                var model = db.Videos.Where(m => m.id == video).FirstOrDefault();
                return PartialView("_Video", model);

            }
            catch (Exception ex)
            {

                Logs.logger.Error(Logs.ToMessageAndCompleteStacktrace(ex));
                Response.StatusCode = 400;
            }
            return PartialView("_Video");

        }

        [GET("Videos/FindVideos/{text}")]
        public ActionResult FindVideos(string text, int page = 1)
        {
            var videos = new List<Videos>();
            //Create a list of select list items - this will be returned as your select list
            List<SelectListItem> newList = new List<SelectListItem>();

            var i = 0;

            videos = db.Videos.ToList();
            var categorias = videos.Select(m => m.Category).Distinct();

            videos = videos.Where(m => m.Title.Contains(text)).ToList();


            SelectListItem itemporn = new SelectListItem() { Value = null, Text = "PornMobile Videos" };
            newList.Add(itemporn);
            foreach (var item in categorias)
            {
                SelectListItem selListItem = new SelectListItem() { Value = i.ToString(), Text = item };
                newList.Add(selListItem);
            }
            ViewBag.categorias = new SelectList(newList, "Value", "Text", null);
            //check for duplicates values
            videos = videos.Distinct(new VideosComparer()).ToList();

            return PartialView("_List", videos.ToPagedList(1, 18));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class VideosComparer : IEqualityComparer<Videos>
    {
        public bool Equals(Videos x, Videos y)
        {
            if (x.Url == y.Url)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetHashCode(Videos obj)
        {
            return obj.Url.GetHashCode();
        }
    }

}