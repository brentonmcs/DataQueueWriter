﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataQueueWriter;
using DataQueueWriter.Containers;
using DataQueueWriter.Attributes;

namespace ConsoleApplication1
{
    public enum LogType
    {
        Info,
        Fail
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            DataQueueWriter.DataQueueWriter dqw = new DataQueueWriter.DataQueueWriter();

            dqw.Queue(() => DataService.TestLock());

            dqw.Queue(()=> DataService.SaveLog("Save SuccessFull 1", LogType.Info));
                        
            dqw.Queue(()=> DataService.SaveLog("Save SuccessFull 2", LogType.Info));
            
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }

    [DataServiceClass("DataService")]
    public class DataService
    {
        [DataServiceMethod(true)]
        public static void TestLock()
        {
            Console.WriteLine("Enter Lock Function");
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Finished Lock Wait");
        }

        [DataServiceMethod()]
        public static void SaveLog(string LogEntry, LogType logType)
        {
            Console.WriteLine("Saving Log {0} - {1}", LogEntry, logType);
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Finished Wait");
            return;
        }
    }
}
