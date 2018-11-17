using FuturesBackTestExportTool.Model;
using FuturesBackTestExportTool.Page;
using FuturesBackTestExportTool.ReportModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using static FuturesBackTestExportTool.WindowsApi;

namespace FuturesBackTestExportTool
{
    public partial class FormMain : Form
    {
        private const string CLASS_CLIENT = "Afx:0000000140000000";
        private IntPtr mainHandle;
        private int category;//0：期货，1：股票
        private List<Exchange> exchanges; //交易所对应的品种、合约
        private List<ModelCategory> modelCategories;//模型
        private List<TimeCycle> cycles;//周期
        private DateTime[] startingEndingDate;//起止时间

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonBackTest_Click(object sender, EventArgs e)
        {
            List<IntPtr> wndHandles = getClientHandles();
            if (wndHandles == null || wndHandles.Count == 0)
            {
                MessageBox.Show("未找到“赢智程序化”客户端");
                return;
            }

            FormChooseClient formChooseClient = new FormChooseClient(wndHandles);
            if (formChooseClient.ShowDialog() == DialogResult.OK)
            {
                mainHandle = formChooseClient.getResult();
                FormChooseCategory formChooseCategory = new FormChooseCategory();
                if (formChooseCategory.ShowDialog() == DialogResult.OK)
                {
                    category = formChooseCategory.getResult();
                    if (category == 0)
                    {
                        FormFuturesChooseVariety formFuturesChooseVariety = new FormFuturesChooseVariety(mainHandle);
                        if (formFuturesChooseVariety.ShowDialog() == DialogResult.OK)
                        {
                            exchanges = formFuturesChooseVariety.getResult();
                            FormChooseModel formChooseModel = new FormChooseModel(mainHandle);
                            if (formChooseModel.ShowDialog() == DialogResult.OK)
                            {
                                modelCategories = formChooseModel.getResult();
                                FormChooseCycle formChooseCycle = new FormChooseCycle();
                                if (formChooseCycle.ShowDialog() == DialogResult.OK)
                                {
                                    cycles = formChooseCycle.getResult();
                                    FormChooseTime formChooseTime = new FormChooseTime();
                                    if (formChooseTime.ShowDialog() == DialogResult.OK)
                                    {
                                        startingEndingDate = formChooseTime.getResult();
                                        // 开始回测
                                        while (this.dataGridViewResult.Rows.Count != 0)
                                        {
                                            this.dataGridViewResult.Rows.RemoveAt(0);
                                        }
                                        startBackTest();
                                    }
                                }
                            }
                        }
                    }
                    else if (category == 1)
                    {
                        MessageBox.Show("正在努力开发中...");
                    }
                }
            }
        }

