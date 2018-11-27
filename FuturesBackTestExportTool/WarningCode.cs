using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuturesBackTestExportTool
{
    public class WarningCode
    {
        //没有警告
        public const int NO_WARNING = 0;
        //起止日期填写错误警告
        public const int WARNING_STARTING_ENDING_DATE = 1;
        //收集报告出错警告
        public const int WARNING_COLLECTION_REPORT = 2;
        //模型不支持周期(不支持的模型)
        public const int WARNING_UNSUPPORTED_MODEL = 3;
    }
}
