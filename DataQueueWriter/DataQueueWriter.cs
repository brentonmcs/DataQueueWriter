using DataQueueWriter.Attributes;
using DataQueueWriter.Containers;
using DataQueueWriter.Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataQueueWriter
{
    public class DataQueueWriter
    {
        private IManagedQueue<DataQueueObject> Q;

        private delegate void HandleQueue();

        private List<DataServiceContainer> DataServices;

        private bool locked;

        public DataQueueWriter()
        {
            Q = new ManagedQueue<DataQueueObject>();
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

        public void Queue(DataQueueObject item)
        {
            Q.Enqueue(item);
        }

        private void HQueue()
        {
            var item = Q.Dequeue();
            var DataService = FindDataService(item.DataServiceName);
            var method = FindMethodForDataService(item.MethodName, DataService);

            while (locked){}

            if (method.RequiresLock)
                locked = true;
            method.Info.Invoke(DataService.ServiceInstance, item.Paramenters);
            locked = false;     
        }

        private static DataServiceMethod FindMethodForDataService(string MethodName, DataServiceContainer DataService)
        {
            var method = DataService.Methods.Where(x => x.Name == MethodName).FirstOrDefault();

            if (method == null)
                throw new DataServiceMethodNotFound(MethodName);
            return method;
        }

        private DataServiceContainer FindDataService(string DataServiceName)
        {
            var DataService = DataServices.Where(x => x.Name == DataServiceName).FirstOrDefault();

            if (DataService == null)
                throw new DataServiceNotFound(DataServiceName);

            return DataService;
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
                    Name = ((DataServiceClassAttribute)GetAttributeValues(ds.attributes)).Name,
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
             select new DataServiceClassContainer { type = t, attributes = attributes }).ToList();
            return DataService;
        }

        private IEnumerable<DataServiceMethod> GetMethods(Type type)
        {
            return
                (from m in type.GetMethods()
                 let attributes = m.GetCustomAttributes(typeof(DataServiceMethodAttribute), true)
                 where attributes != null && attributes.Length > 0
                 let attributeValues = ((DataServiceMethodAttribute)GetAttributeValues(attributes))
                 select new DataServiceMethod { Name = attributeValues.Name, Info = m, RequiresLock= attributeValues.RequireLock });
        }
    }
}