using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCompliant.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Utilities.Tests
{
    [TestClass()]
    public class ThreadUtilitiesTests
    {
        [TestMethod()]
        [ExcludeFromCodeCoverage]
        public void CreateBackgroundThreadTest()
        {
            Thread t = ThreadUtilities.CreateBackgroundThread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    double _ = Math.Pow(i, i); 
                }
            });
            Assert.IsTrue(t.IsBackground);
            t.Start();
            t.Join(); 
           
        }
    }
}