using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataQueueWriter.Attributes
{
    public class DataServiceClassAttribute : Attribute
    {
        public string Name { get; set; }

        public DataServiceClassAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
