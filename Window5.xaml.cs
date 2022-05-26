using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Data.OleDb;
using System.Data;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
        OleDbConnection connection;
        string t = "%";
        DateTime d = DateTime.MinValue;
        int b = 0;
        List<int> d1 = new List<int>();
        List<int> d2 = new List<int>();
        List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
        public Window5()
        {
            InitializeComponent();
            d1.Add(DateTime.Now.Month);
            d1.Add(DateTime.Now.AddMonths(-1).Month);
            d1.Add(DateTime.Now.AddMonths(-2).Month);
            d1.Add(DateTime.Now.Year);
            if (d1.Contains(12) && d1.Contains(1))
                d1.Add(DateTime.Now.AddYears(-1).Year);
            d2.Add(DateTime.Now.Month);
            d2.Add(DateTime.Now.AddMonths(-1).Month);
            d2.Add(DateTime.Now.AddMonths(-2).Month);
            d2.Add(DateTime.Now.AddMonths(-3).Month);
            d2.Add(DateTime.Now.AddMonths(-4).Month);
            d2.Add(DateTime.Now.AddMonths(-5).Month);
            d2.Add(DateTime.Now.Year);
            if (d2.Contains(12) && d2.Contains(1))
                d2.Add(DateTime.Now.AddYears(-1).Year);
        }
        public Window5(OleDbConnection con, RootObject root) : this()
        {
            connection = con;
            chrt();
        }
        public void chrt()
        {
            chart.DataContext = null;
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Work_types", connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "Work_types");
            string[] x = new string[0];
            foreach (DataRow row in ds.Tables["Work_types"].Rows)
                if (Array.IndexOf(x, row["Вид работы"].ToString()) == -1)
                {
                    Array.Resize(ref x, x.Length + 1);
                    x[x.Length - 1] = row["Вид работы"].ToString();
                }
            int[] y = new int[x.Length];
            int i = 0;
            foreach (string x1 in x)
            {
                if (t == "%")
                    da = new OleDbDataAdapter("SELECT Works.Номер, Works.Цена FROM Works INNER JOIN Labs ON Works.Номер = Labs.Номер WHERE Labs.Статус LIKE '%' AND [Вид работы]='" + x1 + "'", connection);
                else
                    da = new OleDbDataAdapter("SELECT Works.Номер, Works.Цена FROM Works INNER JOIN Labs ON Works.Номер = Labs.Номер WHERE Labs.Статус='" + t + "' AND [Вид работы]='" + x1 + "'", connection);
                ds = new DataSet();
                da.Fill(ds, "Works");
                foreach (DataRow row in ds.Tables["Works"].Rows)
                {
                    OleDbDataAdapter da1 = new OleDbDataAdapter("SELECT * FROM Labs WHERE Номер=" + Convert.ToInt32(row["Номер"].ToString()), connection);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1, "Labs");
                    foreach (DataRow row1 in ds1.Tables["Labs"].Rows)
                    {
                        switch (b)
                        {
                            case 0:
                                if (row["Дата ухода"].ToString() != "" && row["Дата ухода"].ToString() != null)
                                    y[i] += Convert.ToInt32(row["Цена"].ToString());
                                break;
                            case 1:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (Convert.ToDateTime(row1["Дата ухода"].ToString()).AddDays(7) >= DateTime.Now)
                                        y[i] += Convert.ToInt32(row["Цена"].ToString());
                                break;
                            case 2:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (Convert.ToDateTime(row1["Дата ухода"].ToString()).Month.Equals(DateTime.Now.Month))
                                        y[i] += Convert.ToInt32(row["Цена"].ToString());
                                break;
                            case 3:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (d1.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Month) && d2.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Year))
                                        y[i] += Convert.ToInt32(row["Цена"].ToString());
                                break;
                            case 4:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (d2.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Month) && d2.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Year))
                                        y[i] += Convert.ToInt32(row["Цена"].ToString());
                                break;
                            case 5:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (Convert.ToDateTime(row1["Дата ухода"].ToString()).Year.Equals(DateTime.Now.Year))
                                        y[i] += Convert.ToInt32(row["Цена"].ToString());
                                break;
                        }
                        i++;
                    }
                }
            }
            valueList.Clear();
            for (i = 0; i < x.Length; i++)
                if (y[i] != 0)
                    valueList.Add(new KeyValuePair<string, int>(x[i], y[i]));
            chart.DataContext = valueList;
        }
        private void C2_Selected(object sender, RoutedEventArgs e)
        {
            t = "Оплачено";
            chrt();
        }

        private void C3_Selected(object sender, RoutedEventArgs e)
        {
            t = "Сдано";
            chrt();
        }

        private void C4_Selected(object sender, RoutedEventArgs e)
        {
            t = "В работе";
            chrt();
        }

        private void C5_Selected(object sender, RoutedEventArgs e)
        {
            t = "Ожидание оплаты";
            chrt();
        }

        private void C6_Selected(object sender, RoutedEventArgs e)
        {
            t = "Долг";
            chrt();
        }

        private void C7_Selected(object sender, RoutedEventArgs e)
        {
            t = "%";
            chrt();
        }

        private void C9_Selected(object sender, RoutedEventArgs e)
        {
            b = 0;
            chrt();
        }

        private void C10_Selected(object sender, RoutedEventArgs e)
        {
            b = 1;
            chrt();
        }

        private void C11_Selected(object sender, RoutedEventArgs e)
        {
            b = 2;
            chrt();
        }

        private void C12_Selected(object sender, RoutedEventArgs e)
        {
            b = 3;
            chrt();
        }

        private void C13_Selected(object sender, RoutedEventArgs e)
        {
            b = 4;
            chrt();
        }

        private void C14_Selected(object sender, RoutedEventArgs e)
        {
            b = 5;
            chrt();
        }
    }
}
