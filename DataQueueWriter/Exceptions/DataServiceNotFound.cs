using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataQueueWriter
{
    [Serializable]
    class DataServiceNotFound : Exception
    {
        public DataServiceNotFound(string DataServiceName)
        {

        }
    }
}
