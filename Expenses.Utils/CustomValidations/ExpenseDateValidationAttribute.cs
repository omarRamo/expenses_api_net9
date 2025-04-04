using System.ComponentModel.DataAnnotations;


namespace Expenses.Utils.CustomValidations;

public class ExpenseDateValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime expenseDate)
        {
            var currentDate = DateTime.Now;
            var threeMonthsAgo = currentDate.AddMonths(-3);

            if (expenseDate < threeMonthsAgo)
            {
                return false;
            }
            return true;
        }
        return false;
    }
}
