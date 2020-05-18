using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString,int page=1,int pageSize=5)
        {
            ProductDAO dao = new ProductDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            ProductDAO dao = new ProductDAO();
            product.CreatedDate = DateTime.Now;
            product.CreatedBy = "admin";
            long id = dao.Create(product);
            
            if(id >0 )
            {
                SetAlert("Thêm sản phẩm thành công", "success");
                SetViewBag(product.CategoryID);
                return RedirectToAction("Index", "Product");

            }
            else
            {
                ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            ProductDAO dao = new ProductDAO();
            var product = dao.ViewDetail(id);
            SetViewBag(product.CategoryID);
            return View(product);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product product)
        {
            ProductDAO dao = new ProductDAO();
            product.ModifiedDate = DateTime.Now;
            product.ModifiedBy = "admin";
            long id = dao.Edit(product);

            if (id > 0)
            {
                SetAlert("Chỉnh sửa sản phẩm thành công", "success");
                SetViewBag(product.CategoryID);
                return RedirectToAction("Index", "Product");

            }
            else
            {
                ModelState.AddModelError("", "Chỉnh sửa sản phẩm thất bại");
            }
            return View();
        }

        public void SetViewBag(long? selectedValue = null)
        {
            var dao = new ProductCategoryDAO();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedValue);
        }
    }
}