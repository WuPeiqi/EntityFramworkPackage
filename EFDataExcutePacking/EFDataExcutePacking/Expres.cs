using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace EFDataExcutePacking
{
    public class Expres
    {
        public static void ExcExpression()
        {
            ModelTest model1=new ModelTest{ID=1,Name="bbb"};
            ModelTest model2 = new ModelTest { ID = 2 };
            Expression<Func<ModelTest, Object>> predicate = m => m.Name;
            Object s1 = predicate.Compile()(model1) ?? "N/A";
            Object s2 = predicate.Compile()(model2) ?? "N/A";
        }
    }
    public class ModelTest
    {
        public int ID { set; get; }

        public string Name { set; get; }
    }
}
