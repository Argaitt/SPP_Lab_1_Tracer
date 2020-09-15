using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

namespace Tracer
{
    public struct TraceResult
    {
        public long time;

        public string methodName;

        public string className;
    }
    interface ITracer 
    { 

        void StartTrace();

        void StopTrace();

        ConcurrentDictionary<int, TraceResult> GetTraceResult();

    }

    public class Tracer:ITracer
    {
        Stopwatch stopwatch;
        public Tracer() 
        {
            stopwatch = new Stopwatch();
            traceResultsList = new ConcurrentDictionary<int, TraceResult>();
            key = 0;
        }

        ConcurrentDictionary<int, TraceResult> traceResultsList;
        int key;
        public void StartTrace()
        {
            stopwatch.Start();
            
        }
        public void StopTrace()
        {
            stopwatch.Stop();
            TraceResult traceResult;
            traceResult.time = stopwatch.ElapsedMilliseconds;
            // получаем имя метода в контекте которого вызван StopTrace()
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrames()[1];
            MethodBase method = frame.GetMethod();
            Type type = method.DeclaringType;
            traceResult.methodName = method.Name;
            traceResult.className = type.Name;
            traceResultsList.TryAdd(key, traceResult);
            key++;
            
        }
        
        public ConcurrentDictionary<int, TraceResult> GetTraceResult()
        {
            return traceResultsList;
        }
    }
}
