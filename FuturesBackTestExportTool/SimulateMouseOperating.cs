using System;
using static FuturesBackTestExportTool.WindowsApi;

namespace FuturesBackTestExportTool
{
    //模拟鼠标操作
    public class SimulateMouseOperating
    {
        // 根据XY轴坐标点击左键 
        public static void ClickLeftMouse(int incrementX, int incrementY)
        {
            SetCursorPos(incrementX, incrementY);
            mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, incrementX, incrementY, 0, UIntPtr.Zero);
        }

        // 根据XY轴坐标双击左键 
        public static void DoubleClickLeftMouse(int incrementX, int incrementY)
        {
            SetCursorPos(incrementX, incrementY);
            mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, incrementX, incrementY, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, incrementX, incrementY, 0, UIntPtr.Zero);
        }

        // 根据XY轴坐标点击右键 
        public static void ClickRightMouse(int incrementX, int incrementY)
        {
            SetCursorPos(incrementX, incrementY);
            mouse_event(MouseEventFlag.RightDown | MouseEventFlag.RightUp, incrementX, incrementY, 0, UIntPtr.Zero);
        }

        // 根据XY轴坐标双击右键 
        public static void DoubleClickRightMouse(int incrementX, int incrementY)
        {
            SetCursorPos(incrementX, incrementY); mouse_event(MouseEventFlag.RightDown | MouseEventFlag.RightUp, incrementX, incrementY, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.RightDown | MouseEventFlag.RightUp, incrementX, incrementY, 0, UIntPtr.Zero);
        }
    }
}
