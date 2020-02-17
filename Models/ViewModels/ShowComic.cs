using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicCorner.Models.ViewModels
{
    public class ShowComic
    {
        //showing ONE Comic
        public virtual Comic Comic { get; set; }
        //showing all categories belong to one comic
        public List<Category> Categories { get; set; }
        //showing category in a drop down list
        public List<Category> ddl_Categories { get; set; }
        //showing all the reviews belong to one comic 
        public List<Review> Reviews { get; set; }
    }
}
