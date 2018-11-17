using FuturesBackTestExportTool.Model;
using FuturesBackTestExportTool.Page;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace FuturesBackTestExportTool
{
    public partial class FormChooseModel : Form
    {
        private List<ModelCategory> modelCategories;
        private IntPtr mainHandle;

        public FormChooseModel(IntPtr mainHandle)
        {
            this.mainHandle = mainHandle;
            InitializeComponent();
        }

        private void FormChooseModel_Load(object sender, EventArgs e)
        {
            if (!getModelFromWH8MainPage())
            {
                //MessageBox.Show("获取模型失败，请重新设置回测参数");
                this.Close();
            }
        }

        //跳转到模型对应的tab，步骤：点击“行情首页”按钮->到数据tab，遍历找到“期货”，并找到其第一个子项->双击子项->点击“K线”按钮
        private bool getModelFromWH8MainPage()
        {
            AutomationElement targetWindow = AutomationElement.FromHandle(mainHandle);
            if (targetWindow == null)
            {
                MessageBox.Show("未找到“赢智程序化”主界面");
                return false;
            }

            PropertyCondition condition0 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_HANGQINGSHOUYE);
            PropertyCondition condition1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
            AutomationElement buttonHangqing = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition0, condition1));
            if (buttonHangqing == null)
            {
                frontMessageBox("未找到主界面“行情首页”按钮");
                return false;
            }
            SimulateOperating.clickButton(buttonHangqing);
            PropertyCondition condition2 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_TREEVIEW_SHUJU);
            PropertyCondition condition3 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
            AutomationElement treeViewShuju = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition2, condition3));
            if (treeViewShuju == null)
            {
                frontMessageBox("未找到主界面“数据”对应的树形结构");
                return false;
            }
            Condition condition4 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
            AutomationElementCollection categoryElementCollection = treeViewShuju.FindAll(TreeScope.Children, condition4);
            if (categoryElementCollection == null || categoryElementCollection.Count == 0)
            {
                frontMessageBox("未找到主界面“数据”对应的树形结构的子项");
                return false;
            }
            bool foundFutures = false;
            foreach (AutomationElement categoryAE in categoryElementCollection)
            {
                if ("期货".Equals(categoryAE.Current.Name))
                {
                    foundFutures = true;
                    SimulateOperating.expandTreeItem(categoryAE);
                    Condition condition5 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection exchangeElementCollection = categoryAE.FindAll(TreeScope.Children, condition5);
                    if (exchangeElementCollection == null || exchangeElementCollection.Count == 0)
                    {
                        frontMessageBox("未找到主界面“数据”树形结构中的子项“期货”的子项");
                        return false;
                    }
                    SimulateOperating.doubleClick(exchangeElementCollection[0]);
                    PropertyCondition condition6 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_BUTTON_KXIAN);
                    PropertyCondition condition7 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                    AutomationElement buttonKXIAN = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition6, condition7));
                    if (buttonKXIAN == null)
                    {
                        frontMessageBox("未找到主界面“K线”按钮");
                        return false;
                    }
                    if (!SimulateOperating.clickButton(buttonKXIAN))
                    {
                        frontMessageBox("点击主界面“K线”按钮失败");
                    }
                    Thread.Sleep(500);

                    PropertyCondition condition8 = new PropertyCondition(AutomationElement.AutomationIdProperty, WH8MainPage.AUTOMATION_ID_TREEVIEW_SHUJU);
                    PropertyCondition condition9 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tree);
                    AutomationElement treeViewModel = targetWindow.FindFirst(TreeScope.Descendants, new AndCondition(condition8, condition9));
                    //这里不需要判断，是同一个treeview，上面能够拿到下面也可以拿到
                    Condition condition10 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                    AutomationElementCollection modelCategoryElementCollection = treeViewModel.FindAll(TreeScope.Children, condition10);
                    if (modelCategoryElementCollection == null || modelCategoryElementCollection.Count == 0)
                    {
                        frontMessageBox("未找到主界面“模型”树形结构的子项");
                        return false;
                    }
                    List<ModelCategory> modelCategories = new List<ModelCategory>();
                    bool foundModelCategory = false;
                    foreach (AutomationElement modelCategoryAE in modelCategoryElementCollection)
                    {
                        string modelCategoryName = modelCategoryAE.Current.Name;
                        if (string.IsNullOrEmpty(modelCategoryName))
                        {
                            continue;
                        }
                        if (modelCategoryName.StartsWith("回测"))
                        {
                            foundModelCategory = true;
                            if (!SimulateOperating.expandTreeItem(modelCategoryAE))
                            {
                                //展开失败
                                continue;
                            }
                            Condition condition11 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TreeItem);
                            AutomationElementCollection modelElementCollection = modelCategoryAE.FindAll(TreeScope.Children, condition11);
                            if (modelElementCollection == null && modelElementCollection.Count == 0)
                            {
                                //未找到自编下的子项
                                continue;
                            }
                            ModelCategory modelCategory = new ModelCategory();
                            modelCategory.modelCategoryName = modelCategoryName;
                            List<string> models = new List<string>();
                            foreach (AutomationElement modelAE in modelElementCollection)
                            {
                                models.Add(modelAE.Current.Name);
                            }
                            modelCategory.models = models;
                            modelCategories.Add(modelCategory);
                        }
                    }
                    if (!foundModelCategory)
                    {
                        frontMessageBox("未找到主界面“模型”树形结构中的“回测”子项");
                        return false;
                    }
                    if (modelCategories == null || modelCategories.Count == 0)
                    {
                        frontMessageBox("未找到主界面“模型”树形结构中的回测模型，请补充！");
                        return false;
                    }
                    fillTreeView(modelCategories);
                    return true;
                }
            }

            if (!foundFutures)
            {
                frontMessageBox("未找到主界面“数据”树形结构中的“期货”子项");
                return false;
            }
            return false;
        }

        private void fillTreeView(List<ModelCategory> modelCategories)
        {
            if (modelCategories == null || modelCategories.Count == 0)
            {
                return;
            }
            foreach (ModelCategory modelCategory in modelCategories)
            {
                TreeNode modelCategoryNode = new TreeNode(modelCategory.modelCategoryName);
                this.treeviewModel.Nodes.Add(modelCategoryNode);
                List<string> models = modelCategory.models;
                if (models == null || models.Count == 0)
                {
                    continue;
                }
                foreach (string model in models)
                {
                    modelCategoryNode.Nodes.Add(model);
                }
            }

            this.TopMost = true;
            Thread.Sleep(100);
            this.TopMost = false;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            modelCategories = new List<ModelCategory>();
            TreeNodeCollection modelCategoryNodes = this.treeviewModel.Nodes;
            if (modelCategoryNodes != null && modelCategoryNodes.Count > 0)
            {
                foreach (TreeNode modelCategoryNode in modelCategoryNodes)
                {
                    bool hasModelChecked = false;
                    ModelCategory modelCategory = new ModelCategory();
                    modelCategory.modelCategoryName = modelCategoryNode.Text;
                    TreeNodeCollection modelNodes = modelCategoryNode.Nodes;
                    List<string> nodes = new List<string>();
                    if (modelNodes != null && modelNodes.Count > 0)
                    {
                        foreach (TreeNode modelNode in modelNodes)
                        {
                            if (modelNode.Checked)
                            {
                                hasModelChecked = true;
                                nodes.Add(modelNode.Text);
                            }
                        }
                    }
                    if (hasModelChecked)
                    {
                        modelCategory.models = nodes;
                        modelCategories.Add(modelCategory);
                    }
                }
            }
            if (modelCategories.Count == 0)
            {
                MessageBox.Show("请选择模型");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<ModelCategory> getResult()
        {
            return modelCategories;
        }

        private void frontMessageBox(string message)
        {
            this.TopMost = true;
            MessageBox.Show(message);
            this.TopMost = false;
        }
    }
}
