using System.ComponentModel.DataAnnotations;

namespace ExcelToSqlProgram.Models
{
    public enum DiscountBandEnum
    {
        None,
        Low,
        Medum,
        High
    }
    public class ExcelData
    {
        [Key]
        public int? Id { get; set; }
        public string? Segment { get; set; }
        public string? Country { get; set; }
        public string? Product { get; set; }
        public DiscountBandEnum? DiscountBand { get; set; }
        public double? Units_Sold { get; set; }
        public string? Manufacturing_Price { get; set; }
        public string? Sale_Price { get; set; }
        public string? Gross_Sales { get; set; }
        public string? Discounts { get; set; }
        public string? Sales { get; set; }
        public string? COGS { get; set; }
        public string? Profit { get; set; }
        public string? Date { get; set; }

    }
}
