using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLCompliant.Modbus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Modbus.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass()]
    public class ModBusDataTests
    {
        [TestMethod()]
        public void ModBusDataEqualsNullAndOtherType()
        {
            ModBusData data = new ModBusData();

            Assert.IsFalse(data.Equals(null));
            Assert.IsFalse(data.Equals(new { x = 5}));

        }

        [TestMethod]
        [DataRow((byte)0x2B, ushort.MaxValue)]
        [DataRow((byte)byte.MaxValue, ushort.MaxValue)]
        [DataRow((byte)byte.MinValue, ushort.MinValue)]
        [DataRow((byte)0x90, (ushort)25000)]
        
        public void ModBusDataAreEqual(byte funcCode, ushort param_1)
        {
            ModBusMessage firstmsg = new(new(), new()); 
            ModBusMessage secondmsg = new(new(), new());
            firstmsg.Data._functionCode = funcCode;
            firstmsg.AddData(param_1);
            secondmsg.Data._functionCode = funcCode;
            secondmsg.AddData(param_1);
            Assert.AreEqual(firstmsg.Data, secondmsg.Data);
            



        }

        


    }
}