using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Checkers2.Classes
{
    public class Pawn : Piece
    {
        public Pawn(bool colorIsWhite, int[] possition) : base(colorIsWhite, possition, false)
        {
        }

        public override bool canMove( string board)
        {
            var pos = this.getPos();
            if (canSkip(board))
            {
                return true;
            }
            if (!getColor())
            {
                return whiteCanMove(pos, board);
            }
            else
            {
                return BlackCanMove(pos, board);
            }
        }

        public override bool canSkip( string board)
        {
            var pos = this.getPos();
            if (getColor())
            {
                return whiteCanSkip(pos, board);
            }
            else
            {
                return blackCanSkip(pos, board);
            }
        }

        public override int[][] Moves( string board)
        {
            var pos = this.getPos();
            if (getColor())
            {
                return getWhiteMoves(pos, board);
            }
            else
            {
                return getBlackMoves(pos, board);
            }
            
           
        }
        private bool blackCanSkip(int[] pos, string board) 
        {
            int x = pos[0];
            int y = pos[1];
            if (x + 2 < 8 && y - 2 >= 0)
            {

                if ((board[(x+1)*8 +y -1] == 'w' || board[(x+1)*8 +y -1] == 'W') && (board[(x+2)*8 +y -2] == 'E' ))
                {

                    return true;
                }
            }
            if (x + 2 < 8 && y + 2 < 8)
            {
                if ((board[(x+1)*8 +y +1] == 'w' || board[(x+1)*8 +y +1] == 'W') && (board[(x+2)*8 +y +2] == 'E' ))
                {
                    return true;
                }
            }
            return false;
        }
        private bool whiteCanSkip(int[] pos, string board)
        {
            int x = pos[0];
            int y = pos[1];
            if (x - 2 >= 0 && y - 2 >= 0)
            {
                if ((board[(x-1)*8 +y -1] == 'B' || board[(x-1)*8 +y -1] == 'b') && (board[(x - 2) * 8 + y - 2] == 'E'))
                {
                    return true;
                }
            }
            if (x - 2 >= 0 && y + 2 < 8)
            {
                if ((board[(x-1)*8 +y +1] == 'B' || board[(x-1)*8 +y +1] == 'b') && (board[(x - 2) * 8 + y + 2] == 'E'))
                {
                    return true;
                }
            }
            return false;
        }
        private bool BlackCanMove(int[] pos, string board)
        {
            int x = pos[0];
            int y = pos[1];
            
            return (x > 0 && y > 0 && board[(x - 1)*8+ y - 1] == 'E') || (x > 0 && y < 7 && board[(x - 1) * 8 + y + 1] == 'E') ;
        }
        private bool whiteCanMove(int[] pos, string board)
        {
            int x = pos[0];
            int y = pos[1];

            return (x < 7 && y > 0 && board[(x + 1)*8 + y - 1] == 'E') || (x < 7 && y < 7 && board[(x + 1)*8 + y + 1]== 'E');
        }
        private int[][] getBlackMoves(int[] pos, string board) 
        {
            int x = pos[0];
            int y = pos[1];
            Stack<int[]> ret = new Stack<int[]>();
            if (canSkip(board))
            {
                if (x + 2 < 8 && y - 2 >= 0)
                {
                    if ((board[(x+1)*8 +y -1] == 'w' || board[(x+1)*8 +y -1] == 'W') && board[(x+2)*8 +y -2] == 'E')
                    {
                        ret.Push(new int[] { x +2, y -2 });
                    }
                }
                if (x + 2 < 8 && y + 2 < 8)
                {
                    if ((board[(x+1)*8 +y +1] == 'w' || board[(x+1)*8 +y +1] == 'W') && board[(x+2)*8 +y +2] == 'E')
                    {
                        ret.Push(new int[] { x +2, y +2 });
                    }
                }
            }
            else
            {
                if (x + 1 < 8 && y - 1 >= 0 && board[(x+1)*8 +y -1] == 'E')
                {

                    ret.Push(new int[] { x + 1, y - 1 });

                }
                if (x + 1 < 8 && y + 1 < 8 && board[(x+1)*8 +y +1] == 'E')
                {

                    ret.Push(new int[] { x + 1, y + 1 });

                }
            }
            return ret.ToArray();
        }
        private int[][] getWhiteMoves(int[] pos, string board)
        {
            int x = pos[0];
            int y = pos[1];
            Stack<int[]> ret = new Stack<int[]>();
            if (canSkip(board))
            {
                if (x - 2 >= 0 && y - 2 >= 0)
                {
                    if ((board[(x-1)*8 +y -1] == 'B' || board[(x-1)*8 +y -1] == 'b') && board[(x-2)*8 +y -2] == 'E')
                    {
                        ret.Push(new int[] { x - 2, y - 2 });
                    }
                }
                if (x - 2 >= 0 && y + 2 < 8)
                {
                    if ((board[(x-1)*8 +y +1] == 'B' || board[(x-1)*8 +y +1] == 'b') && board[(x-2)*8 +y +2] == 'E')
                    {
                        ret.Push(new int[] { x - 2, y + 2 });
                    }
                }
            }
            else
            {
                if (x - 1 >= 0 && y - 1 >= 0 && board[(x-1)*8 +y -1] == 'E')
                {

                    ret.Push(new int[] { x - 1, y - 1 });

                }
                if (x - 1 >= 0 && y + 1 < 8 && board[(x-1)*8 +y +1] == 'E')
                {

                    ret.Push(new int[] { x - 1, y + 1 });
                }
            }
            return ret.ToArray();
        }

    }
}
