using System;

namespace Core.Exceptions
{
    public abstract class HomeBudgetException : Exception
    {
        protected HomeBudgetException(string message) : base(message) { }
    }
}