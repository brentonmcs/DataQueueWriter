using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataQueueWriter
{
    [Serializable]
    class DataServiceMethodNotFound : Exception
    {
        private string _MethodName { get; set; }
        public DataServiceMethodNotFound(string MethodName)
        {
            _MethodName = MethodName;
            
        }
    }
}
