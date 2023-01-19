using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;

namespace NotePad
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FileName;
        private bool IsSaved;
        public static double ZoomKoefficiant = 1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.FileName = String.Empty;
            this.IsSaved = true;

            var properties_font_family = new FontFamily(Properties.Settings.Default.font_family);

            var properties_font_style = Properties.Settings.Default.font_style == "Normal" ?
                                        FontStyles.Normal : FontStyles.Italic;

            var properties_font_weight = Properties.Settings.Default.font_weight == "Normal" ?
                                         FontWeights.Normal : FontWeights.Bold;

            var properties_font_size = Properties.Settings.Default.font_size;

            Application.Current.Resources["TextBoxFontFamily"] = properties_font_family;
            Application.Current.Resources["TextBoxFontStyle"] = properties_font_style;
            Application.Current.Resources["TextBoxFontWeight"] = properties_font_weight;
            Application.Current.Resources["TextBoxFontSizeFixed"] = properties_font_size;
            Application.Current.Resources["TextBoxFontSize"] = properties_font_size;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsSaved)
            {
                SaveMessageBox saveMessageBox = new SaveMessageBox();

                saveMessageBox.ShowDialog();

                if (saveMessageBox.Result == SaveMessageBoxResult.Yes)
                {
                    if (FileName == String.Empty)
                    {
                        this.SaveAs_Click(this, new RoutedEventArgs());
                        if (FileName == String.Empty) return;
                    }
                    else this.Save_Click(this, new RoutedEventArgs());
                }
                if (saveMessageBox.Result == SaveMessageBoxResult.Cancel)
                {
                    return;
                }
            }

            this.NotePad_textbox.Text = String.Empty;
            this.NotePad_textbox.Focus();
            this.IsSaved = true;

            this.Title = "Unnamed - Notepad";
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsSaved)
            {
                SaveMessageBox saveMessageBox = new SaveMessageBox();

                saveMessageBox.ShowDialog();

                if (saveMessageBox.Result == SaveMessageBoxResult.Yes)
                {
                    if (FileName == String.Empty)
                    {
                        this.SaveAs_Click(this, new RoutedEventArgs());
                        if (FileName == String.Empty) return;
                    }
                    else this.Save_Click(this, new RoutedEventArgs());
                }
                if (saveMessageBox.Result == SaveMessageBoxResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog opd = new OpenFileDialog();

            opd.Title = "Open file";
            opd.Filter = "Text File(*.txt)|*.txt";

            if ((bool)opd.ShowDialog())
            {
                this.FileName = opd.FileName;

                this.Title = this.FileName.Substring(this.FileName.LastIndexOf('\\') + 1,
                    this.FileName.Length - this.FileName.LastIndexOf('\\') - 5) + " - Notepad";

                this.NotePad_textbox.Text = File.ReadAllText(this.FileName);
                this.IsSaved = true;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(this.FileName, this.NotePad_textbox.Text);
                this.IsSaved = true;
            }
            catch { }
        }
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Save as...";
            saveFileDialog.Filter = "Text File(*.txt)|*.txt";

            if ((bool)saveFileDialog.ShowDialog())
            {   
                this.FileName = saveFileDialog.FileName;
                this.Title = this.FileName.Substring(this.FileName.LastIndexOf('\\') + 1,
                    this.FileName.Length - this.FileName.LastIndexOf('\\') - 5) + " - Notepad";

                this.IsSaved = true;

                File.WriteAllText(this.FileName, this.NotePad_textbox.Text);
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SaveMessageBox saveMessageBox = new SaveMessageBox();

            saveMessageBox.ShowDialog();

            if (saveMessageBox.Result == SaveMessageBoxResult.Yes)
            {
                if (FileName == String.Empty)
                {
                    this.SaveAs_Click(this, new RoutedEventArgs());
                    if (FileName == String.Empty) return;
                }
                else this.Save_Click(this, new RoutedEventArgs());
            }
            if (saveMessageBox.Result == SaveMessageBoxResult.Cancel)
            {
                return;
            }

            this.Close();
        }
        private void Undo_Click(object sender, RoutedEventArgs e) => this.NotePad_textbox.Undo();
        private void Redo_Click(object sender, RoutedEventArgs e) => this.NotePad_textbox.Redo();
        private void Cut_Click(object sender, RoutedEventArgs e) => this.NotePad_textbox.Cut();
        private void Copy_Click(object sender, RoutedEventArgs e) => this.NotePad_textbox.Copy();
        private void Paste_Click(object sender, RoutedEventArgs e) => this.NotePad_textbox.Paste();
        private void Delete_Click(object sender, RoutedEventArgs e) => this.NotePad_textbox.SelectedText = String.Empty;
        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.NotePad_textbox.Focus();
            this.NotePad_textbox.SelectAll();
        }

        private void WordWrap_Click(object sender, RoutedEventArgs e) => this.NotePad_textbox.TextWrapping = this.WordWrap_MenuItem.IsChecked ? TextWrapping.Wrap : TextWrapping.NoWrap;
        private void Font_Click(object sender, RoutedEventArgs e)
        {
            Font fontWindow = new Font();
            fontWindow.ShowDialog();
        }
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomKoefficiant < 4.9)
            {
                ZoomKoefficiant += 0.1;
                Application.Current.Resources["TextBoxFontSize"] = (double)Application.Current.Resources["TextBoxFontSizeFixed"] * ZoomKoefficiant;
                this.ZoomKoefficiant_StatusBarItem.Content = (ZoomKoefficiant * 100).ToString() + "%";
            }
        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomKoefficiant > 0.2)
            {
                ZoomKoefficiant -= 0.1;
                Application.Current.Resources["TextBoxFontSize"] = (double)Application.Current.Resources["TextBoxFontSizeFixed"] * ZoomKoefficiant;
                this.ZoomKoefficiant_StatusBarItem.Content = (ZoomKoefficiant * 100).ToString() + "%";
            }
        }
        private void ResetZoom_Click(object sender, RoutedEventArgs e)
        {
            ZoomKoefficiant = 1;
            Application.Current.Resources["TextBoxFontSize"] = (double)Application.Current.Resources["TextBoxFontSizeFixed"];
            this.ZoomKoefficiant_StatusBarItem.Content = "100%";
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
        private void Text_Changed(object sender, RoutedEventArgs e)
        {
            SetTextBoxCaretPosition();
            this.IsSaved = false;
        }
        private void Mouse_Capture(object sender, RoutedEventArgs e)
        {
            SetTextBoxCaretPosition();
        }

        private void SetTextBoxCaretPosition()
        {
            int caretIndex = NotePad_textbox.CaretIndex;
            int row = NotePad_textbox.GetLineIndexFromCharacterIndex(caretIndex);
            int col = caretIndex - NotePad_textbox.GetCharacterIndexFromLineIndex(row);
            this.CaretPosition_StatusBarItem.Content = $"Row {row + 1}, Col {col + 1}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                if (e.KeyboardDevice.IsKeyDown(Key.LeftShift) || e.KeyboardDevice.IsKeyDown(Key.RightShift))
                {
                    if (e.Key == Key.S)
                        this.SaveAs_Click(sender, e);
                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.N:
                            this.Clear_Click(sender, e);
                            break;
                        case Key.O:
                            this.Open_Click(sender, e);
                            break;
                        case Key.S:
                            this.Save_Click(sender, e);
                            break;

                        case Key.A:
                            this.SelectAll_Click(sender, e);
                            break;

                        case Key.OemPlus:
                            this.ZoomIn_Click(sender, e);
                            break;
                        case Key.OemMinus:
                            this.ZoomOut_Click(sender, e);
                            break;
                        case Key.D0:
                        case Key.NumPad0:
                            this.ResetZoom_Click(sender, e);
                            break;
                    }
                }
            }
        }
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Delta > 0) this.ZoomIn_Click(sender, e);
                else if (e.Delta < 0) this.ZoomOut_Click(sender, e);
            }
        }
        private void Closing_Window(object sender, CancelEventArgs e)
        {
            if(!this.IsSaved)
            {
                SaveMessageBox saveMessageBox = new SaveMessageBox();

                saveMessageBox.ShowDialog();

                if (saveMessageBox.Result == SaveMessageBoxResult.Yes)
                {
                    if (FileName == String.Empty)
                    {
                        this.SaveAs_Click(this, new RoutedEventArgs());
                        if (FileName == String.Empty) e.Cancel = true;
                    }
                    else this.Save_Click(this, new RoutedEventArgs());
                }
                if (saveMessageBox.Result == SaveMessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}