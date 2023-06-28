using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            //该值确定是否可以选择多个文件,当前不然多选
            dialog.Multiselect = false;
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                var path = dialog.FileName;
               MainVm.Instance.AddWatermarkToImage(path, "output.jpg", "十三");
            }
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            MainVm.Instance.CombineImagesVertically("1.jpg", "2.jpg", "output.jpg");
        }
    }
}
