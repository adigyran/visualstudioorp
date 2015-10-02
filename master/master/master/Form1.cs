﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

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
        BackgroundWorker bf;

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

            bf = new BackgroundWorker();
            bf.DoWork += new DoWorkEventHandler(bf_DoWork);
            bf.RunWorkerCompleted += bf_RunWorkerCompleted;
            bf.WorkerReportsProgress = true;
            bf.WorkerSupportsCancellation = true;
            bf.ProgressChanged += bf_ProgressChanged;
            button2.Enabled = false;



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
             dirs = new List<string>();
            dirs.Add(dirPath);
            dirs.AddRange(Directory.EnumerateDirectories(dirPath, "*", SearchOption.AllDirectories));
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




        void Writetofiledisk(string path, object Data)
        {

        }
        void bf_DoWork(object sender, DoWorkEventArgs e)
        {
            // string dirPath = @"h:\downloads";
            // dirs = new List<string>(Directory.EnumerateDirectories(dirPath, "*", SearchOption.AllDirectories));
            // filesas = new List<FileInfo>();
            // filesasstr = new List<string>();
            // foreach (string dir in dirs)
            // {
            //    DirectoryInfo di = new DirectoryInfo(dir);
            //   // filesa = di.GetFiles("*", SearchOption.AllDirectories);
            //  filesas.AddRange(di.GetFiles("*", SearchOption.TopDirectoryOnly));
            Writetofiledisk("H:\text.txt", filesas);
            bf.ReportProgress(filesas.Count);
            int prgrsess = 0;
            int remain = filesas.Count();


            FileStream fs = new FileStream("DataFile.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                foreach (FileInfo x in filesas)
                {


                    formatter.Serialize(fs, x);
                    bf.ReportProgress(prgrsess);
                    label3.Invoke(new Action(() => label3.Text = remain + " " + x.Name + " " + x.Length.ToString()));
                    Thread.Sleep(1);
                    prgrsess++;
                    remain--;

                }
            }
            catch (Exception exeption)
            { MessageBox.Show(exeption.Message); }

            finally
            {
                fs.Close();
            }    
            
          

            //StreamWriter file = new StreamWriter("test.txt",true);


            //foreach (FileInfo x in filesas)
            // {
            //  file.WriteLine(x);
            //  bf.ReportProgress(prgrsess);
            //   Thread.Sleep(10);
            //    prgrsess++;

            //}

            // file.Close();
            //Thread.Sleep(1);


        }

        void bf_RunWorkerCompleted(object sender,
                 RunWorkerCompletedEventArgs e)
        {


            //listBox1.Invoke(new Action(() => listBox1.DataSource = dirs));
           // richTextBox1.Invoke(new Action(() => richTextBox1.Lines = filefot));
           button2.Invoke(new Action(() => button2.Enabled = true));
            //textBox1.Invoke(new Action(() => textBox1.Enabled = true));


            // {
            // label3.Content = "Random Cancelled " + Process.GetCurrentProcess().Threads.Count;
            //}));
            //listBox1.Invoke(       
            // listBox1.DataSource = dirs;
            //);
        }
        void bf_ProgressChanged(object sender,
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
            button2.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string txtbox = textBox1.Text;
            label2.Text = txtbox;
            List < string > dirsen = new List<string>();
            int directories = 0;
           
            if(txtbox.Length>0 && Directory.Exists(txtbox))
            {
                button1.Enabled = true;
                dirsen = Directory.EnumerateDirectories(txtbox).ToList();
                label2.Text = label2.Text+" "+dirsen.Count.ToString();
                //label2.Text = Directory.EnumerateFiles(txtbox).ToList().

            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //StreamWriter file = new System.IO.StreamWriter(speichern);
            //foreach (string x in ausgabeListe)
            //    file.WriteLine(x);
            //file.Close();
            bf.RunWorkerAsync();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bg.CancelAsync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //string json = "{\"load\":\"load\"," +
            //"\"n\":\"2\"}";

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://tickets.multifest.ru/php/base_connections.php");

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://url");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    user = "Foo",
                    password = "Baz"
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}
