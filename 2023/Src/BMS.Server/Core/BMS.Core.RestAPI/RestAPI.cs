using System;
using RestSharp;

namespace BMS.Core.RestAPI
{
    public class RestAPI
    {
        private const string BaseUrl = "https://api.upbit.com/v1/";

        public string GetTicker(string market)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("ticker", Method.Get);
            request.AddParameter("markets", market);
            request.AddParameter("markets", "KRW-ETH");
            request.AddParameter("markets", "KRW-NEO");
            request.AddParameter("markets", "KRW-MTL");
            request.AddParameter("markets", "KRW-XRP");
            request.AddParameter("markets", "KRW-ETC");
            request.AddParameter("markets", "KRW-SNT");
            request.AddParameter("markets", "KRW-WAVES");
            request.AddParameter("markets", "KRW-XEM");
            request.AddParameter("markets", "KRW-QTUM");
            request.AddParameter("markets", "KRW-LSK");
            request.AddParameter("markets", "KRW-STEEM");
            request.AddParameter("markets", "KRW-XLM");
            request.AddParameter("markets", "KRW-ARDR");
            request.AddParameter("markets", "KRW-ARK");
            request.AddParameter("markets", "KRW-STORJ");
            request.AddParameter("markets", "KRW-GRS");
            request.AddParameter("markets", "KRW-REP");
            request.AddParameter("markets", "KRW-ADA");
            request.AddParameter("markets", "KRW-SBD");
            request.AddParameter("markets", "KRW-POWR");
            request.AddParameter("markets", "KRW-BTG");
            request.AddParameter("markets", "KRW-ICX");
            request.AddParameter("markets", "KRW-EOS");
            request.AddParameter("markets", "KRW-TRX");
            request.AddParameter("markets", "KRW-SC");
            request.AddParameter("markets", "KRW-ONT");
            request.AddParameter("markets", "KRW-ZIL");
            request.AddParameter("markets", "KRW-DOGE");
            request.AddParameter("markets", "KRW-ALGO");
            request.AddParameter("markets", "KRW-NEAR");
            request.AddParameter("markets", "KRW-CELO");
            request.AddParameter("markets", "KRW-SAND");
            request.AddParameter("markets", "KRW-ANKR");


            var response = client.Execute(request);
            return response.Content;
        }

        public string GetTicker_All(string market)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("market/all", Method.Get);

            var response = client.Execute(request);
            return response.Content;
        }
    }
}