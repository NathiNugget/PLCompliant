namespace PLCompliant.Interface
{

    /// <summary>
    /// This is an interface for the data for a protocol implementing this interface
    /// </summary>
    public interface IProtocolData: INetworkDeserializable, INetworkSerializable, IAddableData, IGettableData, IResizable
    {
        
    }
}
