namespace PLCompliant.Interface
{
    /// <summary>
    /// This interface is to be used for protocols using a header before the eventual data comes. As such, this interface is very bare
    /// </summary>
    public interface IProtocolHeader : INetworkSerializable, INetworkDeserializable
    {

    }
}
