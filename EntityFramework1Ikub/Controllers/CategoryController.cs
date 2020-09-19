using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework1Ikub.Models;

namespace EntityFramework1Ikub.Controllers
{
    public class CategoryController : Controller
    {
       EFDBFirstDatabaseEntities1 db = new EFDBFirstDatabaseEntities1();
        // GET: Category
        public ActionResult Index(string search = "", string SortColumn = "ProductName", string IconClass = "fa-sort-asc", int PageNo = 1)
        {
          ViewBag.search = search;
          List<Category> categories = db.Categories.Where(x => x.CategoryName.Contains(search)).ToList();
          ViewBag.SortColumn = SortColumn;
          ViewBag.IconClass = IconClass;

          if (ViewBag.SortColumn == "CategoryID")
          {
            if (ViewBag.IconClass == "fa-sort-asc")
              categories = categories.OrderBy(x => x.CategoryID).ToList();
            else
              categories = categories.OrderByDescending(x => x.CategoryID).ToList();
          }
          else if (ViewBag.SortColumn == "CategoryName")
          {
            if (ViewBag.IconClass == "fa-sort-asc")
              categories = categories.OrderBy(x => x.CategoryName).ToList();
            else
              categories = categories.OrderByDescending(x => x.CategoryName).ToList();
          }

          /* Paging */
          int NoOfRecordsPerPage = 5;
          int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(categories.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
          int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
          ViewBag.PageNo = PageNo;
          ViewBag.NoOfPages = NoOfPages;
          categories = categories.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();

          return View(categories);
        }

        public ActionResult Details(long id)
        {
          Category c = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
          return View(c);
        }

        public ActionResult Edit(long id)
        {
          Category existingCategory = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
          if (TempData["Message"] != null)
          {
            ViewBag.Message = TempData["Message"];
          }
          return View(existingCategory);
        }
        [HttpPost]
        public ActionResult Edit(Category c)
        {
          Category existingCategory = db.Categories.Where(x => x.CategoryID == c.CategoryID).FirstOrDefault();
          List<Category> sameNameCat = db.Categories.Where(x => x.CategoryName == c.CategoryName).ToList();
          if (sameNameCat.Count() == 0)
          {
            existingCategory.CategoryName = c.CategoryName;
            db.SaveChanges();
            return RedirectToAction("Index");
          }
          else
          {
            TempData["Message"] = "A category with this name already exists.";
            return RedirectToAction("Edit");
          }
         
        }

        public ActionResult Delete(long id)
        {
          Category c = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
          return View(c);
        }

        [HttpPost]
        public ActionResult Delete(Category c)
        {
          Category deletecat = db.Categories.Where(x => x.CategoryID == c.CategoryID).FirstOrDefault();
          //lista e produkteve qe kane si kategori, kategorine qe do te fshihet
          List<Product> products = db.Products.Where(p => p.CategoryID == c.CategoryID).ToList();
          db.Products.RemoveRange(products);
          db.Categories.Remove(deletecat);
          db.SaveChanges();
          return RedirectToAction("Index");
        }
       
        public ActionResult Create()
        {
          if (TempData["Message"]!=null)
          {
            ViewBag.Message = TempData["Message"];
          }
          return View();
        }
        
        [HttpPost]
        public ActionResult Create(Category c)
        {
          List<Category> cat = db.Categories.Where(x => x.CategoryName == c.CategoryName).ToList();
          if (cat.Count()==0)
          {
            db.Categories.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index");
          }
          TempData["Message"] = "This category already exists.";
          return RedirectToAction("Create");
           
         
        }
    }
}