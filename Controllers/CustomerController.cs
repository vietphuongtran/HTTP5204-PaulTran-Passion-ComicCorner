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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        private ComicCornerContext db = new ComicCornerContext();
        public ActionResult List(string CustomerSearchKey)
        {
            Debug.WriteLine("I am searching " + CustomerSearchKey);
            string query = "Select * from customers";
            if (CustomerSearchKey != "")
            {
                query += " where CustomerName like " + "'%" + CustomerSearchKey + "%'" +
                    " or CustomerEmail like " + "'%" + CustomerSearchKey + "%'" +
                    " or CustomerAddress like " + "'%" + CustomerSearchKey + "%'";
            }
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CustomerSearchKey", CustomerSearchKey);
            db.Customers.SqlQuery(query, sqlparams);
            //Execute the query
            List<Customer> customers = db.Customers.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the customers");
            return View(customers);
        }
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.SqlQuery("select * from customers where CustomerId=@id", new SqlParameter("@id", id)).FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
            //To do: Display reviews customer made
        }
        public ActionResult Update(int id)
        {
            //Display customer
            Customer selectedcustomer = db.Customers.SqlQuery("select * from customers where CustomerId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(selectedcustomer);
        }
        [HttpPost]
        public ActionResult Update(string CustomerName, string CustomerAddress, int CustomerId, string CustomerEmail, HttpPostedFileBase CustomerPic)
        {
            //This code is borrowed from Christine Bittle used for academic purpose
            int HasPic = 0;
            string customerpicextension = "";

            if (CustomerPic != null)
            {
                Debug.WriteLine("Something identified...");

                if (CustomerPic.ContentLength > 0)
                {
                    Debug.WriteLine("Successfully Identified Image");

                    var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                    var extension = Path.GetExtension(CustomerPic.FileName).Substring(1);

                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the image
                            string fn = CustomerId + "." + extension;

                            //get a direct file path to ~/Content/Categories/{id}.{extension}
                            string path = Path.Combine(Server.MapPath("~/Content/Customers/"), fn);

                            //save the file
                            CustomerPic.SaveAs(path);
                            //if these are all successful then we can set these fields
                            HasPic = 1;
                            customerpicextension = extension;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Customer Image was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }
                    }
                }
            }
            //Write the query
            string query = "Update Customers set CustomerName = @CustomerName, CustomerAddress = @CustomerAddress, CustomerEmail = @CustomerEmail, HasPic=@HasPic, PicExtension=@customerpicextension where CustomerId = @CustomerId";
            Debug.WriteLine("I am trying to update " + CustomerName);
            //Parameterized query
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@CustomerName", CustomerName);
            sqlparams[1] = new SqlParameter("@CustomerAddress", CustomerAddress);
            sqlparams[2] = new SqlParameter("@CustomerEmail", CustomerEmail);
            sqlparams[3] = new SqlParameter("@HasPic", HasPic);
            sqlparams[4] = new SqlParameter("@customerpicextension", customerpicextension);
            sqlparams[5] = new SqlParameter("@CustomerId", CustomerId);
            //Execute Query
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //return to the List
            return RedirectToAction("List");
        }
    }
}