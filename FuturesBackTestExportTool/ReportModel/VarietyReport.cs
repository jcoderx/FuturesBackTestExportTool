using System;
using System.Collections.Generic;

namespace FuturesBackTestExportTool.ReportModel
{
    //一个品种的回测报告，一个品种包含多个周期回测报告
    public class VarietyReport
    {
        //品种名称
        public string varietyName;
        //合约
        public int agreement;
        //起始日期
        public DateTime startingDate;
        //终止日期
        public DateTime endingDate;
        //多个周期回测报告
        public List<CycleReport> cycleReports;
    }
}
