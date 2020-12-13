using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xxx.Data
{
    public class servicioXXX
    {
        pornmaleonEntities db = new pornmaleonEntities();
        //public List<Videos> ObtenerVideos() {
        //    return db.Videos.;
        //}
        public IQueryable<Videos> GetVideos()
        {
            return db.Set<Videos>();
        }
    }
}