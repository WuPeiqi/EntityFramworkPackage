using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDAL;
using EFramework;
namespace DataBll.Common
{
    public class BllHelper
    {
        //执行DAL中的 泛型方法
        public int InsertUser(T_User user)
        {
            DaoTemplate dao = new DaoTemplate();
            return dao.CallMethod<T_User>(new Func<T_User, int>(dao.AddObject), user);
        }

        //执行DAL中的 普通方法
        public int InsertUser(Object obj)
        {
            DaoTemplate dao = new DaoTemplate();
            //Func<object, int> predicate = dao.AddObject;
            //return dao.CallMethod(predicate, obj);
            return dao.CallMethod(new Func<Object, int>(dao.AddObject), obj);
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
    }
}
