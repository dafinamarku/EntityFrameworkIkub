using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework1Ikub.Models;

namespace EntityFramework1Ikub.Controllers
{
    public class ProductController : Controller
    {
        EFDBFirstDatabaseEntities1 db = new EFDBFirstDatabaseEntities1();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListOfProducts()
        {
          List<Product> products = db.Products.ToList(); 
          return View(products);
        }

        public ActionResult storedProcedure()
        {
          SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter("@BrandID", 2) };
          List<Product> products = db.Database.SqlQuery<Product>("exec getProductsByBrandID @BrandID", sqlParameters).ToList();
          return View(products);
        }

        public ActionResult Create()
        { 
          ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName") ;
          ViewBag.Brands = new SelectList(db.Brands, "BrandID", "BrandName");
          return View();
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
          db.Products.Add(p);
          db.SaveChanges();
          return RedirectToAction("ListOfProducts");
        }

        public ActionResult Edit(long id)
        {
          Product p = db.Products.Where(x => x.ProductID == id).FirstOrDefault();
          ViewBag.Categories = new SelectList(db.Categories, "CategoryID", "CategoryName");
          ViewBag.Brands = new SelectList(db.Brands, "BrandID", "BrandName");
          return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Product p)
        {
          Product existingProduct = db.Products.Where(x => x.ProductID == p.ProductID).FirstOrDefault();
          existingProduct.ProductName = p.ProductName;
          existingProduct.Price = p.Price;
          existingProduct.CategoryID = p.CategoryID;
          existingProduct.BrandID = p.BrandID;
          existingProduct.AvailabilityStatus = p.AvailabilityStatus;
          existingProduct.DateOfPurchase = p.DateOfPurchase;
          existingProduct.Active = p.Active;
          db.SaveChanges();
          return RedirectToAction("ListOfProducts");
        }

        public ActionResult Details(long id)
        {
          Product p = db.Products.Where(x => x.ProductID == id).FirstOrDefault();
          return View(p);
        }

        public ActionResult Delete(long id)
        {
          Product p = db.Products.Where(x => x.ProductID == id).FirstOrDefault();
          return View(p);
        }

        [HttpPost]
        public ActionResult Delete(long id, Product p)
        {
          Product pr = db.Products.Where(x => x.ProductID == id).FirstOrDefault();
          db.Products.Remove(pr);
          db.SaveChanges();
          return RedirectToAction("ListOfProducts");
        }
    }
}