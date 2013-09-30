using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DataDAL
{
    public  static class DaoTemplateExtendCmd
    {
        //扩展方法必须在非泛型的静态类中=扩展类必须是静态and非泛型
        //扩展方法必须是静态的

        /// <summary>
        /// 扩展方法：执行原始SQL查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dao">扩展的类型</param>
        /// <param name="commandText">要执行查询的SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回符合条件的对象的集合</returns>
        public static ICollection<T> ExecuteSqlQuery<T>(this DaoTemplate dao, string commandText, params DbParameter[] parameters)
        {
            return dao.entityModel.ExecuteStoreQuery<T>(commandText, parameters).ToList<T>();
        }

        /// <summary>
        /// 扩展方法：执行原始SQL命令
        /// </summary>
        /// <param name="dao">扩展的类型</param>
        /// <param name="commandText">SQL命令</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlNonQuery(this DaoTemplate dao,string commandText, params DbParameter[] parameters)
        {
            return dao.entityModel.ExecuteStoreCommand(commandText, parameters);
        }
    }
}
