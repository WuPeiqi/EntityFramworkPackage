using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataDAL;
using EFramework;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data.SqlClient;
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

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public int DeleteUsers()
        {
            DaoTemplate dao = new DaoTemplate();
            
            return dao.DeleteObjects<T_User>("Nid>@0", new Object[] {7 });
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <returns></returns>
        public int UpdateUsers()
        {
            DaoTemplate dao = new DaoTemplate();
            //Expression是一个表达式，编译应该变成SQL语句
            Expression<Func<T_User, T_User>> predicate = user1 => new T_User { Name ="111" };
            return dao.UpdateObjects<T_User>("Nid>@0", predicate, new Object[] { 2 });
        }

        /// <summary>
        /// 执行sql命令（调用的是DaoTemplate的扩展方法）
        /// </summary>
        /// <returns></returns>
        public ICollection<T_User> ExcuteCmd()
        {
            DaoTemplate dao = new DaoTemplate();
            SqlParameter pra = new SqlParameter("@Nid", System.Data.SqlDbType.Int);
            pra.Value = 2;
            return dao.ExecuteSqlQuery<T_User>("select * from T_User where Nid<>@Nid", new DbParameter[] { pra });
        }

    }
}
