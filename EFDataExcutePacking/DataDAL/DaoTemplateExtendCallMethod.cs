using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DataDAL
{
    public static class DaoTemplateExtendCallMethod
    {
        public static int CallMethod<T>(this DaoTemplate dao, Func<T, int> method, T param)
        {
            int ret = -1;
            try
            {
                ret = method(param);

            }
            catch (Exception ex)
            {
                //写日志抛错误
                //Console.WriteLine(ex.ToString());
            }


            return ret;
        }


        /// <summary>
        /// 根据参数数量循环调用方法（隐式事务）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dao">扩展的类</param>
        /// <param name="method">要调用的方法名</param>
        /// <param name="param">方法的参数集合（根据参数数量循环执行方法）</param>
        /// <returns>是否全部执行成功</returns>
        public static bool CallMethod_TranScope<T>(this DaoTemplate dao,Func<T, int> method, params T[] param)
        {
            bool ret = false;
            //using语句可保证Dispose()
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    foreach (T t in param)
                    {
                        method(t);
                    }
                    tran.Complete();
                    ret = true;
                    //TransactionScope其实就是：最后调用Complete方法后才提交给数据库
                }
                catch (Exception ex)
                {
                    //写错误日志
                    ret = false;
                }

            }
            return ret;
        }
        
        /// <summary>
        /// 根据参数数量循环调用方法（显示事务）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dao">扩展的类</param>
        /// <param name="method">要调用的方法名</param>
        /// <param name="param">方法的参数集合（根据参数数量循环执行方法）</param>
        /// <returns>是否全部执行成功</returns>
        public static bool CallMethod_TranCommit<T>(this DaoTemplate dao, Func<T, int> method, params T[] param)
        {
            bool ret = false;
            Transaction oldAmbient = Transaction.Current;
            CommittableTransaction tran = new CommittableTransaction();
            Transaction.Current = tran;
            try
            {
                foreach (T t in param)
                {
                    method(t);
                }
                tran.Commit();
                ret = true;
            }
            catch
            {
                tran.Rollback();
                ret = false;
            }
            finally
            {
                Transaction.Current = oldAmbient;
            }

            return ret;
        }

    }
}
