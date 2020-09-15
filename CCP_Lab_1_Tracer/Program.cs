using System;
using System.Threading;
using Tracer;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace CCP_Lab_1_Tracer
{
    
    class Program
    {
        static void Main(string[] args)
        {
            
            Tracer.Tracer tracer1 = new Tracer.Tracer();
            tracer1.StartTrace();
            Thread.Sleep(50);
            tracer1.StopTrace();
            tracer1.StartTrace();
            Thread.Sleep(100);
            tracer1.StopTrace();
            ConcurrentDictionary<int, TraceResult> arr = new ConcurrentDictionary<int, TraceResult>();
            arr = tracer1.GetTraceResult();
        }
    }
}
