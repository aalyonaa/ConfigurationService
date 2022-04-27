namespace MarvelousConfigs.BLL.Infrastructure.Exceptions
{
    public class BadGatewayException : Exception
    {
        public BadGatewayException(string message) : base(message) { }
    }
}