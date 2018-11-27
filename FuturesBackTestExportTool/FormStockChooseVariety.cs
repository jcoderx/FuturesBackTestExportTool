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
    public partial class FormStockChooseVariety : Form
    {
        private List<StockExchange> stockExchanges;
        private IntPtr mainHandle;

        public FormStockChooseVariety(IntPtr mainHandle)
        {
            this.mainHandle = mainHandle;
            InitializeComponent();
        }

        private void FormStockChooseVariety_Load(object sender, EventArgs e)
        {
            if (PageUtils.toFuturesAnalysisPage(this, mainHandle))
            {
                List<StockExchange> stockExchanges = getAllStockExchanges();
                if (stockExchanges != null)
                {
                    fillTreeView(stockExchanges);
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
        private List<StockExchange> getAllStockExchanges()
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
                    List<StockExchange> stockExchanges = new List<StockExchange>();

                    Condition condition2 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection exchangeElementCollection = treeviewExchange.FindAll(TreeScope.Children, condition2);
                    if (exchangeElementCollection != null && exchangeElementCollection.Count > 0)
                    {
                        foreach (AutomationElement exchangeAE in exchangeElementCollection)
                        {
                            string exchangeName = exchangeAE.Current.Name;
                            if (FuturesAnalysisPage.isDomesticStock(exchangeName))
                            {
                                StockExchange stockExchange = new StockExchange();
                                stockExchange.stockExchangeName = exchangeName;
                                List<string> stocks = new List<string>();
                                stockExchange.stocks = stocks;
                                if (SimulateOperating.selectTreeItem(exchangeAE))
                                {
                                    PropertyCondition condition3 = new PropertyCondition(AutomationElement.AutomationIdProperty, FuturesAnalysisPage.AUTOMATION_ID_LISTBOX_VARIETY);
                                    PropertyCondition condition4 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List);
                                    AutomationElement listBoxStock = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition3, condition4));
                                    if (listBoxStock != null)
                                    {
                                        Condition condition5 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem);
                                        AutomationElementCollection stockElementCollection = listBoxStock.FindAll(TreeScope.Children, condition5);
                                        if (stockElementCollection != null && stockElementCollection.Count > 0)
                                        {
                                            foreach (AutomationElement stockAE in stockElementCollection)
                                            {
                                                stocks.Add(stockAE.Current.Name);
                                            }
                                            stockExchanges.Add(stockExchange);
                                        }
                                        else
                                        {
                                            PageUtils.frontMessageBox(this, "在“挑选要分析的股票，合约或品种”界面，股票为空");
                                            return null;
                                        }
                                    }
                                    else
                                    {
                                        PageUtils.frontMessageBox(this, "未找到“挑选要分析的股票，合约或品种”界面中的股票列表");
                                        return null;
                                    }
                                }
                                else
                                {
                                    PageUtils.frontMessageBox(this, "选择“挑选要分析的股票，合约或品种”界面中的“" + exchangeAE.Current.Name + "”子项失败");
                                    WindowsApiUtils.closeWindow(handle);
                                    return null;
                                }
                            }
                        }
                        WindowsApiUtils.closeWindow(handle);
                        return stockExchanges;
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

        private void fillTreeView(List<StockExchange> stockExchanges)
        {
            if (stockExchanges == null)
            {
                return;
            }
            foreach (StockExchange stockExchange in stockExchanges)
            {
                TreeNode stockExchangeNode = new TreeNode(stockExchange.stockExchangeName);
                List<string> stocks = stockExchange.stocks;
                if (stocks == null || stocks.Count == 0)
                {
                    continue;
                }
                foreach (string stock in stocks)
                {
                    TreeNode stockNode = stockExchangeNode.Nodes.Add(stock);
                }
                this.treeviewStockExchange.Nodes.Add(stockExchangeNode);
            }
            this.TopMost = true;
            Thread.Sleep(1000);
            this.TopMost = false;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            stockExchanges = new List<StockExchange>();
            TreeNodeCollection stockExchangeNodes = this.treeviewStockExchange.Nodes;
            if (stockExchangeNodes != null && stockExchangeNodes.Count > 0)
            {
                foreach (TreeNode stockExchangeNode in stockExchangeNodes)
                {
                    bool hasVarietyChecked = false;
                    StockExchange stockExchange = new StockExchange();
                    stockExchange.stockExchangeName = stockExchangeNode.Text;
                    stockExchange.stocks = new List<string>();

                    TreeNodeCollection varietyNodes = stockExchangeNode.Nodes;
                    if (varietyNodes != null && varietyNodes.Count > 0)
                    {
                        foreach (TreeNode varietyNode in varietyNodes)
                        {
                            if (varietyNode.Checked)
                            {
                                hasVarietyChecked = true;
                                stockExchange.stocks.Add(varietyNode.Text);
                            }
                        }
                    }
                    if (hasVarietyChecked)
                    {
                        stockExchanges.Add(stockExchange);
                    }
                }
            }
            if (stockExchanges.Count == 0)
            {
                MessageBox.Show("请选择股票");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<StockExchange> getResult()
        {
            return stockExchanges;
        }
    }
}
