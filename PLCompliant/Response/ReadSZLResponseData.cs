using PLCompliant.Interface;
using PLCompliant.STEP_7;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;
using System.Text;

namespace PLCompliant.Response
{
    [StructLayout(LayoutKind.Explicit, Size = 20, CharSet = CharSet.Ansi)]
    public struct OrderNumBuffer
    {
        // POV: You don't have inline arrays (too recent in 2023 ); 
        public const int SIZE = 20;
        [FieldOffset(0)] byte item1;
        [FieldOffset(1)] byte item2;
        [FieldOffset(2)] byte item3;
        [FieldOffset(3)] byte item4;
        [FieldOffset(4)] byte item5;
        [FieldOffset(5)] byte item6;
        [FieldOffset(6)] byte item7;
        [FieldOffset(7)] byte item8;
        [FieldOffset(8)] byte item9;
        [FieldOffset(9)] byte item10;
        [FieldOffset(10)] byte item11;
        [FieldOffset(11)] byte item12;
        [FieldOffset(12)] byte item13;
        [FieldOffset(13)] byte item14;
        [FieldOffset(14)] byte item15;
        [FieldOffset(15)] byte item16;
        [FieldOffset(16)] byte item17;
        [FieldOffset(17)] byte item18;
        [FieldOffset(18)] byte item19;
        [FieldOffset(19)] byte item20;


        public OrderNumBuffer(string input)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(input);
            if(input.Length != SIZE)
            {
                throw new ArgumentException("Length of string must be equal to the buffer size (20)");
            }
            for(int i=0; i<SIZE; i++)
            {
                this[i] = stringBytes[i];
            }
        }
        public override readonly string ToString()
        {
            StringBuilder sb = new StringBuilder(SIZE);

           
            sb.Append((char)item1);
            sb.Append((char)item2);
            sb.Append((char)item3);
            sb.Append((char)item4);
            sb.Append((char)item5);
            sb.Append((char)item6);
            sb.Append((char)item7);
            sb.Append((char)item8);
            sb.Append((char)item9);
            sb.Append((char)item10);
            sb.Append((char)item11);
            sb.Append((char)item12);
            sb.Append((char)item13);
            sb.Append((char)item14);
            sb.Append((char)item15);
            sb.Append((char)item16);
            sb.Append((char)item17);
            sb.Append((char)item18);
            sb.Append((char)item19);
            sb.Append((char)item20);
            return sb.ToString();
        }
        public byte this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return item1;
                    case 1: return item2;
                    case 2: return item3;
                    case 3: return item4;
                    case 4: return item5;
                    case 5: return item6;
                    case 6: return item7;
                    case 7: return item8;
                    case 8: return item9;
                    case 9: return item10;
                    case 10: return item11;
                    case 11: return item12;
                    case 12: return item13;
                    case 13: return item14;
                    case 14: return item15;
                    case 15: return item16;
                    case 16: return item17;
                    case 17: return item18;
                    case 18: return item19;
                    case 19: return item20;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: item1 = value; break;
                    case 1: item2 = value; break;
                    case 2: item3 = value; break;
                    case 3: item4 = value; break;
                    case 4: item5 = value; break;
                    case 5: item6 = value; break;
                    case 6: item7 = value; break;
                    case 7: item8 = value; break;
                    case 8: item9 = value; break;
                    case 9: item10 = value; break;
                    case 10: item11 = value; break;
                    case 11: item12 = value; break;
                    case 12: item13 = value; break;
                    case 13: item14 = value; break;
                    case 14: item15 = value; break;
                    case 15: item16 = value; break;
                    case 16: item17 = value; break;
                    case 17: item18 = value; break;
                    case 18: item19 = value; break;
                    case 19: item20 = value; break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }
    [StructLayout(LayoutKind.Explicit, Size = 28, CharSet = CharSet.Ansi)]
    public struct ReadSZLDataItem : IEndianConvertable
    {
        [FieldOffset(0)] private UInt16 _index;
        [FieldOffset(2)] private OrderNumBuffer _orderNum;
        [FieldOffset(22)] private UInt16 _moduleTypeId;
        [FieldOffset(24)] private UInt16 _version;
        [FieldOffset(26)] private UInt16 _pgDescriptionFile;


        public ReadSZLDataItem(UInt16 index, ref OrderNumBuffer orderNum, UInt16 moduleTypeId, UInt16 version, UInt16 pgDescriptionFile)
        {
            _index = index;
            _orderNum = orderNum;
            _moduleTypeId = moduleTypeId;
            _version = version;
            _pgDescriptionFile = pgDescriptionFile;
        }

        public UInt16 PgDescriptionFile
        {
            get { return _pgDescriptionFile; }
            set { _pgDescriptionFile = value; }
        }


        public UInt16 Version
        {
            get { return _version; }
            set { _version = value; }
        }


        public UInt16 ModuleTypeId
        {
            get { return _moduleTypeId; }
            set { _moduleTypeId = value; }
        }


        public OrderNumBuffer OrderNum
        {
            get { return _orderNum; }
            set { _orderNum = value; }
        }


        public UInt16 Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public void FromHostToNetwork()
        {
            _index = EndianConverter.FromHostToNetwork(_index);
            _moduleTypeId = EndianConverter.FromHostToNetwork(_moduleTypeId);
            _version = EndianConverter.FromHostToNetwork(_version);
            _pgDescriptionFile = EndianConverter.FromHostToNetwork(_pgDescriptionFile);
        }

        public void FromNetworkToHost()
        {
            _index = EndianConverter.FromNetworkToHost(_index);
            _moduleTypeId = EndianConverter.FromNetworkToHost(_moduleTypeId);
            _version = EndianConverter.FromNetworkToHost(_version);
            _pgDescriptionFile = EndianConverter.FromNetworkToHost(_pgDescriptionFile);
        }
    }
    public class ReadSZLResponseData : ResponseData
    {
        private ReadSZLResponseHeader _header = new();
        private List<ReadSZLDataItem> _objects = new List<ReadSZLDataItem>();


        public ReadSZLResponseData(ReadSZLResponseHeader header)
        {
            _header = header;
        }
        public ReadSZLResponseData()
        {

        }

        public List<ReadSZLDataItem> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }




        public ReadSZLResponseHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public override string ToCSV()
        {
            StringBuilder sb = new StringBuilder(40);
            foreach (var item in Objects)
            {
                if (item.Index == 0x0007)
                {
                    string orderNumber = item.OrderNum.ToString();
                    byte[] versionBytes = BitConverter.GetBytes(item.Version);
                    byte[] releaseBytes = BitConverter.GetBytes(item.PgDescriptionFile);
                    char versionChar = (char)versionBytes[1];
                    string majorReleaseNum = versionBytes[0].ToString();
                    string mediumReleaseNum = releaseBytes[1].ToString();
                    string minorReleaseNum = releaseBytes[0].ToString();

                    sb.Append(IPAddr);
                    sb.Append(GlobalVars.CSV_SEPARATOR);
                    sb.Append(orderNumber);
                    sb.Append(GlobalVars.CSV_SEPARATOR);
                    sb.Append(versionChar);
                    sb.Append(majorReleaseNum);
                    sb.Append('.');
                    sb.Append(mediumReleaseNum);
                    sb.Append(".");
                    sb.Append(minorReleaseNum);
                    return sb.ToString();
                }
            }
            return $"{GlobalVars.CSV_SEPARATOR}{GlobalVars.CSV_SEPARATOR}";

        }
    }
}
