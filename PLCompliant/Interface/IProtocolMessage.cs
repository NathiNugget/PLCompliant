namespace PLCompliant.Interface
{
    /// <summary>
    /// This is an interface for the whole message to send over the protcol implementing this interface
    /// </summary>
    public interface IProtocolMessage : INetworkSerializable, INetworkMessageDeserializable, IAddableData, IGettableData
    {

    }
}
