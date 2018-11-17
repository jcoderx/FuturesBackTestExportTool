using System.Collections.Generic;

namespace FuturesBackTestExportTool.Model
{
    //交易所、品种、合约
    public class Exchange
    {
        public string exchangeName;

        public List<Variety> varieties;
    }
}
