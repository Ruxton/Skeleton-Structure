using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SkeletonStructure
{
	public class PropertyPage : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.Container components = null;

		public PropertyPage()
		{
			InitializeComponent();
		}

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
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Overridables
		public new virtual string Text
		{
			get { return this.GetType().Name; }
		}

		public virtual Image Image
		{
			get { return null; }
		}

		public virtual void OnSetActive()
		{
		}

		public virtual void OnApply()
		{
		}

		#endregion
	}
}
