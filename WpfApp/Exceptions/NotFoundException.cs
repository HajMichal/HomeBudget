
namespace Core.Exceptions
{
    public sealed class NotFoundException : HomeBudgetException
    {
        public NotFoundException(string name): base($"{name} is required.")
        {
        }
    }
}