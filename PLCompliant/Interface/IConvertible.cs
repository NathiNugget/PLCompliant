using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// This interface implements convertion to the specified type
    /// </summary>
    /// <typeparam name="T">The type to convert to</typeparam>
    /// <typeparam name="A">An optional argument to pass to the convertion method</typeparam>
    public interface IConvertible<T, A>
    {
        /// <summary>
        /// Converts this object to a objet of type T
        /// </summary>
        /// <param name="arg">Argment to pass to the function</param>
        /// <returns>The converted item of type T</returns>
        T Convert(A arg);
    }
}
