using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataQueueWriter.Containers
{
    public class DataServiceMethod
    {
        public MethodInfo Info { get; set; }
        public string Name { get; set; }
        public bool RequiresLock { get; set; }
    }
}
