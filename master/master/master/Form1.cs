using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        static BackgroundWorker bg;

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
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActiveForm.Text = "fgfgf";
            try
            {

                //Thread newThread = new Thread(loaddirect);
                //newThread.Start();
                bg.RunWorkerAsync();
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

        static void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            string dirPath = @"h:\";
             dirs = new List<string>(Directory.EnumerateDirectories(dirPath, "*", SearchOption.AllDirectories));
        }
        static void bg_RunWorkerCompleted(object sender,
                RunWorkerCompletedEventArgs e)
        {

            listBox1.Invoke(new Action(() => listBox1.DataSource = dirs));
           // {
               // label3.Content = "Random Cancelled " + Process.GetCurrentProcess().Threads.Count;
            //}));
            //listBox1.Invoke(       
            // listBox1.DataSource = dirs;
            //);
        }


        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
           string choiseddir =  listBox1.SelectedValue.ToString();
            //richTextBox1.Text = listBox1.SelectedValue.ToString();
            DirectoryInfo di = new DirectoryInfo(choiseddir);
            System.IO.FileInfo []files =  di.GetFiles("*",SearchOption.AllDirectories);
            List<string> fileinfotext = new List<string>();
            long filesize = 0;
            long directorysize = 0;
            foreach (FileInfo file in files)
            {
                filesize = (file.Length / 1024)/1024;
                fileinfotext.Add(file.Name + " " +filesize+" MBytes "+file.Extension);
                directorysize += filesize;
            }
            fileinfotext.Add(directorysize.ToString() + " Mbytes");
            richTextBox1.Lines = fileinfotext.ToArray();
        }
    }
}
