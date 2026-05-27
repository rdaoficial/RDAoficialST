using System;
using System.Windows;

namespace RDAoficialST
{
    public partial class SiteWindow : Window
    {
        public SiteWindow()
        {
            InitializeComponent();

            Browser.Source = new Uri("https://generator.ryuu.lol/fixes#");
        }
    }
}