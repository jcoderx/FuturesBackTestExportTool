using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FuturesBackTestExportTool
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class ThreeStateTreeview : TreeView
    {

        private ImageList imageList1;
        private IContainer components;
        private List<TreeNode> _indeterminateds = new List<TreeNode>();
        private Graphics _graphics;
        private Image _imgIndeterminate;
        private bool _skipCheckEvents = false;

        public ThreeStateTreeview()
        {
            InitializeComponent();
            base.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            base.CheckBoxes = true;
            _imgIndeterminate = imageList1.Images[2];
            //imageList1.Images.RemoveAt(2);
            base.StateImageList = imageList1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _graphics != null)
            {
                _graphics.Dispose();
                _graphics = null;
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _graphics = this.CreateGraphics();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_graphics != null)
            {
                _graphics.Dispose();
                _graphics = this.CreateGraphics();
            }
        }

        protected override void OnBeforeCheck(System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            if (_skipCheckEvents) return;
            /* if((e.Node.StateImageIndex == 1) == e.Node.Checked) return;   suppress redundant 
                 BeforeCheck-event, if an already checked node programmatically is set to checked */
            base.OnBeforeCheck(e);
        }

        #region Article-Code

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            /* Logic: All children of an (un)checked Node inherit its Checkstate
             * Parents recompute their state: if all children of a parent have same state, that one 
             * will be taken over as parents state - otherwise take Indeterminate 
             */
            if (_skipCheckEvents) return;/* changing any Treenodes .Checked-Property will raise 
                                           another Before- and After-Check. Skip'em */
            _skipCheckEvents = true;
            try
            {
                TreeNode nd = e.Node;
                /* uninitialized Nodes have StateImageIndex -1, so I associate StateImageIndex as follows:
                 *  0: Unchecked
                 *  1: Checked
                 *  2: Indeterminate
                 *  That corresponds to the System.Windows.Forms.Checkstate - enumeration.
                 *  Furthermore I ordered the images in that manner
                 */
                int state = Math.Max(0, nd.StateImageIndex) == 1 ? 0 : 1;      /* this state is already toggled.
                Note: -1,0 (Unchecked) and 2 (Indeterminate) both toggle to 1 (Checked) */
                if ((state == 1) != nd.Checked) return;       //suppress redundant AfterCheck-event
                InheritCheckstate(nd, state);         // inherit Checkstate to children
                                                      // Parents recompute their state
                for (nd = nd.Parent; nd != null; nd = nd.Parent)
                {
                    if (state != 2 && nd.Nodes.Cast<TreeNode>().Any(ndChild => ndChild.StateImageIndex != state)) state = 2;
                    AssignState(nd, state);
                }
                base.OnAfterCheck(e);
            }
            finally { _skipCheckEvents = false; }
        }

        private void AssignState(TreeNode nd, int state)
        {
            bool ck = state == 1;
            bool stateInvalid = nd.StateImageIndex != state;
            if (stateInvalid) nd.StateImageIndex = state;
            if (nd.Checked != ck)
            {
                nd.Checked = ck;            // changing .Checked-Property raises Invalidating internally
            }
            else if (stateInvalid)
            {
                // in general: the less and small the invalidated area, the less flickering
                // so avoid calling Invalidate() if possible, and only call, if really needed.
                this.Invalidate(GetCheckRect(nd));
            }
        }

        private void InheritCheckstate(TreeNode nd, int state)
        {
            AssignState(nd, state);
            foreach (TreeNode ndChild in nd.Nodes)
            {
                InheritCheckstate(ndChild, state);
            }
        }

        public CheckState GetNodeCheckState(TreeNode nd) { return (CheckState)Math.Max(0, nd.StateImageIndex); }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            // here nothing is drawn. Only collect Indeterminated Nodes, to draw them later (in WndProc())
            // because drawing Treenodes properly (Text, Icon(s) Focus, Selection...) is very complicated
            if (e.Node.StateImageIndex == 2) _indeterminateds.Add(e.Node);
            e.DrawDefault = true;
            base.OnDrawNode(e);
        }

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
                foreach (TreeNode nd in _indeterminateds)
                {
                    _graphics.DrawImage(_imgIndeterminate, GetCheckRect(nd).Location);
                }
                _indeterminateds.Clear();
            }
        }

        #endregion Article-Code

        private Rectangle GetCheckRect(TreeNode nd)
        {
            var pt = nd.Bounds.Location;
            pt.X -= this.ImageList == null ? 16 : 35;
            return new Rectangle(pt.X, pt.Y, 16, 16);
        }

        // since ThreeStateTreeview comes along with its own StateImageList, prevent assigning the StateImageList- Property from outside. Shadow and re-attribute original property
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageList StateImageList
        {
            get { return base.StateImageList; }
            set { }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreeStateTreeview));


            this.imageList1 = getImageList();
            // 
            // ThreeStateTreeview
            // 
            this.LineColor = System.Drawing.Color.Black;
            this.ResumeLayout(false);

        }

        private ImageList getImageList()
        {
            ImageList StateImageList = new System.Windows.Forms.ImageList();

            // populate the image list, using images from the System.Windows.Forms.CheckBoxRenderer class
            for (int i = 0; i < 3; i++)
            {
                // Create a bitmap which holds the relevent check box style
                // see http://msdn.microsoft.com/en-us/library/ms404307.aspx and http://msdn.microsoft.com/en-us/library/system.windows.forms.checkboxrenderer.aspx

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(16, 16);
                System.Drawing.Graphics chkGraphics = System.Drawing.Graphics.FromImage(bmp);
                switch (i)
                {
                    // 0,1 - offset the checkbox slightly so it positions in the correct place
                    case 0:
                        System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(chkGraphics, new System.Drawing.Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                        break;
                    case 1:
                        System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(chkGraphics, new System.Drawing.Point(0, 1), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);
                        break;
                    case 2:
                        System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(chkGraphics, new System.Drawing.Point(0, 2), System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal);
                        break;
                }
                StateImageList.Images.Add(bmp);

            }

            return StateImageList;
        }
    }
}
