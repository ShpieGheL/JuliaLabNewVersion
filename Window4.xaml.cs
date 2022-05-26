using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.OleDb;
using System.Data;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        string t = "%";
        DateTime d = DateTime.MinValue;
        int b = 0;
        List<int> d1 = new List<int>();
        List<int> d2 = new List<int>();
        OleDbConnection connection;
        List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
        public Window4()
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
        public Window4(OleDbConnection con, RootObject root) : this()
        {
            connection = con;
            chrt();
        }

        public void chrt()
        {
            chart.DataContext = null;
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Labs", connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "Labs");
            string[] x = new string[0];
            int[] y = new int[0];
            foreach (DataRow row in ds.Tables["Labs"].Rows)
                if (Array.IndexOf(x, row["Название клиники"].ToString()) == -1)
                {
                    Array.Resize(ref x, x.Length + 1);
                    x[x.Length - 1] = row["Название клиники"].ToString();
                }
            foreach (string s in x)
            {
                Array.Resize(ref y, y.Length + 1);
                if (t == "%")
                    da = new OleDbDataAdapter("SELECT * FROM Labs WHERE [Название клиники]='" + s + "' AND Статус LIKE '" + t + "'", connection);
                else
                    da = new OleDbDataAdapter("SELECT * FROM Labs WHERE [Название клиники]='" + s + "' AND Статус='" + t + "'", connection);
                ds = new DataSet();
                da.Fill(ds, "Labs");
                foreach (DataRow row1 in ds.Tables["Labs"].Rows)
                {
                    if (b == 0)
                    {
                        da = new OleDbDataAdapter("SELECT * FROM Comm WHERE Номер=" + Convert.ToInt32(row1["Номер"].ToString()), connection);
                        ds = new DataSet();
                        da.Fill(ds, "Comm");
                        foreach (DataRow row2 in ds.Tables["Comm"].Rows)
                            y[y.Length - 1] += Convert.ToInt32(row2["Цена"].ToString());
                    }
                    else
                        switch (b)
                        {
                            case 1:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (Convert.ToDateTime(row1["Дата ухода"].ToString()).AddDays(7) >= DateTime.Now)
                                    {
                                        da = new OleDbDataAdapter("SELECT * FROM Comm WHERE Номер=" + Convert.ToInt32(row1["Номер"].ToString()), connection);
                                        ds = new DataSet();
                                        da.Fill(ds, "Comm");
                                        foreach (DataRow row2 in ds.Tables["Comm"].Rows)
                                            y[y.Length - 1] += Convert.ToInt32(row2["Цена"].ToString());
                                    }
                                break;
                            case 2:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (Convert.ToDateTime(row1["Дата ухода"].ToString()).Month.Equals(d.Month))
                                    {
                                        da = new OleDbDataAdapter("SELECT * FROM Comm WHERE Номер=" + Convert.ToInt32(row1["Номер"].ToString()), connection);
                                        ds = new DataSet();
                                        da.Fill(ds, "Comm");
                                        foreach (DataRow row2 in ds.Tables["Comm"].Rows)
                                            y[y.Length - 1] += Convert.ToInt32(row2["Цена"].ToString());
                                    }
                                break;
                            case 3:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (d1.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Month) && d1.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Year))
                                    {
                                        da = new OleDbDataAdapter("SELECT * FROM Comm WHERE Номер=" + Convert.ToInt32(row1["Номер"].ToString()), connection);
                                        ds = new DataSet();
                                        da.Fill(ds, "Comm");
                                        foreach (DataRow row2 in ds.Tables["Comm"].Rows)
                                            y[y.Length - 1] += Convert.ToInt32(row2["Цена"].ToString());
                                    }
                                break;
                            case 4:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (d2.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Month) && d2.Contains(Convert.ToDateTime(row1["Дата ухода"].ToString()).Year))
                                    {
                                        da = new OleDbDataAdapter("SELECT * FROM Comm WHERE Номер=" + Convert.ToInt32(row1["Номер"].ToString()), connection);
                                        ds = new DataSet();
                                        da.Fill(ds, "Comm");
                                        foreach (DataRow row2 in ds.Tables["Comm"].Rows)
                                            y[y.Length - 1] += Convert.ToInt32(row2["Цена"].ToString());
                                    }
                                break;
                            case 5:
                                if (row1["Дата ухода"].ToString() != "" && row1["Дата ухода"].ToString() != null)
                                    if (Convert.ToDateTime(row1["Дата ухода"].ToString()).Year.Equals(DateTime.Now.Year))
                                    {
                                        da = new OleDbDataAdapter("SELECT * FROM Comm WHERE Номер=" + Convert.ToInt32(row1["Номер"].ToString()), connection);
                                        ds = new DataSet();
                                        da.Fill(ds, "Comm");
                                        foreach (DataRow row2 in ds.Tables["Comm"].Rows)
                                            y[y.Length - 1] += Convert.ToInt32(row2["Цена"].ToString());
                                    }
                                break;
                        }
                }
            }
            valueList.Clear();
            for (int i = 0; i < x.Length; i++)
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
