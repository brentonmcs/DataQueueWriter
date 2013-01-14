using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataQueueWriter.Containers
{
    public class DataQueueObject
    {
        public string DataServiceName { get; set; }
        public string MethodName { get; set; }
        public object[] Paramenters { get; set; }
    }

}
