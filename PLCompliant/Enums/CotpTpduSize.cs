namespace PLCompliant.Enums
{
    /// <summary>
    /// Represents TPDU-size for the COTP-packet
    /// </summary>
    /// For more information sizes and their meaning, go read https://datatracker.ietf.org/doc/html/rfc892#section-8.3.4
    public enum CotpTpduSize : byte
    {
        /// <summary>
        ///  Default TPDU-size
        /// </summary>
        Octets128 = 0x7,
        Octets256 = 0x8,
        Octets512 = 0x9,
        Octets1024 = 0xA,
        Octets2048 = 0xB,
        Octets4096 = 0xC,
        Octets8192 = 0xD,
    }
}
