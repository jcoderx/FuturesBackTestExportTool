using FuturesBackTestExportTool.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    //跳转界面
    public class PageUtils
    {
        //打开“挑选要分析的股票，合约或品种”界面
        public static bool toFuturesAnalysisPage(Form from, IntPtr mainHandle)
        {
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow != null)
            {
                WindowsApiUtils.clearOtherWindows(mainHandle, null);
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_HANGQINGSHOUYE);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AutomationElement buttonHangqing = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (buttonHangqing != null)
                {
                    //这里突然一天就不行了11.29，愁人
                    //SimulateOperating.clickButton(buttonHangqing);
                    targetWindow.SetFocus();
                    SimulateOperating.leftClickAutomationElement(buttonHangqing);
                    
                    Thread.Sleep(500);

                    PropertyCondition condition2 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_TREEVIEW_SHUJU);
                    PropertyCondition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
                    AutomationElement treeViewShuju = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition2, condition3));
                    if (treeViewShuju != null)
                    {
                        Condition condition4 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                        AutomationElementCollection automationElementCollection = treeViewShuju.FindAll(TreeScope.Children, condition4);
                        if (automationElementCollection != null && automationElementCollection.Count > 0)
                        {
                            if ("我的篮子".Equals(automationElementCollection[0].Current.Name))
                            {
                                SimulateOperating.doubleClick(automationElementCollection[0]);

                                PropertyCondition condition5 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_KXIAN);
                                PropertyCondition condition6 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                                AutomationElement buttonKXIAN = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition5, condition6));
                                if (buttonKXIAN != null)
                                {
                                    SimulateOperating.clickButton(buttonKXIAN);
                                    return true;
                                }
                                else
                                {
                                    frontMessageBox(from, "未找到主界面“K线”按钮");
                                    return false;
                                }
                            }
                            else
                            {
                                frontMessageBox(from, "未找到主界面“数据”树形结构中的“我的篮子”子项");
                                return false;
                            }
                        }
                        else
                        {
                            frontMessageBox(from, "未找到主界面“数据”树形结构的子项");
                            return false;
                        }
                    }
                    else
                    {
                        frontMessageBox(from, "未找到主界面“数据”对应的树形结构");
                        return false;
                    }
                }
                else
                {
                    frontMessageBox(from, "未找到主界面“行情首页”按钮");
                    return false;
                }
            }
            else
            {
                frontMessageBox(from, "未找到“赢智程序化”主界面");
                return false;
            }
        }

        public static void frontMessageBox(Form form, string message)
        {
            form.TopMost = true;
            MessageBox.Show(message);
            form.TopMost = false;
        }
    }
}
