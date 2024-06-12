namespace Core.Exceptions
{
    public class PriceException : HomeBudgetException
    {
        public PriceException() : base("Cena musi być większa niz 0")
        {
        }
    }
}