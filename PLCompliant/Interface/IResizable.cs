using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Interface
{
    /// <summary>
    /// Interfaces implementing resizable storage
    /// </summary>
    public interface IResizable
    {
        /// <summary>
        /// Resizes the internal storage to the given amoint
        /// </summary>
        /// <param name="newSize">The new size</param>
        public void ResizeStorage(int newSize);
    }
}
