using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace master
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Великий тест всего сущего";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActiveForm.Text = "fgfgf";
            try
            {
                string dirPath = @"h:\downloads";

                List<string> dirs = new List<string>(Directory.EnumerateDirectories(dirPath,"*", SearchOption.AllDirectories));
               // richTextBox1.Lines = dirs.ToArray();
                listBox1.DataSource = dirs;

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

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = listBox1.SelectedValue.ToString();
        }
    }
}
