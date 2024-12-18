﻿using Newtonsoft.Json;

namespace Domain.ApiKataEsPublico
{
    public class OnlineOrderApiKataResponse
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("item_type")]
        public string ItemType { get; set; }

        [JsonProperty("sales_channel")]
        public string SalesChannel { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("ship_date")]
        public string ShipDate { get; set; }

        [JsonProperty("units_sold")]
        public int UnitsSold { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("unit_cost")]
        public decimal UnitCost { get; set; }

        [JsonProperty("total_revenue")]
        public decimal TotalRevenue { get; set; }

        [JsonProperty("total_cost")]
        public decimal TotalCost { get; set; }

        [JsonProperty("total_profit")]
        public decimal TotalProfit { get; set; }

        [JsonProperty("links")]
        public LinkSelfApiKataResponse Links { get; set; }
    }
}
