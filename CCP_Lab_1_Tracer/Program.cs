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
            
            Tracer.Tracer tracer = new Tracer.Tracer();
            Foo foo = new Foo(tracer);
            Bar bar = new Bar(tracer);
            //Thread myThread = new Thread(new ThreadStart(bar.InnerMethodForSecondThread));
            //myThread.Start(); // запускаем поток
            foo.MyMethod();
            bar.InnerMethod();
            bar.InnerMethod1();
            bar.InnerMethod2();

        }
        
    }
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }
        public void MyMethod2()
        {
            _tracer.StartTrace();
            Thread.Sleep(111);
            _bar.InnerMethod();

            _tracer.StopTrace();
        }
        public void MyMethod()
        {
            _tracer.StartTrace();
           
            _bar.InnerMethod();
            
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
        public void InnerMethod1()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
        public void InnerMethod2()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
        public void InnerMethodForSecondThread()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
    }
}
