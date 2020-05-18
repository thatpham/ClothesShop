using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductCategoryDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDAO();
                //var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                //product.ModifiedBy = session.UserName;
                productCategory.CreatedBy = "admin";
                productCategory.CreatedDate = DateTime.Now;
                long id = dao.Create(productCategory);
                if (id > 0)
                {
                    SetAlert("Thêm danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    SetAlert("Thêm thất bại", "error");
                }

            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ProductCategoryDAO();
            var model = dao.ViewDetail(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDAO();            
                //var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                //product.ModifiedBy = session.UserName;
                productCategory.ModifiedBy = "admin";
                productCategory.ModifiedDate = DateTime.Now;
                long id = dao.Edit(productCategory);
                if (id > 0)
                {
                    SetAlert("Chỉnh sửa danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    SetAlert("Chỉnh sửa thất bại", "error");
                }

            }
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            var dao = new ProductCategoryDAO();
            bool result = dao.Delete(id);
            return RedirectToAction("Index");
        }
    }
 
}