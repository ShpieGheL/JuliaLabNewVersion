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
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        readonly OleDbConnection connection;
        List<Staff> ListOfWorks = new();
        List<Parts> ListOfParts = new();
        string checkedteeths = "";
        readonly SolidColorBrush StandartForeground = new();
        List<string> statuses = new() { "Оплачено", "Сдано", "В клинике", "В работе", "Ожидание оплаты", "Долг", "Отменён" };
        readonly string num = "";
        RootObject root;

        public Window2(string num, OleDbConnection connection, RootObject root, SolidColorBrush standart, ItemCollection clinics, ItemCollection doctors, ItemCollection workers, ItemCollection works, ItemCollection parts)
        {
            InitializeComponent();
            this.num = num;
            Title += num;
            Tab3B1.Content += num;
            this.root = root;
            this.connection = connection;
            StandartForeground = standart;
            Tab3CB1.ItemsSource = clinics;
            Tab3CB2.ItemsSource = doctors;
            Tab3CB3.ItemsSource = workers;
            Tab3CB5.ItemsSource = works;
            Tab3CB6.ItemsSource = parts;
            DataSet ds = new();
            new OleDbDataAdapter("SELECT Parts.* FROM Parts INNER JOIN Labs ON Labs.Номер = Parts.Номер WHERE Labs.Номер = " + num, connection).Fill(ds, "Parts");
            new OleDbDataAdapter("SELECT Works.* FROM Works INNER JOIN Labs ON Labs.Номер = Works.Номер WHERE Labs.Номер = " + num, connection).Fill(ds, "Works");
            new OleDbDataAdapter("SELECT Comm.* FROM Comm INNER JOIN Labs ON Labs.Номер = Comm.Номер WHERE Labs.Номер = " + num, connection).Fill(ds, "Comm");
            new OleDbDataAdapter("SELECT * FROM Labs WHERE Номер = " + num, connection).Fill(ds, "Labs");
            Tab3D1.Text = ds.Tables["Labs"].Rows[0]["Дата прихода"].ToString();
            Tab3D2.Text = ds.Tables["Labs"].Rows[0]["Дата ухода"].ToString();
            Tab3CB1.Text = ds.Tables["Labs"].Rows[0]["Название клиники"].ToString();
            Tab3CB2.Text = ds.Tables["Labs"].Rows[0]["ФИО врача"].ToString();
            Tab3T1.Text = ds.Tables["Labs"].Rows[0]["ФИО пациента"].ToString();
            Tab3CB3.Text = ds.Tables["Labs"].Rows[0]["Ответственный сотрудник"].ToString();
            Tab3CB4.SelectedIndex = statuses.IndexOf(ds.Tables["Labs"].Rows[0]["Статус"].ToString());
            Tab3D5.Text = ds.Tables["Comm"].Rows[0]["Дата Курьера"].ToString();
            Tab3T7.Text = ds.Tables["Comm"].Rows[0]["Время"].ToString();
            foreach (var c in Tab3SP1.Children.OfType<CheckBox>())
                if (c.Content.ToString() == ds.Tables["Comm"].Rows[0]["Пол"].ToString())
                    c.IsChecked = true;
            foreach (var c in Tab3SP2.Children.OfType<CheckBox>())
                if (c.Content.ToString() == ds.Tables["Comm"].Rows[0]["Тип лица"].ToString())
                    c.IsChecked = true;
            foreach (var c in Tab3SP3.Children.OfType<CheckBox>())
                if (c.Content.ToString() == ds.Tables["Comm"].Rows[0]["Челюсть"].ToString())
                    c.IsChecked = true;
            Tab3T2.Text = ds.Tables["Comm"].Rows[0]["Возраст"].ToString();
            Tab3T3.Text = ds.Tables["Comm"].Rows[0]["Цвет зубов"].ToString();
            Tab3T4.Text = ds.Tables["Comm"].Rows[0]["Комментарий"].ToString();
            int i = 1;
            string c1 = "";
            foreach (char c in ds.Tables["Comm"].Rows[0]["Зубы"].ToString())
            {
                c1 += c;
                if (i % 2==0)
                {
                    foreach (var l in Gt3.Children.OfType<Label>())
                        if (l.Content.ToString() == c1)
                        {
                            checkedteeths += c1;
                            l.Foreground = new SolidColorBrush(Colors.Red);
                        }
                    c1 = "";
                }
                i++;
            }
            foreach (DataRow row in ds.Tables["Works"].Rows)
            {
                ListOfWorks.Add(new Staff
                {
                    Тип = row["Вид работы"].ToString(),
                    Количество = Convert.ToInt32(row["Количество"].ToString()),
                    Цена = Convert.ToInt32(row["Цена"].ToString()),
                    Итог = Convert.ToInt32(row["Цена"].ToString()) * Convert.ToInt32(row["Количество"].ToString())
                });
            }
            ShowListOfWorks();
            foreach (DataRow row in ds.Tables["Parts"].Rows)
            {
                ListOfParts.Add(new Parts
                {
                    Этап = row["Название этапа"].ToString(),
                    Начало = row["Дата прихода"].ToString(),
                    Конец = row["Дата ухода"].ToString()
                });
            }
            ShowListOfParts();
            Setcolor();
        }

        private void Setcolor()
        {
            var objcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Objcolor));
            var backcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Backcolor));
            var textcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Textcolor));
            var textinobj = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.TextInObjcolor));
            var TBfontsize = root.FontTB;
            var Bfontsize = root.FontButton;
            var elements = root.Elements;
            var CBfontsize = root.FontCB;
            G3.Background = backcolor;
            dataGrid8.Background = backcolor;
            dataGrid9.Background = backcolor;
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

        private void CheckedSkull(object sender, RoutedEventArgs e)
        {
            int[] x = new int[] { 10, 20, 30 };
            if ((sender as CheckBox).Name == "Tab3Ch7")
                x = new int[] { 30, 40, 50 };
            foreach (var l in Gt3.Children.OfType<Label>())
                if (l.Content.ToString() != "")
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
                new OleDbCommand("UPDATE Labs SET [Дата прихода]='" + Tab3D1.Text + "',[Дата ухода]='" + Tab3D2.Text + "',[Название клиники]='" + Tab3CB1.Text + "',[ФИО врача]='" + Tab3CB2.Text + "',[ФИО пациента]='" + Tab3T1.Text + "',Работы='" + works + "',[Ответственный сотрудник]='" + Tab3CB3.Text + "',Статус='" + Tab3CB4.Text + "' WHERE Номер=" + num, connection).ExecuteReader();
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
                new OleDbCommand("UPDATE Comm SET Пол='" + sex + "',[Тип лица]='" + form + "',Возраст='" + Tab3T2.Text + "',[Цвет зубов]='" + Tab3T3.Text + "',Челюсть='" + skull + "',Зубы='" + checkedteeths + "',Комментарий='" + Tab3T4.Text + "',Цена=" + price + ",[Дата курьера]='" + Tab3D5.Text + "',Время='" + Tab3T7.Text + "' WHERE Номер=" + num, connection).ExecuteReader();
                new OleDbCommand("DELETE * FROM Parts WHERE Номер=" + num, connection).ExecuteScalar();
                new OleDbCommand("DELETE * FROM Works WHERE Номер=" + num, connection).ExecuteScalar();
                foreach (var c in ListOfParts)
                    new OleDbCommand("INSERT INTO Parts ([Название этапа],[Дата прихода],[Дата ухода],Номер) VALUES ('" + c.Этап + "','" + c.Начало + "','" + c.Конец + "'," + num + ")", connection).ExecuteReader();
                foreach (var c in ListOfWorks)
                    new OleDbCommand("INSERT INTO Works ([Вид работы], Количество, Цена, Номер) VALUES ('" + c.Тип + "','" + c.Количество + "','" + c.Цена + "'," + num + ")", connection).ExecuteReader();
                (Owner as MainWindow).SecondaryDataBaseUpdate();
                if (MessageBox.Show("Данные успешно обновлены, закрыть окно?", "Успех", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    Close();
            }
            else
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            foreach (var c in Gt3.Children.OfType<Label>())
            {
                checkedteeths = "";
                c.Foreground = new SolidColorBrush(Colors.Gray);
                c.IsEnabled = false;
            }
        }

        private void AddType(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Tab3T6.Text, out int result))
                result = 1;
            DataSet ds = new();
            new OleDbDataAdapter("SELECT Цена FROM Work_types WHERE [Вид работы] = '" + Tab3CB5.Text + "'", connection).Fill(ds, "Work_types");
            ListOfWorks.Add(new Staff()
            {
                Тип = Tab3CB5.Text,
                Количество = result,
                Цена = Convert.ToInt32(ds.Tables["Work_types"].Rows[0]["Цена"].ToString()),
                Итог = Convert.ToInt32(ds.Tables["Work_types"].Rows[0]["Цена"].ToString()) * result
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

        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            ListOfWorks.Clear();
            ShowListOfWorks();
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

        private void Tab3CB4_I(object sender, RoutedEventArgs e)
        {
            Tab3CB4.Foreground = (sender as ComboBoxItem).Foreground;
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
    }
}
