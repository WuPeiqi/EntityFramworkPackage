using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFramework;
using DataBll;
using DataBll.Common;
namespace EFDataExcutePacking
{
    public class ExcuteData
    {
        public static int AddUser()
        {
            BllHelper bllHelper = new BllHelper();
            T_User user = new T_User {  Name="Cally", Address="USA" };
            object obj=new {Name="Cally22", Address="USA"};
            bllHelper.InsertUser(obj);
            return bllHelper.InsertUser(user);
        }

        public static T_User GetUser()
        {
            //BllHelper bllHelper = new BllHelper();
            //T_User t=bllHelper.GetUser("Nid = @0", new object[] { 5 });
            //return t;
            BllHelper bllHelper = new BllHelper();
            T_User t = bllHelper.GetUser(5);
            return t;
        }
    }
}
