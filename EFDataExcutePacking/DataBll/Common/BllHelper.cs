using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDAL;
using EFramework;
using System.Linq.Expressions;
using System.Transactions;
namespace DataBll.Common
{
    public class BllHelper
    {
        #region 插入
        //执行DAL中的 泛型方法
        public int InsertUser(T_User user)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.CallMethod<T_User>(new Func<T_User, int>(dao.AddObject), user);
        }
        #endregion

        #region 获取
        public T_User GetUser(string filter, params object[] args)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.GetObject<T_User>(filter, args);
        }

        public T_User GetUser(Object keyValue)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.GetObject<T_User>(keyValue);
        }

        public ICollection<T_User> GetUsers()
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.GetObjects<T_User>("Nid>@0", new object[] { 2 });
        }
        #endregion

        #region 删除
        public int DeleteUsers(string filter,params Object[] args)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.DeleteObjects<T_User>(filter, args);
        }

        public int DeleteUsers(Expression<Func<T_User,bool>> expression)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.DeleteObjects<T_User>(expression);
        }

        #endregion 

        #region 更新
        public int UpdateUser()
        {
            DaoTemplate dao = new DaoTemplate();
            T_User user =new T_User{ Nid=1, Name="Tommy",Address="USA"};
            return dao.UpdateObject<T_User>(user);

        }

        public int UpdateUsers()
        {
            DaoTemplate dao = new DaoTemplate();
            //Expression是一个表达式，编译应该变成SQL语句
            Expression<Func<T_User, T_User>> predicate = user1 => new T_User { Name ="111" };
            return dao.UpdateObjects<T_User>("Nid>@0", predicate, new Object[] { 2 });

        }
        //如下面的代码所示，表达式的参数为Employee类型，而调用此方法时并没有传入参数，而是在方法内通过 参数的.Where来使用表达式的
        //public ActionResult Index3(Expression<Func<Employee, bool>> predicate)
        //{
        //    var employees = repository.Query().Where(predicate);
        //    return View("Index", employees);
        //}

        #endregion

        #region Attach
        public int AttachAdd()
        {
            DaoTemplate dao = new DaoTemplate();
            T_User user=new T_User{ Name="attach", Address="BBS"};
            return dao.AddObject_Attach<T_User>(user);
        }


        public int AttachDelete()
        {
            DaoTemplate dao = new DaoTemplate();
            T_User user = new T_User { Nid=18, Name = "attach", Address = "BBS" };
            return dao.DeleteObject_Attach<T_User>(user);
        }
        #endregion

        public void Test()
        {
            DaoTemplate dao = new DaoTemplate();
            int i = 0;
            T_User user1 = new T_User { Nid = 13, Name = "11", Address = "111" };
            T_User user2 = new T_User { Nid = 14, Name = "22", Address = "222" };
            T_User user3 = new T_User { Nid = 1, Name = "33", Address = "333" };
            T_User[] users = new T_User[3];
            users[0] = user1;
            users[1] = user2;
            users[2] = user3;

            dao.CallMethod<T_User>(new Func<T_User, int>(dao.AddObject), users);

        }
    }
}
