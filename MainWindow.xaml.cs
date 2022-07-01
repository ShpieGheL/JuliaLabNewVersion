using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json;
using System.Windows.Data;
using System.Collections.Generic;
using System.Diagnostics;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ToastNotifications.Core.MessageOptions options = new();
        private OleDbConnection connection = new();
        List<string> NameTables = new() { "Labs", "Comm", "Part_types", "Parts", "Work_types", "Works", "Workers" };
        List<DataSet> DataTables = new();
        List<Staff> ListOfWorks = new();
        List<Parts> ListOfParts = new();
        readonly SolidColorBrush StandartForeground = new(Colors.Black);
        string checkedteeths = "";
        DataSet DataSumm = new();
        string checkedwork = "";
        string checkedworker = "";
        public string adapter = "SELECT * FROM Labs ORDER BY Номер DESC";
        public bool auth = false;
        RootObject root;
        private string path = "";

        public MainWindow()
        {
            if (File.Exists("path.txt"))
                path = File.ReadAllText("path.txt");
            InitializeComponent();
            root = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", optional: false).Build().Get<RootObject>();
            DataBasePathChecker();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            while (auth == false)
            {
                Window5 w5 = new();
                w5.Owner = this;
                w5.ShowDialog();
            }
        }

        public void IsAuth(string pass)
        {
            if (pass == "123")
                auth = true;
        }

        private void OpenDataBase()
        {
            connection.Open();
            DeleteAll(null, null);
            TabCB1.Items.Clear();
            Tab3CB3.Items.Clear();
            Tab3CB5.Items.Clear();
            Tab5T1.Text = "";
            Tab5CB1.Items.Clear();
            Tab5CB2.Items.Clear();
            Tab6CB2.Items.Clear();
            Tab6CB2.Text = "";
            Tab6T1.Text = "";
            Tab2T1.Text = "";
            Tab2T2.Text = "";
            dataGrid1.ItemsSource = null;
            dataGrid2.ItemsSource = null;
            dataGrid3.ItemsSource = null;
            dataGrid6.ItemsSource = null;
            dataGrid7.ItemsSource = null;
            dataGrid8.ItemsSource = null;
            dataGrid9.ItemsSource = null;
            dataGrid10.ItemsSource = null;
            foreach (DataRow row in DataTables[NameTables.IndexOf("Labs")].Tables["Labs"].Rows)
            {
                TabCB1.Items.Add(row["Номер"].ToString());
                if (!Tab3CB1.Items.Contains(row["Название клиники"].ToString()) && row["Название клиники"].ToString().Replace(" ", "") != "")
                    Tab3CB1.Items.Add(row["Название клиники"].ToString());
                if (!Tab3CB2.Items.Contains(row["ФИО врача"].ToString()) && row["ФИО врача"].ToString().Replace(" ", "") != "")
                    Tab3CB2.Items.Add(row["ФИО врача"].ToString());
            }
            dataGrid1.ItemsSource = DataTables[NameTables.IndexOf("Work_types")].Tables["Work_types"].DefaultView;
            foreach (DataRow row in DataTables[NameTables.IndexOf("Work_types")].Tables["Work_types"].Rows)
                Tab3CB5.Items.Add(row["Вид работы"].ToString());
            dataGrid2.ItemsSource = DataTables[NameTables.IndexOf("Workers")].Tables["Workers"].DefaultView;
            foreach (DataRow row in DataTables[NameTables.IndexOf("Workers")].Tables["Workers"].Rows)
            {
                Tab6CB2.Items.Add(row["Имя сотрудника"].ToString());
                Tab3CB3.Items.Add(row["Имя сотрудника"].ToString());
            }
            SecondaryDataBaseUpdate();
            SetColor();
        }

        public void SecondaryDataBaseUpdate()
        {
            Tab3CB6.Items.Clear();
            Tab3CB1.Items.Clear();
            Tab3CB2.Items.Clear();
            Tab3T1.Text = "";
            Tab3T2.Text = "";
            Tab3T3.Text = "";
            Tab3D1.Text = "";
            Tab3D2.Text = "";
            Tab3D5.Text = "";
            Tab3Ch1.IsChecked = false;
            Tab3Ch2.IsChecked = false;
            Tab3Ch3.IsChecked = false;
            Tab3Ch4.IsChecked = false;
            Tab3Ch5.IsChecked = false;
            Tab3Ch6.IsChecked = false;
            Tab3Ch7.IsChecked = false;
            Tab3CB3.Text = "";
            Tab3CB4.Text = "";
            Tab3T7.Text = "";
            DataSet ds = new();
            DataSet ds1 = new();
            new OleDbDataAdapter(adapter, connection).Fill(ds1, "Labs");
            new OleDbDataAdapter("SELECT * FROM Labs", connection).Fill(ds, "Labs");
            new OleDbDataAdapter("SELECT * FROM Parts", connection).Fill(ds, "Parts");
            new OleDbDataAdapter("SELECT * FROM Part_types", connection).Fill(ds, "Part_types");
            dataGrid.ItemsSource = ds1.Tables["Labs"].DefaultView;
            foreach (DataRow row in ds.Tables["Labs"].Rows)
            {
                if (!Tab3CB1.Items.Contains(row["Название клиники"].ToString()) && row["Название клиники"].ToString().Replace(" ", "") != "")
                    Tab3CB1.Items.Add(row["Название клиники"].ToString());
                if (!Tab3CB2.Items.Contains(row["ФИО врача"].ToString()) && row["ФИО врача"].ToString().Replace(" ","") != "")
                    Tab3CB2.Items.Add(row["ФИО врача"].ToString());
            }
            foreach (DataRow row in ds.Tables["Parts"].Rows)
                if (!Tab3CB6.Items.Contains(row["Название этапа"].ToString()))
                    Tab3CB6.Items.Add(row["Название этапа"].ToString());
        }

        private void SetColor()
        {
            var objcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Objcolor));
            var backcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Backcolor));
            var textcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Textcolor));
            var textinobj = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.TextInObjcolor));
            var TBfontsize = root.FontTB;
            var Bfontsize = root.FontButton;
            var elements = root.Elements;
            var CBfontsize = root.FontCB;
            dataGrid.Background = backcolor;
            dataGrid1.Background = backcolor;
            dataGrid2.Background = backcolor;
            dataGrid3.Background = backcolor;
            dataGrid6.Background = backcolor;
            dataGrid7.Background = backcolor;
            dataGrid8.Background = backcolor;
            dataGrid9.Background = backcolor;
            dataGrid10.Background = backcolor;
            G1.Background = backcolor;
            G2.Background = backcolor;
            G3.Background = backcolor;
            G5.Background = backcolor;
            G6.Background = backcolor;
            G7.Background = backcolor;
            foreach (var c in G2.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G2.Children.OfType<TextBox>())
            {
                c.Foreground = textcolor;
                c.Background = objcolor;
                c.FontSize = TBfontsize;
            }
            foreach (var c in G2.Children.OfType<Button>())
                c.FontSize = Bfontsize;
            foreach (var c in G3.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G3.Children.OfType<TextBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = TBfontsize;
            }
            foreach (var c in G3.Children.OfType<ComboBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = CBfontsize;
                if (c != Tab3CB5)
                    c.MaxDropDownHeight = 1.375 * elements * CBfontsize;
            }
            foreach (var c in G3.Children.OfType<Button>())
                c.FontSize = Bfontsize;
            foreach (var c in G3.Children.OfType<DatePicker>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
            }
            foreach (var c in G31.Children.OfType<ComboBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = CBfontsize;
                if (c != Tab3CB5)
                    c.MaxDropDownHeight = 1.375 * elements * CBfontsize;
            }
            foreach (var c in G31.Children.OfType<Button>())
                c.FontSize = Bfontsize;
            foreach (var c in G31.Children.OfType<DatePicker>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
            }
            foreach (var c in G5.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G5.Children.OfType<Button>())
                c.FontSize = Bfontsize;
            foreach (var c in G51.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G51.Children.OfType<ComboBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = CBfontsize;
                c.MaxDropDownHeight = 1.375 * elements * CBfontsize;
            }
            foreach (var c in G51.Children.OfType<TextBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = TBfontsize;
            }
            foreach (var c in G6.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G6.Children.OfType<Button>())
                c.FontSize = Bfontsize;
            foreach (var c in G6.Children.OfType<ComboBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = CBfontsize;
                c.MaxDropDownHeight = 1.375 * elements * CBfontsize;
            }
            foreach (var c in G6.Children.OfType<TextBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = TBfontsize;
            }
            foreach (var c in G61.Children.OfType<DatePicker>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
            }
            foreach (var c in G61.Children.OfType<Label>())
                c.Foreground = textcolor;
            Tab6T2.Foreground = textinobj;
            Tab6T2.Background = objcolor;
            Tab6T2.FontSize = TBfontsize;
            Tab6CB2.Foreground = textinobj;
            Tab6CB2.Background = objcolor;
            Tab6CB2.FontSize = CBfontsize;
            Tab6CB2.MaxDropDownHeight = 1.375 * elements * CBfontsize;
            foreach (var c in G71.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G71.Children.OfType<Button>())
                c.FontSize = Bfontsize;
            foreach (var c in G71.Children.OfType<Microsoft.Windows.Controls.ColorPicker>())
                c.Background = objcolor;
            foreach (var c in G72.Children.OfType<TextBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = TBfontsize;
            }
            foreach (var c in G72.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G73.Children.OfType<TextBox>())
            {
                c.Foreground = textinobj;
                c.Background = objcolor;
                c.FontSize = TBfontsize;
            }
            foreach (var c in G73.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G32.Children.OfType<StackPanel>())
            {
                foreach (var c1 in c.Children.OfType<Label>())
                    c1.Foreground = textcolor;
                foreach (var c1 in c.Children.OfType<TextBox>())
                {
                    c1.Foreground = textinobj;
                    c1.Background = objcolor;
                    c1.FontSize = TBfontsize;
                }
                foreach (var c1 in c.Children.OfType<CheckBox>())
                    c1.Foreground = textcolor;
            }
            Tab3L8.Foreground = textcolor;
        }

        private async void OpenNewDataBase(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Title = "Выберите файл";
            openFileDialog.Filter = "Access files (*.accdb)|*.accdb";
            if (openFileDialog.ShowDialog() == true)
                path = openFileDialog.FileName;
            else
                return;
            File.WriteAllText("path.txt", path);
            var RootObject = new RootObject
            {
                Backcolor = root.Backcolor,
                Textcolor = root.Textcolor,
                Objcolor = root.Objcolor,
                TextInObjcolor = root.TextInObjcolor,
                Elements = root.Elements,
                FontCB = root.FontCB,
                FontTB = root.FontTB,
                FontButton = root.FontButton
            };
            var encoderSettings = new TextEncoderSettings();
            encoderSettings.AllowRanges(UnicodeRanges.BasicLatin, UnicodeRanges.CyrillicExtendedC);
            using FileStream createStream = File.Create("config.json");
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true
            };
            await JsonSerializer.SerializeAsync(createStream, RootObject, options);
            DataBasePathChecker();
        }

        private void DataBasePathChecker()
        {
            try
            {
                connection = new OleDbConnection($@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path}");
                SetData();
                dataGrid.Visibility = Visibility.Visible;
                OpenDataBase();
            }
            catch { dataGrid.Visibility = Visibility.Hidden; }
        }

        private void SetData()
        {
            foreach(var name in NameTables)
            {
                DataSet data = new();
                string group = "";
                if (name == "Labs")
                    group = " ORDER BY Номер DESC";
                new OleDbDataAdapter("SELECT * FROM " + name + group, connection).Fill(data, name);
                DataTables.Add(data);
            }
        }

        private void FindOneNumber(object sender, RoutedEventArgs e)
        {
            ConcurrentDictionary<MenuItem, DataGrid> datas = new();
            datas.TryAdd(Tab1M1, dataGrid);
            datas.TryAdd(Tab6M1, dataGrid10);
            datas.TryAdd(Tab2M1, dataGrid7);
            datas.TryAdd(Tab4M1, dataGrid6);
            datas.TryAdd(Tab5M1, dataGrid3);
            string number = "";
            if (sender.GetType().Name == "Button")
                number = TabCB1.Text;
            else
            {
                datas.TryGetValue(sender as MenuItem, out DataGrid dg);
                if (dg.SelectedIndex != -1)
                    number = ((DataRowView)dg.SelectedItem)["Номер"].ToString();
            }
            if (int.TryParse(number, out int _))
            {
                Window2 w2 = new(number, connection, root, StandartForeground, Tab3CB1.Items, Tab3CB2.Items, Tab3CB3.Items, Tab3CB5.Items, Tab3CB6.Items);
                w2.Owner = this;
                w2.Show();
            }
        }

        private void OpenFilter(object sender, RoutedEventArgs e)
        {
            Window1 w1 = new(connection, root);
            w1.Owner = this;
            w1.Show();
        }

        private static string CheckDate()
        {
            DateTime dt = DateTime.Now.AddDays(1);
            string day = dt.Day.ToString();
            string month = dt.Month.ToString();
            if (day.Length == 1)
                day = "0" + day;
            if (month.Length == 1)
                month = "0" + month;
            return $"{day}.{month}.{dt.Year}";
        }

        private void FindNumbersOutingTomorrow(object sender, RoutedEventArgs e)
        {
            adapter = "SELECT * FROM Labs WHERE [Дата ухода]=" + $"'{CheckDate()}' ORDER BY Номер Desc";
            SecondaryDataBaseUpdate();
        }

        private void FindNumbersOutingTomorrowByCurier(object sender, RoutedEventArgs e)
        {
            adapter = "SELECT Labs.* FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Comm.[Дата курьера]=" + $"'{CheckDate()}' ORDER BY Labs.Номер Desc";
            SecondaryDataBaseUpdate();
        }

        private void FindNumbersOutingTomorrowByAll(object sender, RoutedEventArgs e)
        {
            adapter = "SELECT Labs.* FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Comm.[Дата курьера]=" + $"'{CheckDate()}' OR Labs.[Дата ухода]=" + $"'{CheckDate()}' ORDER BY Labs.Номер Desc";
            SecondaryDataBaseUpdate();
        }

        private void FindNumbersInWork(object sender, RoutedEventArgs e)
        {
            adapter = "SELECT * FROM Labs WHERE Статус='В работе' ORDER BY Номер Desc";
            SecondaryDataBaseUpdate();
        }

        private void SaveCurrentDataBase(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "Access files(.accdb)| *.accdb";
            saveFileDialog.DefaultExt = ".accdb";
            saveFileDialog.FileName = $"{DateTime.Now.Year}DataBase";
            if (saveFileDialog.ShowDialog() == true)
            {
                DataSet ds = new();
                new OleDbDataAdapter("SELECT * FROM Work_types", connection).Fill(ds, "Work_types");
                new OleDbDataAdapter("SELECT * FROM Workers", connection).Fill(ds, "Workers");
                File.Copy(path, saveFileDialog.FileName, true);
                File.Copy("Empty.accdb", "Empty1.accdb", true);
                var con = new OleDbConnection($@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Empty1.accdb");
                con.Open();
                foreach (DataRow row in ds.Tables["Work_types"].Rows)
                    new OleDbCommand("INSERT INTO Work_types ([Вид работы],Цена) VALUES ('" + row["Вид работы"].ToString() + "','" + row["Цена"].ToString() + "')", con).ExecuteReader();
                foreach (DataRow row in ds.Tables["Workers"].Rows)
                    new OleDbCommand("INSERT INTO Workers ([Имя сотрудника],Должность,[Тип платы],Плата) VALUES ('" + row["Имя сотрудника"].ToString() + "','" + row["Должность"].ToString() + "','" + row["Тип платы"].ToString() + "','" + row["Плата"].ToString() + "')", con).ExecuteReader();
                File.WriteAllText("path.txt", "Empty1.accdb");
                con.Close();
                DataBasePathChecker();
            }
        }

        private void DeleteNumberInMainDataBase(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1)
                return;
            if (MessageBox.Show("Вы точно хотите удалить данный наряд?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                new OleDbCommand("DELETE * FROM Labs WHERE Номер=" + ((DataRowView)dataGrid.SelectedItem)["Номер"].ToString(), connection).ExecuteScalar();
                SecondaryDataBaseUpdate();
            }
        }

        private void Edits(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex == -1)
                return;
            new OleDbCommand("UPDATE Labs SET Статус=" + $"'{(sender as MenuItem).Header.ToString().Replace("Установить статус: ", "")}' WHERE Номер=" + ((DataRowView)dataGrid.SelectedItem)["Номер"].ToString(), connection).ExecuteReader();
            SecondaryDataBaseUpdate();
        }

        private void DeleteWork(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно желаете удалить вид работы?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                new OleDbCommand("DELETE * FROM Work_types WHERE [Вид работы]=" + $"'{checkedwork}'", connection).ExecuteScalar();
                foreach (var c in ListOfWorks.ToList())
                    if (c.Тип == checkedwork)
                        ListOfWorks.Remove(c);
            }
            UpdateWorks();
        }

        private void ChangeWork(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Tab2T2.Text, out int price))
                price = 0;
            new OleDbCommand("UPDATE Work_types SET [Вид работы]='" + Tab2T1.Text + "', Цена=" + price + " WHERE [Вид работы]='" + checkedwork + "'", connection).ExecuteReader();
            List<Staff> ind = new();
            foreach (var c in ListOfWorks)
                if (c.Тип == checkedwork)
                    ind.Add(c);
            if (ind.Count != 0)
                foreach (var c in ind)
                {
                    ListOfWorks.Insert(ListOfWorks.IndexOf(c), new Staff
                    {
                        Тип = Tab2T1.Text,
                        Количество = c.Количество,
                        Цена = price,
                        Итог = price * c.Количество
                    });
                    ListOfWorks.Remove(c);
                }
            UpdateWorks();
        }

        private void CheckedMaleFemale(object sender, RoutedEventArgs e)
        {
            foreach (var c in Tab3SP1.Children.OfType<CheckBox>())
                if (c != sender as CheckBox)
                    c.IsChecked = false;
        }

        private void CheckedForm(object sender, RoutedEventArgs e)
        {
            foreach (var c in Tab3SP2.Children.OfType<CheckBox>())
                if (c != sender as CheckBox)
                    c.IsChecked = false;
        }

        private void CheckedSkull(object sender, RoutedEventArgs e)
        {
            int[] x = new int[] { 10, 20, 30 };
            if ((sender as CheckBox).Name == "Tab3Ch7")
                x = new int[] { 30, 40, 50 };
            foreach (var l in GridTab3.Children.OfType<Label>())
                if (Convert.ToInt32(l.Content) > x[0] && Convert.ToInt32(l.Content) < x[1] || Convert.ToInt32(l.Content) > x[1] && Convert.ToInt32(l.Content) < x[2])
                {
                    if ((sender as CheckBox).IsChecked == true)
                    {
                        l.IsEnabled = true;
                        if (l.Foreground != new SolidColorBrush(Colors.Red))
                            l.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        l.IsEnabled = false;
                        l.Foreground = new SolidColorBrush(Colors.Gray);
                        checkedteeths = checkedteeths.Replace(l.Content.ToString(), "");
                    }
                }
        }

        private void Tab3CB4_I(object sender, RoutedEventArgs e)
        {
            Tab3CB4.Foreground = (sender as ComboBoxItem).Foreground;
        }

        private void NewWork(object sender, RoutedEventArgs e)
        {
            string error = "Ошибки при введении данных:\n";
            if (!DateTime.TryParse(Tab3D1.Text, out _))
                error += "Неправильно введена дата прихода.\n";
            if (!DateTime.TryParse(Tab3D2.Text, out _) && Tab3D2.Text != "")
                error += "Неправильно введена дата ухода.\n";
            if (DateTime.TryParse(Tab3D2.Text, out var date1) && DateTime.TryParse(Tab3D2.Text, out var date2))
                if (DateTime.Compare(date1, date2) > 0)
                    error += "Дата ухода раньше даты прихода.\n";
            if (!DateTime.TryParse(Tab3D5.Text, out _) && Tab3D5.Text != "")
                error += "Неправильно введена дата передачи курьеру.\n";
            if (Tab3CB1.Text == "" && Tab3CB2.Text == "")
                error += "Не указаны клиника или врач.\n";
            if (Tab3CB4.Text == "")
                error += "Не указан статус.\n";
            if (error == "Ошибки при введении данных:\n")
            {
                string works = "";
                foreach (var c in ListOfWorks)
                {
                    works += c.Тип;
                    if (ListOfWorks.IndexOf(c) != ListOfWorks.Count - 1)
                        works += "\n";
                }
                new OleDbCommand("INSERT INTO Labs ([Дата прихода],[Дата ухода],[Название клиники],[ФИО врача],[ФИО пациента],Работы,[Ответственный сотрудник],Статус) VALUES ('" + Tab3D1.Text + "','" + Tab3D2.Text + "','" + Tab3CB1.Text + "','" + Tab3CB2.Text + "','" + Tab3T1.Text + "','" + works + "','" + Tab3CB3.Text + "','" + Tab3CB4.Text + "')", connection).ExecuteReader();
                DataSet ds = new();
                new OleDbDataAdapter("SELECT MAX(Номер) AS Ном FROM Labs", connection).Fill(ds, "Labs");
                if (!int.TryParse(ds.Tables["Labs"].Rows[0]["Ном"].ToString(), out int number))
                    number = 1;
                foreach (var c in ListOfWorks)
                    new OleDbCommand("INSERT INTO Works ([Вид работы], Количество, Цена, Номер) VALUES ('" + c.Тип + "','" + c.Количество + "','" + c.Цена + "'," + number + ")", connection).ExecuteReader();
                string sex = "";
                foreach (var c in Tab3SP1.Children.OfType<CheckBox>())
                    if (c.IsChecked == true)
                        sex = c.Content.ToString();
                string form = "";
                foreach (var c in Tab3SP2.Children.OfType<CheckBox>())
                    if (c.IsChecked == true)
                        form = c.Content.ToString();
                string skull = "";
                foreach (var c in Tab3SP3.Children.OfType<CheckBox>())
                    if (c.IsChecked == true)
                        skull = c.Content.ToString();
                if (!int.TryParse(Tab3T5.Text, out int price))
                    price = 0;
                new OleDbCommand("INSERT INTO Comm (Пол,[Тип лица],Возраст,[Цвет зубов],Челюсть,Зубы,Комментарий,Цена,[Дата курьера],Время,Номер) VALUES ('" + sex + "','" + form + "','" + Tab3T2.Text + "','" + Tab3T3.Text + "','" + skull + "','" + checkedteeths + "','" + Tab3T4.Text + "'," + price + ", '" + Tab3D5.Text + "', '" + Tab3T7.Text + "'," + number + ")", connection).ExecuteReader();
                foreach (var c in ListOfParts)
                    new OleDbCommand("INSERT INTO Parts ([Название этапа],[Дата прихода],[Дата ухода],Номер) VALUES ('" + c.Этап + "','" + c.Начало + "','" + c.Конец + "'," + number + ")", connection).ExecuteReader();
                notifier.ShowSuccess("Новый наряд успешно создан.");
                SecondaryDataBaseUpdate();
            }
            else
                notifier.ShowWarning(error);
                
        }

        private void AddType(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Tab3T6.Text, out int result))
                result = 1;
            DataSet ds = new();
            new OleDbDataAdapter("SELECT Цена FROM Work_types WHERE [Вид работы] = '" + Tab3CB5.Text + "'", connection).Fill(ds, "Work_types");
            if (!int.TryParse(ds.Tables["Work_types"].Rows[0]["Цена"].ToString(), out int price))
                price = 0;
            ListOfWorks.Add(new Staff()
            {
                Тип = Tab3CB5.Text,
                Количество = result,
                Цена = price,
                Итог = price * result
            });
            ShowListOfWorks();
        }

        private void DeleteWorkFromAdd(object sender, RoutedEventArgs e)
        {
            if (dataGrid8.SelectedIndex == -1)
                return;
            ListOfWorks.RemoveAt(dataGrid8.SelectedIndex);
            ShowListOfWorks();
        }

        private void ShowListOfWorks()
        {
            dataGrid8.ItemsSource = ListOfWorks;
            dataGrid8.Items.Refresh();
            int pr = 0;
            foreach (var p in ListOfWorks)
                pr += p.Итог;
            Tab3T5.Text = pr.ToString();
        }

        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            ListOfWorks.Clear();
            ShowListOfWorks();
        }

        private void TeethChecker(object sender, MouseButtonEventArgs e)
        {
            if (!checkedteeths.Contains((sender as Label).Content.ToString()))
            {
                checkedteeths += (sender as Label).Content.ToString();
                (sender as Label).Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                checkedteeths = checkedteeths.Replace((sender as Label).Content.ToString(), "");
                (sender as Label).Foreground = StandartForeground;
            }
        }

        private void ClearAllTeeth(object sender, RoutedEventArgs e)
        {
            foreach (var c in GridTab3.Children.OfType<Label>())
            {
                checkedteeths = "";
                c.Foreground = new SolidColorBrush(Colors.Gray);
                c.IsEnabled = false;
            }
        }

        private void UpdateWorkers()
        {
            DataSet ds = new();
            new OleDbDataAdapter("SELECT * FROM Workers", connection).Fill(ds, "Workers");
            dataGrid2.ItemsSource = ds.Tables["Workers"].DefaultView;
            Tab3CB3.Items.Clear();
            Tab6CB2.Items.Clear();
            foreach (DataRow row in ds.Tables["Workers"].Rows)
            {
                Tab3CB3.Items.Add(row["Имя сотрудника"].ToString());
                Tab6CB2.Items.Add(row["Имя сотрудника"].ToString());
            }
        }

        private void AddNewWorker(object sender, RoutedEventArgs e)
        {
            List<string> names = new();
            DataSet ds = new();
            new OleDbDataAdapter("SELECT * FROM Workers", connection).Fill(ds, "Workers");
            foreach (DataRow row in ds.Tables["Workers"].Rows)
                names.Add(row["Имя сотрудника"].ToString());
            if (!names.Contains(Tab5T1.Text) && Tab5T1.Text != "")
            {
                new OleDbCommand("INSERT INTO Workers ([Имя сотрудника],Должность,[Тип платы],Плата) VALUES ('" + Tab5T1.Text + "','" + Tab5CB1.Text + "','" + Tab5CB2.Text + "','" + Tab5T2.Text + "')", connection).ExecuteReader();
                notifier.ShowSuccess("Новый сотрудник добавлен.");
                UpdateWorkers();
            }
            else
                notifier.ShowWarning("Ошибки при записи нового сотрудника, возможно имя не указано или отсутствует");
        }

        private void ChangeWorker(object sender, RoutedEventArgs e)
        {
            new OleDbCommand("UPDATE Workers SET [Имя сотрудника]='" + Tab5T1.Text + "', Должность='" + Tab5CB1.Text + "', [Тип платы]='" + Tab5CB2.Text + "', Плата='" + Tab5T2.Text + "' WHERE [Имя сотрудника]='" + checkedworker + "'", connection).ExecuteReader();
            notifier.ShowInformation("Сотрудник обновлён");
            UpdateWorkers();
        }

        private void FireWorker(object sender, RoutedEventArgs e)
        {
            new OleDbCommand("DELETE * FROM Workers WHERE [Имя сотрудника]=" + $"'{checkedworker}'").ExecuteReader();
            notifier.ShowSuccess("Сотрудник уволен");
            UpdateWorkers();
        }

        private void SummByYear(object sender, RoutedEventArgs e)
        {
            DataSumm = new();
            string adapter = "SELECT labs.Номер, Labs.[Дата прихода], Labs.[Дата ухода], Comm.Цена FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE [Дата ухода] LIKE '" + $"%.{DateTime.Now.Year}" + "'";
            if (Tab6CB3.Text != "Любой статус" && Tab6CB3.Text != "")
                adapter += " AND Labs.Статус=" + $"'{Tab6CB3.Text}'";
            new OleDbDataAdapter(adapter, connection).Fill(DataSumm, "Labs");
            dataGrid3.ItemsSource = DataSumm.Tables["Labs"].DefaultView;
            Summ();
        }

        private void SummByQuartal(object sender, RoutedEventArgs e)
        {
            DataSumm = new();
            List<List<string>> quartals = new();
            quartals.Add(new List<string> { "1", "2", "3" });
            quartals.Add(new List<string> { "4", "5", "6" });
            quartals.Add(new List<string> { "7", "8", "9" });
            quartals.Add(new List<string> { "10", "11", "12" });
            foreach (var c in quartals)
                if (c.Contains(DateTime.Now.Month.ToString()))
                    foreach (var c1 in c)
                    {
                        string c2 = $"{c1}.{DateTime.Now.Year}";
                        if (c1.Length == 1)
                            c2 = $"0{c1}.{DateTime.Now.Year}";
                        string adapter = "SELECT labs.Номер, Labs.[Дата прихода], Labs.[Дата ухода], Comm.Цена FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Labs.[Дата ухода] LIKE '" + $"%{c2}%" + "'";
                        if (Tab6CB3.Text != "Любой статус" && Tab6CB3.Text != "")
                            adapter += " AND Labs.Статус=" + $"'{Tab6CB3.Text}'";
                        new OleDbDataAdapter(adapter, connection).Fill(DataSumm, "Labs");
                    }
            dataGrid3.ItemsSource = DataSumm.Tables["Labs"].DefaultView;
            Summ();
        }

        private void SummByMonth(object sender, RoutedEventArgs e)
        {
            DataSumm = new();
            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            string adapter = "SELECT labs.Номер, Labs.[Дата прихода], Labs.[Дата ухода], Comm.Цена FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Labs.[Дата ухода] LIKE '" + $"%.{month}.%" + "'";
            if (Tab6CB3.Text != "Любой статус" && Tab6CB3.Text != "")
                adapter += " AND Labs.Статус=" + $"'{Tab6CB3.Text}'";
            new OleDbDataAdapter(adapter, connection).Fill(DataSumm, "Labs");
            dataGrid3.ItemsSource = DataSumm.Tables["Labs"].DefaultView;
            Summ();
        }

        private void Summ()
        {
            double summa = 0;
            foreach (DataRow row in DataSumm.Tables["Labs"].Rows)
                if (int.TryParse(row["Цена"].ToString(), out int result))
                    summa += result;
            Tab6T1.Text = summa.ToString();
        }

        private void SetColorPreview(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            ColorPreview();
        }

        private void ColorPreview()
        {
            if (!int.TryParse(ElementsCB.Text, out int elements))
            {
                elements = 8;
            }
            else if (elements == 0)
            {
                elements = 8;
            }
            if (!int.TryParse(FontCB.Text, out int fontcombo))
            {
                fontcombo = 20;
            }
            else if (fontcombo == 0)
            {
                fontcombo = 8;
            }
            if (!int.TryParse(FontTB.Text, out int fonttext))
            {
                fonttext = 20;
            }
            else if (fonttext == 0)
            {
                fonttext = 20;
            }
            if (!int.TryParse(FontButtons.Text, out int fontbuttons))
            {
                fontbuttons = 20;
            }
            else if (fontbuttons == 0)
            {
                fontbuttons = 20;
            }
            CBPreview.MaxDropDownHeight = 1.375 * elements * fontcombo;
            CBPreview.FontSize = fontcombo;
            TBPreview.FontSize = fonttext;
            ButtonPreview.FontSize = fontbuttons;
            PreviewGrid.Background = new SolidColorBrush(ColorBack.SelectedColor);
            CBPreview.Background = new SolidColorBrush(ColorObject.SelectedColor);
            LBPreview.Background = new SolidColorBrush(ColorObject.SelectedColor);
            DPPreview.Background = new SolidColorBrush(ColorObject.SelectedColor);
            TBPreview.Background = new SolidColorBrush(ColorObject.SelectedColor);
            LabelPreview.Foreground = new SolidColorBrush(ColorText.SelectedColor);
            CBPreview.Foreground = new SolidColorBrush(ColorTextObject.SelectedColor);
            LBPreview.Foreground = new SolidColorBrush(ColorTextObject.SelectedColor);
            DPPreview.Foreground = new SolidColorBrush(ColorTextObject.SelectedColor);
            TBPreview.Foreground = new SolidColorBrush(ColorTextObject.SelectedColor);
            ElementsCB.Text = elements.ToString();
            FontCB.Text = fontcombo.ToString();
            FontTB.Text = fonttext.ToString();
            FontButtons.Text = fontbuttons.ToString();
        }

        private void SetColorPreviewText(object sender, RoutedEventArgs e)
        {
            ColorPreview();
        }

        private void SaveConfig(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(ElementsCB.Text, out int elements))
                elements = 8;
            if (!int.TryParse(FontCB.Text, out int fonts))
                fonts = 20;
            if (!int.TryParse(FontTB.Text, out int fonts1))
                fonts1 = 20;
            if (!int.TryParse(FontButtons.Text, out int fonts2))
                fonts2 = 20;
            root = new RootObject
            {
                Backcolor = ColorBack.SelectedColor.ToString(),
                Textcolor = ColorText.SelectedColor.ToString(),
                Objcolor = ColorObject.SelectedColor.ToString(),
                TextInObjcolor = ColorTextObject.SelectedColor.ToString(),
                Elements = elements,
                FontCB = fonts,
                FontTB = fonts1,
                FontButton = fonts2
            };
            var encoderSettings = new TextEncoderSettings();
            encoderSettings.AllowRanges(UnicodeRanges.BasicLatin, UnicodeRanges.CyrillicExtendedC);
            using FileStream createStream = File.Create("config.json");
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true
            };
            JsonSerializer.SerializeAsync(createStream, root, options);
            createStream.Dispose();
            notifier.ShowInformation("Настройки сохранены, данные вступят в силу после перезагрузки приложения.", this.options);
        }

        private void DefaultColors(object sender, RoutedEventArgs e)
        {

        }

        private void OpenStatistic(object sender, RoutedEventArgs e)
        {
            int ind = 0;
            switch ((sender as MenuItem).Header.ToString())
            {
                case "Статистика по клиникам":
                    ind = 1;
                    break;
                case "Статистика по клинике":
                    ind = 2;
                    break;
                case "Статистика по видам работ":
                    ind = 3;
                    break;
            }
            Window4 w4 = new(connection, root, ind);
            w4.Owner = this;
            w4.Show();
        }

        private void AddNewWork(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).Name == "Tab2M1")
                if (dataGrid1.SelectedIndex == -1)
                    return;
            Window3 w3 = new(connection, root);
            w3.Owner = this;
            w3.Show();
        }

        public void UpdateWorks()
        {
            DataSet ds = new();
            new OleDbDataAdapter("SELECT * FROM Work_types", connection).Fill(ds, "Work_types");
            dataGrid1.ItemsSource = ds.Tables["Work_types"].DefaultView;
            Tab3CB5.Items.Clear();
            ShowListOfWorks();
            foreach (DataRow row in ds.Tables["Work_types"].Rows)
                Tab3CB5.Items.Add(row["Вид работы"].ToString());
        }

        private void CreateCopyOfDataBase(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Copy(path, $"C:/Users/{Environment.UserName}/Desktop/{DateTime.Now.Year}Copy.accdb", true);
                MessageBox.Show($"База данных успешно сохранена на рабочий стол под названием {DateTime.Now.Year}Copy", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                File.Copy(path, $"{DateTime.Now.Year}Copy.accdb", true);
                MessageBox.Show($"Не удалось сохранить базу данных на рабочий стол, файл отправлен в корневую папку приложения под названием {DateTime.Now.Year}Copy", "Успешно", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CheckedWorker(object sender, SelectionChangedEventArgs e)
        {
            checkedworker = ((DataRowView)dataGrid2.SelectedItem)["Имя сотрудника"].ToString();
            Tab5T1.Text = ((DataRowView)dataGrid2.SelectedItem)["Имя сотрудника"].ToString();
            Tab5CB1.Text = ((DataRowView)dataGrid2.SelectedItem)["Должность"].ToString();
            Tab5CB2.Text = ((DataRowView)dataGrid2.SelectedItem)["Тип платы"].ToString();
            Tab5T2.Text = ((DataRowView)dataGrid2.SelectedItem)["Плата"].ToString();
            DataSet ds = new();
            new OleDbDataAdapter("SELECT Номер, [Дата прихода], [Дата ухода] FROM Labs WHERE Labs.[Ответственный сотрудник] = '" + checkedworker + "'", connection).Fill(ds, "Labs");
            dataGrid6.ItemsSource = ds.Tables["Labs"].DefaultView;
        }

        private void CheckedWorks(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.SelectedIndex == -1)
                return;
            checkedwork = ((DataRowView)dataGrid1.SelectedItem)["Вид работы"].ToString();
            Tab2T1.Text = ((DataRowView)dataGrid1.SelectedItem)["Вид работы"].ToString();
            Tab2T2.Text = ((DataRowView)dataGrid1.SelectedItem)["Цена"].ToString();
            DataSet ds = new();
            new OleDbDataAdapter("SELECT Labs.Номер AS Номер, [Дата прихода], [Дата ухода] FROM Labs INNER JOIN Works ON Labs.Номер = Works.Номер WHERE Works.[Вид работы]='" + checkedwork + "'", connection).Fill(ds, "Labs");
            dataGrid7.ItemsSource = ds.Tables["Labs"].DefaultView;
        }

        private void FindWorksOfWorker(object sender, RoutedEventArgs e)
        {
            dataGrid10.ItemsSource = null;
            if (!DateTime.TryParse(Tab6DP1.Text, out DateTime date1) || !DateTime.TryParse(Tab6DP2.Text, out DateTime date2))
            {
                MessageBox.Show("Не удаётся определить введённые даты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DataSet ds = new();
            DataSet ds1 = new();
            new OleDbDataAdapter("SELECT Labs.Номер, Labs.[Дата прихода], Labs.[Дата ухода], Comm.Цена FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Labs.Номер = -1", connection).Fill(ds1, "Labs");
            new OleDbDataAdapter("SELECT Номер, [Дата прихода], [Дата ухода] FROM Labs WHERE [Ответственный сотрудник] = '" + Tab6CB2.Text + "'", connection).Fill(ds, "Labs");
            foreach (DataRow row in ds.Tables["Labs"].Rows)
                if (DateTime.TryParse(row["Дата ухода"].ToString(), out DateTime workdate))
                    if (DateTime.Compare(date1, workdate) < 0 && DateTime.Compare(date2, workdate) > 0)
                        new OleDbDataAdapter("SELECT Labs.Номер, Labs.[Дата прихода], Labs.[Дата ухода], Comm.Цена FROM Labs INNER JOIN Comm ON Labs.Номер = Comm.Номер WHERE Labs.Номер = " + row["Номер"].ToString(), connection).Fill(ds1, "Labs");
            int allprice = 0;
            foreach (DataRow row in ds1.Tables["Labs"].Rows)
                if (int.TryParse(row["Цена"].ToString(), out int price))
                    allprice += price;
            if (ds1 != null)
                dataGrid10.ItemsSource = ds1.Tables["Labs"].DefaultView;
            Tab6T2.Text = allprice.ToString();
        }

        private void AddPart(object sender, RoutedEventArgs e)
        {
            ListOfParts.Add(new Parts()
            {
                Этап = Tab3CB6.Text,
                Начало = Tab3D3.Text,
                Конец = Tab3D4.Text
            });
            if (Tab3D4.Text != "")
                Tab3D2.Text = Tab3D4.Text;
            ShowListOfParts();
        }

        private void DeletePart(object sender, RoutedEventArgs e)
        {
            if (dataGrid9.SelectedIndex == -1)
                return;
            ListOfParts.RemoveAt(dataGrid9.SelectedIndex);
            ShowListOfParts();
        }

        private void ShowListOfParts()
        {
            List<Parts> prts1 = new();
            List<Parts> prts2 = new();
            prts2.AddRange(ListOfParts);
            while (prts2.Count > 0)
            {
                DateTime dt = DateTime.Now.AddYears(100);
                Parts j = null;
                foreach (var c in prts2)
                {
                    if (DateTime.TryParse(c.Начало, out var date))
                        if (DateTime.Compare(date, dt) <= 0)
                        {
                            dt = date;
                            j = c;
                        }
                }
                if (j != null)
                {
                    prts1.Add(j);
                    prts2.Remove(j);
                }
                int k = 0;
                foreach (var c in prts2)
                    if (!DateTime.TryParse(c.Начало, out var _))
                        k++;
                if (k == prts2.Count)
                {
                    prts1.AddRange(prts2);
                    break;
                }
            }
            dataGrid9.ItemsSource = prts1;
            dataGrid9.Items.Refresh();
        }

        private void UpdateMain(object sender, RoutedEventArgs e)
        {
            adapter = "SELECT * FROM Labs ORDER BY Номер DESC";
            SecondaryDataBaseUpdate();
        }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
    }
}
