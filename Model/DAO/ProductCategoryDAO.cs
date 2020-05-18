using Common;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductCategoryDAO
    {
        OnlineShopDBContext db = null;
        public  ProductCategoryDAO()
        {
            db = new OnlineShopDBContext();
        }

        public long Create(ProductCategory productCategory)
        {
            db.ProductCategories.Add(productCategory);
            db.SaveChanges();
            return productCategory.ID;
        }
        public long Edit(ProductCategory productCategory)
        {
            if(string.IsNullOrEmpty(productCategory.MetaTitle))
            {
                if(!string.IsNullOrEmpty(productCategory.Name))
                {
                    productCategory.MetaTitle = StringHelper.ToUnsignString(productCategory.Name);
                }
                else
                {
                    productCategory.MetaTitle = "";
                }
            }
            var query = db.ProductCategories.Where(x => x.ID == productCategory.ID).SingleOrDefault();
            if(query!=null)
            {
                query.Name = productCategory.Name;
                query.MetaTitle = productCategory.MetaTitle;
                query.ParentID = productCategory.ParentID;
                query.DisplayOrder = productCategory.DisplayOrder;
                query.SeoTitle = productCategory.SeoTitle;
                query.ModifiedBy = productCategory.ModifiedBy;
                query.ModifiedDate = productCategory.ModifiedDate;
                query.MetaDescriptions = productCategory.MetaDescriptions;
                query.MetaKeywords = productCategory.MetaKeywords;
                query.Status = productCategory.Status;
                query.ShowOnHome = productCategory.ShowOnHome;
            }
            db.SaveChanges();
            return productCategory.ID;
        }

        public bool Delete(long id)
        {
            var query = db.Contents.SingleOrDefault(x=>x.ID==id);
            if(query!=null)
            {
                db.Contents.Remove(query);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public  List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<ProductCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}
