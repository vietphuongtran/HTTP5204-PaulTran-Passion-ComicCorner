using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicCorner.Models.ViewModels
{
    public class ShowCategory
    {
        //What we need:
        //Showing one category
        public virtual Category Category { get; set; }
        //Showing a list of all comic belong to that Category
        public List<Comic> Comics { get; set; }


    }
}