using Minimax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Checkers2.Classes.Setups;


namespace Checkers2.Classes
{
    public class MiniMax
    {
        Board board;
        bool turn;
        Button[,] buttons = new Button[8, 8];
        public NTree t { get; set; }
        int maxDepth;
        public MiniMax(int depth , bool turn)
        {
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    buttons[i, j] = new Button();

                }

            }
            this.maxDepth = depth;
            this.turn = turn;
        }
        public int[] GetNextMove(string b)
        {
            int[] from = new int[] { 0, 0 };
            t = new NTree(from,from,b);

            buildBoard(b);

            treeBulid(t, b);
            
            int maxVal = turn? int.MaxValue:  int.MinValue;
            List<int[]> possipleMoves = new List<int[]>();

            foreach (var child in t.getAllChildren())
            {
                int[] mov = { child.from[0], child.from[1], child.to[0], child.to[1] };
                if(child.value > maxVal && !turn)
                {
                    maxVal = child.value;
                    possipleMoves.Clear();
                    possipleMoves.Add(mov);
                }
                else if(child.value == maxVal)
                {
                    possipleMoves.Add(mov);
                }
                else if(child.value < maxVal && turn)
                {
                    maxVal = child.value;
                    possipleMoves.Clear();
                    possipleMoves.Add(mov);
                }
            }
            Random rnd = new Random();
            int index = rnd.Next(possipleMoves.Count);
           
            return possipleMoves[index];
        }
        void buildBoard(string st)
        {
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    if (st[(i * DIMENSION) + j] == 'w')
                    {
                        buttons[i, j].Background = white_p;
                    }
                    else if (st[(i * DIMENSION) + j] == 'W')
                    {
                        buttons[i, j].Background = white_k;
                    }
                    else if (st[(i * DIMENSION) + j] == 'b')
                    {
                        buttons[i, j].Background = black_p;
                    }
                    else if (st[(i * DIMENSION) + j] == 'B')
                    {
                        buttons[i, j].Background = black_k;
                    }
                    else if (st[(i * DIMENSION) + j] == 'e')
                    {
                        buttons[i, j].Background = Brushes.White;
                    }
                    else if (st[(i * DIMENSION) + j] == 'E')
                    {
                        buttons[i, j].Background = Brushes.Black;
                    }
                    else if (st[(i * DIMENSION) + j] == 'r')
                    {
                        buttons[i, j].Background = Brushes.Red;
                    }
                    buttons[i, j].Name = "A_" + i + "_" + j;

                }
            }
            

            
            board = new Board(buttons);
            board.setTurn(turn);
            board.curBoard = st;
            board.getPlayer(turn).update(turn,st);

        }
        private void treeBulid(NTree root, string b, int curDepth = 0)
        {
            if (!b.Contains("b") && !b.Contains("B"))
            {
                root.value = int.MinValue;
                return;
            }
            if (!b.Contains("w") && !b.Contains("W"))
            {
                root.value = int.MaxValue;
                return;
            }
            if (curDepth == maxDepth)
            {

                
                return;
            }

            buildBoard(b);

            var player = board.getPlayer(turn);

            foreach (Piece piece in player.getAllPeices()) 
            {

                if (board.piceCanMove(piece.getPos(), board.ToString(),turn))
                {

                    foreach (int[] move in board.pieceMoves(piece.getPos(), b, turn))
                    {
                        root.AddChild(piece.getPos(), move, board.ToString());
                        board.click(piece.getPos());
                        board.click(move);

                        
                        buildBoard(b);
                    }
                }
            

            }
            foreach (var child in root.getAllChildren())
            {
                treeBulid(child, child.board, curDepth + 1);
            }   
        }

    }
}
