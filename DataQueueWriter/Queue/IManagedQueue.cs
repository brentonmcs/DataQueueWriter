using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataQueueWriter.Queue
{
    interface IManagedQueue<T>
    {
        event EventHandler Changed;
        void Enqueue(T item);
        T Dequeue();
        int Count { get;  }
    }
}
