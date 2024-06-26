using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Checkers2.Classes.Setups;


namespace Checkers2.Classes
{
    public class Board
    {
        Player p1;
        Player p2;
        public string curBoard { set; get; }
        Button[,] boardButtons;
        bool turn = whiteTurn;
        public Button lastButton;
        int[] lastEat;
        public bool moveApplied;
        public Board(Button[,] buttons)
        {

            
            curBoard = "";

            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    if (i < (DIMENSION / 2) - 1 && (i + j) % 2 == 0)
                    {

                        curBoard += "b";
                    }
                    else if (i > (DIMENSION / 2) && (i + j) % 2 == 0)
                    {

                        curBoard += "w";
                    }
                    else
                    {
                        curBoard += (i + j) % 2 == 0 ? "E" : "e";
                    }
                }
            }

            boardButtons = buttons;
            p1 = new Player(whiteTurn, curBoard);
            p2 = new Player(blackTurn, curBoard);
        }

        public void click(int[] pos)
        {

            var curPlayer = turn ? p1 : p2;
            var otherPlayer = !turn ? p1 : p2;
            moveApplied = false;
            var piece = curPlayer.getPiece(pos) ;
            if (piece != null)
            {

                redToBlack();
                if (curPlayer.canSkip(curBoard) && !piece.canSkip(curBoard))
                {


                    return;
                }
                

                
                var moves = piece.Moves(curBoard);
                
                for(int i = 0;i < moves.Length;i++)
                {
                    

                    var mov = moves[i];
                    boardButtons[mov[0], mov[1]].Background = Brushes.Red;


                }
            }
            else
            {

                if ( boardButtons[pos[0], pos[1]].Background == Brushes.Red)
                {

                    int x = Convert.ToInt32(lastButton.Name.Split('_')[1]);
                    int y = Convert.ToInt32(lastButton.Name.Split('_')[2]);
                    int[] from = new int[] { x, y };
                    piece = curPlayer.getPiece(from);
                    moveApplied = true;
                    if (piece.canSkip(curBoard)) {
                        var lostPawnPos = calcEatenPicePos(x, y, pos);
                        otherPlayer.pawnLost(lostPawnPos);
                        boardButtons[lostPawnPos[0], lostPawnPos[1]].Background= Brushes.Black;
                        lastEat = pos;
                    }
                    else
                    {
                        lastEat = null;
                    }

                    piece = curPlayer.update(piece, pos);
                    updateBoard(from, pos);
                    if (pos[0] == (DIMENSION-1)*(turn ? 0 : 1) && !piece.isKing())
                    {
                       
                        promote(turn, pos);    
                        curPlayer.promote(pos);
                        piece = curPlayer.getPiece(pos);
                    }
                    if (!(piece.canSkip(curBoard) && lastEat == piece.getPos())) {
                        turn = !(turn);
                    }
                   
                    removeBorders();
                    boardButtons[x,y].BorderThickness = new Thickness(3);
                    boardButtons[x, y].BorderBrush = Brushes.Red;
                    boardButtons[pos[0], pos[1]].BorderThickness = new Thickness(3);
                    boardButtons[pos[0], pos[1]].BorderBrush = Brushes.Red;
                }
                
                redToBlack();
            }

            
            curBoard = BoardToString();
            lastButton = boardButtons[pos[0],pos[1]];
        }
        private void updateBoard(int[] from , int[] to)
        {
            boardButtons[to[0], to[1]].Background = boardButtons[from[0], from[1]].Background;
            boardButtons[from[0], from[1]].Background = Brushes.Red;
            curBoard = BoardToString();
        }
        public void redToBlack()
        {
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    if (boardButtons[i,j].Background == Brushes.Red)
                    {
                        boardButtons[i, j].Background = Brushes.Black;
                    }
                }
            }
       }

        public  string BoardToString(bool countRed = false)
        {

            string s = "";
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    if (boardButtons[i, j].Background == white_k)
                    {
                        s += "W";
                    }
                    else if (boardButtons[i, j].Background == white_p)
                    {
                        s += "w";
                    }
                    else if (boardButtons[i, j].Background == black_p)
                    {
                        s += "b";
                    }
                    else if (boardButtons[i, j].Background == black_k)
                    {
                        s += "B";
                    }
                    else if (boardButtons[i, j].Background == Brushes.White)
                    {
                        s += "e";
                    }
                    else if ( boardButtons[i, j].Background == Brushes.Red && countRed)
                    {
                        s += "r";
                    }
                    else
                    {
                        s += "E";
                    }
                }
            }
            return s;
        }
        public void promote(bool color, int[]pos)
        {
            boardButtons[pos[0],pos[1]].Background = color? white_k : black_k ;
        }
        public int[] calcEatenPicePos(int x, int y , int[] to)
        {
           return new int[] { x > to[0] ? to[0] + 1 : to[0] - 1 , y> to[1] ? to[1] + 1 : to[1] - 1 };
        }
        public void removeBorders()
        {
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    if (boardButtons[i, j].BorderBrush == Brushes.Red)
                    {
                        boardButtons[i, j].BorderThickness = new Thickness(0);
                        boardButtons[i, j].BorderBrush = Brushes.Black;
                    }
                }
            }
        }
        public int getPlayerNumPieces(bool Player1)
        {
            return Player1? p1.numPieces() : p2.numPieces();
        }
        public bool getTurn()
        {
            return turn;
        }
        public void setTurn(bool turn)
        {
             this.turn = turn;
        }
        public Player getPlayer(bool Player)
        {
            return Player ? p1 : p2;
        }
        public bool piceCanMove(int[] pos, string board,bool player)
        {
            var p = player ? p1 : p2;
            var pieces = p.getAllPeices();

            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i]?.getPos()[0] == pos[0] && pieces[i]?.getPos()[1] == pos[1])
                {
                    return (!p.canSkip(board) && pieces[i].canMove(board)) || pieces[i].canSkip(board);
                }
            }
            return false;
        }
        public int[][] pieceMoves(int[] pos, string board, bool player)
        {
            var pieces = player ? p1.getAllPeices() : p2.getAllPeices();
            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i]?.getPos()[0] == pos[0] && pieces[i]?.getPos()[1] == pos[1])
                {

                    return pieces[i].Moves(board);
                }
            }
            return null;
        }
        public override string ToString()
        {
            
            return curBoard;
        }
        public void envokeBot()
        {
            
            boardButtons[0, 1].RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
    
}
