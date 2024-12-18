using CsvHelper.Configuration.Attributes;

namespace Domain.Csv
{
    public class OnlineOrderCsv
    {
        [Name("Order ID")]
        [Index(0)]
        public string Id { get; set; }

        [Name("Order Priority")]
        [Index(1)]
        public string Priority { get; set; }

        [Name("Order Date")]
        [Index(2)]
        [Format("dd/MM/yyyy")]
        public DateTime Date { get; set; }

        [Name("Region")]
        [Index(3)]
        public string Region { get; set; }

        [Name("Country")]
        [Index(4)]
        public string Country { get; set; }

        [Name("Item Type")]
        [Index(5)]
        public string ItemType { get; set; }

        [Name("Sales Channel")]
        [Index(6)]
        public string SalesChannel { get; set; }

        [Name("Ship Date")]
        [Index(7)]
        [Format("dd/MM/yyyy")]
        public DateTime ShipDate { get; set; }

        [Name("Units Sold")]
        [Index(8)]
        public int UnitsSold { get; set; }

        [Name("Unit Price")]
        [Index(9)]
        public decimal UnitPrice { get; set; }

        [Name("Unit Cost")]
        [Index(10)]
        public decimal UnitCost { get; set; }

        [Name("Total Revenue")]
        [Index(11)]
        public decimal TotalRevenue { get; set; }

        [Name("Total Cost")]
        [Index(12)]
        public decimal TotalCost { get; set; }

        [Name("Total Profit")]
        [Index(13)]
        [Format("0.00")]
        public decimal TotalProfit { get; set; }
    }
}
