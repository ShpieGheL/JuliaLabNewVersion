using System.Windows;
using System.Data.OleDb;
using System.Windows.Media;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        readonly OleDbConnection connection;
        RootObject root;

        public Window3(OleDbConnection connection, RootObject root)
        {
            this.connection = connection;
            this.root = root;
            InitializeComponent();
            Setcolor();
        }

        private void Setcolor()
        {
            var objcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Objcolor));
            var backcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Backcolor));
            var textcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.Textcolor));
            var textinobj = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root.TextInObjcolor));
            var TBfontsize = root.FontTB;
            G1.Background = backcolor;
            Tab1L1.Foreground = textcolor;
            Tab1L2.Foreground = textcolor;
            Tab1T1.Background = objcolor;
            Tab1T2.Background = objcolor;
            Tab1T1.Foreground = textinobj;
            Tab1T2.Foreground = textinobj;
            B1.FontSize = TBfontsize;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string error = "Ошибки при добавлении работы:\n";
            if (!int.TryParse(Tab1T2.Text, out int price))
                error += "\nНеверно написана цена.";
            if (Tab1T1.Text == "")
                error += "\nНе указано название.";
            if (error == "Ошибки при добавлении работы:\n")
            {
                new OleDbCommand("INSERT INTO Work_types ([Вид работы],Цена) VALUES ('" + Tab1T1.Text + "','" + price + "')", connection).ExecuteReader();
                (this.Owner as MainWindow).UpdateWorks();
                MessageBox.Show("Новый вид работы успешно добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}