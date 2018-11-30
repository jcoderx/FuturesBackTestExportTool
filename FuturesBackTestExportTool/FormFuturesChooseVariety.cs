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
            if (PageUtils.toFuturesAnalysisPage(this, mainHandle))
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

        //获取所有交易所的品种、合约
        private List<Exchange> getAllExchanges()
        {
            List<IntPtr> wndHandles = WindowsApiUtils.findWindowHandlesByClassTitleFuzzy(CLASS_DIALOG, FuturesAnalysisPage.FUTURES_ANALYSIS_PAGE_TITLE);
            if (wndHandles.Count == 0)
            {
                PageUtils.frontMessageBox(this, "打开“挑选要分析的股票，合约或品种”界面失败，未找到该界面");
                return null;
            }
            else if (wndHandles.Count > 1)
            {
                PageUtils.frontMessageBox(this, "找到多个“挑选要分析的股票，合约或品种”界面，请关闭多余界面");
                return null;
            }
            IntPtr handle = wndHandles[0];

            AutomationElement targetWindow = AutomationElement.FromHandle(handle);
            if (targetWindow != null)
            {
                WindowsApiUtils.clearOtherWindows(mainHandle, new List<IntPtr> { handle });
                PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, FuturesAnalysisPage.AUTOMATION_ID_TREEVIEW_EXCHANGE);
                PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
                AutomationElement treeviewExchange = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));

                if (treeviewExchange != null)
                {
                    List<Exchange> exchanges = new List<Exchange>();

                    Condition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection exchangeElementCollection = treeviewExchange.FindAll(TreeScope.Children, condition2);
                    if (exchangeElementCollection != null && exchangeElementCollection.Count > 0)
                    {
                        foreach (AutomationElement exchangeAE in exchangeElementCollection)
                        {
                            string exchangeName = exchangeAE.Current.Name;
                            if (FuturesAnalysisPage.isDomesticFutures(exchangeName))
                            {
                                if (SimulateOperating.expandTreeItem(exchangeAE))
                                {
                                    Exchange exchange = new Exchange();
                                    exchange.exchangeName = exchangeName;
                                    List<Variety> varieties = new List<Variety>();
                                    exchange.varieties = varieties;
                                    exchanges.Add(exchange);

                                    Condition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                                    AutomationElementCollection varietyElementCollection = exchangeAE.FindAll(TreeScope.Children, condition3);
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
                                    PageUtils.frontMessageBox(this, "展开“挑选要分析的股票，合约或品种”界面中的“" + exchangeAE.Current.Name + "”子项失败");
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
                        PageUtils.frontMessageBox(this, "未找到“挑选要分析的股票，合约或品种”界面中的“品种”树形结构的子项");
                        WindowsApiUtils.closeWindow(handle);
                        return null;
                    }
                }
                else
                {
                    PageUtils.frontMessageBox(this, "未找到“挑选要分析的股票，合约或品种”界面中的“品种”树形结构");
                    WindowsApiUtils.closeWindow(handle);
                    return null;
                }
            }
            else
            {
                PageUtils.frontMessageBox(this, "未找到“挑选要分析的股票，合约或品种”界面");
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
                    if ("普麦".Equals(variety.varietyName))
                    {
                        varietyNode.Nodes.Add("指数");
                    }
                    else if ("纸浆".Equals(variety.varietyName))
                    {
                        varietyNode.Nodes.Add("指数");
                        varietyNode.Nodes.Add("连续");
                    }
                    else
                    {
                        varietyNode.Nodes.Add("指数");
                        varietyNode.Nodes.Add("主力");
                        varietyNode.Nodes.Add("连续");
                    }
                }
                this.treeviewExchange.Nodes.Add(exchangeNode);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
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

        private void FormFuturesChooseVariety_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.TopMost = false;
        }

        private void checkedAllAgreement(string agreement, bool isChecked)
        {
            TreeNodeCollection exchangeNodes = this.treeviewExchange.Nodes;
            if (exchangeNodes != null && exchangeNodes.Count > 0)
            {
                foreach (TreeNode exchangeNode in exchangeNodes)
                {
                    TreeNodeCollection varietyNodes = exchangeNode.Nodes;
                    if (varietyNodes != null && varietyNodes.Count > 0)
                    {
                        foreach (TreeNode varietyNode in varietyNodes)
                        {
                            TreeNodeCollection agreementNodes = varietyNode.Nodes;
                            if (agreementNodes != null && agreementNodes.Count > 0)
                            {

                                foreach (TreeNode agreementNode in agreementNodes)
                                {
                                    if (agreement.Equals(agreementNode.Text))
                                    {
                                        agreementNode.Checked = isChecked;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.treeviewExchange.Refresh();
        }

        private void checkBoxIndex_CheckedChanged(object sender, EventArgs e)
        {
            checkedAllAgreement("指数", this.checkBoxIndex.Checked);
        }

        private void checkBoxDominant_CheckedChanged(object sender, EventArgs e)
        {
            checkedAllAgreement("主力", this.checkBoxDominant.Checked);
        }

        private void checkBoxContinuous_CheckedChanged(object sender, EventArgs e)
        {
            checkedAllAgreement("连续", this.checkBoxContinuous.Checked);
        }
    }
}
