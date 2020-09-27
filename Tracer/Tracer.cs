using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
            //Получаем имя метода и класс к кторому он принадлежит
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            MethodBase method = frame.GetMethod();
            Type type = method.DeclaringType;
            traceResult.time = 0;
            traceResult.methodName = method.Name;
            traceResult.className = type.Name;
            //Запускаем stopwatch и помещаем его в структуру traceResult
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            traceResult.stopwatch = stopwatch;
            traceResult.traceResultList = new List<TraceResult>();
            //Создаем новый стек для потока, если он еще не был создан
            threadsResults.GetOrAdd(Thread.CurrentThread.ManagedThreadId, _ => new Stack<TraceResult>());
            threadsResults.TryGetValue(Thread.CurrentThread.ManagedThreadId, out stackTraceResult);
            //Создаем корневую структуру, которая будет считать общее время выполнения.
            if (stackTraceResult.Count == 0)
            {
                TraceResult root;
                root.methodName = Thread.CurrentThread.ManagedThreadId.ToString();
                root.className = "Thread";
                root.time = 0;
                Stopwatch stopwatch1 = new Stopwatch();
                stopwatch1.Start();
                root.stopwatch = stopwatch1;
                //инициализация предложенная вижлой, ей виднее, в дебаггере криминала не видно
                root.traceResultList = new List<TraceResult>
                {
                    traceResult
                };
                stackTraceResult.Push(root);
            }
            else
            {
                stackTraceResult.Push(traceResult);
            }
            
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
            stackTraceResult.Peek().traceResultList.Add(traceResult);
            threadsResults.TryAdd(Thread.CurrentThread.ManagedThreadId, stackTraceResult);
            //Console.WriteLine(traceResult.methodName);
            //Console.WriteLine(traceResult.time + '\n');
             
        }
    }
}
