using FuturesBackTestExportTool.Page;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using static FuturesBackTestExportTool.WindowsApi;

namespace FuturesBackTestExportTool
{
    public class WindowsApiUtils
    {
        //关闭窗口
        public static void closeWindow(IntPtr handle)
        {
            SendMessage(handle, WM_CLOSE, 0, 0);
        }

        //根据窗口类型和窗口标题，查找对应窗口，模糊搜索
        public static List<IntPtr> findWindowHandlesByClassTitleFuzzy(string expectedClassName, string expectedTitle)
        {
            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);

                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (className.ToString().Contains(expectedClassName) && title.ToString().Contains(expectedTitle))
                {
                    wndHandles.Add(h);
                }
                return true;
            }, 0);
            return wndHandles;
        }

        //根据窗口类型和窗口标题，查找对应窗口，精确搜索
        public static List<IntPtr> findWindowHandlesByClassTitleExact(string expectedClassName, string expectedTitle)
        {
            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);

                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (className.ToString().Equals(expectedClassName) && title.ToString().Equals(expectedTitle))
                {
                    wndHandles.Add(h);
                }
                return true;
            }, 0);
            return wndHandles;
        }

        //根据控件类名查找所有控件
        public static List<IntPtr> findControlHandlesByClassName(IntPtr hWnd, string expectedClassName)
        {
            List<IntPtr> controlHandles = new List<IntPtr>();
            EnumChildWindows(hWnd, (h, l) =>
            {
                if (GetParent(h) == hWnd)
                {
                    StringBuilder className = new StringBuilder(200);
                    int len;
                    len = GetClassName(h, className, 200);
                    if (expectedClassName.Equals(className.ToString()))
                    {
                        controlHandles.Add(h);
                    }
                }
                return true;
            }, 0);
            return controlHandles;
        }

        //查找上下文菜单
        public static List<IntPtr> findContextMenuHandles()
        {
            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);

                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (className.ToString().Equals(CLASS_MENU))
                {
                    wndHandles.Add(h);
                }
                return true;
            }, 0);
            return wndHandles;
        }

        public static List<string> EXCLUDE_PAGE_TITLES = new List<string>() { "", "自定义分析周期", "已触发预警列表", "文华布告栏", "模型下单", "条件单选择加载对话框", "设置线型、颜色、粗细", "持仓匹配校验信息", "期货运行模组", "运行日志" };
        public static void clearOtherWindows(IntPtr mainHandle, List<IntPtr> excludeHandles)
        {
            int mainProcessId;
            GetWindowThreadProcessId(mainHandle, out mainProcessId);

            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                int processId;
                GetWindowThreadProcessId(h, out processId);

                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);
                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (mainProcessId == processId && className.ToString().Equals(CLASS_DIALOG) && !EXCLUDE_PAGE_TITLES.Contains(title.ToString()))
                {
                    //特殊界面特殊处理
                    if (isSupplyDataHintPage(title.ToString(), h))
                    {
                        closeSupplyDataHintPage(h);
                    }
                    else
                    {
                        wndHandles.Add(h);
                    }
                }

                return true;
            }, 0);
            bool hasOtherWindows = false;
            foreach (IntPtr handle in wndHandles)
            {
                if (excludeHandles != null && excludeHandles.Contains(handle))
                {
                    continue;
                }
                else
                {
                    hasOtherWindows = true;
                    closeWindow(handle);
                }
            }
            if (hasOtherWindows)
            {
                Thread.Sleep(500);
            }
        }

        public static void clearOtherWindowsByTitle(IntPtr mainHandle, List<string> excludeTitles)
        {
            int mainProcessId;
            GetWindowThreadProcessId(mainHandle, out mainProcessId);

            List<IntPtr> wndHandles = new List<IntPtr>();
            EnumWindows((h, l) =>
            {
                int processId;
                GetWindowThreadProcessId(h, out processId);

                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);
                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (mainProcessId == processId && className.ToString().Equals(CLASS_DIALOG) && !EXCLUDE_PAGE_TITLES.Contains(title.ToString()))
                {
                    if (excludeTitles != null && excludeTitles.Contains(title.ToString()))
                    {
                    }
                    else
                    {
                        if (isSupplyDataHintPage(title.ToString(), h))
                        {
                            closeSupplyDataHintPage(h);
                        }
                        else
                        {
                            wndHandles.Add(h);
                        }
                    }
                }

                return true;
            }, 0);
            if (wndHandles.Count > 0)
            {
                foreach (IntPtr handle in wndHandles)
                {
                    closeWindow(handle);
                }
                Thread.Sleep(500);
            }
        }

        //清理“提醒补充数据界面”，这个界面的关闭按钮无效，需要特殊处理
        private static void closeSupplyDataHintPage(IntPtr handle)
        {
            AutomationElement supplyDataHintPageWindow = AutomationElement.FromHandle(handle);
            if (supplyDataHintPageWindow == null)
            {
                return;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataHintPage.AUTOMATION_ID_BUTTON_NO);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
            AutomationElement buttonNo = supplyDataHintPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (buttonNo != null)
            {
                SimulateOperating.clickButton(buttonNo);
            }
        }

        private static bool isSupplyDataHintPage(string title, IntPtr handle)
        {
            if (!SupplyDataHintPage.SUPPLY_DATA_HINT_PAGE_TITLE.Equals(title))
            {
                return false;
            }
            AutomationElement supplyDataHintPageWindow = AutomationElement.FromHandle(handle);
            if (supplyDataHintPageWindow == null)
            {
                return false;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataHintPage.AUTOMATION_ID_LABEL_HINT);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
            AutomationElement labelHint = supplyDataHintPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (labelHint != null && labelHint.Current.Name.Contains(SupplyDataHintPage.HINT_MESSAGE))
            {
                return true;
            }
            return false;
        }

        public static bool hasWarningPageAndClose()
        {
            List<IntPtr> handles = findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, WarningPage.WARNING_PAGE_TITLE);
            if (handles != null && handles.Count >= 0)
            {
                bool has = false;
                foreach (IntPtr handle in handles)
                {
                    AutomationElement warningPageWindow = AutomationElement.FromHandle(handle);
                    if (warningPageWindow == null)
                    {
                        continue;
                    }
                    PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataHintPage.AUTOMATION_ID_LABEL_HINT);
                    PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                    AutomationElement labelMsg = warningPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                    if (labelMsg != null && !"".Equals(labelMsg.Current.Name))
                    {
                        has = true;
                        closeWindow(handle);
                    }
                }
                return has;
            }
            return false;
        }

        public static bool hasHintPageAndClose()
        {
            List<IntPtr> handles = findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, HintPage.HINT_PAGE_TITLE);
            if (handles != null && handles.Count >= 0)
            {
                bool has = false;
                foreach (IntPtr handle in handles)
                {
                    AutomationElement hintPageWindow = AutomationElement.FromHandle(handle);
                    if (hintPageWindow == null)
                    {
                        continue;
                    }
                    PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, HintPage.AUTOMATION_ID_LABEL_HINT);
                    PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                    AutomationElement labelMsg = hintPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                    if (labelMsg != null && !"".Equals(labelMsg.Current.Name))
                    {
                        has = true;
                        closeWindow(handle);
                    }
                }
                return has;
            }
            return false;
        }
    }
}
