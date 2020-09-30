using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracer;
using System.Threading;

namespace TracerTest
{
    [TestClass]
    public class TestTracer
    {
        [TestMethod]
        public void StartTraceTest() 
        { 

        }

        [TestMethod]
        public void StopTraceTest() 
        { 

        }

        [TestMethod]
        public void CommonTest1() 
        {
            Tracer.Tracer tracer = new Tracer.Tracer();
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
            string[] actual = tracer.GetTraceResult();
            string[] expected = { "[\r\n  {\r\n    \"time\": 0,\r\n    \"methodName\": \"1\",\r\n    \"className\": \"Thread\",\r\n    \"traceResultList\": [\r\n      {\r\n        \"time\": 116,\r\n        \"methodName\": \"Main\",\r\n        \"className\": \"Program\",\r\n        \"traceResultList\": []\r\n      }\r\n    ]\r\n  }\r\n]",
                                  "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfTraceResult xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <TraceResult>\r\n    <stopwatch />\r\n    <time>0</time>\r\n    <methodName>1</methodName>\r\n    <className>Thread</className>\r\n    <traceResultList>\r\n      <TraceResult>\r\n        <stopwatch />\r\n        <time>106</time>\r\n        <methodName>Main</methodName>\r\n        <className>Program</className>\r\n        <traceResultList />\r\n      </TraceResult>\r\n    </traceResultList>\r\n  </TraceResult>\r\n</ArrayOfTraceResult>" };
            Assert.AreEqual(expected, actual);

        }
    }
}
