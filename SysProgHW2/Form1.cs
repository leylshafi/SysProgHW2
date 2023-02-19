using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace SysProgHW2
{
    public partial class Form1 : Form
    {
        Thread thread=null;
        bool isSuspensed = false;
        bool isAborted = false;
        public Form1()
        {
            InitializeComponent();
            thread = new Thread(CopyMethod);
        }

        private void CopyMethod()
        {
            progressBar1.Value = 0;

            using (FileStream fsRead = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fsWrite = new FileStream(textBox2.Text, FileMode.Create, FileAccess.Write))
                {
                    var fileSize = fsRead.Length;
                    byte[] buffer = new byte[10];
                    var copiedBytes = 0;
                    do
                    {
                        var bytesToCopy = buffer.Length;
                        if ((fileSize - copiedBytes) < 10)
                            bytesToCopy = (int)(fileSize - copiedBytes);
                        fsRead.Read(buffer, 0, bytesToCopy);
                        fsWrite.Write(buffer, 0, bytesToCopy);
                        copiedBytes += bytesToCopy;
                        var progress = (int)(copiedBytes * 100 / fileSize);
                        if (IsHandleCreated)
                        {
                            
                            progressBar1.Invoke(new MethodInvoker(delegate
                            {
                                progressBar1.Value =progress;
                            }));

                        }
                        else
                        {
                            MessageBox.Show("Error occurred");
                        }



                        Thread.Sleep(500);
                    } while (copiedBytes < fileSize);

                }
            }

        }

        private void btnFileFrom_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void btnFileTo_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog2.FileName;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter all information");
                return;
            }

            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("Entered wrong file or folder path");
                return;
            }

            if (!textBox2.Text.Contains(".txt"))
                textBox2.Text += ".txt";


            thread.Start();

        }

        private void btnAbort_Click(object sender, EventArgs e)
        {

            try
            {
                thread.Abort();
                MessageBox.Show("Thread Aborted!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                thread.Resume();
                MessageBox.Show("Thread Resuming!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSuspend_Click(object sender, EventArgs e)
        {

            try
            {
                thread.Suspend();
                MessageBox.Show("Thread Suspended!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
