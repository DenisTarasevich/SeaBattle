﻿using System;
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
                if (i % 2 == 0)
                {
                    test.Deck = 0;
                }
                else
                {
                    test.Deck = 1;
                }
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
            if (x != null)
            {
                TcpListener listener = null;
                try
                {
                    listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 11111);
                        listener.Start();
                        TcpClient newClient = listener.AcceptTcpClient();

                        NetworkStream tcpStream = newClient.GetStream();

                        string send = x.Name.ToString(); // начало формирования ответа
                        byte[] msg = Encoding.UTF8.GetBytes(send);
                        tcpStream.Write(msg, 0, msg.Length);


                        byte[] bytes = new byte[newClient.ReceiveBufferSize]; // буфер входящей информации
                        tcpStream.Read(bytes, 0, bytes.Length);
                        MessageBox.Show("Полученный текст: " +
    Encoding.UTF8.GetString(bytes, 0, bytes.Length)); // получили инфу


                        tcpStream.Close();
                    }

                catch (SocketException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                finally
                {
                    if (listener != null)
                    {
                        listener.Stop();
                    }
                }
            }
        }

        public void Client(Ship x)
        {
            try
            {
                TcpClient client = new TcpClient();

                client.Connect("127.0.0.1", 11111);

                NetworkStream tcpStream = client.GetStream();

                byte[] bytes = new byte[client.ReceiveBufferSize]; // буфер входящей информации
                tcpStream.Read(bytes, 0, bytes.Length);

                // проверка попал ли запрос по кораблю
                string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                int read = Convert.ToInt32(str);
                if (MyShips.ElementAt(read).Deck == 0)
                {
                    string negative = "Промах по клетке " + read;
                    byte[] ans = Encoding.UTF8.GetBytes(negative);
                    tcpStream.Write(ans, 0, ans.Length);
                }
                else
                {
                    string positive = "Убит по клетке " + read;
                    byte[] ans = Encoding.UTF8.GetBytes(positive);
                    tcpStream.Write(ans, 0, ans.Length);
                }

                tcpStream.Close();
                client.Close();
            }

            catch (SocketException ex)
            {
                MessageBox.Show("Exception: " + ex.ToString());
            }

        }
    }
}
