using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SkeletonStructure
{
    public partial class editFile : Form
    {
        private Int32 id = -1;
        private mainForm form;
        public editFile(Int32 id, mainForm form )
        {
            InitializeComponent();
            this.id = id;
            this.form = form;

            DataSet dataSet1 = form.getDataSet();

            DataRow row = dataSet1.Tables["files"].Rows.Find(id);
            filenameLabel.Text = row["filename"].ToString();
            textBox1.Text = row["contents"].ToString();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void save_Click(object sender, EventArgs e)
        {
            DataSet dataSet1 = form.getDataSet();

            DataRow row = dataSet1.Tables["files"].Rows.Find(id);

            row["contents"] = textBox1.Text;
            dataSet1.AcceptChanges();

            form.setDataSet(dataSet1);
            this.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Shown(object sender, EventArgs e)
        {
        }

        private void Form2_ParentChanged(object sender, EventArgs e)
        {
        }
    }
}