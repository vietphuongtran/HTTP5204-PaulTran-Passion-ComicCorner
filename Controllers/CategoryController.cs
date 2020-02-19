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
using System.IO;


namespace ComicCorner.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        private ComicCornerContext db = new ComicCornerContext();
        public ActionResult List(string CategorySearchKey)
        {
            Debug.WriteLine("I am searching " + CategorySearchKey);
            string query = "Select * from Categories";
            if (CategorySearchKey != "")
            {
                query += " where CategoryName like " + "'%" + CategorySearchKey + "%'" +
                    " or CategoryDesc like " + "'%" + CategorySearchKey + "%'";
            }
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CategorySearchKey", CategorySearchKey);
            db.Comics.SqlQuery(query, sqlparams);
            //Execute the query
            List<Category> categories = db.Categories.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the categories");
            return View(categories);
        }

        public ActionResult Show(int? id, string ComicSearchKey)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.SqlQuery("select * from categories where categoryid=@CategoryId", new SqlParameter("@CategoryId", id)).FirstOrDefault();
            if (category == null)
            {
                return HttpNotFound();
            }
            //display all the comics belong to a categories
            string DisplayComicQuery = "select * from Comics inner join ComicCategories on Comics.ComicId = ComicCategories.Comic_ComicId where Category_CategoryId = @id";
            SqlParameter param = new SqlParameter("@id", id);
            List<Comic> Comics = db.Comics.SqlQuery(DisplayComicQuery, param).ToList();           

            ShowCategory viewmodel = new ShowCategory();
            viewmodel.Category = category;//display the chosen category
            viewmodel.Comics = Comics; //display all the comics  

            string SearchComicQuery = "Select * from comics";
            if (ComicSearchKey != "")
            {
                SearchComicQuery += " where ComicName like " + "'%" + ComicSearchKey + "%'" +
                    " or ComicYear like " + "'%" + ComicSearchKey + "%'" +
                    " or ComicDesc like " + "'%" + ComicSearchKey + "%'" + " or ComicPrice like " + "'%" + ComicSearchKey + "%'";
            }
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@ComicSearchKey", ComicSearchKey);
            db.Comics.SqlQuery(SearchComicQuery, sqlparams);

            return View(viewmodel);           
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string CategoryName, string CategoryDesc)
        {
            //Write the query
            string query = "Insert into categories (CategoryName, CategoryDesc) values (@CategoryName, @CategoryDesc)";
            Debug.WriteLine("I am trying to add" + CategoryName);
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@CategoryName", CategoryName);
            sqlparams[1] = new SqlParameter("@CategoryDesc", CategoryDesc);            
            //Execute Query
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //return to the List
            return RedirectToAction("List");
        }
        public ActionResult Update(int id)
        {
            //Display selected categories
            Category selectedcategory = db.Categories.SqlQuery("select * from categories where CategoryId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(selectedcategory);
        }
        [HttpPost]
        public ActionResult Update(string CategoryName, string CategoryDesc, int CategoryId, HttpPostedFileBase CategoryPic)
        {
            //This code is borrowed from Christine Bittle used for academic purpose
            int HasPic = 0;
            string categorypicextension = "";

            if (CategoryPic != null)
            {
                Debug.WriteLine("Something identified...");

                if (CategoryPic.ContentLength > 0)
                {
                    Debug.WriteLine("Successfully Identified Image");

                    var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                    var extension = Path.GetExtension(CategoryPic.FileName).Substring(1);

                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the image
                            string fn = CategoryId + "." + extension;

                            //get a direct file path to ~/Content/Categories/{id}.{extension}
                            string path = Path.Combine(Server.MapPath("~/Content/Categories/"), fn);

                            //save the file
                            CategoryPic.SaveAs(path);
                            //if these are all successful then we can set these fields
                            HasPic = 1;
                            categorypicextension = extension;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Category Image was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }
                    }
                }
            }
            //Write the query
            string query = "Update categories set CategoryName = @CategoryName, CategoryDesc = @CategoryDesc, HasPic=@HasPic, PicExtension=@categorypicextension where CategoryId = @CategoryId";
            Debug.WriteLine("I am trying to update" + CategoryName);
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@CategoryName", CategoryName);
            sqlparams[1] = new SqlParameter("@CategoryDesc", CategoryDesc);
            sqlparams[2] = new SqlParameter("@CategoryId", CategoryId);
            sqlparams[3] = new SqlParameter("@HasPic", HasPic);
            sqlparams[4] = new SqlParameter("@categorypicextension", categorypicextension);
            //Execute Query
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //return to the List
            return RedirectToAction("List");
        }
        //Show the Delete Confirm
        public ActionResult DeleteConfirm(int id)
        {
            //Display selected categories
            Category selectedcategory = db.Categories.SqlQuery("select * from categories where CategoryId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(selectedcategory);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int CategoryId, int id)
        {
            //Delete the category from Bridging table
            string DeleteFromBridge = "Delete from ComicCategories where Category_CategoryId = @CategoryId";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CategoryId", CategoryId);
            db.Database.ExecuteSqlCommand(DeleteFromBridge, sqlparams);

            //Delete the category from Categories Table
            string DeleteFromTable = "Delete from Categories where CategoryId = @CategoryId";
            SqlParameter[] sqlparams2 = new SqlParameter[1];
            sqlparams2[0] = new SqlParameter("@CategoryId", CategoryId);
            db.Database.ExecuteSqlCommand(DeleteFromTable, sqlparams2);
            
            return RedirectToAction("List");
        }
    }
}