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
        public OleDbConnection connection;
        public OleDbDataAdapter da;
        public DataSet ds;
        public string Back = "";
        public string Text = "";
        public string Set = "";
        public char c = ' ';
        public string pathdb;
        string num1;
        DateTime d2;
        string[] t;
        public Window2()
        {
            InitializeComponent();
            Tab3I2.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}/TN.png"));
            Tab3I1.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}/TNI.png"));
        }
        public Window2(string num, OleDbConnection connection, RootObject root) : this()
        {
            num1 = num;
            this.connection = connection;
            this.Title = $"Наряд №{num}";
            Tab3B1.Content = $"Изменить наряд №{num}";
            da = new OleDbDataAdapter("SELECT * FROM Labs WHERE Номер=" + num, connection);
            ds = new DataSet();
            da.Fill(ds, "Labs");
            foreach (DataRow row in ds.Tables["Labs"].Rows)
            {
                if (row["Номер"].ToString() == num)
                {
                    Tab3D1.Text = row["Дата прихода"].ToString();
                    Tab3D2.Text = row["Дата ухода"].ToString();
                    if (row["Дата ухода"].ToString() != "")
                        d2 = Convert.ToDateTime(row["Дата ухода"].ToString());
                    Tab3CB1.Text = row["Название клиники"].ToString();
                    Tab3CB2.Text = row["ФИО врача"].ToString();
                    Tab3CB3.Text = row["Ответственный сотрудник"].ToString();
                    Tab3T1.Text = row["ФИО пациента"].ToString();
                    Tab3CB4.Text = row["Статус"].ToString();
                }
            }
            da = new OleDbDataAdapter("SELECT * FROM Labs", connection);
            ds = new DataSet();
            da.Fill(ds, "Labs");
            foreach (DataRow row in ds.Tables["Labs"].Rows)
            {
                if (Tab3CB1.Items.Contains(row["Название клиники"].ToString()) == false && (row["Название клиники"].ToString() != "" || row["Название клиники"].ToString() != null))
                    Tab3CB1.Items.Add(row["Название клиники"].ToString());
                if (Tab3CB2.Items.Contains(row["ФИО врача"].ToString()) == false && (row["ФИО врача"].ToString() != "" || row["ФИО врача"].ToString() != null))
                    Tab3CB2.Items.Add(row["ФИО врача"].ToString());
                if (Tab3CB3.Items.Contains(row["Ответственный сотрудник"].ToString()) == false && (row["Ответственный сотрудник"].ToString() != "" || row["Ответственный сотрудник"].ToString() != null))
                    Tab3CB3.Items.Add(row["Ответственный сотрудник"].ToString());
            }
            da = new OleDbDataAdapter("SELECT * FROM Work_types", connection);
            ds = new DataSet();
            da.Fill(ds, "Work_types");
            foreach (DataRow l in ds.Tables["Work_types"].Rows)
                Tab3CB5.Items.Add(l["Вид работы"].ToString());
            da = new OleDbDataAdapter("SELECT * FROM Comm WHERE Номер=" + num, connection);
            ds = new DataSet();
            da.Fill(ds, "Comm");
            Array.Resize(ref t, 1);
            SolidColorBrush col = new SolidColorBrush(Colors.Red);
            foreach (DataRow row in ds.Tables["Comm"].Rows)
            {
                int i = 0;
                foreach (char c in row["Зубы"].ToString())
                {
                    t[t.Length - 1] += c;
                    i++;
                    if (i == 2)
                    {
                        Array.Resize(ref t, t.Length + 1);
                        i = 0;
                    }
                }
                foreach (string l in t)
                    foreach (Label n in G3.Children.OfType<Label>())
                        if (n.Name == $"L{l}")
                            n.Foreground = col;
                Tab3D3.Text = row["Дата курьера"].ToString();
                TabT6.Text = row["Время"].ToString();
                Tab3D5.Text = row["Дата Курьера"].ToString();
            }
            foreach (DataRow row in ds.Tables["Comm"].Rows)
            {
                switch (row["Пол"].ToString())
                {
                    case "Мужской":
                        Tab3Ch1.IsChecked = true;
                        break;
                    case "Женский":
                        Tab3Ch2.IsChecked = true;
                        break;
                }
                switch (row["Челюсть"].ToString())
                {
                    case "Верхняя":
                        Tab3Ch6.IsChecked = true;
                        break;
                    case "Нижняя":
                        Tab3Ch7.IsChecked = true;
                        break;
                }
                switch (row["Тип лица"].ToString())
                {
                    case "Круглое":
                        Tab3Ch3.IsChecked = true;
                        break;
                    case "Треугольное":
                        Tab3Ch4.IsChecked = true;
                        break;
                    case "Квадратное":
                        Tab3Ch5.IsChecked = true;
                        break;
                }
                Tab3T2.Text = row["Возраст"].ToString();
                Tab3T3.Text = row["Цвет зубов"].ToString();
                Tab3T4.Text = row["Комментарий"].ToString();
                Tab3T5.Text = row["Цена"].ToString();
            }
            da = new OleDbDataAdapter("SELECT * FROM Part_types", connection);
            ds = new DataSet();
            da.Fill(ds, "Part_types");
            foreach (DataRow row in ds.Tables["Part_types"].Rows)
                Tab3CB6.Items.Add(row["Название этапа"].ToString());
            da = new OleDbDataAdapter("SELECT * FROM Works WHERE Номер=" + num1, connection);
            ds = new DataSet();
            da.Fill(ds, "Works");
            foreach (DataRow row in ds.Tables["Works"].Rows)
            {
                Tab3LB1.Items.Add(row["Вид работы"].ToString());
                Tab3LB2.Items.Add(row["Количество"].ToString());
                Tab3LB3.Items.Add(row["Цена"].ToString());
            }
            da = new OleDbDataAdapter("SELECT * FROM Parts WHERE Номер=" + num1, connection);
            ds = new DataSet();
            da.Fill(ds, "Parts");
            foreach (DataRow row in ds.Tables["Parts"].Rows)
            {
                Tab3LB4.Items.Add(row["Название этапа"].ToString());
                Tab3LB5.Items.Add(row["Дата прихода"].ToString());
                Tab3LB6.Items.Add(row["Дата ухода"].ToString());
            }
        }

        private void ALot(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Tab3L25.Content = $"Кол-во: {Tab3S2.Value}";
        }

        private void AddType(object sender, RoutedEventArgs e)
        {
            OleDbConnection connection = new OleDbConnection($@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={pathdb}");
            connection.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Work_types", connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "Work_types");
            string pr = "";
            foreach (DataRow row in ds.Tables["Work_types"].Rows)
                if (row["Вид работы"].ToString() == Tab3CB5.Text)
                    pr = (Convert.ToInt32(row["Цена"]) * Tab3S2.Value).ToString();
            try
            {
                Tab3T5.Text = (Convert.ToInt32(pr) + Convert.ToInt32(Tab3T5.Text)).ToString();
            }
            catch (Exception)
            {
                Tab3T5.Text = pr;
            }
            Tab3LB1.Items.Add(Tab3CB5.Text);
            Tab3LB2.Items.Add(Tab3S2.Value.ToString());
            Tab3LB3.Items.Add(pr);
        }

        private void Tab3LB1_S(object sender, SelectionChangedEventArgs e)
        {
            Tab3LB2.SelectedIndex = Tab3LB1.SelectedIndex;
            Tab3LB3.SelectedIndex = Tab3LB1.SelectedIndex;
        }

        private void Tab3LB2_S(object sender, SelectionChangedEventArgs e)
        {
            Tab3LB1.SelectedIndex = Tab3LB2.SelectedIndex;
            Tab3LB3.SelectedIndex = Tab3LB2.SelectedIndex;
        }

        private void Tab3LB3_S(object sender, SelectionChangedEventArgs e)
        {
            Tab3LB1.SelectedIndex = Tab3LB3.SelectedIndex;
            Tab3LB2.SelectedIndex = Tab3LB3.SelectedIndex;
        }

        private void Tab2CB4_I1(object sender, RoutedEventArgs e)
        {
            Color c = Color.FromRgb(0, 128, 0);
            SolidColorBrush co = new SolidColorBrush(c);
            Tab3CB4.Foreground = co;
        }

        private void Tab2CB4_I2(object sender, RoutedEventArgs e)
        {
            Color c = Color.FromRgb(144, 238, 144);
            SolidColorBrush co = new SolidColorBrush(c);
            Tab3CB4.Foreground = co;
        }

        private void Tab2CB4_I3(object sender, RoutedEventArgs e)
        {
            Color c = Color.FromRgb(222, 184, 135);
            SolidColorBrush co = new SolidColorBrush(c);
            Tab3CB4.Foreground = co;
        }

        private void Tab2CB4_I4(object sender, RoutedEventArgs e)
        {
            Color c = Color.FromRgb(210, 105, 30);
            SolidColorBrush co = new SolidColorBrush(c);
            Tab3CB4.Foreground = co;
        }

        private void Tab2CB4_I5(object sender, RoutedEventArgs e)
        {
            Color c = Color.FromRgb(255, 0, 0);
            SolidColorBrush co = new SolidColorBrush(c);
            Tab3CB4.Foreground = co;
        }

        private void Tab3Ch1_C(object sender, RoutedEventArgs e)
        {
            Tab3Ch2.IsChecked = false;
        }

        private void Tab3Ch2_C(object sender, RoutedEventArgs e)
        {
            Tab3Ch1.IsChecked = false;
        }

        private void Tab3Ch3_C(object sender, RoutedEventArgs e)
        {
            Tab3Ch4.IsChecked = false;
            Tab3Ch5.IsChecked = false;
        }

        private void Tab3Ch4_C(object sender, RoutedEventArgs e)
        {
            Tab3Ch3.IsChecked = false;
            Tab3Ch5.IsChecked = false;
        }

        private void Tab3Ch5_C(object sender, RoutedEventArgs e)
        {
            Tab3Ch3.IsChecked = false;
            Tab3Ch4.IsChecked = false;
        }

        private void L18L(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush col1 = new SolidColorBrush(Colors.Red);
            SolidColorBrush col2 = new SolidColorBrush(Colors.Black);
            foreach (Label l in G3.Children.OfType<Label>())
            {
                var c = (Color)ColorConverter.ConvertFromString(l.Foreground.ToString());
                if (l.ToString() == sender.ToString() && c == Colors.Black)
                    l.Foreground = col1;
                if (l.ToString() == sender.ToString() && c == Colors.Red)
                    l.Foreground = col2;
            }
        }


        private void Tab3B4_Click(object sender, RoutedEventArgs e)
        {
            foreach (Label l in G3.Children.OfType<Label>())
                for (int i = 11; i <= 48; i++)
                    if (l.Name == $"L{i}" && l.IsEnabled == true)
                        l.BorderThickness = new Thickness(0);
        }

        private void PartDel2(object sender, RoutedEventArgs e)
        {
            if (Tab3LB4.SelectedIndex != -1)
            {
                int ind = Tab3LB4.SelectedIndex;
                Tab3LB4.Items.RemoveAt(ind);
                Tab3LB5.Items.RemoveAt(ind);
                Tab3LB6.Items.RemoveAt(ind);
            }
        }

        private void PartDel1(object sender, RoutedEventArgs e)
        {
            if (Tab3LB1.SelectedIndex != -1)
            {
                int ind = Tab3LB1.SelectedIndex;
                Tab3LB1.Items.RemoveAt(ind);
                Tab3LB2.Items.RemoveAt(ind);
                Tab3LB3.Items.RemoveAt(ind);
            }
        }

        private void Tab3LB4_S(object sender, SelectionChangedEventArgs e)
        {
            Tab3LB5.SelectedIndex = Tab3LB4.SelectedIndex;
            Tab3LB6.SelectedIndex = Tab3LB4.SelectedIndex;
        }

        private void Tab3LB5_S(object sender, SelectionChangedEventArgs e)
        {
            Tab3LB4.SelectedIndex = Tab3LB5.SelectedIndex;
            Tab3LB6.SelectedIndex = Tab3LB5.SelectedIndex;
        }

        private void Tab3LB6_S(object sender, SelectionChangedEventArgs e)
        {
            Tab3LB5.SelectedIndex = Tab3LB6.SelectedIndex;
            Tab3LB4.SelectedIndex = Tab3LB6.SelectedIndex;
        }

        private void Tab3Ch6_Checked(object sender, RoutedEventArgs e)
        {
            SolidColorBrush col2 = new SolidColorBrush(Colors.Red);
            SolidColorBrush col1 = new SolidColorBrush(Colors.Black);
            SolidColorBrush col = new SolidColorBrush(Colors.Gray);
            Tab3Ch7.IsChecked = false;
            foreach (Label l in G3.Children.OfType<Label>())
                for (int i = 11; i <= 28; i++)
                {
                    if (l.Name == $"L{i}")
                    {
                        l.IsEnabled = true;
                        l.Foreground = col1;
                    }
                }
            foreach (string l in t)
                foreach (Label n in G3.Children.OfType<Label>())
                    if (n.Name == $"L{l}")
                        n.Foreground = col2;
            foreach (Label l in G3.Children.OfType<Label>())
                for (int i = 31; i <= 48; i++)
                    if (l.Name == $"L{i}")
                    {
                        l.IsEnabled = false;
                        l.Foreground = col;
                    }
        }

        private void Tab3Ch7_Checked(object sender, RoutedEventArgs e)
        {
            SolidColorBrush col1 = new SolidColorBrush(Colors.Black);
            SolidColorBrush col = new SolidColorBrush(Colors.Gray);
            SolidColorBrush col2 = new SolidColorBrush(Colors.Red);
            Tab3Ch6.IsChecked = false;
            foreach (Label l in G3.Children.OfType<Label>())
                for (int i = 31; i <= 48; i++)
                    if (l.Name == $"L{i}")
                    {
                        l.IsEnabled = true;
                        l.Foreground = col1;
                    }
            foreach (string l in t)
                foreach (Label n in G3.Children.OfType<Label>())
                    if (n.Name == $"L{l}")
                        n.Foreground = col2;
            foreach (Label l in G3.Children.OfType<Label>())
                for (int i = 11; i <= 28; i++)
                    if (l.Name == $"L{i}")
                    {
                        l.IsEnabled = false;
                        l.Foreground = col;
                    }
        }

        private void Tab3B1_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand co = new("DELETE FROM Parts WHERE Номер=" + num1, connection);
            co.ExecuteScalar();
            co = new OleDbCommand("DELETE FROM Works WHERE Номер=" + num1, connection);
            co.ExecuteScalar();
            OleDbDataAdapter da = new("SELECT * FROM Labs", connection);
            DataSet ds = new DataSet();
            da.Fill(ds, "Labs");
            string[] l1 = new string[Tab3LB1.Items.Count];
            int ik = 0;
            string s = "";
            foreach (string l in Tab3LB3.Items)
            {
                l1[ik] = l;
                ik++;
            }
            ik = 0;
            foreach (string l in Tab3LB1.Items)
            {
                s += $"{l} ({l1[ik]})";
                if (ik != l1.Length - 1)
                    s += Environment.NewLine;
                ik++;
            }
            co = new OleDbCommand("UPDATE Labs SET [Дата прихода]='" + Tab3D1.Text + "',[Дата ухода]='" + Tab3D2.Text + "',[Название клиники]='" + Tab3CB1.Text + "',[ФИО врача]='" + Tab3CB2.Text + "',[ФИО пациента]='" + Tab3T1.Text + "',Работы='" + s + "',[Ответственный сотрудник]='" + Tab3CB3.Text + "',Статус='" + Tab3CB4.Text + "' WHERE Номер=" + num1, connection);
            co.ExecuteNonQuery();
            string sex = "";
            if (Tab3Ch1.IsChecked == true)
                sex = "Мужской";
            if (Tab3Ch2.IsChecked == true)
                sex = "Женский";
            string face = "";
            if (Tab3Ch3.IsChecked == true)
                face = "Круглое";
            if (Tab3Ch4.IsChecked == true)
                face = "Треугольное";
            if (Tab3Ch5.IsChecked == true)
                face = "Квадратное";
            string j = "";
            string sk = "";
            if (Tab3Ch6.IsChecked == true)
                sk = "Верхняя";
            if (Tab3Ch7.IsChecked == true)
                sk = "Нижняя";
            SolidColorBrush col = new SolidColorBrush(Colors.Red);
            foreach (Label l in G3.Children.OfType<Label>())
                for (int i = 11; i <= 48; i++)
                    if (l.Name == $"L{i}" && col.ToString() == l.Foreground.ToString())
                        j += l.Content.ToString();
            int comm = 0;
            try
            {
                comm = Convert.ToInt32(Tab3T5.Text);
            }
            catch
            {
            }
            co = new OleDbCommand("UPDATE Comm SET Пол='" + sex + "',[Тип лица]='" + face + "',Возраст='" + Tab3T2.Text + "',[Цвет зубов]='" + Tab3T3.Text + "',Челюсть='" + sk + "',Зубы='" + j + "',Комментарий='" + Tab3T4.Text + "',Цена=" + comm + ",[Дата курьера]='" + Tab3D5.Text + "',Время='" + TabT6.Text + "' WHERE Номер=" + num1, connection);
            co.ExecuteNonQuery();
            int p = 0;
            string m1, m2;
            foreach (string l in Tab3LB4.Items)
            {
                Tab3LB5.SelectedIndex = p;
                Tab3LB6.SelectedIndex = p;
                m1 = Tab3LB5.SelectedItem.ToString();
                m2 = Tab3LB6.SelectedItem.ToString();
                co = new OleDbCommand("INSERT INTO Parts ([Название этапа],[Дата прихода],[Дата ухода],Номер) VALUES ('" + l + "','" + m1 + "','" + m2 + "'," + num1 + ")", connection);
                co.ExecuteNonQuery();
                p++;
            }
            p = 0;
            foreach (string l in Tab3LB1.Items)
            {
                Tab3LB2.SelectedIndex = p;
                Tab3LB3.SelectedIndex = p;
                m1 = Tab3LB2.SelectedItem.ToString();
                m2 = Tab3LB3.SelectedItem.ToString();
                co = new OleDbCommand("INSERT INTO Works ([Вид работы],Количество,Цена,Номер) VALUES ('" + l + "','" + m1 + "','" + m2 + "'," + num1 + ")", connection);
                co.ExecuteNonQuery();
                p++;
            }
            co = new OleDbCommand("DELETE FROM Control WHERE Описание='" + $"Прайс наряда №{num1}" + "'", connection);
            co.ExecuteNonQuery();
            if (Tab3CB4.Text == "Оплачено")
            {
                DateTime d1 = d2;
                try
                {
                    d1 = Convert.ToDateTime(Tab3D2.Text);
                }
                catch
                {
                }
                co = new OleDbCommand("INSERT INTO Control (Дата, Описание, Заработок, Траты) VALUES ('" + d1 + "','" + $"Прайс наряда №{num1.ToString()}" + "'," + comm + ",0)", connection);
                co.ExecuteNonQuery();
            }
            MessageBox.Show("Наряд успешно изменён.", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Tab3CB6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Tab3LB4.Items.Add(Tab3CB6.Text);
                Tab3LB5.Items.Add(Tab3D3.Text);
                Tab3LB6.Items.Add(Tab3D4.Text);
                Tab3D2.SelectedDate = Tab3D4.SelectedDate;
            }
        }
    }
}
