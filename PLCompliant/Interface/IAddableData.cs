using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface for allowing an object to add data to its internal storage. 
    /// </summary>
    public interface IAddableData
    {
        /// <summary>
        /// Adds data from an unmanaged object of type T to the objects internal data storage
        /// </summary>
        /// <typeparam name="T">An unmanaged type of the data to add</typeparam>
        /// <param name="inputData">The data to add</param>
        /// <param name="type">A byte denoting the type. This may be interpreted by diffrent implementors as meaning diffrent parts of the data segemnt (eg. paramenter-data, and other-data)</param>
        /// <returns>The amount of data added in bytes</returns>
        public int AddData<T>(T inputData, byte type) where T : unmanaged, IEndianConvertable;
        /// <summary>
        /// Adds data from an Uint16 to the objects internal data storage
        /// </summary>
        /// <param name="inputData">The Uint16 to add</param>
        /// <param name="type">A byte denoting the type. This may be interpreted by diffrent implementors as meaning diffrent parts of the data segemnt (eg. paramenter-data, and other-data)</param>
        /// <returns>The amount of data added in bytes</returns>
        public int AddData(UInt16 inputData, byte type);
        /// <summary>
        /// Adds data from an byte to the objects internal data storage
        /// </summary>
        /// <param name="inputData">The byte to add</param>
        /// <param name="type">A byte denoting the type. This may be interpreted by diffrent implementors as meaning diffrent parts of the data segemnt (eg. paramenter-data, and other-data)</param>
        /// <returns>The amount of data added in bytes</returns>
        public int AddData(byte inputData, byte type);

        /// <summary>
        /// Adds data from an Span of bytes to the objects internal data storage
        /// </summary>
        /// <param name="binaryData"> a ReadonlySpan of bytes to add</param>
        /// <param name="type">A byte denoting the type. This may be interpreted by diffrent implementors as meaning diffrent parts of the data segemnt (eg. paramenter-data, and other-data)</param>
        /// <returns>The amount of data added in bytes</returns>
        public int AddData(ReadOnlySpan<byte> binaryData, byte type);
    }
}
