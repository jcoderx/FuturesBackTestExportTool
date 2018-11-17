using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace FuturesBackTestExportTool
{
    public class Utils
    {
        public static string getExportDir()
        {
            String dir = System.Windows.Forms.Application.StartupPath + "\\export\\";
            Utils.mkdir(dir);
            return dir;
        }

        //截取文件夹名称
        public static string cutDirName(string fileName)
        {
            if (fileName.Contains("\\"))
            {
                string temp = fileName.Substring(0, fileName.LastIndexOf("\\"));
                if (temp.Contains("\\"))
                {
                    fileName = temp.Substring(temp.LastIndexOf("\\") + 1);
                }
            }
            return fileName;
        }

        public static void mkdir(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public static String getDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string formatDate(DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd");
        }

        public static String getTimeMillisecond()
        {
            return DateTime.Now.Ticks + "";
        }

        public static int compareDate(DateTime dt0, DateTime dt1)
        {
            if (dt0.Year > dt1.Year)
            {
                return 1;
            }
            else if (dt0.Year < dt1.Year)
            {
                return -1;
            }
            else
            {
                if (dt0.Month > dt1.Month)
                {
                    return 1;
                }
                else if (dt0.Month < dt1.Month)
                {
                    return -1;
                }
                else
                {
                    if (dt0.Day > dt1.Day)
                    {
                        return 1;
                    }
                    else if (dt0.Day < dt1.Day)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        //补零，用于日期控件填写，如6，补成2位，则为06
        public static string zeroize(int digit, int length)
        {
            string digitStr = digit.ToString();
            while (digitStr.Length < length)
            {
                digitStr = "0" + digitStr;
            }
            return digitStr;
        }


        public static List<List<string>> getDataFromListView(AutomationElement dataGridAE)
        {
            List<List<string>> result = new List<List<string>>();
            Condition condition0 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.DataItem);
            AutomationElementCollection automationElementCollection = dataGridAE.FindAll(TreeScope.Children, condition0);
            if (automationElementCollection != null && automationElementCollection.Count > 0)
            {
                foreach (AutomationElement dataItemAE in automationElementCollection)
                {
                    Condition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                    AutomationElementCollection textElementCollection = dataItemAE.FindAll(TreeScope.Children, condition1);
                    if (textElementCollection != null && textElementCollection.Count > 0)
                    {
                        List<string> row = new List<string>();
                        foreach (AutomationElement textAE in textElementCollection)
                        {
                            string text = textAE.Current.Name;
                            if (string.IsNullOrEmpty(text))
                            {
                                text = "@";
                            }
                            row.Add(text);
                        }
                        result.Add(row);
                    }
                }
            }
            return result;
        }

        public static bool compareDownloadDate(string downloadDateStr, DateTime targetDate, string cycle)
        {
            DateTime downloadDate = DateTime.MinValue;
            try
            {
                downloadDate = DateTime.ParseExact(downloadDateStr, "yyyy/M/d", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                return false;
            }
            int days = 0;
            switch (cycle)
            {
                case "TICK":
                    days = 3;
                    break;
                case "1秒钟":
                    days = 3;
                    break;
                case "1分钟":
                    days = 21;
                    break;
                case "15分钟":
                    days = 365;
                    break;
                case "1日":
                    days = 3650;
                    break;
                default:
                    return false;
            }
            downloadDate = downloadDate.AddDays(days);
            return compareDate(downloadDate, targetDate) < 0;
        }
    }
}
