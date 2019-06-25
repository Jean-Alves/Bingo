using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BingoChaBebe
{
    public partial class Form1 : Form
    {
        private delegate void FormClosedInvoker();

        private static Form2 _form2;
        private static Form3 _form3;
        private static string _caminho;

        private static int count = 0;

        public Form1()
        {
            InitializeComponent();

            foreach (var control in Controls)
            {
                if (control is Label)
                {
                    var label = control as Label;
                    if (!(label.Name.Contains("26") || label.Name.Contains("27")))
                    {
                        label.Text = "";
                    }
                }
            }
        }

        
        private void MontarTabela()
        {
            string palavras = System.Configuration.ConfigurationSettings.AppSettings["Palavras"].ToString();
            
            var nomes = new ArrayList ();

            var listPalavras = palavras.Split(Convert.ToChar(","));

            foreach (var listPalavra in listPalavras)
            {
                nomes.Add(listPalavra);
            }

            var tabela = new string[5, 5];

            int lbl = 0;
            int result;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var random = new Random().Next(0, nomes.Count);
                    lbl++;
                    var control = Controls.Find(string.Format("label{0}", lbl), true).FirstOrDefault() as Label;

                    if (control != null)
                    {
                        control.Text = (string)nomes[random];
                        nomes.RemoveAt(random);
                    }
                }
            }
        }


        public void Print( bool msg = false)
        {
            var nomeTabela = "cartela" + count;

            try
            {
                if (msg)
                {
                    Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    Graphics graphics = Graphics.FromImage(bitmap as Image);
                    graphics.CopyFromScreen(Location.X+9,Location.Y+30, 0, 0, bitmap.Size);
                    bitmap.Save(string.Format(@"{0}\{1}.jpg",_caminho, nomeTabela), ImageFormat.Jpeg);
                    Process.Start("Explorer", _caminho);
                }
                else
                {
                    Bitmap bitmap = new Bitmap(_form2.Width, _form2.Height);
                    Graphics graphics = Graphics.FromImage(bitmap as Image);
                    graphics.CopyFromScreen(_form2.Location.X, _form2.Location.Y, 0, 0, bitmap.Size);
                    bitmap.Save(string.Format(@"{0}\{1}.jpg", _caminho, nomeTabela), ImageFormat.Jpeg);
                }

                

            }
            catch (ArgumentException)
            {
                MessageBox.Show(@"There was an error. Check the path to the image file.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
                Print(true);
            
        }



        private void button2_Click(object sender, EventArgs e)
        {
            MontarTabela();
            count++;
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(numericUpDown1.Text) || numericUpDown1.Text == "0")
            {
                MessageBox.Show(@"Informe a quantidade de cartelas!");
            }
            else
            {
                var form3 = new Form3();
                _form3 = form3;
                Visible = false;
                var count2 = 0;
                var thread = new Thread(() =>
                {
                    form3.ShowDialog();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                while (count2 < int.Parse(numericUpDown1.Text))
                {
                    count2++;
                    count++;
                    var form2 = new Form2();
                    _form2 = form2;
                    form2.GotFocus += form2_GotFocus;
                    form2.Closing += form2_Closing;
                    form2.ShowDialog();
                    Thread.Sleep(1000);
                }
                Process.Start("Explorer", _caminho);
                FormClose2();
                Visible = true;
            }
        }

        void form2_Closing(object sender, CancelEventArgs e)
        {
            Print();
        }

        void form2_GotFocus(object sender, EventArgs e)
        {
            var thread = new Thread(() =>
            {
                while (string.IsNullOrEmpty(_form2.label13.Text))
                {
                    
                }
                Thread.Sleep(1000);
                FormClose();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private static void FormClose()
        {
            if (_form2.InvokeRequired)
            {
                _form2.Invoke(new FormClosedInvoker(FormClose));
            }
            else
            {
                _form2.Close();
            }
        }

        private static void FormClose2()
        {
            if (_form3.InvokeRequired)
            {
                _form3.Invoke(new FormClosedInvoker(FormClose2));
            }
            else
            {
                _form3.Close();
            }
        }

   
        private void Form1_Load(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                _caminho = fbd.SelectedPath;
                Focus();
            }
            else
            {
                Environment.Exit(0);
            }

            
        }

    }
}
