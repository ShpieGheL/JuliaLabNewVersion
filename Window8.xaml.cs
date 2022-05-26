using System;
using System.Windows;
using System.Data.OleDb;
using System.Data;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window8.xaml
    /// </summary>
    public partial class Window8 : Window
    {
        OleDbConnection connection;
        int n;
        string d1;
        public Window8()
        {
            InitializeComponent();
        }

        public Window8(string i, string d, string op, string mp, string mm, RootObject root, OleDbConnection con) : this()
        {
            d1 = d;
            n = Convert.ToInt32(i);
            connection = con;
            D1.Text = d;
            T1.Text = op;
            T2.Text = mp;
            T3.Text = mm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand co = new("UPDATE Control SET Дата='" + D1.SelectedDate + "', Описание='" + T1.Text + "', Заработок=" + T2.Text + ", Траты=" + T3.Text + " WHERE Номер=" + n, connection);
            co.ExecuteNonQuery();
            MessageBox.Show("Запись успешно изменена.", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
