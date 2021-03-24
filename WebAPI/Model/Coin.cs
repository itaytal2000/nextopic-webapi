namespace WebAPI.Model
{
    public class Coin
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public decimal price_usd { get; set; }
        public decimal current_marketcap_usd { get; set; }
    }
}