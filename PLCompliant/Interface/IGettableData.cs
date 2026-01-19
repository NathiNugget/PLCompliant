using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface implementing gettable data from the objects internal storage
    /// </summary>
    public interface IGettableData
    {
        /// <summary>
        /// Get data from the objects internal storage reinterpreted as a unmanaged type
        /// </summary>
        /// <typeparam name="T">An unmanaged datatype to get</typeparam>
        /// <param name="type">A byte denoting the type. This may be interpreted by diffrent implementors as meaning diffrent parts of the data segemnt (eg. paramenter-data, and other-data)</param>
        /// <param name="index">The index if the internal storage to begin interpreting from</param>
        /// <returns>The reinterpreted data</returns>
        public T GetData<T>(int index, byte type) where T : unmanaged, IEndianConvertable;
    }
}
