namespace Core.Exceptions
{
    public sealed class NameIsRequiredException : HomeBudgetException
    {
        public NameIsRequiredException() : base("Name is required.")
        {  
        }
    }
}