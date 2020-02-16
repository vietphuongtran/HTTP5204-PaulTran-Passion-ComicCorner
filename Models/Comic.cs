using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicCorner.Models
{
    public class Comic
    {
        [Key]
        public int ComicId { get; set; }
        public string ComicName { get; set; }        
        public string ComicDesc { get; set; }
        //This will only include the public year of the comic so I use int rather than DateTime
        public int ComicYear { get; set; }
        //Currency: CAD   
        public decimal ComicPrice { get; set; }
        //One comic belong to many categories and 1 category can have many comics
        public ICollection<Category> Categories { get; set; }
    }
}