using System.Collections.Generic;

namespace FuturesBackTestExportTool.Model
{
    public class TimeCycle
    {
        public static List<string> DEFAULT_CYCLE = new List<string>() { "5秒", "10秒", "15秒", "30秒", "1分钟", "3分钟", "5分钟", "10分钟", "15分钟", "30分钟", "1小时", "2小时", "3小时", "4小时" };
        public int value = 0;
        //单位
        //秒、分钟、小时、日
        //value=0，判断：日线、周线、月线、季线、年线
        public string unit;

        public string getName()
        {
            if (value != 0)
            {
                return value + unit;
            }
            else
            {
                return unit;
            }
        }

        public bool isDefaultCycle()
        {
            return DEFAULT_CYCLE.Contains(getName());
        }
    }
}
