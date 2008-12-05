using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SkeletonStructure
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Panel pagePanel;
		private EtchedLine etchedLine1;
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.Panel leftPanel;
		private System.Windows.Forms.ImageList imageList;
		private System.ComponentModel.IContainer components;

		public OptionsDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.etchedLine1 = new EtchedLine();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pagePanel = new System.Windows.Forms.Panel();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.bottomPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.etchedLine1);
            this.bottomPanel.Controls.Add(this.cancelButton);
            this.bottomPanel.Controls.Add(this.okButton);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 143);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(314, 48);
            this.bottomPanel.TabIndex = 0;
            // 
            // etchedLine1
            // 
            this.etchedLine1.DarkColor = System.Drawing.SystemColors.ControlDark;
            this.etchedLine1.Dock = System.Windows.Forms.DockStyle.Top;
            this.etchedLine1.LightColor = System.Drawing.SystemColors.ControlLightLight;
            this.etchedLine1.Location = new System.Drawing.Point(0, 0);
            this.etchedLine1.Name = "etchedLine1";
            this.etchedLine1.Size = new System.Drawing.Size(314, 8);
            this.etchedLine1.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(232, 12);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(152, 12);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // listView
            // 
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.HideSelection = false;
            this.listView.LargeImageList = this.imageList;
            this.listView.Location = new System.Drawing.Point(8, 8);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(88, 127);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "clipboard_copy_lined_32.png");
            // 
            // pagePanel
            // 
            this.pagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagePanel.Location = new System.Drawing.Point(104, 0);
            this.pagePanel.Name = "pagePanel";
            this.pagePanel.Size = new System.Drawing.Size(210, 143);
            this.pagePanel.TabIndex = 1;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.listView);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Padding = new System.Windows.Forms.Padding(8);
            this.leftPanel.Size = new System.Drawing.Size(104, 143);
            this.leftPanel.TabIndex = 2;
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(314, 191);
            this.Controls.Add(this.pagePanel);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsDialog_Load);
            this.bottomPanel.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void okButton_Click(object sender, System.EventArgs e)
		{
			foreach (PropertyPage page in _pages)
			{
				page.OnApply();
			}
		}

		private void OptionsDialog_Load(object sender, System.EventArgs e)
		{

			Size maxPageSize = pagePanel.Size;

			foreach (PropertyPage page in _pages)
			{
				pagePanel.Controls.Add(page);

				AddListItemForPage(page);

				if (page.Width > maxPageSize.Width)
					maxPageSize.Width = page.Width;
				if (page.Height > maxPageSize.Height)
					maxPageSize.Height = page.Height;

				page.Dock = DockStyle.Fill;
				page.Visible = false;
			}

			Size newSize = new Size();
			newSize.Width = maxPageSize.Width + (this.Width - pagePanel.Width);
			newSize.Height = maxPageSize.Height + (this.Height - pagePanel.Height);

			this.Size = newSize;
			CenterToParent();

			if (listView.Items.Count != 0)
				listView.Items[0].Selected = true;
		}

		PropertyPage _activePage;

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_activePage != null)
				_activePage.Visible = false;

			if (listView.SelectedItems.Count != 0)
			{
				ListViewItem selectedItem = listView.SelectedItems[0];
				PropertyPage page = (PropertyPage)selectedItem.Tag;
				_activePage = page;
			}

			if (_activePage != null)
			{
				_activePage.Visible = true;
				_activePage.OnSetActive();
			}
		}

		void AddListItemForPage(PropertyPage page)
		{
			int imageIndex = 0;

			Image image = page.Image;
			if (image != null)
			{
				imageList.Images.Add(image);
				imageIndex = imageList.Images.Count - 1;
			}

			ListViewItem item = new ListViewItem(page.Text, imageIndex);
			item.Tag = page;
			listView.Items.Add(item);
		}

		// TODO: Make this type-safe.
		ArrayList _pages = new ArrayList();

		public IList Pages
		{
			get { return _pages; }
		}
	}
}
