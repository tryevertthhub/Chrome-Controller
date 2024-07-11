using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
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
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Net;
using System.Net.Http;

namespace ChromeController
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

        private void SwitchAccount(string account)
        {
            currentIndex++;
            string chromePath = @"C:\Users\Administrator\AppData\Local\Google\Chrome\Application\chrome.exe";

            string profilePath = @"C:\Users\Administrator\AppData\Local\Google\Chrome\User Data";
         
            ProcessStartInfo startInfo = new ProcessStartInfo(chromePath);
            startInfo.Arguments = $"--user-data-dir=\"{profilePath}\" --profile-directory=\"{account}\"";
            try
            {
                Process chromeProcess = Process.Start(startInfo);
                timer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
                private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

        if (this.Frontend.Content.ToString() == "Frontend Server Start")
            {
                string projectPath = @"C:\Users\Administrator\Documents\Server\frontend";
                string command = "npm run dev";

                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", $"/c cd /d {projectPath} && {command}");
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;

                FrontendProcess = new Process();
                FrontendProcess.StartInfo = startInfo;

                StringBuilder outputBuilder = new StringBuilder();

                FrontendProcess.OutputDataReceived += (s, eventArgs) =>
                {
                    if (!string.IsNullOrEmpty(eventArgs.Data))
                    {
                        string output = eventArgs.Data;
                        output = ModifyOutputFormat(output);

                        // Update the UI with the output
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            outputBuilder.AppendLine(output);
                            this.Output.Text = outputBuilder.ToString();
                            Console.WriteLine(output);
                        });
                    }
                };

                FrontendProcess.Start();
                this.Frontend.Content = "Stop";
                FrontendProcess.BeginOutputReadLine();

                await Task.Run(() =>
                {
                    FrontendProcess.WaitForExit();
                });
            }
        else
            {
                StopFrontend();
            }
           

            
        }
    }
}


