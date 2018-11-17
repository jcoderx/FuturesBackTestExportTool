namespace FuturesBackTestExportTool.Model
{
    public class TimeCycle
    {
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
    }
}
