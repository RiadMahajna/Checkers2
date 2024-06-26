using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using static Checkers2.Classes.Setups;


namespace Checkers2.Classes
{
    public  class Player
    {
        List<Piece> pieces = new List<Piece>();
        public bool color { get; }

        public Player(bool isWhite,string board) {
            color = isWhite;

            update(isWhite, board);
        }

        public Piece update(Piece p1 , int[] pos)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (p1 == pieces[i])
                {
                    pieces[i].MoveTo(pos);
                    return pieces[i];
                }
            }
            return p1;
        }
        public  Piece getPiece(int[] pos)
        {
           

            for (int i = 0; i < pieces.Count; i++)
            {

                if (pieces[i].getPos()[0] == pos[0] && pieces[i].getPos()[1] == pos[1])
                {
                    
                    return pieces[i];
                }
            }
            
            return null;
        }
        public Piece[] getAllPeices()
        {
            return pieces.ToArray();
        }
        public int numPieces()
        {
            return pieces.Count;
        }
        public bool canSkip(string board)
        {
            for(int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i].canSkip(board))
                {
                    return true;
                }
            }
            return false;
        }
        public void promote(int[] pos)
        {
            for(int i=0;i<pieces.Count; i++)
            {
                if (pieces[i]?.getPos()[0] == pos[0] && pieces[i]?.getPos()[1] == pos[1])
                {
                    pieces[i] = new King(color, pos);
                    return;
                }
            }
        }
        public void pawnLost(int[] pos)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i]?.getPos()[0] == pos[0] && pieces[i]?.getPos()[1] == pos[1])
                {
                    pieces.RemoveAt(i);
                    return;
                }
            }
        }
        public override string ToString()
        {
            string s = "";
            foreach(var piece in pieces)
            {
                s += "" + piece.getPos()[0] + ", " + +piece.getPos()[1] + "\n";
            }
            return s;
        }
        public void update(bool isWhite , string board)
        {
            pieces.Clear();
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    if (board[i * DIMENSION + j] == (isWhite ? 'w' : 'b'))
                    {
                        var t = new Pawn(isWhite, new int[] { i, j });
                        pieces.Add(t);
                    }
                    else if (board[i * DIMENSION + j] == (isWhite ? 'W' : 'B'))
                    {
                        var t = new King(isWhite, new int[] { i, j });
                        pieces.Add(t);
                    }
                }
            }
        }



    }
}
