using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicCorner.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }


        //1: HasPic, 0: No pic
        public int HasPic { get; set; }
        //contain options like .jpg, .jpeg, .gif, .png
        public string PicExtension { get; set; }

        //One comic belong to many categories and 1 category can have many comics
        public ICollection<Comic> Comics { get; set; }
    }
}