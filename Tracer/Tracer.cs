using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Tracer
{
    
    public struct TraceResult
    {
        public Stopwatch stopwatch;
        public long time;

        public string methodName;

        public string className;

        public List<TraceResult> traceResultList;
        
    }
    public interface ITracer 
    { 

        void StartTrace();

        void StopTrace();

    }

    public class Tracer:ITracer
    {
        
        ConcurrentDictionary<int, Stack<TraceResult>> threadsResults;

        public Tracer() 
        {
            threadsResults = new ConcurrentDictionary<int, Stack<TraceResult>>();
        }

        public void StartTrace()
        {
            TraceResult traceResult;
            Stack<TraceResult> stackTraceResult = new Stack<TraceResult>();
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            MethodBase method = frame.GetMethod();
            Type type = method.DeclaringType;
            traceResult.time = 0;
            traceResult.methodName = method.Name;
            traceResult.className = type.Name;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            traceResult.stopwatch = stopwatch;
            traceResult.traceResultList = new List<TraceResult>();
            threadsResults.GetOrAdd(Thread.CurrentThread.ManagedThreadId, _ => new Stack<TraceResult>());
            threadsResults.TryGetValue(Thread.CurrentThread.ManagedThreadId, out stackTraceResult);
            stackTraceResult.Push(traceResult);
            threadsResults.TryAdd(Thread.CurrentThread.ManagedThreadId, stackTraceResult);
        }
        public void StopTrace()
        {
            Stack<TraceResult> stackTraceResult = new Stack<TraceResult>();
            threadsResults.TryGetValue(Thread.CurrentThread.ManagedThreadId, out stackTraceResult);
            TraceResult traceResult = stackTraceResult.Pop();
            Stopwatch stopwatch = traceResult.stopwatch;
            stopwatch.Stop();
            traceResult.time = stopwatch.ElapsedMilliseconds;
            if (stackTraceResult.Count != 0)
            {
                stackTraceResult.Peek().traceResultList.Add(traceResult);
                threadsResults.TryAdd(Thread.CurrentThread.ManagedThreadId, stackTraceResult);
            }
            else
            {
                stackTraceResult.Push(traceResult);
                threadsResults.TryAdd(Thread.CurrentThread.ManagedThreadId, stackTraceResult);
            }
            
        }
    }
}
