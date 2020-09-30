using System;
using System.Threading;
using Tracer;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.IO;

namespace CCP_Lab_1_Tracer
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
            Tracer.Tracer tracer = new Tracer.Tracer();
            //Foo foo = new Foo(tracer);
            //Bar bar = new Bar(tracer);
            //WriterResult wr = new WriterResult();
            //Thread myThread = new Thread(new ThreadStart(bar.InnerMethodForSecondThread));
            //myThread.Start();
            //foo.MyMethod();
            //bar.InnerMethod();
            //bar.InnerMethod1();
            //bar.InnerMethod2();
            
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
            string[] actual = tracer.GetTraceResult();
            string[] expected = { "[\r\n  {\r\n    \"time\": 0,\r\n    \"methodName\": \"1\",\r\n    \"className\": \"Thread\",\r\n    \"traceResultList\": [\r\n      {\r\n        \"time\": 116,\r\n        \"methodName\": \"Main\",\r\n        \"className\": \"Program\",\r\n        \"traceResultList\": []\r\n      }\r\n    ]\r\n  }\r\n]",
                                  "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfTraceResult xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <TraceResult>\r\n    <stopwatch />\r\n    <time>0</time>\r\n    <methodName>1</methodName>\r\n    <className>Thread</className>\r\n    <traceResultList>\r\n      <TraceResult>\r\n        <stopwatch />\r\n        <time>106</time>\r\n        <methodName>Main</methodName>\r\n        <className>Program</className>\r\n        <traceResultList />\r\n      </TraceResult>\r\n    </traceResultList>\r\n  </TraceResult>\r\n</ArrayOfTraceResult>" };
            //wr.JsonXmlToConsole(tracer.GetTraceResult());
            //wr.JsonXmlToFile(tracer.GetTraceResult());
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
    class WriterResult 
    {
        public void JsonXmlToConsole(string[] jsonAndXml)
        {
            foreach (var item in jsonAndXml)
            {
                Console.WriteLine(item);
            }
            
        }

        public void JsonXmlToFile(string[] jsonAndXml)
        {
            File.WriteAllText("result.json", jsonAndXml[0]);
            File.WriteAllText("result.xml", jsonAndXml[1]);
        }
    }
}
