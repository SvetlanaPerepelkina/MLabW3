using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;


namespace Paint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Polygon> figures;
        ObservableCollection<string> polygons;
        

        public MainWindow()
        {
            InitializeComponent();
            FillColors.ItemsSource = typeof(Colors).GetProperties();
            BorderColors.ItemsSource = typeof(Colors).GetProperties();

            CommandBinding bOpen = new CommandBinding(ApplicationCommands.Open);
            this.CommandBindings.Add(bOpen);
            CommandBinding bSave = new CommandBinding(ApplicationCommands.Save);
            this.CommandBindings.Add(bSave);
            bOpen.Executed += OpenExe;
            bSave.Executed += SaveExe;
            colorFill = new ColorRGB();
            colorStroke = new ColorRGB();
            colorFill.red = 0;
            colorFill.green = 0;
            colorFill.blue = 0;
            colorStroke.blue = 0;
            colorStroke.red = 0;
            colorStroke.green = 0;
            this.inkCanvas.DefaultDrawingAttributes.Color = Colors.White;
            figures = new List<Polygon>();
            polygons = new ObservableCollection<string>();
            File.WriteAllText("pictureTemp.dat", "");
            File.WriteAllText("picture.dat", "");
        }

        private ColorRGB colorFill;
        private ColorRGB colorStroke;
        private Color color;
        private Color color1;
        private Color color2;
        readonly string path = "pictureTemp.dat";
        readonly string path1 = "picture.dat";

        Polygon polygon;

        private void SaveAsExe(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "picture";
            dialog.Title = "Save as ";
            dialog.Filter = "Files (dat)|*.dat|All Files|*.*";
 
            if (figures.Count == 0)
            {
                MessageBox.Show("Фигур нет");
            }
            else if (figures.Count > 0)
            {
               if( dialog.ShowDialog() == true)
                {
                    StreamReader reader = new StreamReader(path);
                    string text =  File.ReadAllText(path);
                    File.WriteAllText(path1, text);
                    DataPolygonSave(path1);
                }
            }
        }

        void SaveAsCanExe(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void SaveExe(object sender, ExecutedRoutedEventArgs e)
        {
            if (figures.Count == 0)
            {
                MessageBox.Show("Фигур нет");
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                string text = File.ReadAllText(path);
                File.WriteAllText(path1, text);
                DataPolygonSave(path1);
            }
        }

        void SaveCanExe(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = inkCanvas.Children.Count != 0;
        }


        void OpenExe(object target, ExecutedRoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
           

            var dialog = new OpenFileDialog();
            dialog.FileName = "picture";
            dialog.Filter = "Files (dat)|*.dat|All Files|*.*";

                if (dialog.ShowDialog() == true)
                {
                StreamReader reader = new StreamReader(path);
                window1.ReadFile.Text = File.Exists(path) ? File.ReadAllText(path) : "Файл не найден";
                }

        }

        void OpenCanExe(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }


        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            figures.Clear();
            inkCanvas.Children.Clear();
            Count.Text = string.Empty;

            string polygonData = "";
            File.WriteAllText(path, polygonData);

        }

        private void IncCanvas_Mouse(object sender, MouseEventArgs e)
        {
            Point position = Mouse.GetPosition(this);
            TextBlock4.Text = "  X: " + position.X + "\tY: " + position.Y;
        }


        private void Information(object sender, RoutedEventArgs e)
        {
            Window1 aboutThisProgramm = new Window1();
            aboutThisProgramm.Width = 200;
            aboutThisProgramm.Height = 200;
            aboutThisProgramm.WindowStyle = WindowStyle.ToolWindow;
            Label label = new Label();
            label.Content = "About this programm: \nPaintS. 2024";
            label.FontSize = 16;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Background = Brushes.Orange;
            aboutThisProgramm.Content = label;
            label.Foreground = Brushes.White;
            aboutThisProgramm.ShowDialog();
        }

        private void Color_ValueChanged2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ColorSelect(sender);                             
          
            color1 = Color.FromRgb(colorFill.red, colorFill.green, colorFill.blue);
            this.lFill.Background = new SolidColorBrush(color1);
            this.polygon.Fill = lFill.Background;
        }

        private void Color_ValueChanged3(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ColorSelect(sender);
 
                color2 = Color.FromRgb(colorStroke.red, colorStroke.green, colorStroke.blue);
                SolidColorBrush solidColorBrush = new SolidColorBrush(color2);
                this.polygon.Stroke = solidColorBrush;
            }
            catch { new Exception(); }
        }

        private void Color_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ColorSelect(sender);
  
                color = Color.FromRgb(colorStroke.red, colorStroke.green, colorStroke.blue);
                SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(colorStroke.red, colorStroke.green, colorStroke.blue));
                this.lbl2.Background = solidColorBrush;
                this.inkCanvas.DefaultDrawingAttributes.Color = color;
            }
            catch { new Exception(); }
        }

        private void FillColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Color selectedColor = (Color)(FillColors.SelectedItem as PropertyInfo).GetValue(null, null);
                this.polygon.Fill = new SolidColorBrush(selectedColor);
            }
            catch { new Exception(); }
        }

        private void BorderColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Color selectedColor = (Color)(BorderColors.SelectedItem as PropertyInfo).GetValue(null, null);
                this.polygon.Stroke = new SolidColorBrush(selectedColor);
            }
            catch { new Exception(); }
        }

        private void Line_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                var slider = sender as Slider;
                string crlName = slider.Name;
                double value = slider.Value;

                if (crlName.Equals("StrokeThickness"))
                {
                    this.polygon.StrokeThickness = Convert.ToByte(value);
                   // this.inkCanvas.DefaultDrawingAttributes.Width = value;
                    Line.Text = Convert.ToInt32(value).ToString();
                    Line2.Text = Convert.ToInt32(value).ToString();
                }
            }
            catch
            {
                new Exception();
            }

        }

        private void Line2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                Line.Text = Line2.Text;
            }
            catch { new Exception(); }
        }

        private void CreatePolygon(object sender, MouseButtonEventArgs e)
        {
            var dialog = new SaveFileDialog();
           // string path = "pictureTemp.dat";
           

            if (figures.Count !=0)
            {
                DataPolygonSave(path);
            }
            
            polygon = new Polygon();
            PointCollection points = new PointCollection();
            points.Add(new Point(0, 0));
            points.Add(new Point(200, 0));
            points.Add(new Point(100, 100));
            points.Add(new Point(-100, 100));

            Point p = e.GetPosition(this.inkCanvas);

            polygon.Points = points;
            polygon.Stroke = Brushes.Yellow;      BorderColors.SelectedValue = Brushes.Yellow;
            polygon.Fill = Brushes.Yellow;        FillColors.SelectedValue = Brushes.Yellow;
            polygon.StrokeThickness = 1;          Line.Text = polygon.StrokeThickness.ToString();
            inkCanvas.Children.Add(polygon);
            InkCanvas.SetTop(polygon, p.Y);
            InkCanvas.SetLeft(polygon, p.X);

            figures.Add(polygon);
            polygons.Add(Count.Text);

            Count.Text = figures.Count.ToString();

        }
         
        public void DataPolygonSave(string path )
        {
            string polygonData =
                "\n" + figures.Count.ToString() + " polygon" +
                "\n" + polygon.PointToScreen(new Point(0, 0)) +
                "\n" + polygon.Stroke.ToString() +
                "\n" + polygon.Fill.ToString() +
                "\n" + polygon.StrokeThickness;
            if (figures.Count < 2) 
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "picture";
                dialog.Title = "Save as ";
                dialog.Filter = "Files (dat)|*.dat|All Files|*.*";
            }
           if(figures.Count >= 2) File.AppendAllText(path, polygonData);
           else                  File.WriteAllText(path, polygonData);
        }


        private void ColorSelect (object sender)
        {
            var slider = sender as Slider; //Определяем имя контрола, который покрутили
            string crlName = slider.Name;  // Получаем значение контрола
            double value = slider.Value;   //В зависимости от выбранного контрола, меняем ту или иную компоненту и переводим ее в тип byte
            if (crlName.Equals("RedColor") || crlName.Equals("Red"))
            {
                colorStroke.red = Convert.ToByte(value);
            }
            if (crlName.Equals("GreenColor") || crlName.Equals("Green"))
            {
                colorStroke.green = Convert.ToByte(value);
            }
            if (crlName.Equals("BlueColor") || crlName.Equals("Blue"))
            {
                colorStroke.blue = Convert.ToByte(value);
            }
        }
    }
}
    


