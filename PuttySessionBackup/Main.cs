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
                        checkedListBox1.Items.Add(sessionName);
                    }
                }

                toolStripStatusLabel2.Text = "Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBox1.CheckedItems.Count > 0)
                {
                    
                    var result = saveFileDialog1.ShowDialog();
                    


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
                    throw new NotImplementedException();
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
                checkedListBox1.Enabled = false;

                //Select all items
                for (int itemIndex = 0; itemIndex < checkedListBox1.Items.Count; itemIndex++)
                {
                    checkedListBox1.SetItemChecked(itemIndex, true);
                }
            }
            else
            {
                checkedListBox1.Enabled = true;

                for (int itemIndex = 0; itemIndex < checkedListBox1.Items.Count; itemIndex++)
                {
                    checkedListBox1.SetItemChecked(itemIndex, false);
                }
            }
        }
    }
}
