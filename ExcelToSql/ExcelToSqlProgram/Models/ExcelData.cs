using System.ComponentModel.DataAnnotations;

namespace ExcelToSqlProgram.Models
{
    //public enum DiscountBandEnum
    //{
    //    None,
    //    Low,
    //    Medum,
    //    High
    //}
    public class ExcelData
    {
        [Key]
        public int? Id { get; set; }
        public string? Segment { get; set; }
        public string? Country { get; set; }
        public string? Product { get; set; }
        public string? Discount_Band { get; set; }
        public float? Units_Sold { get; set; }
        public float? Manufacturing_Price { get; set; }
        public float? Sale_Price { get; set; }
        public float? Gross_Sales { get; set; }
        public float? Discounts { get; set; }
        public float? Sales { get; set; }
        public float? COGS { get; set; }
        public float? Profit { get; set; }
        public DateTime? Date { get; set; }

    }
}
