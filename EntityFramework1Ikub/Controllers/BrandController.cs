using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework1Ikub.Models;

namespace EntityFramework1Ikub.Controllers
{
    public class BrandController : Controller
    {
        EFDBFirstDatabaseEntities1 db = new EFDBFirstDatabaseEntities1();
        // GET: Brand
        public ActionResult Index(string search = "", string SortColumn = "ProductName", string IconClass = "fa-sort-asc", int PageNo = 1)
        {
          ViewBag.search = search;
          List<Brand> brands = db.Brands.Where(x => x.BrandName.Contains(search)).ToList();
          ViewBag.SortColumn = SortColumn;
          ViewBag.IconClass = IconClass;

          if (ViewBag.SortColumn == "BrandID")
          {
            if (ViewBag.IconClass == "fa-sort-asc")
              brands = brands.OrderBy(x => x.BrandID).ToList();
            else
              brands = brands.OrderByDescending(x => x.BrandID).ToList();
          }
          else if (ViewBag.SortColumn == "BrandName")
          {
            if (ViewBag.IconClass == "fa-sort-asc")
              brands = brands.OrderBy(x => x.BrandName).ToList();
            else
              brands = brands.OrderByDescending(x => x.BrandName).ToList();
          }

          /* Paging */
          int NoOfRecordsPerPage = 5;
          int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(brands.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
          int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
          ViewBag.PageNo = PageNo;
          ViewBag.NoOfPages = NoOfPages;
          brands = brands.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();

          return View(brands);
        }

        public ActionResult Details(long id)
        {
          Brand c = db.Brands.Where(x => x.BrandID == id).FirstOrDefault();
          return View(c);
        }

        public ActionResult Edit(long id)
        {
         Brand existingBrand = db.Brands.Where(x => x.BrandID == id).FirstOrDefault();
          if (TempData["Message"] != null)
          {
            ViewBag.Message = TempData["Message"];
          }
          return View(existingBrand);
        }
        [HttpPost]
        public ActionResult Edit(Brand c)
        {
          Brand existingBrand = db.Brands.Where(x => x.BrandID == c.BrandID).FirstOrDefault();
          List<Brand> sameNameBrand = db.Brands.Where(x => x.BrandName == c.BrandName).ToList();
          if (sameNameBrand.Count() == 0)
          {
            existingBrand.BrandName = c.BrandName;
            db.SaveChanges();
            return RedirectToAction("Index");
          }
          else
          {
            TempData["Message"] = "A brand with this name already exists.";
            return RedirectToAction("Edit");
          }
        }

        public ActionResult Delete(long id)
        {
          Brand c = db.Brands.Where(x => x.BrandID == id).FirstOrDefault();
          return View(c);
        }

        [HttpPost]
        public ActionResult Delete(Brand c)
        {
          Brand deletecat = db.Brands.Where(x => x.BrandID == c.BrandID).FirstOrDefault();
          //lista e produkteve qe kane si brand, brand-in qe do te fshihet
          List<Product> products = db.Products.Where(p => p.BrandID == c.BrandID).ToList();
          db.Products.RemoveRange(products);
          db.Brands.Remove(deletecat);
          db.SaveChanges();
          return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
          if (TempData["Message"] != null)
          {
            ViewBag.Message = TempData["Message"];
          }
          return View();
        }

        [HttpPost]
        public ActionResult Create(Brand c)
        {
          List<Brand> br = db.Brands.Where(x => x.BrandName == c.BrandName).ToList();
          if (br.Count() == 0)
          {
            db.Brands.Add(c);
            db.SaveChanges();
            return RedirectToAction("Index");
          }
          TempData["Message"] = "This brand already exists.";
          return RedirectToAction("Create");


        }
  }
}