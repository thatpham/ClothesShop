using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Model.DAO
{
    public class UserDAO
    {
        private OnlineShopDBContext db = null;
        public UserDAO()
        {
            db = new OnlineShopDBContext();
        }

        public long Add(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.ID;
        }
        public long Edit(User entity)
        {
                var user = db.Users.SingleOrDefault(x => x.ID == entity.ID);
                if(user!=null)
                {
                    user.UserName = entity.UserName;
                    if (!string.IsNullOrEmpty(entity.Password))
                    {
                        user.Password = entity.Password;
                    }
                    user.Name = entity.Name;
                    user.Address = entity.Address;
                    user.Email = entity.Email;
                    user.Phone = entity.Phone;
                    user.ModifiedBy = entity.ModifiedBy;
                    user.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                }
                return entity.ID;
        }
        public bool Delete(long id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<User> ListAllPaging(string searchString,int page,int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public int Login(string userName,string passWord,bool isLoginAdmin=false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if(result==null)
            {
                return 0;
            }
            else
            {
                if(isLoginAdmin)
                {
                    if(result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }
        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName==userName); 
        }
        public User ViewDetail(long id)
        {
            return db.Users.Find(id);
        }
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

    }
     
}
