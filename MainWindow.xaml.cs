﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeaBattle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel xxx = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = xxx;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.xxx.LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e) // связь
        {
            object number = (sender as Button).Tag;
            if ((sender as Button).IsDescendantOf(EnemyField))
            {
                EnemyField.SelectedIndex = EnemyField.Items.IndexOf(number);
            }
            if ((sender as Button).IsDescendantOf(MyField))
            {
                MyField.SelectedIndex = MyField.Items.IndexOf(number);
            }
        }
    }
}
