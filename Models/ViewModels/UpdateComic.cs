using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicCorner.Models.ViewModels
{
    public class UpdateComic
    {
        //We need one comic and a list of all categories
        public Comic Comic { get; set; }
        public List<Category> Category { get; set; }
    }
}