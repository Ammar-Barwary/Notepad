using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notepad.Class;
using System.Windows.Forms;
using System.IO;
using Notepad.Froms;

namespace Notepad
{
    public partial class Form1 : Form
    {
        MenuStripClass menuStripClass = new MenuStripClass();
        Dictionary<string, string> filePath = new Dictionary<string, string>();

        string pathLable = "Path: ";
        int count = 1;
        int action = 0;

        public Form1()
        {
            InitializeComponent();

            string tabName = "Untitled-" + count;
            count++;

            filePath.Add(tabName, null);
            TextPath.Text = pathLable + "";
            menuStripClass.NewFile(tabControl1, TextMenuStrip, tabName);
        }

        #region File Menu Tools
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            action = 1;
            string tabName = "Untitled-" + count;
            count++;

            filePath.Add(tabName, null);
            TextPath.Text = pathLable + "";
            menuStripClass.NewFile(tabControl1, TextMenuStrip, tabName);
            action = 0;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            action = 1;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Any File|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string name = Path.GetFileName(dialog.FileName);
                string path = dialog.FileName;

                if (menuStripClass.OpenFile(tabControl1, TextMenuStrip, name, path) == true)
                {
                    filePath.Add(name, path);
                    TextPath.Text = pathLable + filePath[name];
                }
                else
                    MessageBox.Show("You can't open the file with the same name.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            action = 0;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStripClass.SaveFile(tabControl1, filePath);
            TextPath.Text = pathLable + filePath[tabControl1.SelectedTab.Text];
        }

        private void SaveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            menuStripClass.SaveAsFile(tabControl1, filePath);
            TextPath.Text = pathLable + filePath[tabControl1.SelectedTab.Text];
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Edit Menu Tools
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // used in the Edit menu and Text menu strip.
            ((RichTextBox)tabControl1.SelectedTab.Controls["textBox"]).Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // used in the Edit menu and Text menu strip.
            ((RichTextBox)tabControl1.SelectedTab.Controls["textBox"]).Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // used in the Edit menu and Text menu strip.
            ((RichTextBox)tabControl1.SelectedTab.Controls["textBox"]).Paste();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl1.SelectedTab.Controls["textBox"]).Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl1.SelectedTab.Controls["textBox"]).Redo();
        }
        #endregion

        #region Text Menu Strip Tools
        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl1.SelectedTab.Controls["textBox"]).SelectAll();
        }

        private void CloseTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filePath.Remove(tabControl1.SelectedTab.Text);
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
        }

        private void CloseTabAndSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStripClass.SaveAsFile(tabControl1, filePath);

            filePath.Remove(tabControl1.SelectedTab.Text);
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
        }
        #endregion

        #region Setting Menu Strip
        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog dialog = new FontDialog();
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Font = dialog.Font;
                Properties.Settings.Default.Save();

                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    ((RichTextBox)tabControl1.TabPages[i].Controls["textBox"]).Font = Properties.Settings.Default.Font;
                }
            }
        }

        private void FontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Font_Color = dialog.Color;
                Properties.Settings.Default.Save();

                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    ((RichTextBox)tabControl1.TabPages[i].Controls["textBox"]).ForeColor = Properties.Settings.Default.Font_Color;
                }
            }
        }

        private void BackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Background_Color = dialog.Color;
                Properties.Settings.Default.Save();

                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    ((RichTextBox)tabControl1.TabPages[i].Controls["textBox"]).BackColor = Properties.Settings.Default.Background_Color;
                }
            }
        }

        private void DefoaltSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            Properties.Settings.Default.Font_Color = Color.Black;
            Properties.Settings.Default.Background_Color = Color.White;
            Properties.Settings.Default.Save();

            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                ((RichTextBox)tabControl1.TabPages[i].Controls["textBox"]).Font = Properties.Settings.Default.Font;
                ((RichTextBox)tabControl1.TabPages[i].Controls["textBox"]).ForeColor = Properties.Settings.Default.Font_Color;
                ((RichTextBox)tabControl1.TabPages[i].Controls["textBox"]).BackColor = Properties.Settings.Default.Background_Color;
            }
        }
        #endregion

        #region Help Menu Strip
        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }
        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (action == 0)
                TextPath.Text = pathLable + filePath[tabControl1.SelectedTab.Text];
        }
    }
}
