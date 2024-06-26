using Checkers2.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Checkers2.Classes.Setups;

namespace Checkers2.Models
{
    /// <summary>
    /// English draughts
    /// Interaction logic for pvp.xaml
    /// </summary>
    public partial class Pvp : Window
    {
       
        private Button[,] buttons;
        private Board gameBoard;
        private bool turn = whiteTurn;
        private List<string> playedMoves = new List<string>();
        private bool isBotP1 ;
        private bool isBotP2;
        private int depthP1=4;
        private int depthP2=4;
        private bool onlineGame;
        private bool isHost ;
        private Socket socket;
        private BackgroundWorker BGWorker;
        private TcpListener server;
        private TcpClient clint;
        private string IP;


        public Pvp(Double l, Double t, Double w, Double h, WindowState windowState, bool onlineGame = false, bool isHost = false , string IP = null , bool p1BOT = false, bool p2BOT = false)
        {
            InitializeComponent();
            isBotP1 = p1BOT;
            isBotP2 = p2BOT;
            this.isHost = isHost;
            this.IP = IP;
            this.onlineGame = onlineGame;

            res.Visibility = Visibility.Visible;
            draw.Visibility = Visibility.Visible;
            resb.Visibility = Visibility.Visible;
            if (p1BOT)
            {
                res.Visibility = Visibility.Collapsed;
                draw.Visibility = Visibility.Collapsed;
            }
            if (p2BOT)
            {
                resb.Visibility = Visibility.Collapsed;
                draw.Visibility = Visibility.Collapsed;
            }

            if (onlineGame)
            {
                resb.Visibility = Visibility.Collapsed;
                draw.Visibility = Visibility.Visible;
                BGWorker = new BackgroundWorker();
                BGWorker.DoWork += BGWorker_DoWork;
                if (isHost)
                {
                    server = new TcpListener(System.Net.IPAddress.Any, 5734);
                    server.Start();
                    socket = server.AcceptSocket();
                }
                else
                {
                    try
                    {
                        clint = new TcpClient(IP, 5734);
                        socket = clint.Client;
                        BGWorker.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Close();
                    }
                    turn = false;
                }
            }

            

            Init();
            Closed += new EventHandler((se, e) =>
            {
                try
                {
                    res_Click(null, null);
                }
                catch { }
            });

            this.Loaded += new RoutedEventHandler(
      delegate (object sender, RoutedEventArgs args)
      {
          Left = l;
          Top = t;
          Width = w;
          Height = h;
          WindowState = windowState;
      });
        }

