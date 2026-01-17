using PLCompliant.Interface;
using PLCompliant.Utilities;
using System.Runtime.InteropServices;


namespace PLCompliant.STEP_7
{
    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    public struct ReadSZLResponseHeader : IEndianConvertable
    {
        [FieldOffset(0)] private UInt16 _diagnosticTypeMask;
        [FieldOffset(2)] private UInt16 _szlIndex;
        [FieldOffset(4)] private UInt16 _listLength;
        [FieldOffset(6)] private UInt16 _listCount;


        public ReadSZLResponseHeader(UInt16 diagnosticTypeMask, UInt16 szlIndex, UInt16 listLength, UInt16 listCount)
        {
            _diagnosticTypeMask = diagnosticTypeMask;
            _szlIndex = szlIndex;
            _listLength = listLength;
            _listCount = listCount;
        }
        public ReadSZLResponseHeader()
        {
            
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
        public void FromHostToNetwork()
        {
            _diagnosticTypeMask = EndianConverter.FromHostToNetwork(_diagnosticTypeMask);
            _szlIndex = EndianConverter.FromHostToNetwork(_szlIndex);
            _listLength = EndianConverter.FromHostToNetwork(_listLength);
            _listCount = EndianConverter.FromHostToNetwork(_listCount);
        }

        public void FromNetworkToHost()
        {
            _diagnosticTypeMask = EndianConverter.FromNetworkToHost(_diagnosticTypeMask);
            _szlIndex = EndianConverter.FromNetworkToHost(_szlIndex);
            _listLength = EndianConverter.FromNetworkToHost(_listLength);
            _listCount = EndianConverter.FromNetworkToHost(_listCount);
        }
    }
}
