using DataQueueWriter.Attributes;
using DataQueueWriter.Containers;
using DataQueueWriter.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace DataQueueWriter
{
    public class DataQueueWriter 
    {
        private IManagedQueue<Expression<Action>> Q;

        private delegate void HandleQueue();

        private List<DataServiceContainer> DataServices;

        private bool locked;

        public DataQueueWriter()
        {
            Q = new ManagedQueue<Expression<Action>>();
            LocateDataService();
            
            locked = false;

            Q.Changed += new EventHandler(delegate(object s, EventArgs ex)
            {
                if (Q.Count > 0)
                {
                    var handle = new HandleQueue(HQueue);
                    handle.BeginInvoke(null, null);
                }
            });
        }

        public void Queue(Expression<Action> item)
        {
            Q.Enqueue(item);
        }

        private void HQueue()
        {
            var item = Q.Dequeue();
            IFormatterConverter converter = new FormatterConverter();
           var method = FindMethodForDataService(item);
            while (locked){}

            if (method.RequiresLock)
                locked = true;
            item.Compile().Invoke(); 
            locked = false;     
        }

        private DataServiceMethod FindMethodForDataService(Expression<Action> item)
        {
            DataServiceMethod method = null;

            
            var name=  ((MethodCallExpression)item.Body).Method.Name;
            
            foreach (var d in DataServices)
            {
                var m = d.Methods.FirstOrDefault(x => x.Name == name);
                if (m != null)
                {
                    method = m;
                    break;
                }
            }

            if (method == null)
                throw new DataServiceMethodNotFound(name);
            return method;
        }

  

        private void LocateDataService()
        {
            DataServices = new List<DataServiceContainer>();
            FindDataServiceClasses().ForEach(AddDataServiceContainer);
        }

        private void AddDataServiceContainer(DataServiceClassContainer ds)
        {
            DataServices.Add(new DataServiceContainer
                {
                    Name = ds.name,
                    DataServiceClass = ds.type,
                    Methods = GetMethods(ds.type),
                    ServiceInstance = Activator.CreateInstance(ds.type)
                });
        }

        private static object GetAttributeValues(Object[] attributes)
        {
            return attributes.FirstOrDefault();
        }

        private static List<DataServiceClassContainer> FindDataServiceClasses()
        {
            var DataService =

            (from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
             from t in a.GetTypes()
             where t.IsClass == true
             let attributes = t.GetCustomAttributes(typeof(DataServiceClassAttribute), true)
             where attributes != null && attributes.Length > 0
             select new DataServiceClassContainer { type = t, attributes = attributes, name = t.Name }).ToList();
            return DataService;
        }

        private IEnumerable<DataServiceMethod> GetMethods(Type type)
        {
            return
                (from m in type.GetMethods()
                 let attributes = m.GetCustomAttributes(typeof(DataServiceMethodAttribute), true)
                 where attributes != null && attributes.Length > 0
                 let attributeValues = ((DataServiceMethodAttribute)GetAttributeValues(attributes))
                 select new DataServiceMethod { Name = m.Name, Info = m, RequiresLock= attributeValues.RequireLock,  });
        }


    }
}