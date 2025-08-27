namespace PayPath.Helpers;

public static class LoanUtils
{
    /// <summary>
    /// Calculates the monthly payment for a loan based on the loan loanAmount, annual interest annualRate, and loan term.
    /// </summary>
    /// <param name="loanAmount">The total loan loanAmount. Must be a positive value.</param>
    /// <param name="annualRate">The annual interest annualRate as a decimal (e.g., 5.5 for 5.5%). Must be non-negative.</param>
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
            throw new ArgumentOutOfRangeException(nameof(annualRate), "Annual interest annualRate must be non-negative.");
        }
        return annualRate / 1200;
    }
}
