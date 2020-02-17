﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ComicCorner.Data
{
    public class ComicCornerContext : DbContext
    {
        public ComicCornerContext() : base("name=ComicCornerContext")
        {
        }

        public System.Data.Entity.DbSet<ComicCorner.Models.Comic> Comics { get; set; }

        public System.Data.Entity.DbSet<ComicCorner.Models.Category> Categories { get; set; }

    }
}