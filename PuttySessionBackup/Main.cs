using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace PuttySessionBackup
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            //Fill in 
            FillCheckedListBox();
        }

        string FullKeyPath = @"HKEY_CURRENT_USER\Software\SimonTatham\PuTTY\Sessions";

        RegistryKey sessionsKey = Registry.CurrentUser.OpenSubKey(@"Software\SimonTatham\PuTTY\Sessions");

        private void FillCheckedListBox()
        {
            try
            {
                var sessionNames = sessionsKey.GetSubKeyNames();
                if (sessionNames.Count() > 0)
                {
                    foreach (var sessionName in sessionNames)
                    {
                        puttySessionList.Items.Add(sessionName);
                    }
                }

                toolStripStatusLabel2.Text = "Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backupSessions_Click(object sender, EventArgs e)
        {
            try
            {
                if (puttySessionList.CheckedItems.Count > 0)
                {
                    var result = saveFileDialog1.ShowDialog();
                    saveFileDialog1.AddExtension = true;
                    saveFileDialog1.DefaultExt = "reg";
                    if (string.IsNullOrEmpty(saveFileDialog1.FileName))
                        throw new ArgumentNullException("Target file path not set!");

                    try
                    {
                        string path = saveFileDialog1.FileName;
                        string key = FullKeyPath;

                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "regedit.exe";
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        
                        startInfo.Arguments = "/e " + path + " " + key + "";

                        using (Process proc = Process.Start(startInfo))
                        {
                            proc.WaitForExit();
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }else
                {
                    throw new ApplicationException("Session not selected! At least one session must be selected from the list!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(AllSessionsSelected.Checked)
            {
                //Disable checking
                puttySessionList.Enabled = false;

                //Select all items
                for (int itemIndex = 0; itemIndex < puttySessionList.Items.Count; itemIndex++)
                {
                    puttySessionList.SetItemChecked(itemIndex, true);
                }
            }
            else
            {
                puttySessionList.Enabled = true;

                for (int itemIndex = 0; itemIndex < puttySessionList.Items.Count; itemIndex++)
                {
                    puttySessionList.SetItemChecked(itemIndex, false);
                }
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox aboutWindow = new AboutBox();
            aboutWindow.Show();
        }
    }
}
