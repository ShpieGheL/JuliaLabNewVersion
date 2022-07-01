using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Concurrent;
using System.Windows.Media;
using System.Data;
using System.Data.OleDb;

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public int iop = 0;
        public char c = ' ';
        public OleDbConnection connection;
        public DataSet ds;
        public DataSet ds1;
        RootObject root;
        public Window1(OleDbConnection connection, RootObject root)
        {
            InitializeComponent();
            this.connection = connection;
            this.root = root;
            ds = new DataSet();
            new OleDbDataAdapter("SELECT * FROM Labs", connection).Fill(ds, "Labs");
            ds1 = new DataSet();
            new OleDbDataAdapter("SELECT * FROM Work_types", connection).Fill(ds1, "Work_types");
            foreach (DataRow row in ds.Tables["Labs"].Rows)
            {
                if (!C1.Items.Contains(row["Дата прихода"].ToString()))
                    C1.Items.Add(row["Дата прихода"].ToString());
                if (!C2.Items.Contains(row["Дата ухода"].ToString()))
                    C2.Items.Add(row["Дата ухода"].ToString());
                if (!C3.Items.Contains(row["Название клиники"].ToString()))
                    C3.Items.Add(row["Название клиники"].ToString());
                if (!C4.Items.Contains(row["ФИО врача"].ToString()))
                    C4.Items.Add(row["ФИО врача"].ToString());
                if (!C5.Items.Contains(row["ФИО пациента"].ToString()))
                    C5.Items.Add(row["ФИО пациента"].ToString());
                foreach (DataRow row1 in ds1.Tables["Work_types"].Rows)
                    if (row["Работы"].ToString().Contains(row1["Вид работы"].ToString()) && !C6.Items.Contains(row1["Вид работы"].ToString()))
                        C6.Items.Add(row1["Вид работы"].ToString());
                if (!C7.Items.Contains(row["Ответственный сотрудник"].ToString()))
                    C7.Items.Add(row["Ответственный сотрудник"].ToString());
                if (!C8.Items.Contains(row["Статус"].ToString()))
                    C8.Items.Add(row["Статус"].ToString());
                if (!C9.Items.Contains(row["Номер"].ToString()))
                    C9.Items.Add(row["Номер"].ToString());
            }
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
            G1.Background = backcolor;
            foreach (var c in G1.Children.OfType<Label>())
                c.Foreground = textcolor;
            foreach (var c in G1.Children.OfType<ComboBox>())
            {
                c.Background = objcolor;
                c.Foreground = textinobj;
                c.FontSize = CBfontsize;
            }
            B1.FontSize = Bfontsize;
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            ConcurrentDictionary<ComboBox, string> items = new();
            foreach (var c in G1.Children.OfType<ComboBox>())
                if (c.Text != "")
                {
                    if (c == C1)
                        items.TryAdd(c, "Labs.[Дата прихода]");
                    if (c == C2)
                        items.TryAdd(c, "Labs.[Дата ухода]");
                    if (c == C3)
                        items.TryAdd(c, "Labs.[Название клиники]");
                    if (c == C4)
                        items.TryAdd(c, "Labs.[ФИО врача]");
                    if (c == C5)
                        items.TryAdd(c, "Labs.[ФИО пациента]");
                    if (c == C6)
                        items.TryAdd(c, "Works.[Вид работы]");
                    if (c == C7)
                        items.TryAdd(c, "Labs.[Ответственный сотрудник]");
                    if (c == C8)
                        items.TryAdd(c, "Labs.Статус");
                    if (c == C9)
                        items.TryAdd(c, "Labs.Номер");
                }    
            string adapter = "SELECT Labs.* FROM Labs";
            if (C6.Text != "")
                adapter += " INNER JOIN Works ON Labs.Номер = Works.Номер";
            int i = 0;
            foreach (var c in items.Keys)
            {
                items.TryGetValue(c, out var table);
                if (i == 0)
                    adapter += " WHERE ";
                if (i == items.Count - 1)
                    adapter += $"{table}=" + $"'{c.Text}'";
                else
                    adapter += $"{table}=" + $"'{c.Text}' AND ";
                i++;
            }
            var ow = Owner as MainWindow;
            ow.adapter = adapter + " ORDER BY Номер DESC";
            ow.SecondaryDataBaseUpdate();
        }
    }
}
