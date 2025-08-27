using System.ComponentModel.DataAnnotations;

namespace PayPath.Models;

public class Loan
{
    [Required]
    [Range(1, double.MaxValue, ErrorMessage="Purchase Amount Must Be At Least $1")]
    public double Amount { get; set; }

    [Range(0.0,100,MinimumIsExclusive = true,ErrorMessage ="THe Interest Rate Must be Between 0.00 and 100")]
    public double Rate { get; set; }

    [Range(1,100,ErrorMessage ="The Term Must Be Between 1 and 100 Years")]
    public int Term { get; set; }

    public double Payment { get; set; }

    public double TotalInterest { get; set; }

    public double TotalCost { get; set; }

    public List<LoanPayment> Schedule { get; set; } = [];
}
