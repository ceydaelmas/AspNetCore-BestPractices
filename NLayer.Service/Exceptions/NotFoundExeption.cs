namespace NLayer.Service.Exceptions
{
    public class NotFoundExeption : Exception
    {
        public NotFoundExeption(string message) : base(message) { } //string mesajı exceptionun mesajına yollar.

    }
}
