using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
