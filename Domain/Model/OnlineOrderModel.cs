namespace Domain.Model
{
    public class OnlineOrderModel
    {
        public string Uuid { get; set; }

        public string Id { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public string ItemType { get; set; }

        public string SalesChannel { get; set; }

        public string Priority { get; set; }

        public string Date { get; set; }

        public string ShipDate { get; set; }

        public int UnitsSold { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal UnitCost { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalCost { get; set; }

        public decimal TotalProfit { get; set; }

        public LinkSelfModel Link { get; set; }
    }
}
