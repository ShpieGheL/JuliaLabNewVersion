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

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
        public Window5()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (this.Owner as MainWindow).IsAuth(Psw.Password);
            if ((this.Owner as MainWindow).auth)
            {
                MessageBox.Show("Верный пароль", "Доступ получен", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
                MessageBox.Show("Неверный пароль", "Доступ отклонён", MessageBoxButton.OK, MessageBoxImage.Warning);
            Psw.Password = "";
        }
    }
}
