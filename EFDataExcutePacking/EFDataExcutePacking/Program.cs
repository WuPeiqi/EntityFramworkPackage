using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBll.Common;
using System.Linq.Expressions;
using EFramework;
namespace EFDataExcutePacking
{
    class Program
    {
        static void Main(string[] args)
        {
            BllHelper bll = new BllHelper();
            //Expression<Func<T_User, bool>> expression = user => user.Nid > 1;
            //bll.DeleteUsers(expression);
            //bll.TestTransation(); //测试事务
            //Expres.ExcExpression();
            Log4Test.WriteLog();
            
            Console.ReadKey();
        }
    }
}