        private void BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string st = " ";
            this.Dispatcher.Invoke(() =>
            {
                hide.Visibility = Visibility.Visible;
            });
            do
            {
                st = ReceiveMove();
                if (st.Contains("res"))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        endGame("You won\nYour opponent resigned");
                        closeAll();

                    });
                }
                else if (st.Contains("draw"))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        hide.Visibility = Visibility.Visible;
                        hide.Opacity = 0.50;
                        pop.Visibility = Visibility.Visible;
                        accDraw.Visibility = Visibility.Visible;
                        decDraw.Visibility = Visibility.Visible;
                        playAgain.Visibility = Visibility.Collapsed;
                        final.Text = "Your opponent offered you a draw";
                    });
                }
                else if (st.Contains("acce"))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        endGame("Game ended with a Draw");
                        closeAll();
                    });
                }
                else if (st.Contains("decl"))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        accDraw.Visibility = Visibility.Collapsed;
                        decDraw.Visibility = Visibility.Collapsed;
                        pop.Visibility = Visibility.Collapsed;
                        hide.Visibility = Visibility.Collapsed;
                        hide.Opacity = 0.0;
                        playAgain.Visibility= Visibility.Visible;
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        string cbrd = gameBoard.ToString();
                        string[] move = st.Split(' ');
                        int fromX = Int32.Parse(move[0]);
                        int fromY = Int32.Parse(move[1]);
                        int toX = Int32.Parse(move[2]);
                        int toY = Int32.Parse(move[3]);
                        gameBoard.click(new int[] {fromX, fromY});
                        gameBoard.click(new int[] { toX, toY });

                        playedMoves.Add(gameBoard.ToString());
                        
                        if (st[st.Length - 1] != 'c' && pop.Visibility == Visibility.Collapsed)
                        {
                            hide.Visibility = Visibility.Collapsed;
                            whosturn.Text = isHost ? "White's turn" : "Black's turn";

                        }
                        if (repetition(playedMoves, st))
                        {
                            pop.Visibility = Visibility.Visible;
                            hide.Visibility = Visibility.Visible;
                            hide.Opacity = 0.50;
                            final.Text = "Game ended with a Draw\ndue to repetition";
                        }


                    });
                }
            } while (st[st.Length - 1] == 'c');
        }

        private void Init()
        {
            


            int n = DIMENSION;
            buttons = new Button[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Background = (i + j) % 2 == 0 ? Brushes.Black : Brushes.White;

                    Grid.SetColumn(buttons[i, j], j);
                    Grid.SetRow(buttons[i, j], i);

                    if (i <= 2 && (i + j) % 2 == 0)
                    {

                        buttons[i, j].Background = black_p;
                    }
                    else if (i > (n / 2) && (i + j) % 2 == 0)
                    {

                        buttons[i, j].Background = white_p;
                    }
                    buttons[i, j].Name = "A_" + i + "_" + j;

                    buttons[i, j].Click += this.boardClick;


                    buttons[i, j].BorderThickness = new Thickness(0);
                    board.Children.Add(buttons[i, j]);

                }

            }

            gameBoard = new Board(buttons);

        }

        private void resb_Click(object sender, RoutedEventArgs e)
        {
            endGame("White won \nblack resigned");
        }

        private void res_Click(object sender, RoutedEventArgs e)
        {
            endGame("You Resigned");
            if (onlineGame)
            {
                string st = "res" + new string(' ', 9 - 3);
                socket.Send(StrToByte(st));
                closeAll();
            }
        }

        private void draw_Click(object sender, RoutedEventArgs e)
        {
            if (!onlineGame)
            {
                endGame("Game ended with a draw");
            }
            else
            {
                string st = "draw" + new string(' ', 9-4);
                socket.Send(StrToByte(st));
                hide.Visibility = Visibility.Visible;
                hide.Opacity = 0.50;
                pop.Visibility = Visibility.Visible;
                accDraw.Visibility = Visibility.Collapsed;
                decDraw.Visibility = Visibility.Collapsed;

                final.Text = "Waiting for your opponent...";
                BGWorker.RunWorkerAsync();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double l = this.Left;
            double t = this.Top;
            double w = this.Width;
            double h = this.Height;
            var newForm = new Start(l,t,w,h, this.WindowState); //create your new form.
            newForm.Show(); //show the new form.
            this.Close(); //
        }

        private void boardClick(object sender , RoutedEventArgs e)
        {

            Button temp = sender as Button;
            int x = Convert.ToInt32(temp.Name.Split('_')[1]);
            int y = Convert.ToInt32(temp.Name.Split('_')[2]);
            if (turn && isBotP1)
            {
                MiniMax m = new MiniMax(depthP1, turn);

                int[] mov = m.GetNextMove(gameBoard.ToString());

                gameBoard.click(new int[] { mov[0], mov[1] });
                gameBoard.click(new int[] { mov[2], mov[3] });

            }
            else if (!turn && isBotP2)
            {
                MiniMax m = new MiniMax(depthP2, turn);
                int[] mov = m.GetNextMove(gameBoard.ToString());
                gameBoard.click(new int[] { mov[0], mov[1] });
                gameBoard.click(new int[] { mov[2], mov[3] });
                
            }
            else
            {
                var lastButton = gameBoard.lastButton;

                gameBoard.click(new int[] { x, y });
                if (gameBoard.moveApplied && onlineGame)
                {
                   
                    int lastButtonX = Convert.ToInt32(lastButton.Name.Split('_')[1]);
                    int lastButtonY = Convert.ToInt32(lastButton.Name.Split('_')[2]);
                    socket.Send(StrToByte(lastButtonX + " " + lastButtonY + " " + x + " " + y + (gameBoard.getTurn()==isHost? " c" : "  ")));
                    if (gameBoard.getTurn() != isHost)
                    {
                        while (BGWorker.IsBusy) ;
                        BGWorker.RunWorkerAsync();
                    }

                }
                

            }
            if (updateStats() && ((turn && isBotP1 ) || (!turn && isBotP2)) )
            {
                Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
                gameBoard.envokeBot();
            }
           


        }

        public  Boolean repetition(List<string> l, string lastMove)
        {
            int count = 0;
            foreach (string s in l)
            {
                if (s == lastMove)
                {
                    count++;
                }
            }
            return count >= 3;
        }

        public Boolean noCapture(string firstMove, string lastMove)
        {
            return firstMove.Count(t => t == 'E') == lastMove.Count(t => t == 'E');
        }

        public bool updateStats()
        {
            int blackPieces = gameBoard.getPlayerNumPieces(blackTurn);
            int whitePieces = gameBoard.getPlayerNumPieces(whiteTurn);
            turn = gameBoard.getTurn();
            Bcount.Text = "" + blackPieces;
            Wcount.Text = "" + whitePieces;
            whosturn.Text = turn ? "White's turn" : "Black's turn";
            if (whitePieces == 0)
            {

                pop.Visibility = Visibility.Visible;
                hide.Visibility = Visibility.Visible;
                final.Text = "Black won \nwhite have no moves";
                return false;
            }
            else if (blackPieces == 0)
            {

                pop.Visibility = Visibility.Visible;
                hide.Visibility = Visibility.Visible;
                final.Text = "White won \nblack have no moves";
                return false;
            }
            else if (blackPieces <= 2 && whitePieces <= 2)
            {

                pop.Visibility = Visibility.Visible;
                hide.Visibility = Visibility.Visible;
                final.Text = "The game ended in a draw\n dou to insufficient material";
                return false;
            }
            else
            {
                string lastMove = gameBoard.ToString();
                if (playedMoves.Count ==0 || playedMoves.Last() != lastMove)
                {

                    playedMoves.Add(lastMove);
                }
                if (playedMoves.Count > 40)
                    playedMoves.RemoveAt(0);
                if (repetition(playedMoves, lastMove))
                {
                    pop.Visibility = Visibility.Visible;
                    hide.Visibility = Visibility.Visible;
                    hide.Opacity = 0.50;
                    final.Text = "The game ended in a draw\ndue to repetition";
                    return false;
                }
                if (playedMoves.Count == 40 && noCapture(playedMoves.ElementAt(0), lastMove))
                {
                    pop.Visibility = Visibility.Visible;
                    hide.Visibility = Visibility.Visible;
                    hide.Opacity = 0.50;
                    final.Text = "The game ended in a draw\ndue to no capture for 40 moves rule";
                    return false;
                }

            }
            return true;
        }

        bool SocketConnected()
        {
            bool part1 = socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        private string ReceiveMove()
        {
            var buffer = new List<char>();

            
            var currByte = new Byte[onlineMessageLength];//x y X Y c
            var byteCounter = socket.Receive(currByte, onlineMessageLength, SocketFlags.None);

            for (int i = 0; i < onlineMessageLength; i++)
            {
                buffer.Add(Convert.ToChar(currByte[i]));
            }
            return new string(buffer.ToArray());
        }

        void web_Closing(object sender, CancelEventArgs e)
        {
            BGWorker.WorkerSupportsCancellation = true;
            BGWorker.CancelAsync();
            if (server != null)
            {
                server.Stop();
            }

            // If data is dirty, notify user and ask for a response

        }

        byte[] StrToByte(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            return bytes;
        }

        string ByteToStr(byte[] b)
        {
            string str = Encoding.Default.GetString(b);
            return str;
        }

        void opponentDisconnected()
        {
            hide.Visibility = Visibility.Visible;
            hide.Opacity = 0.50;
            pop.Visibility = Visibility.Visible;
            socket.Close();
            if (isHost)
            {
                server.Stop();

            }
            else
            {
                clint.Close();
            }
            final.Text = "You won!!! \n opponent resigned";
        }

        void closeAll()
        {
            socket.Close();
            if (isHost)
            {
                server.Stop();
            }
            else
            {
                clint.Close();
            }
        }

        void endGame(string message)
        {
            pop.Visibility = Visibility.Visible;
            hide.Visibility = Visibility.Visible;
            hide.Opacity = 0.50;
            final.Text = message;
            playAgain.Visibility = Visibility.Visible;
            accDraw.Visibility = Visibility.Collapsed;
            decDraw.Visibility = Visibility.Collapsed;
        }


        void acceptDraw_Click(object sender, RoutedEventArgs r)
        {
            try
            {
                string st = "acce" + new string(' ', 9 - 4);
                socket.Send(StrToByte(st));
                endGame("Game ended with a Draw");
                closeAll();
            }
            catch
            {
               opponentDisconnected();
            }
        }

        void declineDraw_Click(object sender, RoutedEventArgs r)
        {
            try
            {
                string st = "decl" + new string(' ', 9 - 4);
                socket.Send(StrToByte(st));
                accDraw.Visibility = Visibility.Visible;
                decDraw.Visibility = Visibility.Visible;
                pop.Visibility = Visibility.Collapsed;
                hide.Opacity = 0.0;
                BGWorker.RunWorkerAsync();
            }
            catch
            {
                opponentDisconnected();
            }
        }
    }
}