namespace FuturesBackTestExportTool
{
    public enum MouseEventFlag : uint
    {
        Move = 0x0001, //移动鼠标 
        LeftDown = 0x0002, //模仿鼠标左键按下 
        LeftUp = 0x0004, //模仿鼠标左键抬起 
        RightDown = 0x0008, //模仿鼠标右键按下 
        RightUp = 0x0010, //模仿鼠标右键抬起  
        MiddleDown = 0x0020, // 模仿鼠标中键抬起 
        MiddleUp = 0x0040, // 模仿鼠标中键抬起 
        XDown = 0x0080,
        XUp = 0x0100,
        Wheel = 0x0800,
        VirtualDesk = 0x4000,
        Absolute = 0x8000 //标示是否采取绝对坐标 
    }
}
