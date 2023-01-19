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

namespace NotePad
{
    /// <summary>
    /// Логика взаимодействия для SaveMessageBox.xaml
    /// </summary>
    public enum SaveMessageBoxResult
    {
        Yes,
        No,
        Cancel
    }
    public partial class SaveMessageBox : Window
    {
        public SaveMessageBoxResult Result { get; private set; }
        public SaveMessageBox()
        {
            InitializeComponent();
            Result = SaveMessageBoxResult.Cancel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch(((Button)e.OriginalSource).Content)
            {
                case "Yes":
                    this.Result = SaveMessageBoxResult.Yes;
                    break;
                case "No":
                    this.Result = SaveMessageBoxResult.No;
                    break;
                case "Cancel":
                    this.Result = SaveMessageBoxResult.Cancel;
                    break;
            }
            base.Close();
        }
    }
}
