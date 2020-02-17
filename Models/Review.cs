﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicCorner.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string ReviewContent { get; set; }

        //One to Many
        //One review belong to one comic 
        //But one comic can have many review
        public int ComicId { get; set; }
        [ForeignKey("ComicId")]
        public virtual Comic Comic { get; set; }
    }
}