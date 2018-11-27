namespace FuturesBackTestExportTool.ReportModel
{
    //单个模型对应的回测报告
    public class ModelReport
    {
        public bool warning;
        //信号计算开始时间
        public string startingDate;
        //信号计算结束时间
        public string endingDate;
        //模型名称(期望)
        public string modelName;
        //实际模型名称（报表中导出的）
        public string realisticModelName;
        //信号个数
        public string signalNumber;
        //最终权益
        public string lastInterest;
        //夏普比率
        public string sharpeRatio;
        //权益最大回撤
        public string interestMaxRetracement;
        //权益最大回撤比
        public string interestMaxRetracementRatio;
        //风险率
        public string hazardRatio;
        //每手最大亏损
        public string maxLossPerHand;
        //每手平均盈亏
        public string avgProfitLossPerHand;
        //胜率
        public string winRatio;
        //得分
        public string score;
        //最大盈利
        public string maxProfit;
        //最大亏损
        public string maxLoss;
        //最大持续盈利次数
        public string maxContinuousProfitabilityTimes;
        //最大持续亏损次数
        public string maxContinuousLossesTimes;
    }
}