using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataQueueWriter.Attributes
{
    public class DataServiceMethodAttribute : Attribute
    {
        public string Name { get; set; }

        public bool RequireLock { get; set; }

        public DataServiceMethodAttribute(string Name, bool RequireLock = false)
        {
            this.Name = Name;
            this.RequireLock = RequireLock;
        }
    }
}
