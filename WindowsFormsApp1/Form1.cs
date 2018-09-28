using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public string tmp = null;
        public Form1()
        {

            InitializeComponent();

        }

 

        private DataTable ReadCSVFile(string pathToCsvFile)
        {
            int t = 0;
            string sp = "", np ="";
            ServiceReference1.InsurerClient client = new ServiceReference1.InsurerClient();
            //создаём таблицу
            DataTable dt = new DataTable("rep");
            //создаём колонки
            DataColumn DATBEGVST;
            DATBEGVST = new DataColumn("DATBEGVST", typeof(string));
            DataColumn DATENDVST;
            DATENDVST = new DataColumn("DATENDVST", typeof(string));
            DataColumn CNTDAYS;
            CNTDAYS = new DataColumn("CNTDAYS", typeof(string));
            DataColumn NUMPULIS;
            NUMPULIS = new DataColumn("NUMPULIS", typeof(string));
            DataColumn SERPOLIS;
            SERPOLIS = new DataColumn("SERPOLIS", typeof(string));
            DataColumn SERNUMPOLICYOMS;
            SERNUMPOLICYOMS = new DataColumn("SERNUMPOLICYOMS", typeof(string));
            DataColumn IDINSURER;
            IDINSURER = new DataColumn("IDINSURER", typeof(string));
            DataColumn SUMMACCOUNT;
            SUMMACCOUNT = new DataColumn("SUMMACCOUNT", typeof(string));
            DataColumn PR;
            PR = new DataColumn("PR", typeof(string));
            //добавляем колонки в таблицу
            dt.Columns.AddRange(new DataColumn[] {DATBEGVST, DATENDVST, CNTDAYS, NUMPULIS, SERPOLIS, SERNUMPOLICYOMS, IDINSURER, SUMMACCOUNT, PR});
            try
            {
                DataRow dr = null;
                string[] carValues = null;
                string[] cars = File.ReadAllLines(pathToCsvFile);
                for (int i = 0; i < cars.Length; i++)
                {
                    if (!String.IsNullOrEmpty(cars[i]))
                    {
                        carValues = cars[i].Split(';');
                        //создаём новую строку
                        dr = dt.NewRow();
                        dr["DATBEGVST"] = carValues[0];
                        dr["DATENDVST"] = carValues[1];
                        dr["CNTDAYS"] = carValues[2];
                        dr["NUMPULIS"] = carValues[3];
                        dr["SERPOLIS"] = carValues[4];
                        dr["SERNUMPOLICYOMS"] = carValues[5];
                        dr["IDINSURER"] = carValues[6];
                        dr["SUMMACCOUNT"] = carValues[7];
                        //добавляем строку в таблицу
                         t = 3; sp = ""; np = carValues[5];
                        /*if (carValues[7] == "" && carValues[6] !="")
                        { t = 2; sp = ""; np = carValues[6]+carValues[5]; }*/
                        
                        ServiceReference1.InsurerDoc ins = client.GetInsurerDOC(t, sp, np, "");
                         //MessageBox.Show(ins.MO);
                         //MessageBox.Show(carValues[5]);
                        dr["PR"] = ins.MO;
                        carValues[8] = ins.MO;
                        dt.Rows.Add(dr);
                        t++;
                    }
                }

                StringBuilder sb = new StringBuilder();

                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(";", columnNames));

                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(";", fields));
                }

                File.WriteAllText("test.csv", sb.ToString());
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            client.Close();
            return dt;
           
        }
            

        private void label1_Click(object sender, EventArgs e)
        {
                      //
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            dataGridView1.DataSource = ReadCSVFile(@"rep.csv");
        }
    }
}
