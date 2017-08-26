using System;
using Model;
using Common;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;


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
        public RelayCommand<Ship> StartCommand { get; set; }
        public RelayCommand<Ship> SetShipCommand { get; set; }
        public RelayCommand<Ship> ResetCommand { get; set; }
        static int sh { get; set; }
        TcpListener listener = null;
        public ViewModel()
        {
            MyShips = new ObservableCollection<Ship>();
            EnemyShips = new ObservableCollection<Ship>();
            for (int i = 0; i < 100; i++)
            {
                Ship test = new Ship();
                test.Content = "";
                test.Name = i.ToString();
                test.Deck = 0;
                test.IsEnabled = true;
                test.Background = "LightGray";
                MyShips.Add(test);

                Ship test2 = new Ship();
                test2.Content = "";
                test2.Name = i.ToString();
                test2.Deck = 0;
                test2.IsEnabled = true;
                test2.Background = "Gray";
                EnemyShips.Add(test2);
            }
            ShotCommand = new RelayCommand<Ship>(Shot);
            StartCommand = new RelayCommand<Ship>(Start);
            SetShipCommand = new RelayCommand<Ship>(SetShip);
            ResetCommand = new RelayCommand<Ship>(Reset);
            sh = 20; // счетчик палуб (четыре, три, три, два два два, одинодинодинодин)
        }


        public void LoadData()
        {
            MessageBox.Show("Расставьте, пожалуйста, свои корабли!", "Вздёрнем всех на реях!");
        }

        public void SetShip(Ship x)
        {
            if (x != null && x.Deck == 0 && sh != 0) // если выбрано поле, туда еще не ставили корабль и еще остались корабли в остатке
            {
                int coordinate = Convert.ToInt32(x.Name);
                int[] edgeTop = {1,2,3,4,5,6,7,8};
                int[] edgeRight = {19,29,39,49,59,69,79,89};
                int[] edgeButtom = {91,92,93,94,95,96,97,98};
                int[] edgeLeft = {10,20,30,40,50,60,70,80};
                int[] corner = {0, 9, 90, 99};

                Ship shot = new Ship();
                shot.Content = "";
                shot.Name = x.Name;
                shot.Deck = 1;
                shot.IsEnabled = true;
                shot.Background = "Green";
                MyShips.RemoveAt(coordinate);
                MyShips.Insert(coordinate, shot);
                sh--;

                #region Углы
                if (corner.Any(s => s == coordinate & s == 0))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate + 1).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate + 10).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate + 11).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    if (MyShips.ElementAt(coordinate + 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 1);
                        MyShips.Insert(coordinate + 1, not1);
                    }

                    if (MyShips.ElementAt(coordinate + 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 10);
                        MyShips.Insert(coordinate + 10, not2);
                    }

                    if (MyShips.ElementAt(coordinate + 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 11);
                        MyShips.Insert(coordinate + 11, not3);
                    }
                    #endregion
                }
                else if (corner.Any(s => s == coordinate & s == 9))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate - 1).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate + 10).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate + 9).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    if (MyShips.ElementAt(coordinate - 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 1);
                        MyShips.Insert(coordinate - 1, not1);
                    }

                    if (MyShips.ElementAt(coordinate + 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 10);
                        MyShips.Insert(coordinate + 10, not2);
                    }

                    if (MyShips.ElementAt(coordinate + 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 9);
                        MyShips.Insert(coordinate + 9, not3);
                    }
                    #endregion
                }
                else if (corner.Any(s => s == coordinate & s == 90))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate + 1).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate - 10).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate - 9).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    if (MyShips.ElementAt(coordinate + 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 1);
                        MyShips.Insert(coordinate + 1, not1);
                    }

                    if (MyShips.ElementAt(coordinate - 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 10);
                        MyShips.Insert(coordinate - 10, not2);
                    }

                    if (MyShips.ElementAt(coordinate - 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 9);
                        MyShips.Insert(coordinate - 9, not3);
                    }
                    #endregion
                }
                else if (corner.Any(s => s == coordinate & s == 99))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate - 1).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate - 10).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate - 11).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    if (MyShips.ElementAt(coordinate - 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 1);
                        MyShips.Insert(coordinate - 1, not1);
                    }

                    if (MyShips.ElementAt(coordinate - 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 10);
                        MyShips.Insert(coordinate - 10, not2);
                    }

                    if (MyShips.ElementAt(coordinate - 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 11);
                        MyShips.Insert(coordinate - 11, not3);
                    }
                    #endregion
                }
                #endregion
                #region Грани
                else if (edgeTop.Any(s => s == coordinate))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate - 1).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate + 1).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate + 9).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    Ship not4 = new Ship();
                    not4.Name = (coordinate + 10).ToString();
                    not4.Deck = 0;
                    not4.IsEnabled = true;
                    not4.Background = "Red";

                    Ship not5 = new Ship();
                    not5.Name = (coordinate + 11).ToString();
                    not5.Deck = 0;
                    not5.IsEnabled = true;
                    not5.Background = "Red";

                    if (MyShips.ElementAt(coordinate - 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 1);
                        MyShips.Insert(coordinate - 1, not1);
                    }

                    if (MyShips.ElementAt(coordinate + 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 1);
                        MyShips.Insert(coordinate + 1, not2);
                    }

                    if (MyShips.ElementAt(coordinate + 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 9);
                        MyShips.Insert(coordinate + 9, not3);
                    }

                    if (MyShips.ElementAt(coordinate + 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 10);
                        MyShips.Insert(coordinate + 10, not4);
                    }

                    if (MyShips.ElementAt(coordinate + 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 11);
                        MyShips.Insert(coordinate + 11, not5);
                    }
                    #endregion
                }
                else if (edgeRight.Any(s => s == coordinate))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate - 11).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate - 10).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate - 1).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    Ship not4 = new Ship();
                    not4.Name = (coordinate + 9).ToString();
                    not4.Deck = 0;
                    not4.IsEnabled = true;
                    not4.Background = "Red";

                    Ship not5 = new Ship();
                    not5.Name = (coordinate + 10).ToString();
                    not5.Deck = 0;
                    not5.IsEnabled = true;
                    not5.Background = "Red";

                    if (MyShips.ElementAt(coordinate - 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 11);
                        MyShips.Insert(coordinate - 11, not1);
                    }

                    if (MyShips.ElementAt(coordinate - 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 10);
                        MyShips.Insert(coordinate - 10, not2);
                    }

                    if (MyShips.ElementAt(coordinate - 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 1);
                        MyShips.Insert(coordinate - 1, not3);
                    }

                    if (MyShips.ElementAt(coordinate + 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 9);
                        MyShips.Insert(coordinate + 9, not4);
                    }

                    if (MyShips.ElementAt(coordinate + 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 10);
                        MyShips.Insert(coordinate + 10, not5);
                    }
                    #endregion
                }
                else if (edgeButtom.Any(s => s == coordinate))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate - 11).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate - 10).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate - 9).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    Ship not4 = new Ship();
                    not4.Name = (coordinate - 1).ToString();
                    not4.Deck = 0;
                    not4.IsEnabled = true;
                    not4.Background = "Red";

                    Ship not5 = new Ship();
                    not5.Name = (coordinate + 1).ToString();
                    not5.Deck = 0;
                    not5.IsEnabled = true;
                    not5.Background = "Red";

                    if (MyShips.ElementAt(coordinate - 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 11);
                        MyShips.Insert(coordinate - 11, not1);
                    }

                    if (MyShips.ElementAt(coordinate - 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 10);
                        MyShips.Insert(coordinate - 10, not2);
                    }

                    if (MyShips.ElementAt(coordinate - 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 9);
                        MyShips.Insert(coordinate - 9, not3);
                    }

                    if (MyShips.ElementAt(coordinate - 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 1);
                        MyShips.Insert(coordinate - 1, not4);
                    }

                    if (MyShips.ElementAt(coordinate + 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 1);
                        MyShips.Insert(coordinate + 1, not5);
                    }
                    #endregion
                }
                else if (edgeLeft.Any(s => s == coordinate))
                {
                    #region
                    Ship not1 = new Ship();
                    not1.Name = (coordinate - 10).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate + 1).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate - 9).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    Ship not4 = new Ship();
                    not4.Name = (coordinate + 10).ToString();
                    not4.Deck = 0;
                    not4.IsEnabled = true;
                    not4.Background = "Red";

                    Ship not5 = new Ship();
                    not5.Name = (coordinate + 11).ToString();
                    not5.Deck = 0;
                    not5.IsEnabled = true;
                    not5.Background = "Red";

                    if (MyShips.ElementAt(coordinate - 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 10);
                        MyShips.Insert(coordinate - 10, not1);
                    }

                    if (MyShips.ElementAt(coordinate + 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 1);
                        MyShips.Insert(coordinate + 1, not2);
                    }

                    if (MyShips.ElementAt(coordinate - 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 9);
                        MyShips.Insert(coordinate - 9, not3);
                    }

                    if (MyShips.ElementAt(coordinate + 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 10);
                        MyShips.Insert(coordinate + 10, not4);
                    }

                    if (MyShips.ElementAt(coordinate + 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 11);
                        MyShips.Insert(coordinate + 11, not5);
                    }
                    #endregion
                }
                #endregion
                else
                {
                    #region покраска
                    Ship not1 = new Ship();
                    not1.Name = (coordinate - 11).ToString();
                    not1.Deck = 0;
                    not1.IsEnabled = true;
                    not1.Background = "Red";

                    Ship not2 = new Ship();
                    not2.Name = (coordinate - 10).ToString();
                    not2.Deck = 0;
                    not2.IsEnabled = true;
                    not2.Background = "Red";

                    Ship not3 = new Ship();
                    not3.Name = (coordinate - 9).ToString();
                    not3.Deck = 0;
                    not3.IsEnabled = true;
                    not3.Background = "Red";

                    Ship not4 = new Ship();
                    not4.Name = (coordinate - 1).ToString();
                    not4.Deck = 0;
                    not4.IsEnabled = true;
                    not4.Background = "Red";

                    Ship not5 = new Ship();
                    not5.Name = (coordinate + 1).ToString();
                    not5.Deck = 0;
                    not5.IsEnabled = true;
                    not5.Background = "Red";

                    Ship not6 = new Ship();
                    not6.Name = (coordinate + 9).ToString();
                    not6.Deck = 0;
                    not6.IsEnabled = true;
                    not6.Background = "Red";

                    Ship not7 = new Ship();
                    not7.Name = (coordinate + 10).ToString();
                    not7.Deck = 0;
                    not7.IsEnabled = true;
                    not7.Background = "Red";

                    Ship not8 = new Ship();
                    not8.Name = (coordinate + 11).ToString();
                    not8.Deck = 0;
                    not8.IsEnabled = true;
                    not8.Background = "Red";

                    if (MyShips.ElementAt(coordinate - 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 11);
                        MyShips.Insert(coordinate - 11, not1);
                    }

                    if (MyShips.ElementAt(coordinate - 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 10);
                        MyShips.Insert(coordinate - 10, not2);
                    }

                    if (MyShips.ElementAt(coordinate - 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 9);
                        MyShips.Insert(coordinate - 9, not3);
                    }

                    if (MyShips.ElementAt(coordinate - 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate - 1);
                        MyShips.Insert(coordinate - 1, not4);
                    }

                    if (MyShips.ElementAt(coordinate + 1).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 1);
                        MyShips.Insert(coordinate + 1, not5);
                    }

                    if (MyShips.ElementAt(coordinate + 9).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 9);
                        MyShips.Insert(coordinate + 9, not6);
                    }

                    if (MyShips.ElementAt(coordinate + 10).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 10);
                        MyShips.Insert(coordinate + 10, not7);
                    }

                    if (MyShips.ElementAt(coordinate + 11).Background != "Green")
                    {
                        MyShips.RemoveAt(coordinate + 11);
                        MyShips.Insert(coordinate + 11, not8);
                    }
                    #endregion
                }
            }
            if (sh == 0)
            {
                MessageBox.Show("Корабли закончились!");
            }
        }
        public void Start(Ship x)
        {
     /*       // валидация расстановки кораблей
            ObservableCollection<Ship> ValidShips = new ObservableCollection<Ship>();
            foreach (Ship item in MyShips)
            {
                if (item.Background == "Green")
                {
                    ValidShips.Add(item);
                }
            }
                        
            var query4 = from s in MyShips
                         where s.Background == "Green" 
                         && MyShips.ElementAt(Convert.ToInt32(s.Name) + 10).Background == "Green"
                         && MyShips.ElementAt(Convert.ToInt32(s.Name) + 20).Background == "Green"
                         && MyShips.ElementAt(Convert.ToInt32(s.Name) + 30).Background == "Green"
                         select s;
            foreach (var item in query4)
            {
                MessageBox.Show(item.Name);
            }


            if (ValidShips.Count() == 20)
            {    */
                if (
                    MessageBox.Show("Вы сервер?", "Время выбирать, каналья!", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes
                    )
                {
                    MessageBox.Show("Ожидайте выстрела противника!");
                    Server();
                }
         /*   } */
            else
            {
                MessageBox.Show("Вы расставили не все корабли!");
            }


        }
        public void Shot(Ship x)
        {
            if (x != null)
            {
                Shot2(x);
                MessageBox.Show("Ожидайте хода противника!");
                Server();
            }
        }
        public void Shot2(Ship x)
        {
            if (x != null)
            {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", 11111);
                NetworkStream tcpStream = client.GetStream();

                string send = x.Name.ToString(); // начало формирования ответа
                byte[] msg = Encoding.UTF8.GetBytes(send);
                tcpStream.Write(msg, 0, msg.Length);

                byte[] bytes = new byte[client.ReceiveBufferSize]; // буфер входящей информации
                tcpStream.Read(bytes, 0, bytes.Length);
                string input = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                int shoting = Convert.ToInt32(input);
                    if (shoting == 0)
                    {
                        EnemyShips.RemoveAt(Convert.ToInt32(x.Name));
                        Ship shot = new Ship();
                        shot.Content = "";
                        shot.Name = "O";
                        shot.IsEnabled = false;
                        EnemyShips.Insert(Convert.ToInt32(x.Name), shot);
                    }
                else
                {
                    if (shoting == 1)
                    {
                        EnemyShips.RemoveAt(Convert.ToInt32(x.Name));
                        Ship shot = new Ship();
                        shot.Content = "";
                        shot.Name = "X";
                        shot.IsEnabled = false;
                        EnemyShips.Insert(Convert.ToInt32(x.Name), shot);
                    }
                }
                tcpStream.Close();
                client.Close();
                }
                catch (SocketException ex)
                {MessageBox.Show(ex.ToString());}
                catch (Exception ex)
                {MessageBox.Show(ex.ToString());}
            }
        }
        public void Server()
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 11111);
                listener.Start();
                TcpClient newClient = listener.AcceptTcpClient();
                NetworkStream tcpStream = newClient.GetStream();
                byte[] bytes = new byte[newClient.ReceiveBufferSize]; // буфер входящей информации
                tcpStream.Read(bytes, 0, bytes.Length);

                // проверка попал ли выстрел по кораблю
                string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                int read = Convert.ToInt32(str);
                if (MyShips.ElementAt(read).Deck == 0)
                {
                    string negative = "0";
                    byte[] ans = Encoding.UTF8.GetBytes(negative);
                    tcpStream.Write(ans, 0, ans.Length);
                    MyShips.RemoveAt(read);
                    Ship shot = new Ship();
                    shot.Content = "";
                    shot.Name = "O";
                    shot.IsEnabled = false;
                    MyShips.Insert(read, shot);
                }
                else
                {
                    string positive = "1";
                    byte[] ans = Encoding.UTF8.GetBytes(positive);
                    tcpStream.Write(ans, 0, ans.Length);
                    MyShips.RemoveAt(read);
                    Ship shot = new Ship();
                    shot.Content = "";
                    shot.Name = "X";
                    shot.IsEnabled = false;
                    MyShips.Insert(read, shot);
                }

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
        public void Reset(Ship x)
        {
            sh = 20;
            MyShips.Clear();
            MyShips = new ObservableCollection<Ship>();
            for (int i = 0; i < 100; i++)
            {
                Ship test = new Ship();
                test.Content = "";
                test.Name = i.ToString();
                test.Deck = 0;
                test.IsEnabled = true;
                test.Background = "LightGray";
                MyShips.Add(test);
            }
        }
    }
}
