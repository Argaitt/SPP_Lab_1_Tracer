using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tracer
{
    public struct ThreadResult
    {
        public long time;

        public int id;

        public List<TraceResult> traceResultList;

    }
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
        

        Stack<TraceResult> stackTraceResult;

        List<ThreadResult> threadResultsArr;

        public Tracer() 
        {
            stackTraceResult = new Stack<TraceResult>();
            threadResultsArr = new List<ThreadResult>();
        }

        public void StartTrace()
        {
            TraceResult traceResult;
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            MethodBase method = frame.GetMethod();
            Type type = method.DeclaringType;
            traceResult.methodName = method.Name;
            traceResult.className = type.Name;
            traceResult.time = DateTime.Now.Second;
            traceResult.traceResultList = new List<TraceResult>();
            stackTraceResult.Push(traceResult);
        }
        public void StopTrace()
        {
            int time = DateTime.Now.Second;
            TraceResult traceResult = stackTraceResult.Pop();
            traceResult.time = time - traceResult.time;
            if (stackTraceResult.Count != 0)
            {
                stackTraceResult.Peek().traceResultList.Add(traceResult);
            }
            else 
            {
                ThreadResult threadResult;
                threadResult.id = threadResultsArr.Count + 1;
                threadResult.time = traceResult.time;
                threadResult.traceResultList = new List<TraceResult>();
                threadResult.traceResultList.Add(traceResult);
                threadResultsArr.Add(threadResult);
            }
            
        }
    }
}
