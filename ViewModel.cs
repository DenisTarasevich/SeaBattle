using System;
using Model;
using Common;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SeaBattle
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Ship> myships;
        public ObservableCollection<Ship> MyShips
        {
            get
            {
                return myships;
            }
            private set
            {
                if (value != myships)
                {
                    myships = value;
                    NotifyPropertyChanged("MyShips");
                }
            }
        }

        private ObservableCollection<Ship> enemyships;
        public ObservableCollection<Ship> EnemyShips
        {
            get
            {
                return enemyships;
            }
            private set
            {
                if (value != enemyships)
                {
                    enemyships = value;
                    NotifyPropertyChanged("EnemyShips");
                }
            }
        }


        public RelayCommand<Ship> ShotCommand { get; set; }

        public ViewModel()
        {
            MyShips = new ObservableCollection<Ship>();
            EnemyShips = new ObservableCollection<Ship>();
            for (int i = 1; i < 101; i++)
            {
                Ship test = new Ship();
                test.Content = "";
                test.Name = i;
                test.Deck = 0;
                test.IsEnabled = true;
                test.Background = "Gray";
                MyShips.Add(test);

                Ship test2 = new Ship();
                test2.Content = "";
                test2.Name = i;
                test2.Deck = 0;
                test2.IsEnabled = true;
                test2.Background = "Gray";
                EnemyShips.Add(test2);
            }
            ShotCommand = new RelayCommand<Ship>(Shot);
        }

        public void LoadData()
        {

        }

        public void Shot(Ship x)
        {
            
        }


    }
}
