﻿using System.Reflection;
using NUnit.Gui;

namespace TCode.r2rml4net.Mapping.Tests
{
    public class Program
    {
        [System.STAThread]
        static int Main()
        {
            return AppEntry.Main(new string[] { Assembly.GetExecutingAssembly().Location });
        }
    }
}
