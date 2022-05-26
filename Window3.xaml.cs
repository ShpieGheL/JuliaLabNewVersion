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
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        readonly OleDbConnection connection;
        public Window3()
        {
            InitializeComponent();
        }

        public Window3(OleDbConnection connection, RootObject root)
        {
            this.connection = connection;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OleDbDataAdapter da = new("SELECT * FROM Work_types", connection);
            DataSet ds = new();
            da.Fill(ds, "Work_types");
            if (Tab1T1.Text != "" || Tab1T2.Text != "")
            {
                OleDbCommand co = new("INSERT INTO Work_types ([Вид работы],Цена) VALUES ('" + Tab1T1.Text + "','" + Convert.ToInt32(Tab1T2.Text) + "')", connection);
                co.ExecuteNonQuery();
                MessageBox.Show("Вид работы успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Не введены некоторые данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}