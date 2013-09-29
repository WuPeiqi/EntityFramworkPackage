using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDAL;
using EFramework;
using System.Linq.Expressions;
namespace DataBll.Common
{
    public class BllHelper
    {

        //执行DAL中的 普通方法
        public int InsertUser(Object obj)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.CallMethod(new Func<Object, int>(dao.AddObject), obj);
        }

        //执行DAL中的 泛型方法
        public int InsertUser(T_User user)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.CallMethod<T_User>(new Func<T_User, int>(dao.AddObject), user);
        }


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

        public int DeleteUsers()
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.DeleteObjects<T_User>("Nid>@0", new Object[] {7 });
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

    }
}
