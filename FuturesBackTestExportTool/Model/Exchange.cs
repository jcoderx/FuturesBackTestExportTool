using System.Collections.Generic;

namespace FuturesBackTestExportTool.Model
{
    //期货交易所
    public class Exchange
    {
        //期货交易所名称
        public string exchangeName;
        //期货交易所对应的所有品种
        public List<Variety> varieties;
    }
}
