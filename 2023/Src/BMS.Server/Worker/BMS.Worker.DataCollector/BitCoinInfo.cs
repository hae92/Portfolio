using System;
using System.Collections;
using System.Globalization;

namespace BMS.Worker.DataCollector
{
    public class BitCoinInfo
    {
        public String Market = String.Empty;
        public String UpdateyyMMdd = String.Empty;
        public String UpdateHHmmss = String.Empty;
        public DateTime UpdateTime = default(DateTime);
        public Double TradePrice = default(Double);
        public String Change = String.Empty;
        public Double ChangePrice = default(Double);
        public Double ChangeRate = default(Double);
        public Double TradeVolume = default(Double);

        public BitCoinInfo(Dictionary<String, String> data)
        {
            this.Market = data["market"];
            this.UpdateyyMMdd = data["trade_date_kst"];
            this.UpdateHHmmss = data["trade_time_kst"];

            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime.TryParseExact(UpdateyyMMdd + UpdateHHmmss, "yyyyMMddHHmmss", provider, DateTimeStyles.None, out UpdateTime);

            Double.TryParse(data["trade_price"], out this.TradePrice);
            this.Change = data["change"];
            Double.TryParse(data["signed_change_price"], out this.ChangePrice);
            Double.TryParse(data["signed_change_rate"], out this.ChangeRate);
            Double.TryParse(data["trade_volume"], out this.TradeVolume);
        }

        public String GetInsertQuery()
        {
            String query = String.Format("INSERT INTO BTC_MONITORING VALUES ('{0}', '{1}', '{2}', TO_DATE('{3}', 'yyyyMMddHH24MISS'), {4}, '{5}', {6}, {7}, {8})", 
                this.Market, this.UpdateyyMMdd, this.UpdateHHmmss, this.UpdateyyMMdd + this.UpdateHHmmss, this.TradePrice, this.Change, this.ChangePrice, this.ChangeRate, this.TradeVolume);
            return query;
        }

        public String GetUpdateQuery()
        {
            String query = String.Format("UPDATE BTC_MONITORING " +
                "SET" +
                "   UPDATE_YMD = '{0}'," +
                "   UPDATE_HMS = '{1}'," +
                "   UPDATE_TIME = TO_DATE('{2}', 'yyyyMMddHH24MISS')," +
                "   BTC_PRICE = {3}," +
                "   CHANGE = '{4}'," +
                "   CHANGE_PRICE = '{5}'," +
                "   CHANGE_RATE = '{6}'," +
                "   TRADE_VOLUME = '{7}'" +
                "WHERE" +
                "   MARKET = '{8}'",
                this.UpdateyyMMdd, this.UpdateHHmmss, this.UpdateyyMMdd + this.UpdateHHmmss, this.TradePrice, this.Change, this.ChangePrice, this.ChangeRate, this.TradeVolume, this.Market);
            return query;
        }

        public String GetExistQuery()
        {
            String query = String.Format("SELECT COUNT(*) AS COUNT FROM BTC_MONITORING WHERE MARKET = '{0}'", this.Market);
            return query;
        }

    }
}
