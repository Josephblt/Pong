﻿using System;
using System.Windows.Forms;

namespace Pong
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(GameWindow.GetGameWindow());
        }
    }
}
