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

namespace git_saved_users_cleaner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (servers_lb.Items.Contains(server_name.Text) || server_name.Text.Length == 0){
                MessageBox.Show("This server name already add or server_name is empty",  "Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            servers_lb.Items.Add(server_name.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string back = execCMD("cmdkey.exe /list");
            string[] values = back.Split('\n');
            int c = 0;
            foreach (var item in servers_lb.Items)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].Contains(item.ToString()))
                    {
                        Console.WriteLine("Пользователь удален: " + values[i]);
                        c++;
                        string[] us_values = values[i].Split(':');
                        string user = "";
                        for (int j = 1; j < us_values.Length; j++)
                        {
                            if (j == 1)
                            {
                                user += us_values[j];
                                continue;
                            }

                            user += ":" + us_values[j];
                        }
                        user = user.Replace(" ", "");
                        back = execCMD("cmdkey.exe /delete:" + user);
                        Console.WriteLine(back);
                    }
                }
            }
            MessageBox.Show("Users data was deleted successfuly! deleted " + c.ToString() + " users data", "Info", MessageBoxButton.OK, MessageBoxImage.Information);



        }

        public static string execCMD(string command)
        {
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
            pro.StandardInput.WriteLine(command);
            pro.StandardInput.WriteLine("exit");
            pro.StandardInput.AutoFlush = true;
            string output = pro.StandardOutput.ReadToEnd();
            pro.WaitForExit();
            pro.Close();
            return output;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (servers_lb.SelectedIndex != -1)
            {
                servers_lb.Items.RemoveAt(servers_lb.SelectedIndex);
            }
        }
    }
}
