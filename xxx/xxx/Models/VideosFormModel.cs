using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace xxx.Models
{
    public class VideosFormModel
    {
        public VideosFormModel()
        {

        }
        public List<Videos> Videos { get; set; }
        public SelectList Categorias { get; set; }
        public string categoria { get; set; }
    }
}