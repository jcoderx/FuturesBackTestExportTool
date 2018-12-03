using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace FuturesBackTestExportTool
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class ThreeStateTreeview : TreeView
    {

        private const string STATE_UNCHECKED = "0";
        private const string STATE_CHECKED = "1";
        private const string STATE_MIXED = "2";

        private bool skipCheckEvents = false;
        private Graphics graphics;
        private List<TreeNode> mixTreeNodes = new List<TreeNode>();

        public ThreeStateTreeview() : base()
        {
            base.CheckBoxes = true;
            graphics = this.CreateGraphics();
            initTag();
        }

        protected override void OnBeforeCheck(System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            if (skipCheckEvents) return;
            /* if((e.Node.StateImageIndex == 1) == e.Node.Checked) return;   suppress redundant 
                 BeforeCheck-event, if an already checked node programmatically is set to checked */
            base.OnBeforeCheck(e);
        }

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {

            if (skipCheckEvents)
            {
                return;
            }
            skipCheckEvents = true;
            try
            {
                TreeNode node = e.Node;
                if (node.Checked)
                {
                    node.Tag = STATE_CHECKED;
                }
                else
                {
                    node.Tag = STATE_UNCHECKED;
                }
                UpdateChild(node);
                UpdateParent(node);
            }
            finally
            {
                skipCheckEvents = false;
            }
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);
            drawCheckbox(e.Node);
        }

        private void UpdateChild(TreeNode parent)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                node.Checked = parent.Checked;
                if (node.Checked)
                {
                    node.Tag = STATE_CHECKED;
                }
                else
                {
                    node.Tag = STATE_UNCHECKED;
                }
                UpdateChild(node);
            }
        }

        private void UpdateParent(TreeNode child)
        {
            TreeNode parent = child.Parent;
            if (parent == null)
            {
                return;
            }
            if (STATE_MIXED.Equals(child.Tag))
            {
                parent.Tag = STATE_MIXED;
                drawCheckbox(parent);
                addMixedTreeNode(parent);
            }
            else if (IsAllChecked(parent))
            {
                parent.Tag = STATE_CHECKED;
                parent.Checked = true;
                removeMixedTreeNode(parent);
                this.Invalidate(GetCheckRect(parent));
            }
            else if (IsAllUnChecked(parent))
            {
                parent.Tag = STATE_UNCHECKED;
                parent.Checked = false;
                removeMixedTreeNode(parent);
                this.Invalidate(GetCheckRect(parent));
            }
            else
            {
                parent.Tag = STATE_MIXED;
                drawCheckbox(parent);
                addMixedTreeNode(parent);
            }
            UpdateParent(parent);
        }

        private bool IsAllChecked(TreeNode parent)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (!node.Checked || STATE_MIXED.Equals(node.Tag))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAllUnChecked(TreeNode parent)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (node.Checked || STATE_MIXED.Equals(node.Tag))
                {
                    return false;
                }
            }
            return true;
        }

        private Point getTreeNodePoint(TreeNode node)
        {
            Point point = node.Bounds.Location;
            point.X -= 13;
            point.Y += 2;
            return point;
        }

        private void drawCheckbox(TreeNode node)
        {
            if (STATE_MIXED.Equals(node.Tag))
            {
                CheckBoxRenderer.DrawCheckBox(graphics, getTreeNodePoint(node), CheckBoxState.MixedNormal);
            }
        }

        //屏蔽双击；重绘mix
        protected override void WndProc(ref Message m)
        {
            const Int32 WM_LBUTTONDBLCLK = 0x203;
            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                //bugfix on Doubleclick on StateImage (Node-Checkbox)
                var pt = this.PointToClient(Control.MousePosition);
                if (HitTest(pt).Location == TreeViewHitTestLocations.StateImage) return;
            }
            const int WM_Paint = 15;
            base.WndProc(ref m);
            if (m.Msg == WM_Paint)
            {
                // at that point built-in drawing is completed - and I paint over the Indeterminate-Checkboxes
                //redrawCheckBox();
                foreach (TreeNode node in mixTreeNodes)
                {
                    drawCheckbox(node);
                }
            }
        }

        private Rectangle GetCheckRect(TreeNode nd)
        {
            var pt = nd.Bounds.Location;
            pt.X -= this.ImageList == null ? 16 : 35;
            return new Rectangle(pt.X, pt.Y, 16, 16);
        }


        private void initTag()
        {
            TreeNodeCollection rootNodes = this.Nodes;
            if (rootNodes != null)
            {
                foreach (TreeNode node in rootNodes)
                {
                    if (node.Checked)
                    {
                        node.Tag = STATE_CHECKED;
                    }
                    else
                    {
                        node.Tag = STATE_UNCHECKED;
                    }
                    initNodeTag(node);
                }
            }
        }

        private void initNodeTag(TreeNode node)
        {
            TreeNodeCollection nodes = node.Nodes;
            if (nodes != null)
            {
                foreach (TreeNode n in nodes)
                {
                    if (node.Checked)
                    {
                        node.Tag = STATE_CHECKED;
                    }
                    else
                    {
                        node.Tag = STATE_UNCHECKED;
                    }
                    initNodeTag(n);
                }
            }
        }

        private void addMixedTreeNode(TreeNode node)
        {
            if (!mixTreeNodes.Contains(node))
            {
                mixTreeNodes.Add(node);
            }
        }

        private void removeMixedTreeNode(TreeNode node)
        {
            mixTreeNodes.Remove(node);
        }
    }
}
