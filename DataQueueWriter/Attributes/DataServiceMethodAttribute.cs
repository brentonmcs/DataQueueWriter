using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataQueueWriter.Attributes
{
    public class DataServiceMethodAttribute : Attribute
    {
        
        public bool RequireLock { get; set; }

        public DataServiceMethodAttribute(bool RequireLock = false)
        {
            this.RequireLock = RequireLock;
        }
    }
}
