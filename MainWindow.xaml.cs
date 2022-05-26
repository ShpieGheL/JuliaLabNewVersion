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

namespace JuliaUpgrade2._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OleDbConnection connection = new();
        private DataSet allnumbers = new();
        private DataSet allworks = new();
        private DataSet allworkers = new();
        List<Staff> ListOfWorks = new();
        List<List<Staff>> data = new();

        RootObject root;
        private string path = File.ReadAllText("path.txt");

        public MainWindow()
        {
            InitializeComponent();
            root = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json", optional: false).Build().Get<RootObject>();
            DataBasePathChecker();
        }

        private void OpenDataBase()
        {
            TabT1.Items.Clear();
            Tab3CB1.Items.Clear();
            Tab3CB2.Items.Clear();
            Tab3CB3.Items.Clear();
            Tab3CB5.Items.Clear();
            Tab3CB6.Items.Clear();
            Tab5CB1.Items.Clear();
            Tab5CB2.Items.Clear();
            Tab6CB2.Items.Clear();
            Tab2T1.Text = "";
            Tab2T2.Text = "";
            dataGrid7.ItemsSource = null;
            dataGrid8.ItemsSource = null;
            dataGrid9.ItemsSource = null;
            dataGrid10.ItemsSource = null;
            dataGrid.ItemsSource = allnumbers.Tables["Labs"].DefaultView;
            foreach (DataRow row in allnumbers.Tables["Labs"].Rows)
            {
                TabT1.Items.Add(row["Номер"].ToString());
                if (!Tab3CB1.Items.Contains(row["Название клиники"].ToString()) && row["Название клиники"].ToString().Replace(" ","") != "")
                    Tab3CB1.Items.Add(row["Название клиники"].ToString());
                if (!Tab3CB2.Items.Contains(row["ФИО врача"].ToString()) && row["ФИО врача"].ToString().Replace(" ", "") != "")
                    Tab3CB2.Items.Add(row["ФИО врача"].ToString());
            }
            dataGrid1.ItemsSource = allworks.Tables["Work_types"].DefaultView;
            foreach (DataRow row in allworks.Tables["Work_types"].Rows)
                Tab3CB5.Items.Add(row["Вид работы"].ToString());
            dataGrid2.ItemsSource = allworkers.Tables["Workers"].DefaultView;
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
                this.connection = new OleDbConnection($@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path}");
                allnumbers = new();
                allworks = new();
                new OleDbDataAdapter("SELECT * FROM Labs ORDER BY Номер DESC", connection).Fill(allnumbers, "Labs");
                new OleDbDataAdapter("SELECT * FROM Work_types", connection).Fill(allworks, "Work_types");
                new OleDbDataAdapter("SELECT * FROM Workers", connection).Fill(allworkers, "Workers");
                dataGrid.Visibility = Visibility.Visible;
                OpenDataBase();
            }
            catch
            {
                //dataGrid.Visibility = Visibility.Hidden;
                return;
            }
        }

        private void FindOneNumber(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFilter(object sender, RoutedEventArgs e)
        {

        }

        private void FindNumbersOutingTomorrow(object sender, RoutedEventArgs e)
        {

        }

        private void FindNumbersOutingTomorrowByCurier(object sender, RoutedEventArgs e)
        {

        }

        private void FindNumbersOutingTomorrowByAll(object sender, RoutedEventArgs e)
        {

        }

        private void SaveCurrentDataBase(object sender, RoutedEventArgs e)
        {

        }

        private void EditNumberInMainDataBase(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteNumberInMainDataBase(object sender, RoutedEventArgs e)
        {

        }

        private void Edit1(object sender, RoutedEventArgs e)
        {

        }

        private void Edit2(object sender, RoutedEventArgs e)
        {

        }

        private void Edit3(object sender, RoutedEventArgs e)
        {

        }

        private void Edit4(object sender, RoutedEventArgs e)
        {

        }

        private void Edit5(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteWork(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeWork(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedMale(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedFemale(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedCircle(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedTriangle(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedSquare(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedUp(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedDown(object sender, RoutedEventArgs e)
        {

        }

        private void Tab3CB4_I1(object sender, RoutedEventArgs e)
        {

        }

        private void Tab3CB4_I2(object sender, RoutedEventArgs e)
        {

        }

        private void Tab3CB4_I3(object sender, RoutedEventArgs e)
        {

        }

        private void Tab3CB4_I4(object sender, RoutedEventArgs e)
        {

        }

        private void Tab3CB4_I5(object sender, RoutedEventArgs e)
        {

        }

        private void NewWork(object sender, RoutedEventArgs e)
        {

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
                Цена = Convert.ToInt32(ds.Tables["Work_types"].Rows[0]["Цена"].ToString()) * result
            });
            dataGrid8.ItemsSource = ListOfWorks;
            dataGrid8.Items.Refresh();
        }

        private void DeleteWorkFromAdd(object sender, RoutedEventArgs e)
        {
            if (dataGrid8.SelectedIndex == -1)
                return;
            ListOfWorks.RemoveAt(dataGrid8.SelectedIndex);
            dataGrid8.ItemsSource = ListOfWorks;
            dataGrid8.Items.Refresh();
        }

        private void DeleteAll(object sender, RoutedEventArgs e)
        {

        }

        private void TeethChecker(object sender, MouseButtonEventArgs e)
        {

        }

        private void ClearAllTeeth(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewWorker(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeWorker(object sender, RoutedEventArgs e)
        {

        }

        private void SummByYear(object sender, RoutedEventArgs e)
        {

        }

        private void SummByQuartal(object sender, RoutedEventArgs e)
        {

        }

        private void SummByMonth(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeNote(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteNote(object sender, RoutedEventArgs e)
        {

        }

        private void AddLog(object sender, RoutedEventArgs e)
        {

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
            else if (elements==0)
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

        }

        private void DefaultColors(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateList(object sender, RoutedEventArgs e)
        {

        }

        private void OpenStatisticByClinic(object sender, RoutedEventArgs e)
        {

        }

        private void OpenStatisticByCredits(object sender, RoutedEventArgs e)
        {

        }

        private void OpenStatisticByWorks(object sender, RoutedEventArgs e)
        {

        }

        private void OpenStatisticByClinics(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewWork(object sender, RoutedEventArgs e)
        {

        }

        private void CreateCopyOfDataBase(object sender, RoutedEventArgs e)
        {

        }

        private void CheckedWorker(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckedWorks(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
