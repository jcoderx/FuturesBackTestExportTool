using FuturesBackTestExportTool.Model;
using FuturesBackTestExportTool.Page;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using static FuturesBackTestExportTool.WindowsApi;

namespace FuturesBackTestExportTool
{
    public partial class FormFuturesChooseVariety : Form
    {
        private List<Exchange> exchanges;
        private IntPtr mainHandle;

        public FormFuturesChooseVariety(IntPtr mainHandle)
        {
            this.mainHandle = mainHandle;
            InitializeComponent();
        }

        private void FormFuturesChooseVariety_Load(object sender, EventArgs e)
        {
            if (toFuturesAnalysisPage())
            {
                List<Exchange> exchanges = getAllExchanges();
                if (exchanges != null)
                {
                    fillTreeView(exchanges);
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        //打开“挑选要分析的股票，合约或品种”界面
        private bool toFuturesAnalysisPage()
        {
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow != null)
            {
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_HANGQINGSHOUYE);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AutomationElement buttonHangqing = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
                if (buttonHangqing != null)
                {
                    SimulateOperating.clickButton(buttonHangqing);
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

        //获取所有交易所的品种、合约
        private List<Exchange> getAllExchanges()
        {
            List<IntPtr> wndHandles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, FuturesAnalysisPage.FUTURES_ANALYSIS_PAGE_TITLE);
            if (wndHandles.Count == 0)
            {
                frontMessageBox("打开“挑选要分析的股票，合约或品种”界面失败，未找到该界面");
                return null;
            }
            else if (wndHandles.Count > 1)
            {
                frontMessageBox("找到多个“挑选要分析的股票，合约或品种”界面，请关闭多余界面");
                return null;
            }
            IntPtr handle = wndHandles[0];

            AutomationElement targetWindow = AutomationElement.FromHandle(handle);
            if (targetWindow != null)
            {
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, FuturesAnalysisPage.AUTOMATION_ID_TREEVIEW_PRODUCT_CATEGORY);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
                AutomationElement treeviewProduct = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));

                if (treeviewProduct != null)
                {
                    List<Exchange> exchanges = new List<Exchange>();

                    Condition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection automationElementCollection = treeviewProduct.FindAll(TreeScope.Children, condition2);
                    if (automationElementCollection != null && automationElementCollection.Count > 0)
                    {
                        foreach (AutomationElement ae in automationElementCollection)
                        {
                            string exchangeName = ae.Current.Name;
                            if (FuturesAnalysisPage.isDomesticFutures(exchangeName))
                            {
                                if (SimulateOperating.expandTreeItem(ae))
                                {
                                    Exchange exchange = new Exchange();
                                    exchange.exchangeName = exchangeName;
                                    List<Variety> varieties = new List<Variety>();
                                    exchange.varieties = varieties;
                                    exchanges.Add(exchange);

                                    Condition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                                    AutomationElementCollection varietyElementCollection = ae.FindAll(TreeScope.Children, condition3);
                                    if (varietyElementCollection != null && varietyElementCollection.Count > 0)
                                    {
                                        foreach (AutomationElement varietyAE in varietyElementCollection)
                                        {
                                            Variety variety = new Variety();
                                            variety.varietyName = varietyAE.Current.Name;
                                            varieties.Add(variety);
                                        }
                                    }
                                }
                                else
                                {
                                    frontMessageBox("展开“挑选要分析的股票，合约或品种”界面中的“" + ae.Current.Name + "”子项失败");
                                    WindowsApiUtils.closeWindow(handle);
                                    return null;
                                }
                            }
                        }
                        WindowsApiUtils.closeWindow(handle);
                        return exchanges;
                    }
                    else
                    {
                        frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中的“品种”树形结构的子项");
                        WindowsApiUtils.closeWindow(handle);
                        return null;
                    }
                }
                else
                {
                    frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面中的“品种”树形结构");
                    WindowsApiUtils.closeWindow(handle);
                    return null;
                }
            }
            else
            {
                frontMessageBox("未找到“挑选要分析的股票，合约或品种”界面");
                WindowsApiUtils.closeWindow(handle);
                return null;
            }
        }

