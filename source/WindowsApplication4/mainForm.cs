using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SkeletonStructure
{
    public partial class mainForm : Form
    {

        private List<DataSet> openedSets = new List<DataSet>();
        private int openDataSetIndex;
        private int count = 0;

        // Node being dragged
        private TreeNode dragNode = null;

        // Temporary drop node for selection
        private TreeNode tempDropNode = null;

        // Timer for scrolling
        private Timer timer = new Timer();

        public mainForm()
        {
            InitializeComponent();
            
            dataSet1.Prefix = "a" + count.ToString();
            openedSets.Add(dataSet1);
            ListViewItem tempItem = new ListViewItem();
            tempItem.Text = dataSet1.DataSetName;
            tempItem.Tag = count;
            tempItem.ImageIndex = 0;
            openDataSetIndex = count;
            count++;
            listView1.Items.Add(tempItem);
            storeTreeView();
        }

        private void newSite()
        {
            DataSet newData = dataSet1.Clone();
            newData.Prefix = "a"+count.ToString();
            newData.Clear();
            newData.DataSetName = "New Site";
            openedSets.Add(newData);

            ListViewItem tempItem = new ListViewItem();
            tempItem.Text = newData.DataSetName;
            tempItem.Tag = count;
            tempItem.ImageIndex = 0;
            listView1.Items.Add(tempItem);

            TreeNode rootNode = new TreeNode("New Site");
            rootNode.Tag = "folder";

            reloadDataSet(newData, count);

            treeView1.Nodes.Add(rootNode);
            count++;
            storeTreeView();
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                TreeNode selectedNode = treeView1.GetNodeAt(e.X, e.Y);
                if (selectedNode != null)
                    treeView1.SelectedNode = selectedNode;

                contextMenuStrip1.Show(treeView1, e.Location);
            }
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
          if(e.Button == MouseButtons.Right)
          {
            // Select the clicked node
              ListViewItem selectedItem = listView1.GetItemAt(e.X, e.Y);
              if(selectedItem != null)
                selectedItem.Selected = true;

              listViewMenuStrip.Show(listView1, e.Location);
          }
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            newSite();
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode tempNode = new TreeNode("New Folder");
            tempNode.Tag = "folder";            
            if ((treeView1.SelectedNode != null) && (treeView1.SelectedNode.Tag.ToString() == "folder"))
            {
                treeView1.SelectedNode.Nodes.Add(tempNode);
            }
            else
            {
                treeView1.TopNode.Nodes.Add(tempNode);
            }
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode != null)
            {
                TreeNode tempNode = new TreeNode("New File");

                DataRow newRow = dataSet1.Tables["files"].NewRow();

                newRow["filename"] = "New File";
                newRow["contents"] = DBNull.Value;

                dataSet1.Tables["files"].Rows.Add(newRow);
                dataSet1.AcceptChanges();

                tempNode.Tag = newRow["file_id"];
                tempNode.ImageIndex = 1;
                tempNode.SelectedImageIndex = 1;
                treeView1.LabelEdit = true;
                if(treeView1.SelectedNode.Tag.ToString() == "folder")
                    treeView1.SelectedNode.Nodes.Add(tempNode);
                tempNode.EnsureVisible();
                tempNode.BeginEdit();
//                tempNode.EndEdit(true);
//                treeView1.LabelEdit = false;
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.LabelEdit = true;
            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode.BeginEdit();
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if ( (treeView1.SelectedNode != null) && (treeView1.SelectedNode != treeView1.TopNode) )
            {
                if (treeView1.SelectedNode.Tag.ToString() != "folder")
                {
                    String id = treeView1.SelectedNode.Tag.ToString();
                    DataRow row = dataSet1.Tables["files"].Rows.Find(id);
                    dataSet1.Tables["files"].Rows.Remove(row);
                    dataSet1.AcceptChanges();
                }
                treeView1.SelectedNode.Remove();
            }
        }

        private void storeTreeView()
        {
            TreeViewSerialization.TreeViewSerializer saveController = new TreeViewSerialization.TreeViewSerializer();
            String input = saveController.SerializeTreeView(treeView1, saveFileDialog1.FileName);

            DataRow row;
            if (dataSet1.Tables["structure"].Rows.Count > 0)
            {
                row = dataSet1.Tables["structure"].Rows[0];
                row["text"] = input;
            }
            else
            {
                row = dataSet1.Tables["structure"].NewRow();
                row["text"] = input;
                dataSet1.Tables["structure"].Rows.Add(row);
            }



            dataSet1.AcceptChanges();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String initDir = Application.StartupPath + "\\sites";
            if (!(Directory.Exists(initDir)))
            {
                Directory.CreateDirectory(initDir);
            }
            saveFileDialog1.InitialDirectory = initDir;

            storeTreeView();
            saveFileDialog1.ShowDialog();

            FileStream fs = new FileStream(@saveFileDialog1.FileName, FileMode.Create);
            dataSet1.RemotingFormat = SerializationFormat.Binary;

            // Remove the above line, and serialize the dataset; Check the file content.

            BinaryFormatter bfo = new BinaryFormatter();
            try
            {
                bfo.Serialize(fs, this.dataSet1);
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Failed to serialize. Reason: " + ex.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }


        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String initDir = Application.StartupPath+"\\sites";
            if ( !( Directory.Exists(initDir) ) )
            {
                Directory.CreateDirectory(initDir);
            }
            openFileDialog1.InitialDirectory = initDir;
            openFileDialog1.ShowDialog();          
        }

        private void reloadDataSet(DataSet dataSet, int dataIndex)
        {
            storeTreeView();
            DataSet oldData = this.dataSet1;

            DataSet storeData = openedSets.Find(delegate(DataSet ds) { return ds.Prefix == oldData.Prefix; });
            storeData = oldData;
            
//            openedSets[openDataSetIndex] = oldData;
            
            this.dataSet1 = dataSet;
            openDataSetIndex = dataIndex;
            DataRow row;
            if (this.dataSet1.Tables["structure"].Rows.Count > 0)
            {
                row = this.dataSet1.Tables["structure"].Rows[0];
                treeView1.Nodes.Clear();
                TreeViewSerialization.TreeViewSerializer openController = new TreeViewSerialization.TreeViewSerializer();
                openController.DeserializeTreeView(treeView1, row["text"].ToString());
            }
            else
            {
                treeView1.Nodes.Clear();
            }
            bindingSource1.DataSource = dataSet1;
            treeView1.Sort();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            DataSet newData = null;
            FileStream file = File.Open(openFileDialog1.FileName, FileMode.Open);
            try
            {
                BinaryFormatter bfo = new BinaryFormatter();
                newData = (DataSet)bfo.Deserialize(file);
            }
            catch (SerializationException ex)
            {
                MessageBox.Show("Cannot open this file, perhaps\nit wasn't created with Skeleton Structure?", "Cannot Open File");
            }
            finally
            {
                file.Close();
                if (newData != null)
                {
                    openedSets.Add(newData);
                    newData.Prefix = "a"+count.ToString();
                    ListViewItem tempItem = new ListViewItem();
                    tempItem.Text = newData.DataSetName;
                    tempItem.Tag = count;
                    
                    listView1.Items.Add(tempItem);
                    reloadDataSet(newData, count );
                    count++;
                }
            }

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
        }

        public DataSet getDataSet()
        {
            return this.dataSet1;
        }

        public void setDataSet(DataSet dataSetNew)
        {
            this.dataSet1 = dataSetNew;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag.ToString() != "folder")
            {
                Int32 id = Int32.Parse(treeView1.SelectedNode.Tag.ToString());
                editFile form = new editFile(id, this);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show(this);
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Tag.ToString() != "folder")
            {
                String id = e.Node.Tag.ToString();
                DataRow row = dataSet1.Tables["files"].Rows.Find(id);
                if(e.Label != null)
                {
                    row["filename"] = e.Label;
                    dataSet1.AcceptChanges();
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
            else if (e.Node == treeView1.TopNode)
            {
                if (e.Label != null)
                {
                    dataSet1.DataSetName = e.Label;
                    String Tag = dataSet1.Prefix.Substring(1);
                    ListViewItem item = null;
                    foreach (ListViewItem tempItem in listView1.Items)
                    {
                        if (tempItem.Tag.ToString() == Tag)
                            item = tempItem;
                    }
                    if(item != null)
                        item.Text = e.Label;
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ProcessDialog dialog = new ProcessDialog(treeView1,dataSet1);
            dialog.StartPosition = FormStartPosition.CenterParent; 
            dialog.ShowDialog(this);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem selectedItem = null;
            if (listView1.SelectedItems != null)
                selectedItem = listView1.SelectedItems[0];
            if (selectedItem != null)
            {
                int selectedIndex = Int32.Parse(selectedItem.Tag.ToString());
                DataSet tempSet = openedSets.Find(delegate(DataSet ds) { return ds.Prefix == "a"+selectedIndex.ToString(); });
                reloadDataSet(tempSet, selectedIndex);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutBox about = new aboutBox();
            about.ShowDialog(this);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newSite();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog options = new OptionsDialog();
            options.Pages.Add(new GeneralOptionsPage());
            DialogResult result = options.ShowDialog(this);
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = listView1.ClientSize.Width;
            foreach (ListViewItem item in listView1.Items)
            {
            }
        }


        private void treeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            // Get drag node and select it
            this.dragNode = (TreeNode)e.Item;
            this.treeView1.SelectedNode = this.dragNode;

            // Reset image list used for drag image
            this.imageListDrag.Images.Clear();
            this.imageListDrag.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.treeView1.Indent, this.dragNode.Bounds.Height);

            // Create new bitmap
            // This bitmap will contain the tree node image to be dragged
            Bitmap bmp = new Bitmap(this.dragNode.Bounds.Width + this.treeView1.Indent, this.dragNode.Bounds.Height);

            // Get graphics from bitmap
            Graphics gfx = Graphics.FromImage(bmp);

            // Draw node icon into the bitmap
            gfx.DrawImage(this.imageListTreeView.Images[0], 0, 0);

            // Draw node label into bitmap
            gfx.DrawString(this.dragNode.Text,
                this.treeView1.Font,
                new SolidBrush(this.treeView1.ForeColor),
                (float)this.treeView1.Indent, 1.0f);

            // Add bitmap to imagelist
            this.imageListDrag.Images.Add(bmp);

            // Get mouse position in client coordinates
            Point p = this.treeView1.PointToClient(Control.MousePosition);

            // Compute delta between mouse position and node bounds
            int dx = p.X + this.treeView1.Indent - this.dragNode.Bounds.Left;
            int dy = p.Y - this.dragNode.Bounds.Top;

            // Begin dragging image
            if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, dx, dy))
            {
                // Begin dragging
                this.treeView1.DoDragDrop(bmp, DragDropEffects.Move);
                // End dragging image
                DragHelper.ImageList_EndDrag();
            }

        }

        private void treeView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Compute drag position and move image
            Point formP = this.PointToClient(new Point(e.X, e.Y));
            DragHelper.ImageList_DragMove(formP.X - this.treeView1.Left, formP.Y - this.treeView1.Top);

            // Get actual drop node
            TreeNode dropNode = this.treeView1.GetNodeAt(this.treeView1.PointToClient(new Point(e.X, e.Y)));
            if (dropNode == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;

            // if mouse is on a new node select it
            if (this.tempDropNode != dropNode)
            {
                DragHelper.ImageList_DragShowNolock(false);
                this.treeView1.SelectedNode = dropNode;
                DragHelper.ImageList_DragShowNolock(true);
                tempDropNode = dropNode;
            }

            // Avoid that drop node is child of drag node 
            TreeNode tmpNode = dropNode;
            while (tmpNode.Parent != null)
            {
                if (tmpNode.Parent == this.dragNode) e.Effect = DragDropEffects.None;
                tmpNode = tmpNode.Parent;
            }
        }

        private void treeView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Unlock updates
            DragHelper.ImageList_DragLeave(this.treeView1.Handle);

            // Get drop node
            TreeNode dropNode = this.treeView1.GetNodeAt(this.treeView1.PointToClient(new Point(e.X, e.Y)));

            // If drop node isn't equal to drag node, add drag node as child of drop node
            if (this.dragNode != dropNode)
            {
                // Remove drag node from parent
                if (this.dragNode.Parent == null)
                {
                    this.treeView1.Nodes.Remove(this.dragNode);
                }
                else
                {
                    this.dragNode.Parent.Nodes.Remove(this.dragNode);
                }

                // Add drag node to drop node
                dropNode.Nodes.Add(this.dragNode);
                dropNode.ExpandAll();

                // Set drag node to null
                this.dragNode = null;

                // Disable scroll timer
                this.timer.Enabled = false;
                treeView1.Sort();
            }
        }

        private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            DragHelper.ImageList_DragEnter(this.treeView1.Handle, e.X - this.treeView1.Left,
                e.Y - this.treeView1.Top);

            // Enable timer for scrolling dragged item
            this.timer.Enabled = true;
        }

        private void treeView1_DragLeave(object sender, System.EventArgs e)
        {
            DragHelper.ImageList_DragLeave(this.treeView1.Handle);

            // Disable timer for scrolling dragged item
            this.timer.Enabled = false;
        }

        private void treeView1_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                // Show pointer cursor while dragging
                e.UseDefaultCursors = false;
                this.treeView1.Cursor = Cursors.Default;
            }
            else e.UseDefaultCursors = true;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // get node at mouse position
            Point pt = PointToClient(Control.MousePosition);
            TreeNode node = this.treeView1.GetNodeAt(pt);

            if (node == null) return;

            // if mouse is near to the top, scroll up
            if (pt.Y < 30)
            {
                // set actual node to the upper one
                if (node.PrevVisibleNode != null)
                {
                    node = node.PrevVisibleNode;

                    // hide drag image
                    DragHelper.ImageList_DragShowNolock(false);
                    // scroll and refresh
                    node.EnsureVisible();
                    this.treeView1.Refresh();
                    // show drag image
                    DragHelper.ImageList_DragShowNolock(true);

                }
            }
            // if mouse is near to the bottom, scroll down
            else if (pt.Y > this.treeView1.Size.Height - 30)
            {
                if (node.NextVisibleNode != null)
                {
                    node = node.NextVisibleNode;

                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    this.treeView1.Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void flagAsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((treeView1.SelectedNode != null) && (treeView1.SelectedNode != treeView1.TopNode))
            {
                    treeView1.SelectedNode.ImageIndex = 1;
                    treeView1.SelectedNode.SelectedImageIndex = 1;
            }
        }


    }
}