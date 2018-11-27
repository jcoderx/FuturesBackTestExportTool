using System;
using System.Windows;
using System.Windows.Automation;
using static FuturesBackTestExportTool.WindowsApi;

namespace FuturesBackTestExportTool
{
    //模拟操作
    public class SimulateOperating
    {
        public static bool clickButton(AutomationElement ae)
        {
            object temp;
            if (ae.TryGetCurrentPattern(InvokePattern.Pattern, out temp))
            {
                try
                {
                    InvokePattern pattern = temp as InvokePattern;
                    pattern.Invoke();
                    return true;
                }catch(Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public static bool toggleCheckbox(AutomationElement ae)
        {
            object temp;
            if (ae.TryGetCurrentPattern(TogglePattern.Pattern, out temp))
            {
                TogglePattern pattern = temp as TogglePattern;
                pattern.Toggle();
                return true;
            }
            return false;
        }

        public static void doubleClick(AutomationElement ae)
        {
            Rect rect = ae.Current.BoundingRectangle;
            int incrementX = (int)(rect.Left + rect.Width / 2);
            int incrementY = (int)(rect.Top + rect.Height / 2);
            SetCursorPos(incrementX, incrementY);
            SimulateMouseOperating.DoubleClickLeftMouse(incrementX, incrementY);
        }

        public static bool expandTreeItem(AutomationElement ae)
        {
            object temp;
            if (ae.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out temp))
            {
                try
                {
                    ExpandCollapsePattern pattern = temp as ExpandCollapsePattern;
                    pattern.Expand();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public static bool selectTreeItem(AutomationElement ae)
        {
            object temp;
            if (ae.TryGetCurrentPattern(SelectionItemPattern.Pattern, out temp))
            {
                SelectionItemPattern pattern = temp as SelectionItemPattern;
                pattern.Select();
                return true;
            }
            return false;
        }

        public static bool isItemSelected(AutomationElement ae)
        {
            object temp;
            if (ae.TryGetCurrentPattern(SelectionItemPattern.Pattern, out temp))
            {
                SelectionItemPattern pattern = temp as SelectionItemPattern;
                return pattern.Current.IsSelected;
            }
            return false;
        }

        public static bool expandComboBoxItem(AutomationElement ae)
        {
            object temp;
            if (ae.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out temp))
            {
                ExpandCollapsePattern pattern = temp as ExpandCollapsePattern;
                pattern.Expand();
                return true;
            }
            return false;
        }

        public static bool setEditValue(AutomationElement ae, string value)
        {
            object temp;
            if (ae.TryGetCurrentPattern(ValuePattern.Pattern, out temp))
            {
                ValuePattern pattern = temp as ValuePattern;
                pattern.SetValue(value);
                return true;
            }
            return false;
        }

        public static bool selectComboBoxItem(AutomationElement ae)
        {
            object temp;
            if (ae.TryGetCurrentPattern(SelectionItemPattern.Pattern, out temp))
            {
                SelectionItemPattern pattern = temp as SelectionItemPattern;
                pattern.Select();
                return true;
            }
            return false;
        }

        public static void clickAutomationElement(AutomationElement ae)
        {
            Rect rect = ae.Current.BoundingRectangle;
            int incrementX = (int)(rect.Left + rect.Width / 2);
            int incrementY = (int)(rect.Top + rect.Height / 2);
            SetCursorPos(incrementX, incrementY);
            SimulateMouseOperating.ClickLeftMouse(incrementX, incrementY);
        }

        public static void rightClickAutomationElement(AutomationElement ae)
        {
            Rect rect = ae.Current.BoundingRectangle;
            int incrementX = (int)(rect.Left + rect.Width / 2);
            int incrementY = (int)(rect.Top + rect.Height / 2);
            SetCursorPos(incrementX, incrementY);
            SimulateMouseOperating.ClickRightMouse(incrementX, incrementY);
        }
    }
}
