using System.Collections.Specialized;
using System.Windows;

using QuickPingControls;
using Microsoft.VisualBasic;

namespace QuickPing
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var address = Application.Current.Properties["Arguments"] as string ?? "127.0.0.1";

            var addresses = new StringCollection();
            addresses.Add(address);

            int flag = 0;
            int addressesNUM = 0;
            for(;flag==0;)
            {
                string str1 = Interaction.InputBox("请输入地址段起点", "Input", "IP地址", -1, -1);
                if (str1 == "")
                    break;
                string str2 = Interaction.InputBox("请输入地址段终点", "Input", "IP地址（保持前三位一致）或只输最后一位", -1, -1);
                if (str2 == "")
                    break;
                flag++;
                
                if (str1.IndexOf('.')==-1)
                {
                    flag = 0;
                    continue;
                }
                
                if (str2.IndexOf('.')==-1)
                {
                    string[] str1Array = str1.Split('.');
                    addressesNUM = int.Parse(str2) - int.Parse(str1Array[3]) + 1;
                    if (addressesNUM <= 0)
                    {
                        Interaction.MsgBox("区段有误！");
                        flag = 0;
                    }
                    for (int i = 0; i < addressesNUM; i++)
                    {
                        int addressLast = int.Parse(str1Array[3]) + i;
                        string addressTEMP = str1Array[0] + '.' + str1Array[1] + '.' + str1Array[2] + '.' + addressLast.ToString();
                        addresses.Add(addressTEMP);
                    }
                }
                else
                {
                    string[] str1Array = str1.Split('.');
                    string[] str2Array = str2.Split('.');

                    for (int i = 0; i < 3; i++)
                    {
                        if (string.Compare(str1Array[i], str2Array[i]) == 0)
                            continue;
                        else
                        {
                            Interaction.MsgBox("请输入相同子网段！");
                            flag = 0;
                        }
                    }
                    addressesNUM = int.Parse(str2Array[3]) - int.Parse(str1Array[3]) + 1;
                    if (addressesNUM <= 0)
                    {
                        Interaction.MsgBox("区段有误！");
                        flag = 0;
                    }
                    for (int i = 0; i < addressesNUM; i++)
                    {
                        int addressLast = int.Parse(str1Array[3]) + i;
                        string addressTEMP = str1Array[0] + '.' + str1Array[1] + '.' + str1Array[2] + '.' + addressLast.ToString();
                        addresses.Add(addressTEMP);
                    }
                }
            }
            

            foreach (var a in addresses)
            {
                var status = new PingStatusControl(a);

                StackPanel1.Children.Add(status);

                status.Start();
            }
        }
    }
}