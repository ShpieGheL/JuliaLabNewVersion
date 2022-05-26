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

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window9.xaml
    /// </summary>
    public partial class Window9 : Window
    {
        OleDbConnection connection;
        public Window9()
        {
            InitializeComponent();
        }

        public Window9(RootObject root, OleDbConnection con) : this()
        {
            connection = con;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = int.TryParse(T2.Text, out int p);
            if (C1.Text != "")
            {
                OleDbCommand CO = new OleDbCommand("INSERT INTO Tax (Описание, Тип, Плата) VALUES ('" + T1.Text + "', '" + C1.Text + "', " + p + ")", connection);
                CO.ExecuteNonQuery();
                MessageBox.Show("Запись налога успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Не указан тип налога.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
