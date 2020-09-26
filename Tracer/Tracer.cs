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
            
        }

        public void StartTrace()
        {
            TraceResult traceResult;
            Stack<TraceResult> stackTraceResults = new Stack<TraceResult>();
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            MethodBase method = frame.GetMethod();
            Type type = method.DeclaringType;
            traceResult.methodName = method.Name;
            traceResult.className = type.Name;
            traceResult.time = DateTime.Now.Second;
            traceResult.traceResultList = new List<TraceResult>();
            threadsResults.GetOrAdd(Thread.CurrentThread.ManagedThreadId, _ => new Stack<TraceResult>());
            threadsResults.TryGetValue(Thread.CurrentThread.ManagedThreadId, out stackTraceResults);
            stackTraceResults.Push(traceResult);
            threadsResults.TryAdd(Thread.CurrentThread.ManagedThreadId, stackTraceResults);
        }
        public void StopTrace()
        {
            int time = DateTime.Now.Second;
            Stack<TraceResult> stackTraceResult = new Stack<TraceResult>();
            threadsResults.TryGetValue(Thread.CurrentThread.ManagedThreadId, out stackTraceResult);
            TraceResult traceResult = stackTraceResult.Pop();
            traceResult.time = time - traceResult.time;
            stackTraceResult.Peek().traceResultList.Add(traceResult); 

        }
    }
}
