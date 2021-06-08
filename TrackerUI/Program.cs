﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Initillize the database connection
            TrackerLibrary.GlobalConfig.InitializeConnection(DatabaseType.TextFiles);
            Application.Run(new CreatePriceForm());

            //Application.Run(new TournamentDashboardForm());
        }
    }
}
