using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordsearch
{
    internal class GridLetter
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Letter { get; set; }

        public GridLetter() { }
        public GridLetter(int x, int y,char letter)
        {
            X = x;
            Y = y;
            Letter = letter;
        }
    }
}
