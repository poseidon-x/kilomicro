using System;
namespace coreLogic
{
    public interface IHumanCapitalHelper
    {
        System.Collections.Generic.List<staffLoanSchedule> calculateSchedule(double amount, double rate, DateTime loanDate, double tenure);
    }
}
