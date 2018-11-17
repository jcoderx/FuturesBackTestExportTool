using System.Collections.Generic;

namespace FuturesBackTestExportTool.Page
{
    //挑选要分析的股票，合约或品种界面
    public class FuturesAnalysisPage
    {
        public const string FUTURES_ANALYSIS_PAGE_TITLE = "挑选要分析的股票,合约或品种";
        //品种treeView
        public const string AUTOMATION_ID_TREEVIEW_PRODUCT_CATEGORY = "4050";
        //合约列表
        public const string AUTOMATION_ID_LISTBOX_PRODUCT = "1001";

        //主力
        public const string AUTOMATION_ID_BUTTON_ZHULI = "2936";
        //指数
        public const string AUTOMATION_ID_BUTTON_ZHISHU = "2939";
        //连续
        public const string AUTOMATION_ID_BUTTON_LIANXU = "2940";


        public static List<string> PRODUCT_CATEGORY = new List<string>() { "郑州商品", "大连商品", "上海橡胶", "上海金属", "上海能源", "中金所" };

        //国内期货
        public static bool isDomesticFutures(string category)
        {
            return PRODUCT_CATEGORY.Contains(category);
        }
    }
}
