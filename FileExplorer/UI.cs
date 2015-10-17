#define MultiThread

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace FileExplorer
{
    public partial class UI : Form
    {
        FAT fat;

        public UI()
        {
            InitializeComponent();
            #region comboBox init
            comboBox1.Items.Clear();
            foreach (var v in System.IO.DriveInfo.GetDrives())
            {
                //if (v.DriveFormat == "FAT" && v.DriveFormat == "FAT32")
                    comboBox1.Items.Add(v.ToString().Substring(0, 2));
            }
            #endregion


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IO.closeHandle(fat.HandleFile);
            }
            catch
            {
            }
            treeView.Nodes.Clear();
            var driveChar = comboBox1.SelectedItem.ToString().ToCharArray()[0];
            treeView.Text = driveChar.ToString() + ":";
            //_main();

            _mainDirFiller(comboBox1.SelectedItem.ToString().Substring(0, 2));

        }

        private void _mainDirFiller(object driveLetter)
        {
            fat = new FAT(driveLetter as string);
            directoryExplorerFiller(fat);
        }
        private void directoryExplorerFiller(FAT fat)
        {
            var rootTreeNodes = new List<TreeNode>();
            for (int i = 0; i < fat.SubDirectories.Count; i++)
                rootTreeNodes.Add(fat.SubDirectories[i].ToTreeNode());

            foreach (var v in rootTreeNodes)
                treeView.Nodes.Add(v);
            //dirPlorer.Invoke(new MethodInvoker(() => { dirPlorer.Nodes.Add(v); }));
            foreach (var v in fat.SubFiles)
                fileView.Items.Add(v.ToListItem());
            foreach (var v in rootTreeNodes)
            {
                directoryExplorer(v);
            }

        }
        private void directoryExplorer(object parentTreeNodeObj)
        {
            var parentTreeNode = parentTreeNodeObj as TreeNode;
            var directory = parentTreeNode.Tag as Directory;
            foreach (var v in directory.SubDirectories)
            {
                if (v.Name == "." || v.Name == "..")
                    continue;
                var tn = v.ToTreeNode();
                parentTreeNode.Nodes.Add(tn);
                //dirPlorer.Invoke(new MethodInvoker(() => { parentTreeNode.Nodes.Add(tn); }));

                directoryExplorer(tn);
            }
        }
        private void directoryExplorer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dirRClikMenu.Tag = e.Node;

                if ((e.Node.Tag as Directory).IsDeleted)
                    dirToggleToolStripMenuItem.Text = "&Undelete";
                else
                    dirToggleToolStripMenuItem.Text = "&Delete";

                dirRClikMenu.Show(e.Node.TreeView, e.Location);
            }

            fileExplorerFiller(e.Node.Tag as Directory);
            treeView.SelectedNode = e.Node;
        }

        private void fileExplorerFiller(Directory directory)
        {
            fileView.Items.Clear();
            foreach (var v in directory.SubFiles)
            {
                fileView.Items.Add(v.ToListItem());
            }
        }
        private void dirToggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var directory = ((dirRClikMenu.Tag as TreeNode).Tag as Directory);
            if (directory == null)
                throw new InvalidCastException("Can't find directory.");
            if (directory.IsDeleted)
            {
                directory.UnDelete();
            }
            else if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                directory.Delete();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 1.2\nJAN 2015!", "About");
        }


        private void filePlorer_MouseClick(object sender, MouseEventArgs e)
        {
            var v = sender as ListView;
            if (e.Button == System.Windows.Forms.MouseButtons.Right && v.SelectedItems.Count > 0)
            {
                #region check delete or undelete or toggle
                bool toggle = false;
                for (int i = 1; i < v.SelectedItems.Count; i++)
                {
                    if ((v.SelectedItems[i].Tag as File).IsDeleted != (v.SelectedItems[0].Tag as File).IsDeleted)
                    {
                        toggle = true;
                        break;
                    }
                }
                #endregion
                if (toggle)
                    fileToggleToolStripMenuItem.Text = "&Toggle";
                else if ((v.SelectedItems[0].Tag as File).IsDeleted)
                    fileToggleToolStripMenuItem.Text = "&Undelete";
                else
                    fileToggleToolStripMenuItem.Text = "&Delete";
                fileRClikMenu.Show(v, e.Location);
            }
        }

        private void fileToggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileToggleToolStripMenuItem.Text != "&Undelete" && MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                return;
            for (int i = 0; i < fileView.SelectedItems.Count; i++)
            {
                (fileView.SelectedItems[i].Tag as File).ToggleDeleteUndelete();
            }
            //comboBox1_SelectedIndexChanged(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IO.quickFormat(fat.HandleFile, fat);
        }
    }
}
