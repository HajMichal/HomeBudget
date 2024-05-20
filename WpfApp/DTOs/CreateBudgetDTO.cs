using Core.Models;

namespace Core.DTOs
{
    public class CreateBudgetDTO
    {
        public string Name { get; set; }
        public decimal MonthSalary { get; set; }
    }
}