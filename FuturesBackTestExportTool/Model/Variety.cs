using System.Collections.Generic;

namespace FuturesBackTestExportTool.Model
{
    //合约
    public class Variety
    {
        public const int ZHISHU = 0;
        public const int ZHULI = 1;
        public const int LIANXU = 2;

        //品种
        public string varietyName;
        //所选的合约
        public List<int> agreements;

        public static string getAgreementName(int agreement)
        {
            switch (agreement)
            {
                case ZHISHU:
                    return "指数";
                case ZHULI:
                    return "主力";
                case LIANXU:
                    return "连续";
            }
            return "";
        }
    }
}
