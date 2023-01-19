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
    /// Логика взаимодействия для Font.xaml
    /// </summary>
    public partial class Font : Window
    {
        private FontFamily fontFamily;
        private FontStyle fontStyle;
        private FontWeight fontWeight;
        private double fontSize;
        public Font()
        {
            InitializeComponent();
        }
        private void FontWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.fontFamily = (FontFamily)Application.Current.Resources["TextBoxFontFamily"];

            Font_TextBox.Text = fontFamily.ToString();
            Font_TextBox.FontFamily = fontFamily;

            this.fontStyle = (FontStyle)Application.Current.Resources["TextBoxFontStyle"];
            this.fontWeight = (FontWeight)Application.Current.Resources["TextBoxFontWeight"];

            this.Style_TextBox.Text = GetStyle();

            this.fontSize = (double)Application.Current.Resources["TextBoxFontSizeFixed"];
            this.Size_TextBox.Text = fontSize.ToString();

            Example_TextBlock.FontFamily = fontFamily;
            Example_TextBlock.FontStyle = fontStyle;
            Example_TextBlock.FontWeight = fontWeight;
            Example_TextBlock.FontSize = fontSize;
        }
        private string GetStyle()
        {
            if(this.fontStyle == FontStyles.Normal)
            {
                if (this.fontWeight == FontWeights.Normal) return "Normal";
                else return "Bold";
            }
            else
            {
                if (this.fontWeight == FontWeights.Normal) return "Italic";
                else return "Italic Bold";
            }
        }
        private void FontFamily_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object obj = (sender as ListBox).SelectedItem;
            this.fontFamily = (obj as ListBoxItem).FontFamily;

            Font_TextBox.Text = fontFamily.ToString();
            Font_TextBox.FontFamily = fontFamily;

            Example_TextBlock.FontFamily = fontFamily;
        }
        private void FontStyle_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object obj = (sender as ListBox).SelectedItem;
            switch ((obj as ListBoxItem).Content)
            {
                case "Normal":
                    this.fontStyle = FontStyles.Normal;
                    this.fontWeight = FontWeights.Normal;
                    break;
                case "Italic":
                    this.fontStyle = FontStyles.Italic;
                    this.fontWeight = FontWeights.Normal;
                    break;
                case "Bold":
                    this.fontStyle = FontStyles.Normal;
                    this.fontWeight = FontWeights.Bold;
                    break;
                case "Italic Bold":
                    this.fontStyle = FontStyles.Italic;
                    this.fontWeight = FontWeights.Bold;
                    break;
            }

            Style_TextBox.Text = GetStyle();

            Example_TextBlock.FontStyle = fontStyle;
            Example_TextBlock.FontWeight = fontWeight;
        }
        private void FontSize_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object obj = (sender as ListBox).SelectedItem;
            this.fontSize = double.Parse((obj as ListBoxItem).Content.ToString());
            Size_TextBox.Text = this.fontSize.ToString();
            Example_TextBlock.FontSize = this.fontSize;
        }

        private void LostFocus_TextBox(object sender, EventArgs e)
        {
            SetSize();
        }

        private void KeyDown_TextBox(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.Enter))
            {
                SetSize();
            }
        }

        private void SetSize()
        {
            double Size = double.Parse(Size_TextBox.Text);

            if (Size < 1) Size = 1;
            else if (Size > 100) Size = 100;

            Size_TextBox.Text = Size.ToString();

            this.fontSize = Size;
            Example_TextBlock.FontSize = Size;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["TextBoxFontFamily"] = this.fontFamily;
            Application.Current.Resources["TextBoxFontStyle"] = this.fontStyle;
            Application.Current.Resources["TextBoxFontWeight"] = this.fontWeight;
            Application.Current.Resources["TextBoxFontSizeFixed"] = this.fontSize;
            Application.Current.Resources["TextBoxFontSize"] = this.fontSize * MainWindow.ZoomKoefficiant;

            Properties.Settings.Default.font_family = this.fontFamily.ToString();
            Properties.Settings.Default.font_style = this.fontStyle.ToString();
            Properties.Settings.Default.font_weight = this.fontWeight.ToString();
            Properties.Settings.Default.font_size = this.fontSize;

            Properties.Settings.Default.Save();

            base.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => base.Close();

        
    }
}
