using System;
using System.Net;
using System.Data;
using System.Linq;
using WebAPI.Model;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        [HttpGet]
        [Route("getcoins/{sort}/{lines}")]
        public async Task<IList<Coin>> Get(string sort, int lines)
        {
            return coinsList(sort, lines);
        }


        static IList<Coin> coinsList(string psort, int plines)
        {
            string url = $"https://data.messari.io/api/v1/assets?with-metrics";
            List<Coin> SortedList;
            IList<Coin> searchResults = new List<Coin>();
            using (WebClient webClient = new System.Net.WebClient())
            {
                // Get Json data from url
                WebClient wc = new WebClient();
                var json = wc.DownloadString(url);
                string valueOriginal = Convert.ToString(json);
                JObject jobj = JObject.Parse(json);
                IList<JToken> results = jobj["data"].Children().ToList();
                // Loop on all Data from Json
                foreach (JToken result in results)
                {
                    Coin coin = result.ToObject<Coin>();
                    coin.price_usd = Math.Round(decimal.Parse(result["metrics"]["market_data"]["price_usd"].ToString()), 2);
                    coin.current_marketcap_usd = Math.Round(decimal.Parse(result["metrics"]["marketcap"]["current_marketcap_usd"].ToString()), 2);
                    searchResults.Add(coin);
                }
                if (psort == "price_usd") // Order by price
                {
                    SortedList = searchResults.OrderByDescending(o => o.price_usd).Take(plines).ToList();
                }
                else // Order by Market cap.
                {
                    SortedList = searchResults.OrderByDescending(o => o.current_marketcap_usd).Take(plines).ToList();
                }
                return (SortedList);
            }
        }
    }
}