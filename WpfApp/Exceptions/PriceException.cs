namespace Core.Exceptions
{
    public class PriceException : HomeBudgetException
    {
        public PriceException() : base("Price must be greater than 0.")
        {
        }
    }
}