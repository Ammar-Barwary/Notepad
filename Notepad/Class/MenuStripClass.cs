using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Security.Principal;
using System.Drawing;

namespace Notepad.Class
{
    internal class MenuStripClass
    {
        void Setting(TabControl tabControl)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls["textBox"]).Font = Properties.Settings.Default.Font;
            ((RichTextBox)tabControl.SelectedTab.Controls["textBox"]).ForeColor = Properties.Settings.Default.Font_Color;
            ((RichTextBox)tabControl.SelectedTab.Controls["textBox"]).BackColor = Properties.Settings.Default.Background_Color;
        }

        public bool OpenFile(TabControl tabControl, ContextMenuStrip contextMenuStrip, string tabName, string path)
        {
            bool action = true;

            tabControl.TabPages.Add(tabName);
            int tabCount = tabControl.TabPages.Count;
            int ctr = 0;

            for (int i = 0; i < tabCount; i++)
            {
                if (tabControl.TabPages[i].Text == tabName)
                    ctr++;
            }

            if (ctr == 2)
            {
                tabControl.TabPages.RemoveAt(tabCount - 1);
                action = false;
                goto exit;
            }

            RichTextBox textBox = new RichTextBox();

            tabControl.TabPages[tabCount - 1].Controls.Add(textBox);
            textBox.Dock = DockStyle.Fill;
            textBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            textBox.ContextMenuStrip = contextMenuStrip;
            textBox.WordWrap = false;
            textBox.DetectUrls = false;
            textBox.Name = "textBox";
            textBox.BringToFront();

            tabControl.SelectedIndex = tabCount - 1;
            textBox.Focus();

            Setting(tabControl);

            StreamReader sr = new StreamReader(path);
            textBox.Text = sr.ReadToEnd();
            sr.Close();

            exit:;
            return action;
        }

        public void NewFile(TabControl tabControl, ContextMenuStrip contextMenuStrip, string tabName)
        {
            tabControl.TabPages.Add(tabName);
            int tabCount = tabControl.TabPages.Count;

            RichTextBox textBox = new RichTextBox();

            tabControl.TabPages[tabCount - 1].Controls.Add(textBox);
            textBox.Dock = DockStyle.Fill;
            textBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            textBox.ContextMenuStrip = contextMenuStrip;
            textBox.WordWrap = false;
            textBox.DetectUrls = false;
            textBox.Name = "textBox";
            textBox.BringToFront();

            tabControl.SelectedIndex = tabCount - 1;
            textBox.Focus();

            Setting(tabControl);
        }

        public void SaveAsFile(TabControl tabControl, Dictionary<string, string> filePath)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = tabControl.SelectedTab.Text;
            dialog.Filter = "Text File|*.txt|Any File|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(dialog.FileName);
                sw.Write(((RichTextBox)tabControl.SelectedTab.Controls["textBox"]).Text);
                sw.Close();

                filePath.Remove(tabControl.SelectedTab.Text);
                filePath.Add(Path.GetFileName(dialog.FileName), dialog.FileName);
                tabControl.SelectedTab.Text = Path.GetFileName(dialog.FileName);
            }
        }

        public void SaveFile(TabControl tabControl, Dictionary<string, string> FilePath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(FilePath[tabControl.SelectedTab.Text]);
                sw.Write(((RichTextBox)tabControl.SelectedTab.Controls["textBox"]).Text);
                sw.Close();
            } catch { SaveAsFile(tabControl, FilePath); }
        }
    }
}
