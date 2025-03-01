using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{

    public class FGrid : Form
    {
        protected int m_iNumGridElements = 750;
        protected DataGridViewWithDoubleClick General_DataGridView;
        protected Panel buttonPane;
        protected Button Abort_Button;
        protected Button Filter_Button;
        protected string startingWith = "";
        protected int m_iWidthOfButtonPane = 99;
        protected int m_iScreenHeight;
        protected ListBoxWithDoubleClick Building_Listbox;
        protected int m_iScreenWidth;
        public bool m_bListOrderChanged;
        //****************************************************************************************************************************
        public FGrid()
        {
            m_iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            m_iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            this.Load += new EventHandler(FGridDataView_Load);
            InitializeComponent();
            this.Building_Listbox.ContextMenu = new ContextMenu();
            this.Building_Listbox.ContextMenu.MenuItems.Clear();
            Building_Listbox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SelectSpouse_Click);
            //Building_Listbox.Items.Add("New Building");
            Building_Listbox.Items.Add("New Building Name");
            Building_Listbox.Items.Add("New Building Occupant");
            Building_Listbox.SelectedIndex = 0;
            Building_Listbox.Visible = false;
        }
        //****************************************************************************************************************************
        public void SetStartingWith(string startingWith)
        {
            this.startingWith = startingWith;
        }
        //****************************************************************************************************************************
        private void FGridDataView_Load(System.Object sender, System.EventArgs e)
        {
            SetupLayout();
            SetupDataGridView();
            ChangePropertiesOfGridIfNecessary();
            ShowAllValues(startingWith);
        }
        protected virtual void ShowAllValues(string startingWith) { }
        protected virtual void PopulateDataGridViewValues() { }
        protected virtual void SetupDataGridView() { }
        protected virtual void ChangePropertiesOfGridIfNecessary() { }
        protected virtual void SelectRowButton_DoubleClick(object sender, EventArgs e) { }
        //****************************************************************************************************************************
        protected void SetSizeOfGrid(int iWidth)
        {
            int iHeight = 74 + (m_iNumGridElements + 1) * 20;
            int iMaxScreenHeight = m_iScreenHeight - 100;
            int iMaxScreenWidth = m_iScreenWidth - 40;
            if (iHeight > iMaxScreenHeight)
                iHeight = iMaxScreenHeight;
            if (iWidth > iMaxScreenWidth)
                iWidth = iMaxScreenWidth;
            this.Size = new Size(iWidth, iHeight);
            buttonPane.Height = 17;
            buttonPane.Width = m_iWidthOfButtonPane;
            buttonPane.Location = new Point(2, iHeight - 69);
        }
        //****************************************************************************************************************************
        protected virtual void SetupLayout()
        {
            SetSizeOfGrid(m_iScreenWidth);
            General_DataGridView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SelectRowButton_DoubleClick);
        }
        //****************************************************************************************************************************
        private void InitializeComponent()
        {
            this.buttonPane = new System.Windows.Forms.Panel();
            this.Abort_Button = new System.Windows.Forms.Button();
            this.Filter_Button = new System.Windows.Forms.Button();
            this.Building_Listbox = new Utilities.ListBoxWithDoubleClick();
            this.General_DataGridView = new Utilities.DataGridViewWithDoubleClick();
            this.buttonPane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.General_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPane
            // 
            this.buttonPane.Controls.Add(this.Abort_Button);
            this.buttonPane.Controls.Add(this.Filter_Button);
            this.buttonPane.Location = new System.Drawing.Point(0, 0);
            this.buttonPane.Name = "buttonPane";
            this.buttonPane.Size = new System.Drawing.Size(200, 100);
            this.buttonPane.TabIndex = 0;
            // 
            // Abort_Button
            // 
            this.Abort_Button.Location = new System.Drawing.Point(112, 0);
            this.Abort_Button.Name = "Abort_Button";
            this.Abort_Button.Size = new System.Drawing.Size(75, 17);
            this.Abort_Button.TabIndex = 0;
            this.Abort_Button.Text = "Abort";
            // 
            // Filter_Button
            // 
            this.Filter_Button.Location = new System.Drawing.Point(10, 0);
            this.Filter_Button.Name = "Filter_Button";
            this.Filter_Button.Size = new System.Drawing.Size(75, 17);
            this.Filter_Button.TabIndex = 1;
            this.Filter_Button.Text = "Filter";
            // 
            // Building_Listbox
            // 
            this.Building_Listbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Building_Listbox.FormattingEnabled = true;
            this.Building_Listbox.ItemHeight = 15;
            this.Building_Listbox.Location = new System.Drawing.Point(200, 191);
            this.Building_Listbox.Name = "Building_Listbox";
            this.Building_Listbox.Size = new System.Drawing.Size(151, 49);
            this.Building_Listbox.TabIndex = 1;
            // 
            // General_DataGridView
            // 
            this.General_DataGridView.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.General_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.General_DataGridView.Name = "General_DataGridView";
            this.General_DataGridView.Size = new System.Drawing.Size(240, 150);
            this.General_DataGridView.TabIndex = 0;
            // 
            // FGrid
            // 
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1072, 693);
            this.Controls.Add(this.Building_Listbox);
            this.Controls.Add(this.buttonPane);
            this.Location = new System.Drawing.Point(100, 60);
            this.Name = "FGrid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.buttonPane.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.General_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }
        //****************************************************************************************************************************
        protected bool NotWithinLimitsOfGrid(int rowIndex)
        {
            int iNumElements = General_DataGridView.Rows.Count - 1;
            return rowIndex < 0 || rowIndex >= iNumElements; // zero relative
        }
        private int rowIndexFromMouseDown;
        private DataGridViewRow rw;
        //****************************************************************************************************************************
        private void dataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (General_DataGridView.SelectedRows.Count == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    rw = General_DataGridView.SelectedRows[0];
                    rowIndexFromMouseDown = General_DataGridView.SelectedRows[0].Index;
                    General_DataGridView.DoDragDrop(rw, DragDropEffects.Move);
                }
            }
        }
        //****************************************************************************************************************************
        private void dataGridView_DragEnter(object sender, DragEventArgs e)
        {
            if (General_DataGridView.SelectedRows.Count > 0)
            {
                e.Effect = DragDropEffects.Move;
            }
        }
        //****************************************************************************************************************************
        private void dataGridViewxx_DragDrop(object sender, DragEventArgs e)
        {

            int rowIndexOfItemUnderMouseToDrop;
            Point clientPoint = General_DataGridView.PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop = General_DataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Effect == DragDropEffects.Move)
            {
                General_DataGridView.Rows.RemoveAt(rowIndexFromMouseDown);
                General_DataGridView.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rw);
            }
        }
        protected Rectangle dragBoxFromMouseDown;
        protected int rowIndexOfItemUnderMouseToDrop;
        protected void dataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = General_DataGridView.DoDragDrop(
                    General_DataGridView.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        protected void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = General_DataGridView.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        protected void dataGridView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        protected void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = General_DataGridView.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop =
                General_DataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;
                if (rowIndexFromMouseDown != rowIndexOfItemUnderMouseToDrop)
                {
                    General_DataGridView.Rows.RemoveAt(rowIndexFromMouseDown);
                    General_DataGridView.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
                    General_DataGridView.ClearSelection();
                    General_DataGridView.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
                    m_bListOrderChanged = true;
                }
            }
        }        //****************************************************************************************************************************
        protected virtual void SelectSpouse_Click(object sender, System.EventArgs e)
        {
        }
        //****************************************************************************************************************************
    }
}
