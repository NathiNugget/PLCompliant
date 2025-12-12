using PLCompliant.Utilities;
using System.Text;

namespace PLCompliant.Response
{
    public struct OrderNumBuffer
    {
        // POV: You don't have inline arrays (too recent in 2023 ); 
        public const int SIZE = 20;
        char item1;
        char item2;
        char item3;
        char item4;
        char item5;
        char item6;
        char item7;
        char item8;
        char item9;
        char item10;
        char item11;
        char item12;
        char item13;
        char item14;
        char item15;
        char item16;
        char item17;
        char item18;
        char item19;
        char item20;
        public override readonly string ToString()
        {
            StringBuilder sb = new StringBuilder(SIZE);

            sb.Append(item1);
            sb.Append(item2);
            sb.Append(item3);
            sb.Append(item4);
            sb.Append(item5);
            sb.Append(item6);
            sb.Append(item7);
            sb.Append(item8);
            sb.Append(item9);
            sb.Append(item10);
            sb.Append(item11);
            sb.Append(item12);
            sb.Append(item13);
            sb.Append(item14);
            sb.Append(item15);
            sb.Append(item16);
            sb.Append(item17);
            sb.Append(item18);
            sb.Append(item19);
            sb.Append(item20);
            return sb.ToString();
        }
        public char this[int index]
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
    public struct ReadSZLDataItem
    {
        private UInt16 _index;
        private OrderNumBuffer _orderNum;
        private UInt16 _moduleTypeId;
        private UInt16 _version;
        private UInt16 _pgDescriptionFile;


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

    }
    public class ReadSZLResponseData : ResponseData
    {
        private UInt16 _diagnosticTypeMask;
        private UInt16 _szlIndex;
        private UInt16 _listLength;
        private UInt16 _listCount;
        private List<ReadSZLDataItem> _objects = new List<ReadSZLDataItem>();

        public List<ReadSZLDataItem> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }




        public UInt16 ListCount
        {
            get { return _listCount; }
            set { _listCount = value; }
        }


        public UInt16 ListLength
        {
            get { return _listLength; }
            set { _listLength = value; }
        }


        public UInt16 SZLIndex
        {
            get { return _szlIndex; }
            set { _szlIndex = value; }
        }


        public UInt16 DiagnosticTypeMask
        {
            get { return _diagnosticTypeMask; }
            set { _diagnosticTypeMask = value; }
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
