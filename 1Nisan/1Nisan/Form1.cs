using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;


namespace _1Nisan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            CreateTaskRunOnce();
           // if (DateTime.Now.Month == 2 && DateTime.Now.Day == 24 && DateTime.Now.Hour == 15 && DateTime.Now.Minute == 45)
            {
                axWindowsMediaPlayer1.URL = "C:/kappa.mp4";
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
        }
        private void CreateTaskRunOnce()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Kappa";
                td.Triggers.Add(new TimeTrigger() { StartBoundary = Convert.ToDateTime("24-02-2017 15:45:00") });

                td.Actions.Add(new ExecAction(@"C:/1Nisan.exe", null, null));
                ts.RootFolder.RegisterTaskDefinition("OnTime", td);
            }
        }

    }
}
