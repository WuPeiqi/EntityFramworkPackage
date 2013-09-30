using System;
using System.Data.Objects;
using System.Configuration;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Transactions;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using System.Data.Common;
using EntityFramework.Extensions;
using System.Linq.Expressions;
namespace DataDAL
{
    public class DaoTemplate
    {
        //静态的原因是，只需要执行一次即可
        private static String strObjctectContextType = String.Empty;
        private static String strConnectionString = String.Empty;
        private static String strEntityContainerName = String.Empty; //容器的名字，也就是实体模型类型 WuTestEntities

        static DaoTemplate()
        {
            strObjctectContextType = ConfigurationManager.AppSettings["objectContextType"]; 
            strConnectionString = ConfigurationManager.ConnectionStrings["WuTestEntities"].ConnectionString;
            strEntityContainerName = ConfigurationManager.AppSettings["EntityContainerName"];
        }

        private ObjectContext _EntityModel = null;

        internal ObjectContext entityModel
        {
            get
            {
                if (_EntityModel == null)
                {
                    //_EntityModel = Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["ObjectContextType"]), ConfigurationManager.AppSettings["ObjectContextType"]) as ObjectContext;
                   
                    _EntityModel = Activator.CreateInstance(Type.GetType(strObjctectContextType),strConnectionString) as ObjectContext;
                    _EntityModel.MetadataWorkspace.LoadFromAssembly(Type.GetType(strObjctectContextType).Assembly);//没有此句的话，GetObjectByKey就无法获取关系对象
                }
                return _EntityModel;
            }
        }

        public void Commit()
        {
            //还有一个重载，参数是一个枚举。（用于保存到数据源后调用其他方法）
            //entityModel.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
            entityModel.SaveChanges();
        }


        #region 添加记录

