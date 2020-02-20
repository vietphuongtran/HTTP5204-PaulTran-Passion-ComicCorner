using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicCorner.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        
        public string CustomerAddress { get; set; }
        

        //1: HasPic, 0: No pic
        public int HasPic { get; set; }
        //contain options like .jpg, .jpeg, .gif, .png
        public string PicExtension { get; set; }

       public ICollection<Review> Reviews { get; set; }
    }
}