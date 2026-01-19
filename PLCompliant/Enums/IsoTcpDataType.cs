using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCompliant.Enums
{
    /// <summary>
    /// Bitmask for deciding which data payload it should be added to
    /// </summary>
    [Flags]
    public enum IsoTcpDataType : byte
    {
        /// <summary>
        /// Is it meant to be added to COTP data?
        /// </summary>
        COTPData = 1,
        /// <summary>
        /// Is it meant to be added to STEP7 data?
        /// </summary>
        STEP7Data = 2,


        /// <summary>
        /// As STEP7Data, is it for parameter data?
        /// </summary>
        STEP7ParamData = 4,
        /// <summary>
        /// As STEP7Data, is it for regular data?
        /// </summary>
        STEP7RegularData = 8,
    }
}
