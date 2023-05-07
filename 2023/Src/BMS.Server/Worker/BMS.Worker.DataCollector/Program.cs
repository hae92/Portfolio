using System.Reflection;

using BMS.Core.Logger;
using BMS.Core.Common;
using BMS.Actor.Manager;
using BMS.Core.RestAPI;
using BMS.Core.Parser;
using BMS.Core.DB;
using BMS.Core.DB.Common;
using BMS.Worker.DataCollector;


// See https://aka.ms/new-console-template for more information

var logger = Singleton<Logger>.Instance;
var run = true;
var actorSystemName = "ActorSys";
logger.LogWrite(MethodBase.GetCurrentMethod(), "Run Application, BMS.Worker.DataCollector");

Manager actorSystem = new Manager(actorSystemName);
RestAPI client = new RestAPI();
var dbms = Singleton<DB>.Instance;
var existCoinRecord = new Dictionary<String, Boolean>();

String query = "";
dbms.AddDatabase("OracleDB", "localhost", "1521", "1234", "localdb");
var db = dbms.GetDatabase("OracleDB");
db.Connect();





while (run)
{
    string market = "KRW-BTC";
    string tickerResponse = client.GetTicker(market);
    //string tickerResponse = client.GetTicker_All(market);

    string[] BitCoinData = Parser.GetArray(tickerResponse);

    for(int bitCoinIndex = 0; bitCoinIndex < BitCoinData.Length; bitCoinIndex++)
    {
        Boolean UpdateFlag = false;
        Boolean InsertFlag = false;


        Dictionary<String, String> bitCoin = Parser.GetObject(BitCoinData[bitCoinIndex]);
        BitCoinInfo info = new BitCoinInfo(bitCoin);

        // 확인된 코인 정보가 없으면
        if(!existCoinRecord.ContainsKey(info.Market))
        {
            Boolean exist = (db.SelectQuery(info.GetExistQuery())[0]["COUNT"] == "1") ? true : false;

            existCoinRecord.Add(info.Market, exist);

            if (exist)
            {
                UpdateFlag = true;
            }
            else
            {
                InsertFlag = true;
            }
        }
        else
        {
            if(existCoinRecord[info.Market])
            {
                UpdateFlag = true;
            }
            else
            {
                InsertFlag = true;
            }
        }

        if(UpdateFlag)
        {
            db.NonSelectQuery(info.GetUpdateQuery());
        }

        if(InsertFlag)
        {
            db.NonSelectQuery(info.GetInsertQuery());
            existCoinRecord[info.Market] = true;
        }
    }

    //var data = Parser.Serialize(tickerResponse);
    //logger.LogWrite(MethodBase.GetCurrentMethod(), data);

    //var input = Console.ReadLine();
    //actorSystem.SendMessage(input);

    Thread.Sleep(1000 * 1);
}

db.Close();

logger.LogWrite(MethodBase.GetCurrentMethod(), "Stop Application, BMS.Worker.DataCollector");