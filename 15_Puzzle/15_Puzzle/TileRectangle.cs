using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _15_Puzzle
{
    class TileRectangle : Shape
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public TileRectangle() : base()
        {
            
        }

        protected override Geometry DefiningGeometry { get { return new RectangleGeometry(new Rect(0, 0, Width, Height)); } }
    }
}
