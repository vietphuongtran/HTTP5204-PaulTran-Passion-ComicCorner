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
    public class ComicController : Controller
    {
        // GET: Comic
        private ComicCornerContext db = new ComicCornerContext();
        public ActionResult List(string ComicSearchKey)
        {
            Debug.WriteLine("I am searching " + ComicSearchKey);
            string query = "Select * from comics";
            if (ComicSearchKey != "")
            {
                query += " where ComicName like " + "'%" + ComicSearchKey + "%'" +
                    " or ComicYear like " + "'%" + ComicSearchKey + "%'" +
                    " or ComicDesc like " + "'%" + ComicSearchKey + "%'" + " or ComicPrice like " + "'%" + ComicSearchKey + "%'";
            }
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@ComicSearchKey", ComicSearchKey);
            db.Comics.SqlQuery(query, sqlparams);
            //Execute the query
            List<Comic> comics = db.Comics.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the comics");
            return View(comics);
        }
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Comic comic = db.Comics.SqlQuery("select * from comics where ComicId=@ComicId", new SqlParameter("@ComicId", id)).FirstOrDefault();
            
            if (comic == null)
            {
                return HttpNotFound();
            }
            //display the categories belong to a comic
            string ShowAllCategory = "select * from Categories inner join ComicCategories on Categories.CategoryId = ComicCategories.Category_CategoryId where Comic_ComicId = @id";
            SqlParameter param = new SqlParameter("@id", id);
            List<Category> CurrentCategories = db.Categories.SqlQuery(ShowAllCategory, param).ToList();

            //display the dropdownlist to choose a comic
            string ShowDdlCategory = "select * from Categories";
            List<Category>AddedCategories = db.Categories.SqlQuery(ShowDdlCategory).ToList();

            //display dropdown list to choose the one who made the comment
            //in real life, customer may have 2 account in a website under the same email but that's rare
            //this is just to demonstrate feature in this assignment
            string ShowDdlCustomer = "select * from Customers";
            List<Customer>AddedCustomers = db.Customers.SqlQuery(ShowDdlCustomer).ToList();

            //display all comment belong to a comic
            string ShowAllReviews = "select * from Reviews where ComicId=@id";
            SqlParameter param2 = new SqlParameter("@id", id);
            List<Review> AllReviews = db.Reviews.SqlQuery(ShowAllReviews, param2).ToList();
            Debug.WriteLine("I am trying to list all the reviews");

            //display the one who make the comment
            string ShowCustomer = "select * from customers inner join reviews on customers.customerid = reviews.customerid where reviews.comicid = @id";
            SqlParameter param3 = new SqlParameter("@id", id);
            List<Customer> AllCustomer = db.Customers.SqlQuery(ShowCustomer, param3).ToList();
            Debug.WriteLine("I am trying to list all the reviews");

            ShowComic viewmodel = new ShowComic();
            viewmodel.Comic = comic;//display the chosen comics
            viewmodel.Categories = CurrentCategories; //display the category a comic belong to
            viewmodel.ddl_Categories = AddedCategories; //display the dropdown list to add the comics
            viewmodel.Reviews = AllReviews;
            viewmodel.ddl_Customers = AddedCustomers;
            viewmodel.Customers = AllCustomer;

            return View(viewmodel);
        }
        //Add the review into a post
        [HttpPost]
        public ActionResult AddReview(int ComicId, string ReviewContent, int CustomerId)
        {
            string query = "Insert into reviews (ReviewContent, ComicId, CustomerId) values (@ReviewContent, @ComicId, @CustomerId)";
            Debug.WriteLine("I am trying to add a review with the content " + ReviewContent + " into the Comic " + ComicId );
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@ComicId", ComicId);
            sqlparams[1] = new SqlParameter("@ReviewContent", ReviewContent);
            sqlparams[2] = new SqlParameter("@CustomerId", CustomerId);
            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("Show/" + ComicId);
        }
        [HttpPost]
        public ActionResult RemoveReview(int ReviewId, int ComicId)
        {
            string query = "Delete from reviews where ReviewId = @ReviewId";
            Debug.WriteLine("I am trying to delete review " + ReviewId);
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@ReviewId", ReviewId);
            
            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("Show/" + ComicId);
        }
        [HttpPost]
        public ActionResult RemoveCategory(int CategoryId, int ComicId)
        {
            string query = "Delete from ComicCategories where Comic_ComicId = @ComicId and Category_CategoryId = @CategoryId";
            Debug.WriteLine("I am trying to add a Comic with id of " + ComicId + "and a CategoryId of " + CategoryId);
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@ComicId", ComicId);
            sqlparams[1] = new SqlParameter("@CategoryId", CategoryId);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("Show/" + ComicId);
        }
        //Insert the comic category into the bridging table
        [HttpPost] 
        public ActionResult AddCategory(int CategoryId, int ComicId)
        {
            string query = "Insert into ComicCategories (Comic_ComicId, Category_CategoryId) values (@ComicId, @CategoryId)";
            Debug.WriteLine("I am trying to add a Comic with id of " + ComicId + "and a CategoryId of " + CategoryId);
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@ComicId", ComicId);
            sqlparams[1] = new SqlParameter("@CategoryId", CategoryId);
            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("Show/" + ComicId);
        }
        //This code is to display the adding page
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string ComicName, string ComicDesc, int ComicYear, decimal ComicPrice, int HasPic)
        {
            //Write the query
            string query = "Insert into comics (ComicName, ComicDesc, ComicYear, ComicPrice, HasPic) values (@ComicName, @ComicDesc, @ComicYear, @ComicPrice, @HasPic)";
            Debug.WriteLine("I am trying to add" + ComicName);
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@ComicName", ComicName);
            sqlparams[1] = new SqlParameter("@ComicDesc", ComicDesc);
            sqlparams[2] = new SqlParameter("@ComicYear", ComicYear);
            sqlparams[3] = new SqlParameter("@ComicPrice", ComicPrice);
            sqlparams[4] = new SqlParameter("@HasPic", HasPic);

            //Execute Query
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //return to the List
            return RedirectToAction("List");
        }
        //This is to show the Update page
        public ActionResult Update(int id)
        {
            //Display selected comics            
            Comic selectedcomic = db.Comics.SqlQuery("select * from comics where ComicId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to update comic id" + id);
            
            //Display all the categories
            List<Category> Category = db.Categories.SqlQuery("select * from categories").ToList();

            //Instanciate a new instance of the UpdateComic Class
            UpdateComic UpdateComicViewModel = new UpdateComic();
            UpdateComicViewModel.Comic = selectedcomic;
            UpdateComicViewModel.Category = Category;

            return View(UpdateComicViewModel);
        }
        //This is to actually update the comic
        [HttpPost]
        public ActionResult Update(int ComicId, string ComicName, string ComicDesc, int ComicYear, decimal ComicPrice, HttpPostedFileBase ComicPic)
        {
            //This code is borrowed from Christine Bittle used for academic purpose
            int HasPic = 0;
            string comicpicextension = "";
            
            if (ComicPic != null)
            {
                Debug.WriteLine("Something identified...");
                
                if (ComicPic.ContentLength > 0)
                {
                    Debug.WriteLine("Successfully Identified Image");
                    
                    var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                    var extension = Path.GetExtension(ComicPic.FileName).Substring(1);

                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the image
                            string fn = ComicId + "." + extension;

                            //get a direct file path to ~/Content/Comics/{id}.{extension}
                            string path = Path.Combine(Server.MapPath("~/Content/Comics/"), fn);

                            //save the file
                            ComicPic.SaveAs(path);
                            //if these are all successful then we can set these fields
                            HasPic = 1;
                            comicpicextension = extension;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Comic Image was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }
                    }
                }
            }
            //Write the query
            Debug.WriteLine("I am trying to update " + ComicId);
            string UpdateQuery = "Update comics set ComicName = @ComicName, ComicDesc = @ComicDesc, ComicYear = @ComicYear, ComicPrice = @ComicPrice, HasPic=@HasPic, PicExtension=@comicpicextension where ComicId = @ComicId";
            Debug.WriteLine("I am trying to update " + ComicId);
            //Parameterized the query
            SqlParameter[] sqlparams = new SqlParameter[7];
            sqlparams[0] = new SqlParameter("@ComicName", ComicName);
            sqlparams[1] = new SqlParameter("@ComicDesc", ComicDesc);
            sqlparams[2] = new SqlParameter("@ComicYear", ComicYear);
            sqlparams[3] = new SqlParameter("@ComicPrice", ComicPrice);
            sqlparams[4] = new SqlParameter("@ComicId", ComicId);
            sqlparams[5] = new SqlParameter("@HasPic", HasPic);
            sqlparams[6] = new SqlParameter("@comicpicextension", comicpicextension);
            //Execute Query
            db.Database.ExecuteSqlCommand(UpdateQuery, sqlparams);

            //Insert into the join table
            //Problems: multiple id of category will be selected and we don't know how many users choose so we can not use one query to add
            //Solution: try to add in the Show view
            /*string JoinTableQuery = "Insert into ComicCategories (Comic_ComicId, Category_CategoryId) values (@ComicId, @CategoryId)";

            SqlParameter[] sqlparams2 = new SqlParameter[2];
            sqlparams2[0] = new SqlParameter("@ComicId", ComicId);
            sqlparams2[1] = new SqlParameter("@CategoryId", CategoryId);
            //Execute
            db.Database.ExecuteSqlCommand(JoinTableQuery, sqlparams2);*/
            return RedirectToAction("Show/" + ComicId);
        }
        //Show the Delete Confirm
        public ActionResult DeleteConfirm(int id)
        {
            //Display selected comics            
            Comic selectedcomic = db.Comics.SqlQuery("select * from comics where ComicId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to update comic id" + id);
            return View(selectedcomic);
        }
        //Delete the Comic
        [HttpPost]
        //Problem: Can not have method DeleteConfirm with the same parameters
        //Solution: Throw an fake parameters int id in the method.
        public ActionResult DeleteConfirm(int ComicId, int id)
        {
            //Delete the comic from Bridging table
            string DeleteFromBridge = "Delete from ComicCategories where Comic_ComicId = @ComicId";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@ComicId", ComicId);
            db.Database.ExecuteSqlCommand(DeleteFromBridge, sqlparams);

            //Delete all the reviews related to that comics
            string DeleteComment = "Delete from Reviews where ComicId = @ComicId";
            SqlParameter[] sqlparams3 = new SqlParameter[1];
            sqlparams3[0] = new SqlParameter("@ComicId", ComicId);
            db.Database.ExecuteSqlCommand(DeleteComment, sqlparams3);

            //Delete the comic from Comics Table
            string DeleteFromTable = "Delete from Comics where ComicId = @ComicId";
            SqlParameter[] sqlparams2 = new SqlParameter[1];
            sqlparams2[0] = new SqlParameter("@ComicId", ComicId);
            db.Database.ExecuteSqlCommand(DeleteFromTable, sqlparams2);

            

            return RedirectToAction("List");
        }
    }
}