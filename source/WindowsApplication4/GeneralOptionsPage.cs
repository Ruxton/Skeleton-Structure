using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SkeletonStructure
{
	/// <summary>
	/// Summary description for GeneralOptionsPage.
	/// </summary>
	public class GeneralOptionsPage : PropertyPage
	{
        private System.Windows.Forms.GroupBox groupBox1;
        private ComboBox comboBox1;
        private Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GeneralOptionsPage()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 217);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Settings";
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(89, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // GeneralOptionsPage
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "GeneralOptionsPage";
            this.Size = new System.Drawing.Size(360, 234);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public override string Text
		{
			get
			{
				return "General";
			}
		}


		public override Image Image
		{
			get
			{
                return new Bitmap(GetType(), "Bitmaps.NullOptionsPage.bmp");
			}
		}
        public override void OnSetActive()
        {
            comboBox1.Items.Clear();

            if( Properties.Settings.Default.folders != null)
            {
                foreach (String folder in Properties.Settings.Default.folders)
                {
                    comboBox1.Items.Add(folder);
                }
            }
        }
        public override void OnApply()
        {
            if (Properties.Settings.Default.folders != null)
            {
                Properties.Settings.Default.folders.Clear();
            }
            else
            {
                //Properties.Settings.Default.folders.
            }
            foreach (String folder in comboBox1.Items)
            {
                Properties.Settings.Default.folders.Add(folder);
            }
        }
	}
}
