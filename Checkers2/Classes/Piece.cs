using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers2.Classes
{
    public abstract class Piece
    {
        private bool King;
        private bool color;//true = white  false = black
        private int[] pos;
        public Piece(bool colorIsWhite,int[] possition,bool isKing = false) {
            this.King = isKing;
            color = colorIsWhite;
            pos = possition;
        }
        public abstract int[][] Moves( string board);
        public abstract bool canSkip( string board);
        public abstract bool canMove( string board);

        public bool isKing(){return this.King;}
        public bool getColor() { return color; }
        public int[] getPos() { return pos;}
        public void setKing(bool isKing) { King = isKing;}
        public void MoveTo(int[] newPos) {  this.pos = newPos;}

       
    }
}
