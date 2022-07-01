using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.OleDb;
using System.Data;
using System.Windows.Controls;
using System.Collections.Concurrent;
using System.Windows.Media;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        OleDbConnection connection;
        List<KeyValuePair<string, int>> valueList = new();
        ConcurrentDictionary<string, int> items = new();
        readonly int ind = 0;
        RootObject root;

        public Window4(OleDbConnection con, RootObject root, int ind)
        {
            this.root = root;
            DataSet ds = new();
            List<string> comboboxitems;
            InitializeComponent();
            connection = con;
            this.ind = ind;
            comboboxitems = new() { "За последний месяц", "За последний квартал", "За последний год" };
            C2.ItemsSource = comboboxitems;
            switch (ind)
            {
                case 1:
                    comboboxitems = new() { "Оплачено", "Сдано", "В клинике", "В работе", "Ожидание оплаты", "Долг", "Отменён" };
                    C1.ItemsSource = comboboxitems;
                    break;
                case 2:
                    L1.Content = "Клиника:";
                    new OleDbDataAdapter("SELECT * FROM Labs", connection).Fill(ds, "Labs");
                    foreach (DataRow row in ds.Tables["Labs"].Rows)
                        if (!C1.Items.Contains(row["Название клиники"].ToString()) && row["Название клиники"].ToString().Replace(" ","") != "")
                            C1.Items.Add(row["Название клиники"].ToString());
                    break;
                case 3:
                    L1.Content = "Вид:";
                    new OleDbDataAdapter("SELECT * FROM Work_types", connection).Fill(ds, "Work_types");
                    foreach (DataRow row in ds.Tables["Work_types"].Rows)
                        C1.Items.Add(row["Вид работы"].ToString());
                    break;
            }
            Check(null, null);
            Setcolor();
        }

        private void Setcolor()
        {
            var objcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Objcolor));
            var backcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Backcolor));
            var textcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Textcolor));
            var textinobj = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.TextInObjcolor));
            var Bfontsize = root.FontButton;
            var CBfontsize = root.FontCB;
            G.Background = backcolor;
            L1.Foreground = textcolor;
            L2.Foreground = textcolor;
            C1.Background = objcolor;
            C1.Foreground = textinobj;
            C1.FontSize = CBfontsize;
            C2.Background = objcolor;
            C2.Foreground = textinobj;
            C2.FontSize = CBfontsize;
            B.FontSize = Bfontsize;
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            string adapt = "";
            chart.DataContext = null;
            items.Clear();
            valueList.Clear();
            DataSet ds = new();
            switch (ind)
            {
                case 1:
                    adapt = "SELECT * FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Labs.[Название клиники] IS NOT NULL";
                    if (C1.Text != "")
                        adapt += " AND Статус=" + $"'{C1.Text}'";
                    new OleDbDataAdapter(adapt + TimeAdapt(), connection).Fill(ds, "Labs");
                    foreach (DataRow row in ds.Tables["Labs"].Rows)
                    {
                        if (!int.TryParse(row["Цена"].ToString(), out var pricenow))
                            pricenow = 0;
                        items.AddOrUpdate(row["Название клиники"].ToString(), pricenow, (key, keyvalue) => keyvalue + pricenow);
                    }
                    break;
                case 2:
                    adapt = "SELECT * FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Labs.[Название клиники]=" + $"'{C1.Text}'";
                    new OleDbDataAdapter(adapt + TimeAdapt(), connection).Fill(ds, "Labs");
                    foreach (DataRow row in ds.Tables["Labs"].Rows)
                    {
                        if (!int.TryParse(row["Цена"].ToString(), out var pricenow))
                            pricenow = 0;
                        items.AddOrUpdate(row["Статус"].ToString(), pricenow, (key, keyvalue) => keyvalue + pricenow);
                    }
                    break;
                case 3:
                    adapt = "SELECT Labs.Номер AS Нм,Labs.[Название клиники] AS Кл, Works.Цена AS Цн, Works.Количество FROM Labs INNER JOIN Works ON Labs.Номер = Works.Номер WHERE Works.[Вид работы]=" + $"'{C1.Text}'";
                    new OleDbDataAdapter(adapt + TimeAdapt(), connection).Fill(ds, "Labs");
                    foreach (DataRow row in ds.Tables["Labs"].Rows)
                    {
                        if (!int.TryParse(row["Цн"].ToString(), out var pricenow))
                            pricenow = 0;
                        if (!int.TryParse(row["Количество"].ToString(), out var count))
                            pricenow *= count;
                        items.AddOrUpdate(row["Кл"].ToString(), pricenow, (key, keyvalue) => keyvalue + pricenow);
                    }
                    break;
            }
            Visualizer();
        }

        private string TimeAdapt()
        {
            string adapt = "";
            switch (C2.Text)
            {
                case "За последний месяц":
                    string month = DateTime.Now.Month.ToString();
                    if (month.Length == 1)
                        month = "0" + month;
                    adapt = " AND [Дата прихода] LIKE " + $"'%.{month}.{DateTime.Now.Year}'";
                    break;
                case "За последний квартал":
                    List<List<string>> quartals = new();
                    quartals.Add(new List<string> { "01", "02", "03" });
                    quartals.Add(new List<string> { "04", "05", "06" });
                    quartals.Add(new List<string> { "07", "08", "09" });
                    quartals.Add(new List<string> { "10", "11", "12" });
                    string now = DateTime.Now.Month.ToString();
                    if (now.Length == 1)
                        now = "0" + now;
                    foreach (var c in quartals)
                        if (c.Contains(now))
                        {
                            adapt = " AND ([Дата прихода] LIKE " + $"'%.{c[0]}.{DateTime.Now.Year}'" + " OR [Дата прихода] LIKE " + $"'%.{c[1]}.{DateTime.Now.Year}'" + " OR [Дата прихода] LIKE " + $"'%.{c[2]}.{DateTime.Now.Year}'" + ")";
                            break;
                        }
                    break;
                case "За последний год":
                    adapt = " AND [Дата прихода] LIKE " + $"'%.{DateTime.Now.Year}'";
                    break;
                default:
                    adapt = "";
                    break;
            }
            return adapt;
        }

        private void Visualizer()
        {
            foreach (var c in items)
                valueList.Add(new KeyValuePair<string, int>(c.Key, c.Value));
            chart.DataContext = valueList;
        }

        private void Selection(object sender, SelectionChangedEventArgs e)
        {
            if (chart.SelectedItem == null)
                return;
            switch (ind) 
            {
                case 1:
                    string clinic = "";
                    foreach (var c in items.Keys)
                        if (chart.SelectedItem.ToString().Contains(c))
                            clinic = c;
                    (Owner as MainWindow).adapter = $"SELECT * FROM Labs WHERE [Название клиники]=" + $"'{clinic}' ORDER BY Номер DESC";
                    (Owner as MainWindow).SecondaryDataBaseUpdate();
                    break;
                case 2:
                    string status = "";
                    foreach (var c in items.Keys)
                        if (chart.SelectedItem.ToString().Contains(c))
                            status = c;
                    (Owner as MainWindow).adapter = $"SELECT * FROM Labs WHERE [Название клиники]=" + $"'{C1.Text}' AND Статус=" + $"'{status}' ORDER BY Номер DESC";
                    (Owner as MainWindow).SecondaryDataBaseUpdate();
                    break;
                case 3:
                    string work = "";
                    foreach (var c in items.Keys)
                        if (chart.SelectedItem.ToString().Contains(c))
                            work = c;
                    (Owner as MainWindow).adapter = $"SELECT Labs.* FROM Labs INNER JOIN Works ON Labs.Номер = Works.Номер WHERE [Название клиники]=" + $"'{work}' AND Works.[Вид работы]=" + $"'{C1.Text}' ORDER BY Номер DESC";
                    (Owner as MainWindow).SecondaryDataBaseUpdate();
                    break;
            }
        }
    }
}
