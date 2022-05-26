using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public int iop = 0;
        public char c = ' ';
        public OleDbConnection connection;
        public OleDbDataAdapter da;
        public DataSet ds;
        public OleDbDataAdapter da1;
        public DataSet ds1;
        RootObject root;
        public Window1()
        {
            InitializeComponent();
        }
        public Window1(OleDbConnection connection, RootObject root) : this()
        {
            InitializeComponent();
            this.connection = connection;
            this.root = root;
            da = new OleDbDataAdapter("SELECT * FROM Labs", connection);
            ds = new DataSet();
            da.Fill(ds, "Labs");
            da1 = new OleDbDataAdapter("SELECT * FROM Work_types", connection);
            ds1 = new DataSet();
            da1.Fill(ds1, "Work_types");
            foreach (DataRow row in ds.Tables["Labs"].Rows)
            {
                if (C1.Items.Contains(row["Дата прихода"].ToString()) == false)
                    C1.Items.Add(row["Дата прихода"].ToString());
                if (C2.Items.Contains(row["Дата ухода"].ToString()) == false)
                    C2.Items.Add(row["Дата ухода"].ToString());
                if (C3.Items.Contains(row["Название клиники"].ToString()) == false)
                    C3.Items.Add(row["Название клиники"].ToString());
                if (C4.Items.Contains(row["ФИО врача"].ToString()) == false)
                    C4.Items.Add(row["ФИО врача"].ToString());
                if (C5.Items.Contains(row["ФИО пациента"].ToString()) == false)
                    C5.Items.Add(row["ФИО пациента"].ToString());
                foreach (DataRow row1 in ds1.Tables["Work_types"].Rows)
                    if (row["Работы"].ToString().Contains(row1["Вид работы"].ToString()) && C6.Items.Contains(row1["Вид работы"].ToString()) == false)
                        C6.Items.Add(row1["Вид работы"].ToString());
                if (C7.Items.Contains(row["Ответственный сотрудник"].ToString()) == false)
                    C7.Items.Add(row["Ответственный сотрудник"].ToString());
                if (C8.Items.Contains(row["Статус"].ToString()) == false)
                    C8.Items.Add(row["Статус"].ToString());
                if (C9.Items.Contains(row["Номер"].ToString()) == false)
                    C9.Items.Add(row["Номер"].ToString());
            }
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            string[] c1 = new string[8];
            string[] c2 = new string[8];
            c1[0] = C1.Text;
            c1[1] = C2.Text;
            c1[2] = C3.Text;
            c1[3] = C4.Text;
            c1[4] = C5.Text;
            c1[5] = C6.Text;
            c1[6] = C7.Text;
            c1[7] = C8.Text;
            int i = 0;
            int qk = 0;
            foreach (string c in c1)
            {
                string hg = "";
                switch (i)
                {
                    case 0:
                        hg = "[Дата прихода]";
                        break;
                    case 1:
                        hg = "[Дата ухода]";
                        break;
                    case 2:
                        hg = "[Название клиники]";
                        break;
                    case 3:
                        hg = "[ФИО врача]";
                        break;
                    case 4:
                        hg = "[ФИО пациента]";
                        break;
                    case 5:
                        hg = "Работы";
                        break;
                    case 6:
                        hg = "[Ответственный сотрудник]";
                        break;
                    case 7:
                        hg = "Статус";
                        break;
                }
                if (c == "" || c == null)
                    c2[i] = "";
                else
                {
                    qk = 1;
                    c2[i] = $" AND {hg} LIKE '{c}'";
                }
                i++;
            }
            string kj = "SELECT * FROM Labs";
            if (qk == 1)
            {
                kj += " WHERE";
                foreach (string stj in c2)
                    if (stj != "")
                        kj += stj;
            }
            kj = kj.Replace("WHERE AND", "WHERE");
            da = new OleDbDataAdapter(kj, connection);
            ds = new DataSet();
            da.Fill(ds, "Labs");
            int ik = 0;
            string num = "";
            foreach (DataRow row in ds.Tables["Labs"].Rows)
            {
                ik++;
                num = row["Номер"].ToString();
            }
            if (ik != 1)
            {
                this.Close();
            }
            else
            {
                Window2 w2 = new Window2(num, connection, root);
                w2.Show();
            }
        }
    }
}
