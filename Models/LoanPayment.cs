namespace PayPath.Models;

public class LoanPayment
{
    public int Month { get; set; }
    public double Amount { get; set; }
    public double Principle { get; set; }
    public double Interest { get; set; }
    public double TotalInterest { get; set; }
    public double Balance { get; set; }
}
