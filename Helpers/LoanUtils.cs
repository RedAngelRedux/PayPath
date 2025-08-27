using PayPath.Models;

namespace PayPath.Helpers;

public static class LoanUtils
{

    /// <summary>
    /// Calculates the monthly payment schedule for a loan based on the loan amount, annual interest rate, and loan term.
    /// </summary>
    /// <param name="loan"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Loan GetPayments(Loan loan)
    {
        if (loan == null)
        {
            throw new ArgumentNullException(nameof(loan), "Loan cannot be null.");
        }
        if (loan.Amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loan.Amount), "Loan amount must be a positive value.");
        }
        if (loan.Rate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loan.Rate), "Annual interest rate must be non-negative.");
        }
        if (loan.Term <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loan.Term), "Loan term must be a positive integer.");
        }

        loan.Schedule.Clear();

        // Calculate monthly payment
        loan.Payment = CalculateMonthlyPayment(loan.Amount, loan.Rate, loan.Term);
        var loanMonths = loan.Term * 12;
        double monthlyRate = CalculateMonthlyRate(loan.Rate); // Monthly interest rate

        // variables to hold the total interst and balance
        double balance = loan.Amount;  // Balance starts as the loan amount
        double totalInterest = 0.0; // Total interest starts at 0
        double monthlyPrincipal = 0.0; // Monthly principal minus interest starts at 0
        double monthlyInterest = 0.0; // Monthly interest starts at 0

        for(int month = 1; month <= loanMonths; month++)
        {
            // Calculate monthly interest
            monthlyInterest = CalculateMonthlyInterest(balance, monthlyRate);
            
            // Update total interest
            totalInterest += monthlyInterest;

            // Calculate monthly principal
            monthlyPrincipal = loan.Payment - monthlyInterest;

            // Update balance
            balance -= monthlyPrincipal;

            // Ensure balance does not go negative due to rounding
            if (balance < 0) balance = 0;

            // Add payment details to the amortization schedule
            loan.Schedule.Add(new LoanPayment
            {
                Month = month,
                Amount = Math.Round(loan.Payment,2),
                Principle = Math.Round(monthlyPrincipal, 2),
                Interest = Math.Round(monthlyInterest, 2),
                TotalInterest = Math.Round(totalInterest, 2),
                Balance = Math.Round(balance, 2)
            });
        }

        loan.TotalInterest = Math.Round(totalInterest, 2);
        loan.TotalCost = Math.Round(loan.Amount + totalInterest, 2);

        return loan;
    }

    /// <summary>
    /// Calculates the monthly payment for a loan based on the loan loanAmount, annual interest monthlyRate, and loan term.
    /// </summary>
    /// <param name="loanAmount">The total loan loanAmount. Must be a positive value.</param>
    /// <param name="annualRate">The annual interest monthlyRate as a decimal (e.g., 5.5 for 5.5%). Must be non-negative.</param>
    /// <param name="term">The loan term in years. Must be a positive integer.</param>
    /// <returns>The monthly payment loanAmount as a <see cref="double"/>. Returns 0 if the loan term is zero.</returns>
    public static double CalculateMonthlyPayment(double loanAmount, double annualRate, int term)
    {
        var monthlyRate = CalculateMonthlyRate(annualRate);
        var months = term * 12;
        var payment = (loanAmount * monthlyRate) / (1 - Math.Pow(1 + monthlyRate, -months));
        return payment;
    }

    private static double CalculateMonthlyRate(double annualRate)
    {
        if (annualRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(annualRate), "Annual interest monthlyRate must be non-negative.");
        }
        return annualRate / 1200;
    }

    public static double CalculateMonthlyInterest(double balance, double monthlyRate)
    {
        return balance * monthlyRate;
    }
}
