using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuturesBackTestExportTool.Model
{
    //证券交易所
    public class StockExchange
    {
        //证券交易所名称
        public string stockExchangeName;
        //证券交易所对应的所有股票
        public List<string> stocks;
    }
}
