using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BingoChaBebe
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            MontarTabela();

        }
        private void MontarTabela()
        {
            string palavras = System.Configuration.ConfigurationSettings.AppSettings["Palavras"].ToString();

            var nomes = new ArrayList();

            var listPalavras = palavras.Split(Convert.ToChar(","));

            foreach (var listPalavra in listPalavras)
            {
                nomes.Add(listPalavra);
            }

            var tabela = new string[5, 5];

            int lbl = 0;
            int result;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
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
           
            //MessageBox.Show(@"Tabela Gerada");
        }

    }
}
