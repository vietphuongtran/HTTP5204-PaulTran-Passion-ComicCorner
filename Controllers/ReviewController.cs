using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComicCorner.Data;
using ComicCorner.Models;
using System.Diagnostics;
using ComicCorner.Models.ViewModels;

namespace ComicCorner.Controllers
{
    public class ReviewController : Controller
    {
        private ComicCornerContext db = new ComicCornerContext();
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }
    }
}