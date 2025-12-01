using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Enums
{
    /// <summary>
    /// Represents the result of a scan
    /// </summary>
    public enum ScanResult : byte
    {
        /// <summary>
        /// The lock is taken, and no scan was performed
        /// </summary>
        LockTaken = 0,
        /// <summary>
        /// Scan finished sucessfully
        /// </summary>
        Finished = 1,
        /// <summary>
        /// The scan was aborted early
        /// </summary>
        Aborted = 2,
    }
}