        private void fillTreeView(List<Exchange> exchanges)
        {
            if (exchanges == null)
            {
                return;
            }
            foreach (Exchange exchange in exchanges)
            {
                TreeNode exchangeNode = new TreeNode(exchange.exchangeName);
                List<Variety> varieties = exchange.varieties;
                foreach (Variety variety in varieties)
                {
                    TreeNode varietyNode = exchangeNode.Nodes.Add(variety.varietyName);
                    varietyNode.Nodes.Add("指数");
                    varietyNode.Nodes.Add("主力");
                    varietyNode.Nodes.Add("连续");
                }
                this.treeviewExchange.Nodes.Add(exchangeNode);
            }
            this.TopMost = true;
            Thread.Sleep(500);
            this.TopMost = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exchanges = new List<Exchange>();

            TreeNodeCollection exchangeNodes = this.treeviewExchange.Nodes;
            if (exchangeNodes != null && exchangeNodes.Count > 0)
            {
                foreach (TreeNode exchangeNode in exchangeNodes)
                {
                    bool hasVarietyChecked = false;
                    Exchange exchange = new Exchange();
                    exchange.exchangeName = exchangeNode.Text;
                    exchange.varieties = new List<Variety>();

                    TreeNodeCollection varietyNodes = exchangeNode.Nodes;
                    if (varietyNodes != null && varietyNodes.Count > 0)
                    {
                        foreach (TreeNode varietyNode in varietyNodes)
                        {
                            bool hasAgreementChecked = false;
                            Variety variety = new Variety();
                            variety.varietyName = varietyNode.Text;

                            TreeNodeCollection agreementNodes = varietyNode.Nodes;
                            if (agreementNodes != null && agreementNodes.Count > 0)
                            {
                                List<int> agreements = new List<int>();

                                foreach (TreeNode agreementNode in agreementNodes)
                                {
                                    if (agreementNode.Checked)
                                    {
                                        string agreementName = agreementNode.Text;
                                        Console.WriteLine(agreementName);
                                        hasAgreementChecked = true;
                                        hasVarietyChecked = true;
                                        switch (agreementName)
                                        {
                                            case "指数":
                                                agreements.Add(0);
                                                break;
                                            case "主力":
                                                agreements.Add(1);
                                                break;
                                            case "连续":
                                                agreements.Add(2);
                                                break;
                                        }
                                    }
                                }
                                if (hasAgreementChecked)
                                {
                                    variety.agreements = agreements;
                                    exchange.varieties.Add(variety);
                                }
                            }
                        }
                    }
                    if (hasVarietyChecked)
                    {
                        exchanges.Add(exchange);
                    }
                }
            }


            foreach (Exchange exchange in exchanges)
            {
                List<Variety> varieties = exchange.varieties;
                foreach (Variety variety in varieties)
                {
                    List<int> ageements = variety.agreements;
                    foreach (int i in ageements)
                    {
                        switch (i)
                        {
                            case 0:
                                Console.WriteLine(exchange.exchangeName + "," + variety.varietyName + ",指数");
                                break;
                            case 1:
                                Console.WriteLine(exchange.exchangeName + "," + variety.varietyName + ",主力");
                                break;
                            case 2:
                                Console.WriteLine(exchange.exchangeName + "," + variety.varietyName + ",连续");
                                break;
                        }
                    }
                }
            }
            if (exchanges.Count == 0)
            {
                MessageBox.Show("请选择品种及合约");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<Exchange> getResult()
        {
            return exchanges;
        }

        private void frontMessageBox(string message)
        {
            this.TopMost = true;
            MessageBox.Show(message);
            this.TopMost = false;
        }
    }
}
