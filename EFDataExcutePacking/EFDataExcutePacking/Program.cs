using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBll.Common;
namespace EFDataExcutePacking
{
    class Program
    {
        static void Main(string[] args)
        {
            BllHelper bll = new BllHelper();
            int i = bll.UpdateUsers();
           Console.ReadKey();
        }
    }
}
