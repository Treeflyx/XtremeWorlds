
namespace Core.Serialization
{
    public interface ISerializer<TInputType, TOutputType>
    {
        TOutputType Serialize(TInputType rawObject);
        TInputType Deserialize(TOutputType serializedValue);
        TInputType Read(string filename);
        void Write(string filename, TInputType rawObject);
    }
}