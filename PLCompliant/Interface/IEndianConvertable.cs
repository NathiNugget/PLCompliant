
namespace PLCompliant.Interface
{
    /// <summary>
    /// Interface implementing endianness-convertion of the implementor
    /// </summary>
    public interface IEndianConvertable
    {
        /// <summary>
        /// Convert byte order of the object from host to network order
        /// </summary>
        public void FromHostToNetwork();
        /// <summary>
        /// Convert byte order of the object from network to host order
        /// </summary>
        public void FromNetworkToHost();


    }
}
