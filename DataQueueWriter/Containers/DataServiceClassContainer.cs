using System;

namespace DataQueueWriter.Containers
{
 
    internal class DataServiceClassContainer
    {
        public Type type
        {
            get;
            set;
        }

        public Object[] attributes { get; set; }
    }
}
