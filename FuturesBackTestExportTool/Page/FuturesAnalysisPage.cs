using System.Collections.Generic;

namespace FuturesBackTestExportTool.Page
{
    //挑选要分析的股票，合约或品种界面
    public class FuturesAnalysisPage
    {
        public const string FUTURES_ANALYSIS_PAGE_TITLE = "挑选要分析的股票,合约或品种";
        //品种treeView
        public const string AUTOMATION_ID_TREEVIEW_EXCHANGE = "4050";
        //合约列表
        public const string AUTOMATION_ID_LISTBOX_VARIETY = "1001";
        //主力复选框
        public const string AUTOMATION_ID_BUTTON_ZHULI = "2936";
        //指数复选框
        public const string AUTOMATION_ID_BUTTON_ZHISHU = "2939";
        //连续复选框
        public const string AUTOMATION_ID_BUTTON_LIANXU = "2940";
        //确定按钮
        public const string AUTOMATION_ID_BUTTON_OK = "1";

        public static List<string> DOMESTIC_FUTURES = new List<string>() { "郑州商品", "大连商品", "上海橡胶", "上海金属", "上海能源", "中金所", "上期所一", "上期所二" };
        //国内期货
        public static bool isDomesticFutures(string exchangeName)
        {
            return DOMESTIC_FUTURES.Contains(exchangeName);
        }

        //国内股票
        public static List<string> DOMESTIC_STOCKS = new List<string>() { "上证股票", "深证股票", "香港股票" };
        public static bool isDomesticStock(string exchangeName)
        {
            return DOMESTIC_STOCKS.Contains(exchangeName);
        }
    }
}
