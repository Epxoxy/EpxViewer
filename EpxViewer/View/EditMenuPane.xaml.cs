using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EpxViewer
{
    /// <summary>
    /// Interaction logic for EditMenu.xaml
    /// </summary>
    public partial class EditMenuPane : UserControl
    {
        public EditMenuPane()
        {
            InitializeComponent();
            rootGrid.DataContext = this;
        }



        public string NavName
        {
            get { return (string)GetValue(NavNameProperty); }
            set { SetValue(NavNameProperty, value); }
        }
        
        public static readonly DependencyProperty NavNameProperty =
            DependencyProperty.Register("NavName", typeof(string), typeof(EditMenuPane), new PropertyMetadata(string.Empty));
        
        public string RunPath
        {
            get { return (string)GetValue(RunPathProperty); }
            set { SetValue(RunPathProperty, value); }
        }
        
        public static readonly DependencyProperty RunPathProperty =
            DependencyProperty.Register("RunPath", typeof(string), typeof(EditMenuPane), new PropertyMetadata(string.Empty));

        private void onDragEnter(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            RunPath = path;
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            var openFile = new Microsoft.Win32.OpenFileDialog();
            if(openFile.ShowDialog() == true)
            {
                var fileInfo = new System.IO.FileInfo(openFile.FileName);
                if(fileInfo.Exists) RunPath = fileInfo.FullName;
            }
        }
    }

    public class PathRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = value.ToString();
            if (string.IsNullOrEmpty(strValue))
            {
                return new ValidationResult(true, null);
            }
            if (System.IO.Directory.Exists(strValue))
            {
                return new ValidationResult(true, null);
            }else if (System.IO.File.Exists(strValue))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Error path");
            }
        }
    }
}
