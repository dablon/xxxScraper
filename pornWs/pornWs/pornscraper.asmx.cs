using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using pornWs.Scraper;
using pornWs.Models;

namespace pornWs
{
    /// <summary>
    /// Summary description for pornscraper
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class pornscraper : System.Web.Services.WebService
    {


        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// WebMethod to scrap Site pornhub.com
        /// with the scrap info
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getPornHubVideos(int numberPages,int startFromPage)
        {

            var pornhub = new scraper();
            var date = pornhub.getLocalServerDate();
            try
            {
                var objPornhub = new List<Videos>();

                objPornhub = pornhub.getVideosFromPornHub(numberPages,startFromPage);
                //  objPornhub = pornhub.getVideosByCatFromPornHub(numberPages);
                var resultado = "********scrap " + objPornhub.Count() + " videos********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = pornhub.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error);
                return error;
            }

        }

        /// <summary>
        /// WebMethod to scrap Site pornhub.com
        /// by categories
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getX3xtubeVideos(int numberPages, int startFromPage)
        {

            var x3xtube = new scraper();
            var date = x3xtube.getLocalServerDate();
            try
            {
                var objx3xtube = new List<Videos>();

                //objPornhub = pornhub.getVideosFromPornHub(numberPages, startFromPage);
                objx3xtube = x3xtube.getVideosX3xtube(numberPages, startFromPage);
                var resultado = "********scrap " + objx3xtube.Count() + " videos from X3XTUBE********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = x3xtube.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error);
                return error;
            }

        }

        /// <summary>
        /// WebMethod to scrap Site pornhub.com
        /// by categories
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getYouPornVideos(int numberPages, int startFromPage)
        {

            var youPorn = new scraper();
            var date = youPorn.getLocalServerDate();
            try
            {
                var objyouPorn = new List<Videos>();

                //objPornhub = pornhub.getVideosFromPornHub(numberPages, startFromPage);
                objyouPorn = youPorn.getVideosYouPorn(numberPages, startFromPage);
                var resultado = "********scrap " + objyouPorn.Count() + " videos from YOUPORN********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = youPorn.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error);
                return error;
            }

        }

        /// <summary>
        /// WebMethod to scrap Categories from YouJizz.com
        /// by categories
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getYouJizzVideosByCat(int numberPages, int startFromPage, int startFromCat)
        {

            var YouJizz = new scraper();
            var date = YouJizz.getLocalServerDate();
            try
            {
                var objYouJizz = new List<Videos>();

                //objPornhub = pornhub.getVideosFromPornHub(numberPages, startFromPage);
                objYouJizz = YouJizz.getVideosByCatYouJizz(numberPages, startFromPage, startFromCat);
                var resultado = "********scrap " + objYouJizz.Count() + " videos from YouJizz********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = YouJizz.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error + " provider=YouJizz");
                return error;
            }

        }
        /// <summary>
        /// WebMethod to scrap Categories from KeezMovies.com
        /// by categories
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getKeezMoviesVideosByCat(int numberPages, int startFromPage,int startFromCat)
        {

            var KeezMovies = new scraper();
            var date = KeezMovies.getLocalServerDate();
            try
            {
                var objKeezMovies = new List<Videos>();

                //objPornhub = pornhub.getVideosFromPornHub(numberPages, startFromPage);
                objKeezMovies = KeezMovies.getVideosByCatKeezmovies(numberPages, startFromPage,startFromCat);
                var resultado = "********scrap " + objKeezMovies.Count() + " videos from KeezMovies********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = KeezMovies.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error + " provider=KeezMovies");
                return error;
            }

        }
       
 
        /// <summary>
        /// WebMethod to scrap Categories from youporn.com
        /// by categories
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getYouPornVideosByCat(int numberPages, int startFromPage, int startFromCat)
        {

            var youPorn = new scraper();
            var date = youPorn.getLocalServerDate();
            try
            {
                var objyouPorn = new List<Videos>();

                //objPornhub = pornhub.getVideosFromPornHub(numberPages, startFromPage);
                objyouPorn = youPorn.getVideosByCatYouPorn(numberPages, startFromPage,startFromCat);
                var resultado = "********scrap " + objyouPorn.Count() + " videos from YOUPORN********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = youPorn.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error+" provider=YouPorn");
                return error;
            }

        }
       
        /// <summary>
        /// WebMethod to scrap categories from Site pornRabbit.com
        /// by categories
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getPornRabbitVideosByCat(int numberPages, int startFromPage, int startFromCat)
        {

            var pornRabbit = new scraper();
            var date = pornRabbit.getLocalServerDate();
            try
            {
                var objRabbit = new List<Videos>();

                //objPornhub = pornhub.getVideosFromPornHub(numberPages, startFromPage);
                objRabbit = pornRabbit.getVideosByCatFromPornRabbit(numberPages, startFromPage,startFromCat);
                var resultado = "********scrap " + objRabbit.Count() + " videos from PORNRABBIT********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = pornRabbit.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error);
                return error;
            }

        }

        /// <summary>
        /// WebMethod to scrap Site pornhub.com
        /// by categories
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getPornHubByCatVideos(int numberPages, int startFromPage,int startFromCat)
        {

            var pornhub = new scraper();
            var date = pornhub.getLocalServerDate();
            try
            {
                var objPornhub = new List<Videos>();

                //objPornhub = pornhub.getVideosFromPornHub(numberPages, startFromPage);
                objPornhub = pornhub.getVideosByCatFromPornHub(numberPages, startFromPage, startFromCat);
                var resultado = "********scrap " + objPornhub.Count() + " videos from PORNHUB********";
                //  var resultado = pornhub.writeJsonFile(objPornhub, "pornhub");
                //resultado = resultado + " at " + date + " and scrap " + objPornhub.Count() + " porn videos";
                //logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = pornhub.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error);
                return error;
            }

        }
    
        /// <summary>
        /// WebMethod to scrap Site pornhub.com
        /// with the scrap info
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string getPornRabbitVideos(int numberPages, int startFromPage)
        {

            var pornrabbit = new scraper();
            var date = pornrabbit.getLocalServerDate();
            try
            {
                var objPornRabbit = new List<Videos>();

                objPornRabbit = pornrabbit.getVideosFromPornRabbit(numberPages, startFromPage);
                var resultado = pornrabbit.writeJsonFile(objPornRabbit, "pornrabbit");
                resultado = resultado + " at " + date + " and scrap " + objPornRabbit.Count() + " porn videos";
                logger.Info(resultado);
                return resultado;
            }
            catch (Exception ex)
            {
                var error = pornrabbit.ToMessageAndCompleteStacktrace(ex);
                error = error + " at " + date;
                logger.Error(error);
                return error;
            }

        }

        [WebMethod]
        public List<string> getCategories(int maxCategorie) { 
            var pornhub = new scraper();
            var lista=pornhub.CategoriesPornHub(maxCategorie);

            return lista;
        }
    }
}

