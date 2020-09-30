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
        public void GetTraceResultTest() 
        {
            Tracer.Tracer tracer = new Tracer.Tracer();
            tracer.StartTrace();
            Thread.Sleep(100);
            tracer.StopTrace();
            string[] actual = tracer.GetTraceResult();
            string[] expected = { "", "" };
            //Assert.AreEqual()

        }
    }
}
