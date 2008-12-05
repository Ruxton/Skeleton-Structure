﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SkeletonStructure
{
    public partial class ProcessDialog : Form
    {
        private TreeView tree;
        private DataSet data;
        private String startPath;

        public ProcessDialog(TreeView tree, DataSet data)
        {
            this.tree = tree;
            this.data = data;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strPath;
            string strCaption = "Select a directory.";
            DialogResult dlgResult;

            Shell32.ShellClass shl = new Shell32.ShellClass();
            Shell32.Folder2 fld = (Shell32.Folder2)shl.BrowseForFolder(0, strCaption, 0,
                        System.Reflection.Missing.Value);

            if (fld == null)
            {
                strPath = "";
                dlgResult = DialogResult.Cancel;
            }
            else
            {
                strPath = fld.Self.Path;
                dlgResult = DialogResult.OK;
            }
            if (dlgResult == DialogResult.OK)
            {
                comboBox1.Text = strPath;
            }
                

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            progressBar1.Minimum = 0;
            this.tree.PathSeparator = "\\";
            this.startPath = comboBox1.Text;
            TreeNodeCollection nodes = this.tree.Nodes;
            progressBar1.Maximum = nodes.Count;
            collectRecurse(nodes);
            progressLabel.Text = "Completed!";
//            this.Dispose();
        }

        private void collectRecurse(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                this.progressBar1.Value++;
                if (node.Tag.ToString() == "folder")
                    createFolder(node.FullPath);
                else
                {
                    createFile(node.FullPath,node.Tag.ToString());
                }
                recurseFolders(node);
            }
        }

        private Boolean createFolder(String path)
        {
            progressLabel.Text = "Creating folder " + startPath + path + "...";
            try
            {
                Regex re = new Regex("#.*?#", RegexOptions.Compiled | RegexOptions.Singleline);

                StringBuilder sb = new StringBuilder(path);

                foreach (Match m in re.Matches(path))  // assuming text is the text to search
                {
                    string replace = null;
                    // check m.Value against db e.g.
                    if (m.Value == "module")
                    {
                        replace = tree.Nodes[0].Text;
                    }

                    if (replace != null)  // assuming DBCheck returns null for invalid replacement
                        sb.Replace(m.Value, replace);
                } 
                System.IO.Directory.CreateDirectory(@startPath + path);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        private Boolean createFile(String path,String id)
        {
            progressLabel.Text = "Creating file " + startPath + path + "...";

            /**
             * Replace #module# with module name in files 
             */
            Regex re = new Regex("#.*?#", RegexOptions.Compiled | RegexOptions.Singleline);
            StringBuilder sb = new StringBuilder(path);

            foreach (Match m in re.Matches(path))  // assuming text is the text to search
            {
                
                // check m.Value against db e.g.
                string replace = tree.Nodes[0].Text;
                if (replace != null)  // assuming DBCheck returns null for invalid replacement
                    sb.Replace(m.Value, replace);
            }

            /**
             * Replace #module# with module name in file content 
             */

            try {
                DataRow row = data.Tables["files"].Rows.Find(id);
                StringBuilder sbcontents = new StringBuilder(row["contents"].ToString());

                foreach (Match m in re.Matches(row["contents"].ToString()))  // assuming text is the text to search
                {
                    
                    // check m.Value against db e.g.
                    string replace = tree.Nodes[0].Text;
                    if (replace != null)  // assuming DBCheck returns null for invalid replacement
                        sbcontents.Replace(m.Value, replace);
                }

                System.IO.File.WriteAllText(@startPath + sb.ToString(), sbcontents.ToString());
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        private void recurseFolders(TreeNode treeNode)
        {
            progressBar1.Maximum = progressBar1.Maximum + treeNode.Nodes.Count;
            foreach (TreeNode tn in treeNode.Nodes)
            {
                this.progressBar1.Value++;
                if (tn.Tag.ToString() == "folder")
                    createFolder(tn.FullPath);
                else
                {
                    createFile(tn.FullPath,tn.Tag.ToString());
                }
                recurseFolders(tn);
            }

//            return temp;
        }

        private void ProcessDialog_Shown(object sender, EventArgs e)
        {
        }

        private void ProcessDialog_ParentChanged(object sender, EventArgs e)
        {
        }
    }
}