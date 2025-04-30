namespace electro_shop_backend.Exceptions.CustomExceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
