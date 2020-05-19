using Model.DAO;
using Model.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothesShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        public string UserName { set; get; }
        UserDAO dao = new UserDAO();
        // GET: Admin/User
        public ActionResult Index(string searchString,int page=1,int pageSize=10)
        {
            //UserDAO dao = new UserDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Add(User user)
        {
            if(ModelState.IsValid)
            {
                //var dao = new UserDAO();
                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = this.UserName;
                user.Password = encryptedMd5Pas;
                long id = dao.Add(user);
                if (id > 0)
                {
                    SetAlert("Thêm user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công");
                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            //UserDAO dao = new UserDAO();
            var model = dao.ViewDetail(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            UserDAO dao = new UserDAO();
            user.ModifiedBy = this.UserName;
            user.ModifiedDate = DateTime.Now;
            long id = dao.Edit(user);
            if (id > 0)
            {
                SetAlert("Chỉnh sửa  user thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Chỉnh sửa user không thành công");
            }
            return View("Index");
        }
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            //UserDAO dao = new UserDAO();
            bool result = dao.Delete(id);
            if (result)
            {
                SetAlert("Xóa user thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Xóa user không thành công");
            }
            return View();
        }
        public JsonResult ChangeStatus(long id)
        {
            var result = dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}