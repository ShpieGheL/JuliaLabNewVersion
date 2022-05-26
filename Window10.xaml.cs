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
    /// Логика взаимодействия для Window10.xaml
    /// </summary>
    public partial class Window10 : Window
    {
        OleDbConnection connection;
        readonly int n;
        public Window10()
        {
            InitializeComponent();
        }

        public Window10(string i1, string i2, string i3, string i4, RootObject root, OleDbConnection con) : this()
        {
            _ = int.TryParse(i1, out int f);
            n = f;
            connection = con;
            T1.Text = i2;
            C1.Text = i3;
            T2.Text = i4;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = int.TryParse(T2.Text, out int d);
            OleDbCommand co = new("UPDATE Tax SET Описание='" + T1.Text + "', Тип='" + C1.Text + "', Плата=" + d + " WHERE Номер=" + n, connection);
            co.ExecuteNonQuery();
            MessageBox.Show("Информация о налоге успешно изменена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
