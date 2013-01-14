using System;
using System.Collections.Generic;

namespace DataQueueWriter.Containers
{
    public class DataServiceContainer
    {
        public Type DataServiceClass { get; set; }

        public IEnumerable<DataServiceMethod> Methods { get; set; }

        public string Name { get; set; }

        public object ServiceInstance { get; set; }
    }

}