        private void buttonOpenExcelDir_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Utils.getExportDir());
        }

        private List<IntPtr> getClientHandles()
        {
            List<IntPtr> wndHandles = new List<IntPtr>(); //所有"赢智程序化"窗口句柄
            EnumWindows((h, l) =>
            {
                StringBuilder className = new StringBuilder(200);
                GetClassName(h, className, 200);

                StringBuilder title = new StringBuilder(200);
                GetWindowText(h, title, 200);

                if (className.ToString().Contains(CLASS_CLIENT) && title.ToString().Contains("赢智程序化") && title.ToString().Contains("文华"))
                {
                    wndHandles.Add(h);
                }
                return true;
            }, 0);
            return wndHandles;
        }

        //回测流程
        private void startBackTest()
        {
            if (exchanges == null || exchanges.Count == 0)
            {
                MessageBox.Show("未设置品种参数，请重新设置回测参数");
                return;
            }
            foreach (Exchange exchange in exchanges)
            {
                string exchangeName = exchange.exchangeName;
                List<Variety> varieties = exchange.varieties;
                if (varieties == null && varieties.Count == 0)
                {
                    Console.WriteLine(exchangeName + "中的品种为空");
                    continue;
                }
                foreach (Variety variety in varieties)
                {
                    string varietyName = variety.varietyName;
                    List<int> agreements = variety.agreements;
                    if (agreements == null || agreements.Count == 0)
                    {
                        Console.WriteLine(exchangeName + "中" + varietyName + "中的合约为空");
                        continue;
                    }
                    foreach (int i in agreements)
                    {
                        if (!backTestSingleAgreement(exchangeName, varietyName, i))
                        {
                            return;
                        }
                    }
                }
            }
            frontMessageBox("导出完成");
        }

        //回测单个品种、合约
        private bool backTestSingleAgreement(string exchangeName, string varietyName, int agreement)
        {
            //1.打开“挑选要分析的股票，合约或品种”界面
            if (!toFuturesAnalysisPage())
            {
                //MessageBox.Show("打开“挑选要分析的股票，合约或品种”界面失败");
                return false;
            }
            //2.在“挑选要分析的股票，合约或品种”界面选择对应的品种、合约
            if (!chooseVarietyAgreement(exchangeName, varietyName, agreement))
            {
                return false;
            }
            //3.处理多个周期
            if (cycles == null || cycles.Count == 0)
            {
                frontMessageBox("未设置周期参数，请重新设置回测参数");
                return false;
            }
            //for report
            VarietyReport varietyReport = new VarietyReport();
            varietyReport.varietyName = varietyName;
            varietyReport.agreement = agreement;
            varietyReport.startingDate = startingEndingDate[0];
            varietyReport.endingDate = startingEndingDate[1];
            List<CycleReport> cycleReports = new List<CycleReport>();
            varietyReport.cycleReports = cycleReports;
            foreach (TimeCycle cycle in cycles)
            {
                if (!backTestSingleCycle(cycle))
                {
                    //MessageBox.Show("处理周期失败");
                    return false;
                }
                //for report
                CycleReport cycleReport = new CycleReport();
                cycleReport.cycleName = cycle.getName();
                List<ModelReport> modelReports = new List<ModelReport>();
                cycleReport.modelReports = modelReports;
                //4.选择模型
                if (modelCategories == null || modelCategories.Count == 0)
                {
                    frontMessageBox("未设置模型参数，请重新设置回测参数");
                    return false;
                }
                foreach (ModelCategory modelCategory in modelCategories)
                {
                    if (modelCategory != null && modelCategory.models != null && modelCategory.models.Count > 0)
                    {
                        List<string> models = modelCategory.models;
                        foreach (string model in models)
                        {
                            Console.WriteLine("模型:"+ model);
                            //试3次，容错（有时候获取不到报告，报告就一行提示信息）
                            int collectionReportCount = 0;
                            bool collectionReportSuccess = false;
                            while (collectionReportCount < 3)
                            {
                                collectionReportCount++;
                                if (!backTestSingleModel(modelCategory.modelCategoryName, model))
                                {
                                    //MessageBox.Show("处理模型失败");
                                    return false;
                                }
                                //5.选择起止时间
                                if (!chooseStartingEndingTime(startingEndingDate))
                                {
                                    //MessageBox.Show("处理起止时间失败");
                                    return false;
                                }
                                ModelReport modelReport = new ModelReport();
                                modelReport.modelName = model;
                                //6.点击“回测报告”按钮，跳转到回测页面，获取数据
                                if (!collectionReport(modelReport))
                                {
                                    continue;
                                }
                                collectionReportSuccess = true;
                                modelReports.Add(modelReport);
                                break;
                            }
                            if (!collectionReportSuccess)
                            {
                                frontMessageBox("获取回测报告失败");
                                return false;
                            }
                        }
                    }
                }
                cycleReports.Add(cycleReport);
            }
            new ExcelExport().exportExcel(varietyReport);
            return true;
        }

        //打开“挑选要分析的股票，合约或品种”界面
        private bool toFuturesAnalysisPage()
        {
            Thread.Sleep(500);
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow != null)
            {
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_HANGQINGSHOUYE);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AutomationElement buttonHangqing = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (buttonHangqing != null)
                {
                    SimulateOperating.clickButton(buttonHangqing);

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
                                    frontMessageBox("未找到主界面“K线”按钮");
                                    return false;
                                }
                            }
                            else
                            {
                                frontMessageBox("未找到主界面“数据”树形结构中的“我的篮子”子项");
                                return false;
                            }
                        }
                        else
                        {
                            frontMessageBox("未找到主界面“数据”树形结构的子项");
                            return false;
                        }
                    }
                    else
                    {
                        frontMessageBox("未找到主界面“数据”对应的树形结构");
                        return false;
                    }
                }
                else
                {
                    frontMessageBox("未找到主界面“行情首页”按钮");
                    return false;
                }
            }
            else
            {
                frontMessageBox("未找到“赢智程序化”主界面");
                return false;
            }
        }

        private bool backTestSingleCycle(TimeCycle cycle)
        {
            Thread.Sleep(500);
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow == null)
            {
                frontMessageBox("未找到“赢智程序化”主界面");
                return false;
            }
            if (cycle == null || cycle.unit == null)
            {
                frontMessageBox("设置周期参数无效，请重新设置回测参数");
                return false;
            }
            if (cycle.value == 0)
            {
                //选择的是日线、周线、月线、季线、年线
                string automationIdButtonCycle = null;
                switch (cycle.unit)
                {
                    case "日线":
                        automationIdButtonCycle = WH8MainPage.AUTOMATION_ID_BUTTON_RIXIAN;
                        break;
                    case "周线":
                        automationIdButtonCycle = WH8MainPage.AUTOMATION_ID_BUTTON_ZHOUXIAN;
                        break;
                    case "月线":
                        automationIdButtonCycle = WH8MainPage.AUTOMATION_ID_BUTTON_YUEXIAN;
                        break;
                    case "季线":
                        automationIdButtonCycle = WH8MainPage.AUTOMATION_ID_BUTTON_JIXIAN;
                        break;
                    case "年线":
                        automationIdButtonCycle = WH8MainPage.AUTOMATION_ID_BUTTON_NIANXIAN;
                        break;
                }
                if (automationIdButtonCycle == null)
                {
                    frontMessageBox("设置周期参数无效，请重新设置回测参数");
                    return false;
                }
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, automationIdButtonCycle);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AutomationElement buttonCycle = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (buttonCycle == null)
                {
                    frontMessageBox("未找到主界面“" + cycle.unit + "”按钮");
                    return false;
                }
                SimulateOperating.clickButton(buttonCycle);
                return true;
            }
            else
            {
                return chooseCycle(cycle);
            }
        }

        //选择模型
        private bool backTestSingleModel(string modelCategoryName, string model)
        {
            Thread.Sleep(500);
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow == null)
            {
                frontMessageBox("未找到“赢智程序化”主界面");
                return false;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_TREEVIEW_SHUJU);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
            AutomationElement treeViewModel = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (treeViewModel == null)
            {
                frontMessageBox("未找到主界面“模型”对应的树形结构");
                return false;
            }
            Condition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
            AutomationElementCollection modelCategoryElementCollection = treeViewModel.FindAll(TreeScope.Children, condition2);
            if (modelCategoryElementCollection == null || modelCategoryElementCollection.Count == 0)
            {
                frontMessageBox("未找到主界面“模型”树形结构的子项");
                return false;
            }
            bool foundModelCategory = false;
            foreach (AutomationElement modelCategoryAE in modelCategoryElementCollection)
            {
                if (modelCategoryName.Equals(modelCategoryAE.Current.Name))
                {
                    foundModelCategory = true;
                    SimulateOperating.expandTreeItem(modelCategoryAE);

                    Condition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection modelElementCollection = modelCategoryAE.FindAll(TreeScope.Children, condition3);
                    if (modelElementCollection == null && modelElementCollection.Count == 0)
                    {
                        frontMessageBox("未找到主界面“模型”树形结构中的“回测”子项");
                        return false;
                    }
                    foreach (AutomationElement modelAE in modelElementCollection)
                    {
                        if (model.Equals(modelAE.Current.Name))
                        {
                            SimulateOperating.doubleClick(modelAE);
                            return true;
                        }
                    }
                    frontMessageBox("未找到主界面“模型”树形结构子项回测中的“" + model + "”模型");
                    return false;
                }
            }
            if (!foundModelCategory)
            {
                frontMessageBox("未找到主界面“模型”树形结构中的“回测”子项");
                return false;
            }
            return false;
        }

        //选择品种、合约
        private bool chooseVarietyAgreement(string desExchangeName, string desVarietyName, int desAgreement)
        {
            List<IntPtr> wndHandles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, FuturesAnalysisPage.FUTURES_ANALYSIS_PAGE_TITLE);
            if (wndHandles.Count == 0)
            {
                frontMessageBox("打开“挑选要分析的股票，合约或品种”界面失败，未找到该界面");
                return false;
            }
            else if (wndHandles.Count > 1)
            {
                frontMessageBox("找到多个“挑选要分析的股票，合约或品种”界面，请关闭多余界面");
                return false;
            }
            IntPtr handle = wndHandles[0];

            AutomationElement targetWindow = AutomationElement.FromHandle(handle);
            if (targetWindow != null)
            {
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, FuturesAnalysisPage.AUTOMATION_ID_TREEVIEW_PRODUCT_CATEGORY);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
                AutomationElement treeviewExchange = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));

                if (treeviewExchange != null)
                {
                    Condition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection exchangeElementCollection = treeviewExchange.FindAll(TreeScope.Children, condition2);
                    if (exchangeElementCollection != null && exchangeElementCollection.Count > 0)
                    {
                        bool foundExchange = false;
                        foreach (AutomationElement exchangeAE in exchangeElementCollection)
                        {
                            string exchangeName = exchangeAE.Current.Name;
                            if (desExchangeName.Equals(exchangeName))
                            {
                                foundExchange = true;

                                if (SimulateOperating.expandTreeItem(exchangeAE))
                                {
                                    Condition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                                    AutomationElementCollection varietyElementCollection = exchangeAE.FindAll(TreeScope.Children, condition3);
                                    if (varietyElementCollection != null && varietyElementCollection.Count > 0)
                                    {
                                        bool foundVariety = false;
                                        foreach (AutomationElement varietyAE in varietyElementCollection)
                                        {
                                            string varietyName = varietyAE.Current.Name;
                                            if (desVarietyName.Equals(varietyName))
                                            {
                                                foundVariety = true;
                                                SimulateOperating.doubleClick(varietyAE);
                                                // 选择合约
                                                if (chooseAgreement(targetWindow, desAgreement))
                                                {
                                                    return true;
                                                }
                                                else
                                                {
                                                    frontMessageBox("在“挑选要分析的股票，合约或品种”界面，选择“" + desVarietyName + Variety.getAgreementName(desAgreement) + "”失败");
                                                    return false;
                                                }

                                            }
                                        }
                                        if (!foundVariety)
                                        {
                                            frontMessageBox("在“挑选要分析的股票，合约或品种”界面，未找到“" + desVarietyName + "”品种");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中子项“" + exchangeName + "”的子项");
                                        return false;
                                    }
                                }
                                else
                                {
                                    frontMessageBox("在“挑选要分析的股票，合约或品种”界面，展开“" + exchangeName + "”失败");
                                    return false;
                                }
                            }
                        }
                        if (!foundExchange)
                        {
                            frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中“" + desExchangeName + "”子项");
                            return false;
                        }
                    }
                    else
                    {
                        frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中“品种”树形结构的子项");
                        return false;
                    }
                }
                else
                {
                    frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中“品种”树形结构");
                    return false;
                }
            }
            else
            {
                frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面");
                return false;
            }
            //TODO 
            return false;
        }

        private bool chooseAgreement(AutomationElement targetWindow, int agreement)
        {
            string automationId = FuturesAnalysisPage.AUTOMATION_ID_BUTTON_ZHISHU;
            switch (agreement)
            {
                case 0:
                    //指数
                    automationId = FuturesAnalysisPage.AUTOMATION_ID_BUTTON_ZHISHU;
                    break;
                case 1:
                    //主力
                    automationId = FuturesAnalysisPage.AUTOMATION_ID_BUTTON_ZHULI;
                    break;
                case 2:
                    //连续
                    automationId = FuturesAnalysisPage.AUTOMATION_ID_BUTTON_LIANXU;
                    break;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, automationId);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox);
            AutomationElement checkboxAgreement = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (checkboxAgreement != null)
            {
                if (SimulateOperating.toggleCheckbox(checkboxAgreement))
                {
                    PropertyCondition condition2 = new PropertyCondition(AutomationElement.AutomationIdProperty, FuturesAnalysisPage.AUTOMATION_ID_LISTBOX_PRODUCT);
                    PropertyCondition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List);
                    AutomationElement listBoxAgreement = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition2, condition3));
                    if (listBoxAgreement != null)
                    {
                        Condition condition4 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem);
                        AutomationElementCollection automationElementCollection = listBoxAgreement.FindAll(TreeScope.Children, condition4);
                        if (automationElementCollection != null && automationElementCollection.Count > 0)
                        {
                            SimulateOperating.doubleClick(automationElementCollection[0]);
                            return true;
                        }
                        else
                        {
                            frontMessageBox("在“挑选要分析的股票，合约或品种”界面，合约列表为空");
                            return false;
                        }
                    }
                    else
                    {
                        frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中的合约列表");
                        return false;
                    }
                }
                else
                {
                    frontMessageBox("勾选“挑选要分析的股票，合约或品种”界面的复选框失败");
                    return false;
                }
            }
            else
            {
                frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中对应合约");
                return false;
            }
        }

        //打开自定义周期界面，设置周期，关闭
        private bool chooseCycle(TimeCycle cycle)
        {
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow == null)
            {
                frontMessageBox("未找到“赢智程序化”主界面");
                return false;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_ZIDINGYIZHOUQI);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
            AutomationElement buttonZidingyiZhouqi = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (buttonZidingyiZhouqi == null)
            {
                frontMessageBox("未找到主界面“自定义周期”按钮");
                return false;
            }
            if (!SimulateOperating.clickButton(buttonZidingyiZhouqi))
            {
                frontMessageBox("点击主界面“自定义周期”按钮失败");
                return false;
            }

            List<IntPtr> wndHandles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, CustomCyclePage.CUSTOM_CYCLE_PAGE_TITLE);
            if (wndHandles.Count == 0)
            {
                frontMessageBox("打开“自定义分析周期”界面失败，未找到该界面");
                return false;
            }
            else if (wndHandles.Count > 1)
            {
                frontMessageBox("找到多个“自定义分析周期”界面");
                //不存在这种情况
            }
            IntPtr handle = wndHandles[0];
            AutomationElement customCycleWindow = AutomationElement.FromHandle(handle);
            if (customCycleWindow == null)
            {
                frontMessageBox("未找到“自定义分析周期”界面");
                return false;
            }

            //尼玛神坑，通过AutomationElement找不到编辑框，只能遍历所有控件了。。。。
            List<IntPtr> editHandles = WindowsApiUtils.findControlHandlesByClassName(handle, "Edit");
            if (editHandles == null || editHandles.Count == 0)
            {
                frontMessageBox("未找到“自定义分析周期”界面中“时间”编辑框");
                return false;
            }
            AutomationElement editCycle = AutomationElement.FromHandle(editHandles[0]);
            if (editCycle == null)
            {
                frontMessageBox("未找到“自定义分析周期”界面中“时间”编辑框");
                return false;
            }
            if (!SimulateOperating.setEditValue(editCycle, cycle.value.ToString()))
            {
                frontMessageBox("在“自定义分析周期”界面，设置周期失败");
                return false;
            }

            PropertyCondition condition4 = new PropertyCondition(AutomationElement.AutomationIdProperty, CustomCyclePage.AUTOMATION_ID_COMBO_BOX_UNIT);
            PropertyCondition condition5 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ComboBox);
            AutomationElement comboBoxUnit = customCycleWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition4, condition5));
            if (comboBoxUnit == null)
            {
                frontMessageBox("未找到“自定义分析周期”界面中“周期单位选择”下拉框");
                return false;
            }
            PropertyCondition condition6 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem);
            AutomationElementCollection unitElementCollection = comboBoxUnit.FindAll(TreeScope.Subtree, condition6);
            if (unitElementCollection == null || unitElementCollection.Count == 0)
            {
                frontMessageBox("未找到“自定义分析周期”界面中“周期单位选择”下拉框的子项");
                return false;
            }
            bool foundUnit = false;
            foreach (AutomationElement unitAE in unitElementCollection)
            {
                if (cycle.unit.Equals(unitAE.Current.Name))
                {
                    foundUnit = true;
                    if (!SimulateOperating.selectComboBoxItem(unitAE))
                    {
                        frontMessageBox("在“自定义分析周期”界面，选择时间单位失败");
                        return false;
                    }
                }
            }
            if (!foundUnit)
            {
                frontMessageBox("未找到“自定义分析周期”界面中对应的单位");
                return false;
            }
            PropertyCondition condition7 = new PropertyCondition(AutomationElement.AutomationIdProperty, CustomCyclePage.AUTOMATION_ID_BUTTON_USE);
            PropertyCondition condition8 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
            AutomationElement buttonUse = customCycleWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition7, condition8));
            if (buttonUse == null)
            {
                frontMessageBox("未找到“自定义分析周期”界面中的“应用”按钮");
                return false;
            }
            if (!SimulateOperating.clickButton(buttonUse))
            {
                frontMessageBox("点击“自定义分析周期”界面中的“应用”按钮失败");
                return false;
            }
            //
            //MessageBox.Show("查看设置");
            WindowsApiUtils.closeWindow(handle);
            return true;
        }

        private bool chooseStartingEndingTime(DateTime[] startingEndingTime)
        {
            Thread.Sleep(500);
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow != null)
            {
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_QIZHISHIJIAN);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AutomationElement buttonQizhiShijian = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (buttonQizhiShijian != null)
                {
                    SimulateOperating.clickButton(buttonQizhiShijian);
                    return setupStartingEndingTime(startingEndingTime);
                }
                else
                {
                    frontMessageBox("未找到主界面“设定信号计算起止时间”按钮");
                    return false;
                }
            }
            else
            {
                frontMessageBox("未找到“赢智程序化”主界面");
                return false;
            }
        }

        //对“设定信号计算起止时间”界面的操作
        private bool setupStartingEndingTime(DateTime[] startingEndingTime)
        {
            if (startingEndingTime == null && startingEndingTime.Length < 2)
            {
                frontMessageBox("“信号计算起止时间”参数设置有误，请重新设置回测参数");
                return false;
            }

            List<IntPtr> wndHandles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, StartingEndingTimePage.STARTING_ENDING_TIME_PAGE_TITLE);
            if (wndHandles.Count == 0)
            {
                frontMessageBox("打开“设定信号计算起止时间”界面失败，未找到该界面");
                return false;
            }
            else if (wndHandles.Count > 1)
            {
                frontMessageBox("找到多个“设定信号计算起止时间”界面，请关闭多余的界面");
                return false;
            }
            IntPtr handle = wndHandles[0];
            AutomationElement targetWindow = AutomationElement.FromHandle(handle);
            if (targetWindow != null)
            {
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, StartingEndingTimePage.AUTOMATION_ID_BUTTON_OK);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AutomationElement buttonOK = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (buttonOK != null)
                {
                    DateTime startingTime = startingEndingTime[0];
                    DateTime endingTime = startingEndingTime[1];

                    string startingYear = Utils.zeroize(startingTime.Year, 4);
                    string startingMonth = Utils.zeroize(startingTime.Month, 2);
                    string startingDay = Utils.zeroize(startingTime.Day, 2);

                    string endingYear = Utils.zeroize(endingTime.Year, 4);
                    string endingMonth = Utils.zeroize(endingTime.Month, 2);
                    string endingDay = Utils.zeroize(endingTime.Day, 2);

                    //UIAutomation对SysDateTimePick32控件不支持，只能模拟键盘操作，前提是窗口获取焦点
                    buttonOK.SetFocus();
                    SendKeys.SendWait("{TAB}");
                    SendKeys.SendWait("{TAB}");
                    SendKeys.SendWait(startingYear);
                    SendKeys.SendWait("{RIGHT}");
                    SendKeys.SendWait(startingMonth);
                    SendKeys.SendWait("{RIGHT}");
                    SendKeys.SendWait(startingDay);

                    SendKeys.SendWait("{TAB}");
                    SendKeys.SendWait(endingYear);
                    SendKeys.SendWait("{RIGHT}");
                    SendKeys.SendWait(endingMonth);
                    SendKeys.SendWait("{RIGHT}");
                    SendKeys.SendWait(endingDay);
                    //检查一下填入的时间
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{TAB}");
                    SendKeys.SendWait("{TAB}");
                    SendKeys.SendWait("{TAB}");
                    Thread.Sleep(1000);

                    PropertyCondition condition2 = new PropertyCondition(AutomationElement.AutomationIdProperty, StartingEndingTimePage.AUTOMATION_ID_DATE_PICK_STARTING);
                    PropertyCondition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);
                    AutomationElement datePickStarting = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition2, condition3));
                    if (datePickStarting == null)
                    {
                        frontMessageBox("未找到“设定信号计算起止时间”界面的开始时间选择器");
                        return false;
                    }
                    //SimulateOperating.clickAutomationElement(datePickStarting);
                    //Thread.Sleep(500);

                    PropertyCondition condition4 = new PropertyCondition(AutomationElement.AutomationIdProperty, StartingEndingTimePage.AUTOMATION_ID_DATE_PICK_ENDING);
                    PropertyCondition condition5 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);
                    AutomationElement datePickEnding = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition4, condition5));
                    if (datePickEnding == null)
                    {
                        frontMessageBox("未找到“设定信号计算起止时间”界面的结束时间选择器");
                        return false;
                    }

                    string expectedStartingDate = startingTime.Year + "/" + startingTime.Month + "/" + startingTime.Day;
                    string expectedEndingDate = endingTime.Year + "/" + endingTime.Month + "/" + endingTime.Day;
                    Console.WriteLine(expectedStartingDate + "-" + expectedEndingDate);

                    Console.WriteLine("获取：" + datePickStarting.Current.Name + "," + datePickEnding.Current.Name);

                    if (!expectedStartingDate.Equals(datePickStarting.Current.Name) || !expectedEndingDate.Equals(datePickEnding.Current.Name))
                    {
                        frontMessageBox("在“设定信号计算起止时间”界面填写起止时间有误，请检查是否需要补充数据");
                        return false;
                    }

                    SimulateOperating.clickButton(buttonOK);
                    return true;
                }
                else
                {
                    frontMessageBox("未找到“设定信号计算起止时间”界面中的“确定”按钮");
                    return false;
                }
            }
            else
            {
                frontMessageBox("未找到“设定信号计算起止时间”界面");
                return false;
            }
        }

        private bool collectionReport(ModelReport modelReport)
        {
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow == null)
            {
                frontMessageBox("未找到“赢智程序化”主界面");
                return false;
            }
            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_HUICEBAOGAO);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
            AutomationElement buttonHuiceBaogao = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (buttonHuiceBaogao == null)
            {
                frontMessageBox("未找到主界面“回测报告”按钮");
                return false;
            }

            int count = 0;
            while (count < 5)
            {
                count++;
                Console.WriteLine("count:" + count);
                Thread.Sleep(3000);
                List<IntPtr> promptPageHandles = WindowsApiUtils.findWindowHandlesByClassTitleExact(CLASS_DIALOG, "提示");
                if (promptPageHandles != null && promptPageHandles.Count > 0)
                {
                    foreach (IntPtr promptPageHandle in promptPageHandles)
                    {
                        WindowsApiUtils.closeWindow(promptPageHandle);
                    }
                }

                if (!SimulateOperating.clickButton(buttonHuiceBaogao))
                {
                    frontMessageBox("点击主界面“回测报告”按钮失败");
                    return false;
                }

                //判断是否存在正在计算界面，如果有，则关闭
                List<IntPtr> beComputingPageHandles = WindowsApiUtils.findWindowHandlesByClassTitleExact(CLASS_DIALOG, BeComputingPage.BE_COMPUTING_PAGE_TITLE);
                if (beComputingPageHandles != null && beComputingPageHandles.Count > 0)
                {
                    foreach (IntPtr beComputingPageHandle in beComputingPageHandles)
                    {
                        WindowsApiUtils.closeWindow(beComputingPageHandle);
                    }
                }

                List<IntPtr> backTestReportPageHandles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, BackTestReportPage.BE_COMPUTING_PAGE_TITLE);
                if (backTestReportPageHandles != null && backTestReportPageHandles.Count > 0)
                {
                    if (backTestReportPageHandles.Count > 1)
                    {
                        frontMessageBox("发现多个“模型回测报告”界面，请关闭多余界面");
                        return false;
                    }
                    IntPtr backTestReportPageHandle = backTestReportPageHandles[0];
                    // 开始获取数据
                    AutomationElement backTestReportPageWindow = AutomationElement.FromHandle(backTestReportPageHandle);
                    if (backTestReportPageWindow == null)
                    {
                        frontMessageBox("未找到“模型回测报告”界面");
                        WindowsApiUtils.closeWindow(backTestReportPageHandle);
                        return false;
                    }

                    List<List<string>> result = null;
                    int countGetDataGrid = 0;
                    while (countGetDataGrid < 10)
                    {
                        countGetDataGrid++;
                        Console.WriteLine("countGetDataGrid:"+countGetDataGrid);
                        Thread.Sleep(3000);
                        PropertyCondition condition2 = new PropertyCondition(AutomationElement.AutomationIdProperty, BackTestReportPage.AUTOMATION_ID_DATA_GRID_REPORT);
                        PropertyCondition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.DataGrid);
                        AutomationElement dataGrid = backTestReportPageWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition2, condition3));
                        if (dataGrid == null)
                        {
                            frontMessageBox("未找到“模型回测报告”界面中的报告列表");
                            WindowsApiUtils.closeWindow(backTestReportPageHandle);
                            return false;
                        }
                        result = Utils.getDataFromListView(dataGrid);
                        if (result != null && result.Count != 0)
                        {
                            break;
                        }
                    }
                    WindowsApiUtils.closeWindow(backTestReportPageHandle);
                    if (result == null || result.Count < 10)
                    {
                        continue;
                    }
                    Console.WriteLine("有数据，result:"+result.Count);
                    fillDataGridView(result);
                    fillModelReport(result, modelReport);
                    return true;
                }
            }
            return false;
        }

        private void fillDataGridView(List<List<string>> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                List<string> rowList = data[i];
                int row = this.dataGridViewResult.Rows.Add();
                this.dataGridViewResult.Rows[row].Cells[0].Value = i + 1;
                if (i == 0)
                {
                    this.dataGridViewResult.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                }
                for (int j = 0; j < rowList.Count; j++)
                {
                    this.dataGridViewResult.Rows[row].Cells[j + 1].Value = rowList[j];
                }
            }
        }

        private void fillModelReport(List<List<string>> data, ModelReport modelReport)
        {
            if (data != null)
            {
                foreach (List<string> rowData in data)
                {
                    string title = rowData[0];
                    if (title != null)
                    {
                        switch (title)
                        {
                            case "信号个数":
                                modelReport.signalNumber = rowData[1];
                                break;
                            case "最终权益":
                                modelReport.lastInterest = rowData[1];
                                break;
                            case "夏普比率":
                                modelReport.sharpeRatio = rowData[1];
                                break;
                            case "权益最大回撤":
                                modelReport.interestMaxRetracement = rowData[1];
                                break;
                            case "权益最大回撤比":
                                modelReport.interestMaxRetracementRatio = rowData[1];
                                break;
                            case "风险率":
                                modelReport.hazardRatio = rowData[1];
                                break;
                            case "每手最大亏损":
                                modelReport.maxLossPerHand = rowData[1];
                                break;
                            case "每手平均盈亏":
                                modelReport.avgProfitLossPerHand = rowData[1];
                                break;
                            case "胜率":
                                modelReport.winRatio = rowData[1];
                                break;
                            case "模型得分":
                                modelReport.score = rowData[1];
                                break;
                            case "最大盈利":
                                modelReport.maxProfit = rowData[1];
                                break;
                            case "最大亏损":
                                modelReport.maxLoss = rowData[1];
                                break;
                            case "最大持续盈利次数":
                                modelReport.maxContinuousProfitabilityTimes = rowData[1];
                                break;
                            case "最大持续亏损次数":
                                modelReport.maxContinuousLossesTimes = rowData[1];
                                break;
                        }
                    }
                }
            }
        }


        private void frontMessageBox(string message)
        {
            this.TopMost = true;
            MessageBox.Show(message);
            this.TopMost = false;
        }


        //////////////////////////////////////////////////////////////////////////
        //补充数据
        //private IntPtr supplyDataMainHandle;
        private List<Exchange> supplyDataExchanges; //交易所对应的品种、合约
        private List<string> supplyDataCycles;
        private DateTime supplyDataFromDate;

        private void buttonSupplyData_Click(object sender, EventArgs e)
        {
            List<IntPtr> wndHandles = getClientHandles();
            if (wndHandles == null || wndHandles.Count == 0)
            {
                MessageBox.Show("未找到“赢智程序化”客户端");
                return;
            }

            FormChooseClient formChooseClient = new FormChooseClient(wndHandles);
            if (formChooseClient.ShowDialog() == DialogResult.OK)
            {
                mainHandle = formChooseClient.getResult();
                FormFuturesChooseVariety formFuturesChooseVariety = new FormFuturesChooseVariety(mainHandle);
                if (formFuturesChooseVariety.ShowDialog() == DialogResult.OK)
                {
                    supplyDataExchanges = formFuturesChooseVariety.getResult();
                    FormChooseSupplyDataParams formChooseSupplyDataParams = new FormChooseSupplyDataParams();
                    if (formChooseSupplyDataParams.ShowDialog() == DialogResult.OK)
                    {
                        object[] supplyDataParams = formChooseSupplyDataParams.getResult();
                        supplyDataCycles = (List<string>)supplyDataParams[0];
                        supplyDataFromDate = (DateTime)supplyDataParams[1];

                        startSupplyData();
                    }
                }
            }
        }

        private void startSupplyData()
        {
            if (supplyDataExchanges == null || supplyDataExchanges.Count == 0)
            {
                MessageBox.Show("未设置品种参数，请重新设置补充数据参数");
                return;
            }
            if (supplyDataCycles == null || supplyDataCycles.Count == 0)
            {
                MessageBox.Show("未设置周期参数，请重新设置补充数据参数");
                return;
            }
            if (supplyDataFromDate == null)
            {
                MessageBox.Show("未设置时间参数，请重新设置补充数据参数");
                return;
            }

            foreach (Exchange exchange in supplyDataExchanges)
            {
                string exchangeName = exchange.exchangeName;
                List<Variety> varieties = exchange.varieties;
                if (varieties == null && varieties.Count == 0)
                {
                    Console.WriteLine(exchangeName + "中的品种为空");
                    continue;
                }
                foreach (Variety variety in varieties)
                {
                    string varietyName = variety.varietyName;
                    List<int> agreements = variety.agreements;
                    if (agreements == null || agreements.Count == 0)
                    {
                        Console.WriteLine(exchangeName + "中" + varietyName + "中的合约为空");
                        continue;
                    }
                    foreach (int i in agreements)
                    {
                        if (!supplyDataSingleAgreement(exchangeName, varietyName, i))
                        {
                            return;
                        }
                    }
                }
            }
            frontMessageBox("补充历史数据完成");
        }

        private bool supplyDataSingleAgreement(string exchangeName, string varietyName, int agreement)
        {
            //1.打开“挑选要分析的股票，合约或品种”界面
            if (!toFuturesAnalysisPage())
            {
                return false;
            }
            //2.在“挑选要分析的股票，合约或品种”界面选择对应的品种、合约
            if (!chooseVarietyAgreement(exchangeName, varietyName, agreement))
            {
                return false;
            }

            if (!supplyDataSingleAgreementInner(supplyDataCycles, supplyDataFromDate))
            {
                return false;
            }
            return true;
        }

        private bool supplyDataSingleAgreementInner(List<string> applyDataCycles, DateTime targetDate)
        {
            if (applyDataCycles == null || applyDataCycles.Count == 0)
            {
                frontMessageBox("未设置周期参数，请重新设置补充数据参数");
                return false;
            }
            foreach (string applyDataCycle in applyDataCycles)
            {
                Thread.Sleep(500);
                AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
                if (targetWindow == null)
                {
                    frontMessageBox("未找到“赢智程序化”主界面");
                    return false;
                }
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_PANE_MAIN);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);
                AutomationElement paneMain = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (paneMain == null)
                {
                    frontMessageBox("未找到主界面“K线图”控件");
                    return false;
                }
                targetWindow.SetFocus();
                SimulateOperating.rightClickAutomationElement(paneMain);
                Thread.Sleep(1000);

                List<IntPtr> menuHandles = WindowsApiUtils.findContextMenuHandles();
                if (menuHandles == null || menuHandles.Count == 0)
                {
                    frontMessageBox("未找到主界面上下文菜单");
                    return false;
                }
                IntPtr menuHandle = menuHandles[0];
                AutomationElement menuAE = AutomationElement.FromHandle(menuHandle);
                if (menuAE == null)
                {
                    frontMessageBox("未找到主界面上下文菜单");
                    return false;
                }
                Condition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem);
                AutomationElementCollection menuItemElementCollection = menuAE.FindAll(TreeScope.Children, condition2);
                if (menuItemElementCollection == null || menuItemElementCollection.Count == 0)
                {
                    frontMessageBox("未找到主界面上下文菜单子项");
                    return false;
                }
                bool foundSupplyDataMenuItem = false;
                foreach (AutomationElement menuItemAE in menuItemElementCollection)
                {
                    if ("补充历史数据".Equals(menuItemAE.Current.Name))
                    {
                        foundSupplyDataMenuItem = true;
                        if (!SimulateOperating.clickButton(menuItemAE))
                        {
                            frontMessageBox("点击主界面上下文菜单中的“补充历史数据”子项失败");
                            return false;
                        }
                        break;
                    }
                }
                if (!foundSupplyDataMenuItem)
                {
                    frontMessageBox("未找到主界面上下文菜单中的“补充历史数据”子项");
                    return false;
                }
                Thread.Sleep(1000);
                List<IntPtr> supplyDataPageHandles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, SupplyDataPage.SUPPLY_DATA_PAGE_TITLE);
                if (supplyDataPageHandles == null || supplyDataPageHandles.Count == 0)
                {
                    frontMessageBox("未找到“补数据”界面");
                    return false;
                }
                IntPtr supplyDataPageHandle = supplyDataPageHandles[0];
                AutomationElement supplyDataPageAE = AutomationElement.FromHandle(supplyDataPageHandle);
                if (supplyDataPageAE == null)
                {
                    frontMessageBox("未找到“补数据”界面");
                    return false;
                }
                PropertyCondition condition3 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataPage.AUTOMATION_ID_COMBO_BOX_CYCLE);
                PropertyCondition condition4 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ComboBox);
                AutomationElement comboBoxCycle = supplyDataPageAE.FindFirst(TreeScope.Descendants, new AndCondition(condition3, condition4));
                if (comboBoxCycle == null)
                {
                    frontMessageBox("未找到“补数据”界面中“周期选择”下拉框");
                    return false;
                }
                PropertyCondition condition5 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem);
                AutomationElementCollection cycleElementCollection = comboBoxCycle.FindAll(TreeScope.Subtree, condition5);
                if (cycleElementCollection == null || cycleElementCollection.Count == 0)
                {
                    frontMessageBox("未找到“补数据”界面中“周期选择”下拉框的子项");
                    return false;
                }
                bool foundCycle = false;
                foreach (AutomationElement cycleAE in cycleElementCollection)
                {
                    if (applyDataCycle.Equals(cycleAE.Current.Name))
                    {
                        foundCycle = true;
                        if (!SimulateOperating.selectComboBoxItem(cycleAE))
                        {
                            frontMessageBox("在“补数据”界面，选择周期失败");
                            return false;
                        }
                    }
                }
                if (!foundCycle)
                {
                    frontMessageBox("未找到“补数据”界面中对应的周期");
                    return false;
                }

                PropertyCondition condition6 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataPage.AUTOMATION_ID_CHECKBOX_DOWNLOAD_ALL_DATA);
                PropertyCondition condition7 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox);
                AutomationElement checkboxDownloadAllData = supplyDataPageAE.FindFirst(TreeScope.Descendants, new AndCondition(condition6, condition7));
                if (checkboxDownloadAllData == null)
                {
                    frontMessageBox("未找到“补数据”界面中“全部历史数据”复选框");
                    return false;
                }

                if (!SimulateOperating.toggleCheckbox(checkboxDownloadAllData))
                {
                    frontMessageBox("勾选“补数据”界面中“全部历史数据”复选框失败");
                    return false;
                }

                PropertyCondition condition8 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataPage.AUTOMATION_ID_BUTTON_DOWNLOAD);
                PropertyCondition condition9 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AutomationElement buttonDownload = supplyDataPageAE.FindFirst(TreeScope.Descendants, new AndCondition(condition8, condition9));
                if (buttonDownload == null)
                {
                    frontMessageBox("未找到“补数据”界面中“下载”按钮");
                    return false;
                }
                PropertyCondition condition10 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataPage.AUTOMATION_ID_DATE_PICK_DOWNLOAD);
                PropertyCondition condition11 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);
                AutomationElement datePickDownload = supplyDataPageAE.FindFirst(TreeScope.Descendants, new AndCondition(condition10, condition11));
                if (datePickDownload == null)
                {
                    frontMessageBox("未找到“补数据”界面中“日期”控件");
                    return false;
                }
                string downloadDate = datePickDownload.Current.Name;
                if (Utils.compareDownloadDate(downloadDate, targetDate, applyDataCycle))
                {
                    WindowsApiUtils.closeWindow(supplyDataPageHandle);
                    continue;
                }
                //while(true)容错
                while (true)
                {
                    if (!SimulateOperating.clickButton(buttonDownload))
                    {
                        frontMessageBox("点击“补数据”界面中“下载”按钮失败");
                        return false;
                    }

                    bool downloadSuccess = false;
                    while (true)
                    {
                        Thread.Sleep(3000);
                        //TODO clear other window
                        PropertyCondition condition12 = new PropertyCondition(AutomationElement.AutomationIdProperty, SupplyDataPage.AUTOMATION_ID_LABEL_MESSAGE);
                        PropertyCondition condition13 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                        AutomationElement lableMessage = supplyDataPageAE.FindFirst(TreeScope.Descendants, new AndCondition(condition12, condition13));

                        downloadDate = datePickDownload.Current.Name;
                        Console.WriteLine(downloadDate);
                        if ((lableMessage != null && lableMessage.Current.Name.Contains("下载完成")) || Utils.compareDownloadDate(downloadDate, targetDate, applyDataCycle))
                        {
                            Thread.Sleep(1000);
                            downloadSuccess = true;
                            break;
                        }
                    }
                    if (downloadSuccess)
                    {
                        WindowsApiUtils.closeWindow(supplyDataPageHandle);
                        break;
                    }
                }
            }
            return true;
        }
    }
}
