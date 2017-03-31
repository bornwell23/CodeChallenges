using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _15_Puzzle
{
    class TileTextBlock : TextBlock
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
