namespace Core.Exceptions
{
    public sealed class NameIsRequiredException : HomeBudgetException
    {
        public NameIsRequiredException() : base("Nazwa jest wymagana.")
        {  
        }
    }
}