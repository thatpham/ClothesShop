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
    public class ProductDAO
    {
        private OnlineShopDBContext db = null;
        //Create Constructor
        public ProductDAO()
        {
            db = new OnlineShopDBContext();
        }
        //Add product use Entity Framework
        public long Create(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return product.ID;
        }

        //Edit product use Entity Framework
        public long Edit(Product product)
        {
            if (string.IsNullOrEmpty(product.MetaTitle))
            {
                if (!string.IsNullOrEmpty(product.Name))
                {
                    product.MetaTitle = StringHelper.ToUnsignString(product.Name);
                }
                else
                {
                    product.MetaTitle = "";
                }
            }
            var query = db.Products.Where(x => x.ID == product.ID).SingleOrDefault();
            if (query != null)
            {
                query.Name = product.Name;
                query.Code = product.Code;
                query.MetaTitle = StringHelper.ToUnsignString(product.Name);
                query.Description = product.Description;
                query.CategoryID = product.CategoryID;
                query.Price = product.Price;
                query.PromotionPrice = product.PromotionPrice;
                query.includedVAT = product.includedVAT;
                query.Quantity = product.Quantity;
                query.Warranty = product.Warranty;
                query.Image = product.Image;
                query.Detail = product.Detail;
                query.MetaDescriptions = product.MetaDescriptions;
                query.MetaKeywords = product.MetaKeywords;
                query.ModifiedDate = DateTime.Now;
                query.ModifiedBy = product.ModifiedBy;
                query.Status = product.Status;
            }
            db.SaveChanges();
            return product.ID;
        }

        //Delete product use Entity Framework
        public bool Delete(long id)
        {
            var query = db.Contents.SingleOrDefault(x=>x.ID==id);
            if(query!=null)
            {
                db.Contents.Remove(query);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Product> ListAllPaging(string searchString,int page,int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
    }
}
