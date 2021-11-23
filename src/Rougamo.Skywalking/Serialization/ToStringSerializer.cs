namespace Rougamo.Skywalking.Serialization
{
    class ToStringSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return obj.ToString();
        }
    }
}
