using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace master
{
    public partial class Form1 : Form
    {
        static List<string> dirs;
        static string dirPath;
        FileInfo[] filesa;
        static List<FileInfo> filesas;
        static List<string> filesasstr;
        string[] filefot;
        Process currentProc;

        BackgroundWorker bg;

        public static object ListBox1 { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Великий тест всего сущего";

            bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            bg.WorkerReportsProgress = true;
            bg.WorkerSupportsCancellation = true;
            bg.ProgressChanged += bg_ProgressChanged;
            button1.Enabled = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActiveForm.Text = "fgfgf";
           
            try
            {
                dirPath = textBox1.Text;
                //Thread newThread = new Thread(loaddirect);
                //newThread.Start();
                bg.RunWorkerAsync();
                button1.Enabled = false;
                textBox1.Enabled = false;
                // richTextBox1.Lines = dirs.ToArray();
                

                //foreach (var dir in dirs)
                //{
                    //Console.WriteLine("{0}", dir.Substring(dir.LastIndexOf("\\") + 1));
                //}
                //Console.WriteLine("{0} directories found.", dirs.Count);
            }
            catch (UnauthorizedAccessException UAEx)
            {
                //Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
               // Console.WriteLine(PathEx.Message);
            }
        }

         void bg_DoWork(object sender, DoWorkEventArgs e)
        {
           // string dirPath = @"h:\downloads";
             dirs = new List<string>(Directory.EnumerateDirectories(dirPath, "*", SearchOption.AllDirectories));
            filesas = new List<FileInfo>();
            filesasstr = new List<string>();
            foreach (string dir in dirs)
            {
                DirectoryInfo di = new DirectoryInfo(dir);
              // filesa = di.GetFiles("*", SearchOption.AllDirectories);
                filesas.AddRange(di.GetFiles("*", SearchOption.TopDirectoryOnly));
                bg.ReportProgress(filesas.Count);
                //Thread.Sleep(1);

            }
            foreach(FileInfo filesase in filesas)
            {
                filesasstr.Add(filesase.Name + " " + filesase.Length  + " " + filesase.Directory + " " + filesase.CreationTime);
                bg.ReportProgress(filesasstr.Count);
                //Thread.Sleep(1);
            }
             filefot = filesasstr.Select(i => i.ToString()).ToArray();
            // foreach (string filesastr in filesasstr)
            //{
            //   filefot.Concat(filefot);
            //  bg.ReportProgress(filefot.Length);
            //  Thread.Sleep(1);
            //}
        }

       void bg_RunWorkerCompleted(object sender,
                RunWorkerCompletedEventArgs e)
        {
            

            listBox1.Invoke(new Action(() => listBox1.DataSource = dirs));
            richTextBox1.Invoke(new Action(() => richTextBox1.Lines = filefot));
            button1.Invoke(new Action(() => button1.Enabled = true));
            textBox1.Invoke(new Action(() =>textBox1.Enabled = true ));
            
            
            // {
            // label3.Content = "Random Cancelled " + Process.GetCurrentProcess().Threads.Count;
            //}));
            //listBox1.Invoke(       
            // listBox1.DataSource = dirs;
            //);
        }
         void bg_ProgressChanged(object sender,
    ProgressChangedEventArgs e)
        {
           
            //long memory = (currentProc.PrivateMemorySize64/ 1024)/1024;
            label1.Invoke(new Action(() => label1.Text = e.ProgressPercentage.ToString()));

            //  Console.WriteLine("Обработано " + e.ProgressPercentage + "%");

        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
           string choiseddir =  listBox1.SelectedValue.ToString();
            //richTextBox1.Text = listBox1.SelectedValue.ToString();
            DirectoryInfo di = new DirectoryInfo(choiseddir);
            //System.IO.FileInfo []files =  di.GetFiles("*",SearchOption.AllDirectories);
            //FileInfo findtext = new FileInfo("");
           
            FileInfo[] files = filesas.FindAll(x => x.DirectoryName == choiseddir).ToArray();
            
            List<string> fileinfotext = new List<string>();
            long filesize = 0;
            long directorysize = 0;
            foreach (FileInfo file in files)
            {
                filesize = (file.Length / 1024)/1024;
                fileinfotext.Add(file.Name + " " +filesize+" MBytes "+file.Extension+" "+file.Directory);
                directorysize += filesize;
            }
            fileinfotext.Add(directorysize.ToString() + " Mbytes");
            richTextBox1.Lines = fileinfotext.ToArray();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string txtbox = textBox1.Text;
            label2.Text = txtbox;
            if(txtbox.Length>0 && Directory.Exists(txtbox))
            {
                button1.Enabled = true;
                
            }
            else
            {
                button1.Enabled = false;
            }
        }
    }
}
