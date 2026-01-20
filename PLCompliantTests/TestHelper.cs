using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using PLCompliant;
using PLCompliant.Enums;
using PLCompliant.Interface;
using PLCompliant.Modbus;
using PLCompliant.Response;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace PLCompliantTests
{

    [StructLayout(LayoutKind.Explicit, Size = 6, CharSet = CharSet.Ansi)]
    struct TestStruct : IEndianConvertable
    {
        [FieldOffset(0)] public byte b1;
        [FieldOffset(1)] public byte b2;
        [FieldOffset(2)] public UInt16 b3;
        [FieldOffset(4)] public UInt16 b4;

        public void FromHostToNetwork()
        {
            b3 = EndianConverter.FromHostToNetwork(b3);
            b4 = EndianConverter.FromHostToNetwork(b4);
        }

        public void FromNetworkToHost()
        {
            b3 = EndianConverter.FromNetworkToHost(b3);
            b4 = EndianConverter.FromNetworkToHost(b4);
        }
    }

    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {

        public static string getDeviceInfoObject1 = "Schneider Electric";
        public static string getDeviceInfoObject2 = "BMX NOE 0100";
        public static string getDeviceInfoObject3 = "V2.30";

        //This method instantiates a ModBusMessage akin to an response from PLC for ReadDeviceIdentification. 
        public static ModBusMessage CreateExampleReadDeviceInformationResponse()
        {
            ModBusMessage msg = new(new(0, 0, 255), new());
            msg.AddData((byte)ModBusCommandType.read_device_information); // function code
            msg.AddData(0xe);
            msg.AddData(0x2);
            msg.AddData(0x81);
            msg.AddData(0x00);
            msg.AddData(0x00);
            msg.AddData(0x03);

            msg.AddData(0x0);
            msg.AddData(0x12);
            msg.AddData(Encoding.UTF8.GetBytes(getDeviceInfoObject1));

            msg.AddData(0x1);
            msg.AddData(0xc);
            msg.AddData(Encoding.UTF8.GetBytes(getDeviceInfoObject2));

            msg.AddData(0x2);
            msg.AddData(0x5);
            msg.AddData(Encoding.UTF8.GetBytes(getDeviceInfoObject3));

            return msg;
        }


        //This method instantiates a ReadSZLResponseData akin to an response from PLC. 
        public static ReadSZLResponseData CreateExampleReadSZLResponse()
        {
            OrderNumBuffer orderNum = new OrderNumBuffer("6ES7 211-1BE40-0XB0 ");
            ReadSZLResponseData szlResponse = new ReadSZLResponseData(new(17, 1, 28, 3));
            szlResponse.IPAddr = IPAddress.Parse("192.168.123.99");
            szlResponse.Objects.Add(new ReadSZLDataItem((ushort)SZLItemIndex.Module, ref orderNum, 0, 14, 8224));
            szlResponse.Objects.Add(new ReadSZLDataItem((ushort)SZLItemIndex.BasicHardware, ref orderNum, 0, 14, 8224));
            szlResponse.Objects.Add(new ReadSZLDataItem((ushort)SZLItemIndex.BasicFirmware, ref orderNum, 0, 22020, 1281));


            return szlResponse;
        }
















        public static PLCompliantUI MockUI()
        {

            return new PLCompliantUI();
        }

        public static void DoubleClickItem(this Actions action, WindowsElement elem)
        {
            action.DoubleClick(elem).Perform();
        }


    }
}
