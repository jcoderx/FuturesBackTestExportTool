using System.Collections.Generic;

namespace FuturesBackTestExportTool.ReportModel
{
    //一个周期包含多个模型回测报告
    public class CycleReport
    {
        //周期名称
        public string cycleName;
        //多个模型回测报告
        public List<ModelReport> modelReports;
    }
}
