using Checkers2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Minimax
{
    public class NTree//simple tree to use in MiniMax algorithm
    {
        public int value { get; set; }
        public int[] from { get; set; }
        public int[] to { get; set; }
        public string board {  get; set; }
        List<NTree> children = new List<NTree>();

        public NTree(int[] from ,int[] to, string board)
        {
            this.to = to;
            this.board = board;
            int wp = 0, bp = 0;
            wp = board.Count(t => t == 'w') + board.Count(t => t == 'W');
            bp = board.Count(t => t == 'b') + board.Count(t => t == 'B');

            value = bp - wp;
            this.from = from;

        }

        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }

        public void AddChild(int[] from, int[] to , string board )
        {

            children.Add(new NTree(from , to, board));

        }

        public NTree GetChild(int i)
        {
            if(children.Count > i)
            {
                return children[i];
            }
            return null;
        }

        public int GetChildrenNum()
        {
            return children.Count;
        }
        public List<NTree> getAllChildren()
        {
            return children;
        }
    }
}