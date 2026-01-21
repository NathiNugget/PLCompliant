using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Utilities
{
    public class MemoryUtilities
    {
        
        public static bool CompareMemory<T>(ref T left, ref T right) where T: unmanaged
        {
            var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left, 1);
            var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right, 1);
            ReadOnlySpan<byte> leftBytesSpan = MemoryMarshal.Cast<T, byte>(leftSpan);
            ReadOnlySpan<byte> rightBytesSpan = MemoryMarshal.Cast<T, byte>(rightSpan);
            return rightBytesSpan.SequenceEqual(leftBytesSpan);
        }
    }
}
