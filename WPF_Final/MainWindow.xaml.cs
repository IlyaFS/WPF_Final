using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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

namespace WPF_Final
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> calcs; // создание источника данных для вычислений
        public MainWindow()
        {
            InitializeComponent();
            calcs = new List<string>();


            foreach (UIElement element in Panel.Children) // Обращение к объектам в Panel
            {
                if (element is Button)
                {
                    ((Button)element).Click += ButtonClick;
                }
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            try // ловим ошибки
            {
                Button name = (Button)sender;
                string textButton = (string)name.Content;

                textEq.Text = textBox.Text + "" + textButton;

                if (textBox.Text == "0") //Исключение "0" после начала набора цифр
                {
                    textBox.Text = null;
                    textEq.Text = null;
                }

                if (textButton == "←") // обрботка действия клавиши backspace
                {
                    if (textBox.Text.Length > 1)
                    {
                        textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                        textEq.Text = "";
                    }
                    else
                    {
                        textBox.Text = "0";
                        textBox.Foreground = Brushes.Black;
                        textEq.Text = "";
                    }
                }
                else if (textButton == "C") // обрботка действия клавиши C
                {
                    textBox.Text = "0";
                    textBox.Foreground = Brushes.Black;
                    textEq.Text = "";
                }
                else if (textButton == "=") // обрботка действия клавиши =
                {
                    string val = new DataTable().Compute(textBox.Text, null).ToString();
                    textBox.Text = val;
                    textBox.Foreground = Brushes.DarkRed; // выделение результата вычислений красным шрифтом

                    calcs.Add(textEq.Text + textBox.Text); //запись результатов вычислений
                }
                else textBox.Text += textButton;
            }
            catch (Exception)
            {
                MessageBox.Show("ошибка!"); // выводим сообщение об ошибке
            }
        }

        private void SaveEx(object sender, ExecutedRoutedEventArgs e) // сохранение результатов вычислений
        {
            string path = Directory.GetCurrentDirectory() + @"\" + "Вычисления.txt"; // путь сохранения и название файла
            using (StreamWriter streamWriter = new StreamWriter(path, false))
            {
                int i = 0;
                foreach (string Compute in calcs)
                {
                    i++;
                    streamWriter.WriteLine(i.ToString() + ") " + Compute);
                }
                MessageBox.Show("Результат вычислений был сохранен:" + path); // сообщение на экран куда сохранился файл
            }
        }
    }
}