        /// <summary>
        /// 利用泛型在表中插入一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int AddObject<T>(T entity) where T : EntityObject
        {
            entityModel.AddObject(entity.GetType().Name, entity);
            return entityModel.SaveChanges();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int AddObject_CreateObjectSet<T>(T entity) where T : EntityObject
        {
            entityModel.CreateObjectSet<T>().AddObject(entity);
            return entityModel.SaveChanges();
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int AddObject_Attach<T>(T entity) where T : EntityObject
        {
            //状态为Detached:对象存在，但没有被跟踪。 在创建实体之后、但将其添加到对象上下文之前，该实体处于此状态
            entityModel.AttachTo(entity.GetType().Name, entity);//将对象附加到对象上下文
            //状态为Unchanged: 此对象尚未经过修改附加到上下文中后，或自上次调用 (调用了SaveChange方法后所有的对象都改为Unchanged状态)
            
            entityModel.ObjectStateManager.ChangeObjectState(entity, EntityState.Added);
            //状态为Added：对象为新对象，并且已添加到对象上下文，但尚未调用 

            return entityModel.SaveChanges();
        }

        #endregion

        #region 删除记录

        public virtual int DeleteObject<T>(T obj) where T : EntityObject
        {
            entityModel.DeleteObject(obj);
            return entityModel.SaveChanges();
        }

        public virtual int DeleteObject_CreateObjectSet<T>(T obj) where T : EntityObject
        {
            entityModel.CreateObjectSet<T>().DeleteObject(obj);
            return entityModel.SaveChanges();
        }

        public virtual int DeleteObject_Attach<T>(T obj) where T : EntityObject
        {

            entityModel.AttachTo(obj.GetType().Name, obj);
            entityModel.ObjectStateManager.ChangeObjectState(obj, EntityState.Deleted);
            return entityModel.SaveChanges();
        }

        public virtual int DeleteObjects<T>(string filter, params object[] args) where T : EntityObject
        {
            return (entityModel.CreateObjectSet<T>() as IQueryable<T>).Where(filter, args).Delete();
        }

        public virtual int DeleteObjects<T>(Expression<Func<T,bool>> deleteExpression) where T : EntityObject
        {
            return (entityModel.CreateObjectSet<T>() as IQueryable<T>).Delete(deleteExpression);
        }
        #endregion

        #region 更新记录

        public virtual int UpdateObject<T>(T obj) where T : EntityObject
        {
            //如果想调用Attach方法的话，需要 entityModel.T_User.Attach();   也就是这里需要指明实体
            //Attach方法 = 在对象具有实体键时将对象或对象图附加到对象上下文。
            //将对象或对象图附加到特定实体集中的对象上下文。
            entityModel.AttachTo(obj.GetType().Name, obj);
            //1.获取对象上下文用于跟踪对象更改的对象状态管理器。返回 ObjectStateManager类型
            //2.将特定对象的 System.Data.Objects.ObjectStateEntry 状态更改为指定的 entityState。
            entityModel.ObjectStateManager.ChangeObjectState(obj, EntityState.Modified);
            return entityModel.SaveChanges();

            
        }

        public virtual int UpdateObjects<T>(string filter, Expression<Func<T, T>> updateExpression, params object[] args) where T : class
        {
            return (entityModel.CreateObjectSet<T>() as IQueryable<T>).Where(filter, args).Update(updateExpression);
        }

        public virtual int UpdateObjects<T>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression) where T : class
        {
            return (entityModel.CreateObjectSet<T>() as IQueryable<T>).Update<T>(filterExpression, updateExpression);
        }

        #endregion 

        #region 查找记录

        public T GetObject<T>(string filter, params object[] args) where T : EntityObject
        {
            return entityModel.CreateObjectSet<T>().Where(filter, args).FirstOrDefault<T>();
        }

        public T GetObject<T>(Object keyValue) where T : EntityObject
        {
            EntityKey ek = new EntityKey(strEntityContainerName+"."+typeof(T).Name,GetKeyName(typeof(T)), keyValue);
            return entityModel.GetObjectByKey(ek) as T;
        }

        public T GetObject<T>(Object keyValue,int no)where T : EntityObject
        {
            Object entity = null;
            EntityKey ek = new EntityKey(strEntityContainerName + "." + typeof(T).Name, GetKeyName(typeof(T)), keyValue);
            entityModel.TryGetObjectByKey(ek,out entity);
            return entity as T;
        }

        public ICollection<T> GetObjects<T>(string filter,params object[] args)where T:EntityObject
        {
            return entityModel.CreateObjectSet<T>().Where(filter, args).ToList();            
        }

        public virtual List<DbDataRecord> SelecteDataTable<T>(string filter, string orderbySelector, string selector, params ObjectParameter[] filterargs) where T : EntityObject
        {
            //AsNoTracking()
            var qure = entityModel.CreateObjectSet<T>().Where(filter, filterargs).OrderBy(orderbySelector).Select(selector);
            qure.MergeOption = System.Data.Objects.MergeOption.NoTracking;
            return qure.ToList();

            //return EM.CreateObjectSet<T>().Where(filter, filterargs).OrderBy(orderbySelector).Select(selector).ToList();            
        }
       
        public virtual List<DbDataRecord> SelecteDataTable_Page<T>(int pageSize, int pageIndex, ref int RecordCount, string filter, string orderbySelector, string selector, params ObjectParameter[] filterargs) where T : EntityObject
        {
            RecordCount = (entityModel.CreateObjectSet<T>().Where(filter, filterargs)).Count();
            return entityModel.CreateObjectSet<T>().Where(filter, filterargs).Select(selector).Skip(orderbySelector, "@skip", new ObjectParameter("skip", (pageIndex - 1) * pageSize)).Top("@limit", new ObjectParameter("limit", pageSize)).ToList();
        }
        #endregion

        /// <summary>
        /// 根据类型元数据获得一个实体的主键名称
        /// </summary>
        /// <param name="entityType">实体类的类型信息对象</param>
        /// <returns>主键名称</returns>
        public virtual string GetKeyName(Type entityType)
        {
            //获取该类型的所有属性，然后对属性遍历
            foreach (var pi in entityType.GetProperties())
            {
                //获取属性上的特性，通过特性来判断是否是主键。
                //查看实体对象T_User，他的属性上有特性 EdmScalarPropertyAttribute、DataMemberAttribute
                //对象T_User的Nid为主键，可以看出他的EdmScalarPropertyAttribute特性中，EntityKeyProperty=true
                
                
                var attr = pi.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), false).FirstOrDefault() as EdmScalarPropertyAttribute;
                var attr2 = pi.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault();
                //attr!=null说明该对象上存在特性EdmScalarPropertyAttribute
                //attr2!= null说明该对象上存在特性DataMemberAttribute
                //attr.EntityKeyProperty说明该对象上的特性EdmScalarPropertyAttribute的属性EntityKeyProperty=true
                if (attr != null && attr2!= null &&attr.EntityKeyProperty)
                    return pi.Name;
            }
            return string.Empty;
        }
    }

}
