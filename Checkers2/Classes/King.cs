using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Checkers2.Classes
{
    public class King : Piece
    {
        public King(bool colorIsWhite, int[] possition) : base(colorIsWhite, possition, true)
        {
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

        public override bool canMove( string board)
        {
            var pos = this.getPos();
            int x = pos[0];
            int y = pos[1];
            return (x > 0 && y > 0 && board[(x - 1) * 8 + y - 1] == 'E') || (x > 0 && y < 7 && board[(x - 1) * 8 + y + 1] == 'E') || (x < 7 && y > 0 && board[(x + 1) * 8 + y - 1] == 'E') || (x < 7 && y < 7 && board[(x + 1) * 8 + y + 1] == 'E');

        }

        public override int[][] Moves( string board)
        {
            var pos = this.getPos();
            if (canSkip( board))
            {
                if (getColor())
                {
                    return getWhiteMoves(pos, board);
                }
                else
                {
                    return getBlackMoves(pos, board);
                }
            }
            else
            {
                int x = pos[0];
                int y = pos[1];
                Stack<int[]> ret = new Stack<int[]>();
                for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++,j++)
                {

                    if (board[i*8+j] != 'E')
                    {
                        break;
                    }
                    ret.Push(new int[] { i, j });
                }
                for (int i = x + 1, j = y - 1; i < 8 && j >= 0; i++,j--)
                {

                    if (board[i*8+j] != 'E')
                    {
                        break;
                    }
                    ret.Push(new int[] { i, j });
                }
                for (int i = x - 1, j = y + 1; i >= 0 && j < 8; i--,j++)
                {

                    if (board[i*8+j] != 'E')
                    {
                        break;
                    }
                    ret.Push(new int[] { i, j });
                }
                for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--,j--)
                {

                    if (board[i*8+j] != 'E')
                    {
                        break;
                    }
                    ret.Push(new int[] { i, j });
                }
                return ret.ToArray();
            }
        }

        private bool blackCanSkip(int[] pos, string board)
        {
            int x = pos[0];
            int y = pos[1];

            for (int i = x + 1, j = y + 1; i < 7 && j < 7; i++)
            {

                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && (board[(i + 1) * 8 + j + 1] == 'E' || board[(i + 1) * 8 + j + 1] == 'E'))
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                j++;
            }
            for (int i = x + 1, j = y - 1; i < 7 && j > 0; i++)
            {

                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && (board[(i + 1) * 8 + j - 1] == 'E' || board[(i + 1) * 8 + j - 1] == 'E'))
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                j--;
            }
            for (int i = x - 1, j = y + 1; i > 0 && j < 7; i--)
            {

                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && (board[(i - 1) * 8 + j + 1] == 'E' || board[(i - 1) * 8 + j + 1] == 'E'))
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                j++;
            }
            for (int i = x - 1, j = y - 1; i > 0 && j > 0; i--)
            {

                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && (board[(i - 1) * 8 + j - 1] == 'E' || board[(i - 1) * 8 + j - 1] == 'E'))
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                j--;
            }

            return false;
        }
        private bool whiteCanSkip(int[] pos, string board)
        {
            int x = pos[0];
            int y = pos[1];
           
            for (int i = x + 1, j = y + 1; i < 7 && j < 7; i++,j++)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i+1)*8+j+1] == 'E' )
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                
            }
            for (int i = x + 1, j = y - 1; i < 7 && j > 0; i++,j--)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i+1)*8+j-1] == 'E')
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                
            }
            for (int i = x - 1, j = y + 1; i > 0 && j < 7; i--,j++)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i-1)*8+j+1] == 'E')
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                
            }
            for (int i = x - 1, j = y - 1; i > 0 && j > 0; i--,j--)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i-1)*8+j-1] == 'E' )
                {
                    return true;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
                
            }

            return false;
        }
        private int[][] getBlackMoves(int[] pos,string board)
        {
            int x = pos[0];
            int y = pos[1];
            Stack<int[]> ret = new Stack<int[]>();

            for (int i = x + 1, j = y + 1; i < 7 && j < 7; i++, j++)
            {

                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && board[(i+1)*8+j+1] == 'E')
                {
                    ret.Push(new int[]{i+1,j+1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            for (int i = x + 1, j = y - 1; i < 7 && j > 0; i++, j--)
            {


                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && board[(i+1)*8+j-1] == 'E')
                {
                    ret.Push(new int[]{i+1,j-1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            for (int i = x - 1, j = y + 1; i > 0 && j < 7; i--, j++)
            {
                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && board[(i-1)*8+j+1] == 'E')
                {
                    ret.Push(new int[]{i-1,j+1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            for (int i = x - 1, j = y - 1; i > 0 && j > 0; i--, j--)
            {

                if ((board[i*8+j] == 'w' || board[i*8+j] == 'W') && board[(i-1)*8+j-1] == 'E')
                {
                    ret.Push(new int[]{i-1,j-1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            return ret.ToArray();
           
            
        }
        private int[][] getWhiteMoves(int[] pos , string board)
        {
            int x = pos[0];
            int y = pos[1];
            Stack<int[]> ret = new Stack<int[]>();
            for (int i = x + 1, j = y + 1; i < 7 && j < 7; i++,j++)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i+1)*8+j+1] == 'E')
                {
                   ret.Push(new int[]{i+1,j+1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            for (int i = x + 1, j = y - 1; i < 7 && j > 0; i++,j--)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i+1)*8+j-1] == 'E')
                {
                    ret.Push(new int[]{i+1,j-1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            for (int i = x - 1, j = y + 1; i > 0 && j < 7; i--,j++)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i-1)*8+j+1] == 'E')
                {
                    ret.Push(new int[]{i-1,j+1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            for (int i = x - 1, j = y - 1; i > 0 && j > 0; i--,j--)
            {

                if ((board[i*8+j] == 'b' || board[i*8+j] == 'B') && board[(i-1)*8+j-1] == 'E')
                {
                    ret.Push(new int[]{i-1,j-1});
                    break;
                }
                else if (board[i*8+j] == 'w' || board[i*8+j] == 'W' || board[i*8+j] == 'B' || board[i*8+j] == 'b')
                {
                    break;
                }
            }
            return ret.ToArray();

        }
    }
}
