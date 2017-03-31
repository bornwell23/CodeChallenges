using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace _15_Puzzle
{
    public partial class MainWindow : Window
    {
        private Color m_TileColors = Colors.Aqua;
        private int m_Size = 4;
        private TileRectangle m_RectangleToMove = null;
        private TileTextBlock m_TextBlockToMove = null;

        public MainWindow()
        {
            InitializeComponent();
            InitTiles();
        }

        private void InitTiles()
        {
            try { 
                for (int i = 0; i < m_Size; ++i)
                {
                    for (int j = 0; j < m_Size; ++j)
                    {
                        TileRectangle tileRectangle = new TileRectangle();
                        tileRectangle.Name = "_" + i + "x" + j;
                        tileRectangle.Width = 100;
                        tileRectangle.Height = 100;
                        tileRectangle.Row = i;
                        tileRectangle.Column = j;
                        tileRectangle.Fill = new SolidColorBrush(m_TileColors);
                        Grid.SetRow(tileRectangle, i);
                        Grid.SetColumn(tileRectangle, j);
                        TileGrid.Children.Add(tileRectangle);
                    }
                }
                Random random = new Random();
                int randomI = random.Next(m_Size);
                int randomJ = random.Next(m_Size);
                TileGrid.Children.RemoveAt(randomI*m_Size+randomJ);
                List<int> intList = new List<int>();
                for (int i = 1; i < (m_Size*m_Size); ++i)
                {
                    intList.Add(i);
                }
                TileRectangle[] tileRectangleArray = new TileRectangle[m_Size*m_Size-1];
                TileGrid.Children.CopyTo(tileRectangleArray, 0);
                foreach(TileRectangle tileRectangle in tileRectangleArray)
                {
                    TileTextBlock tileTextBlock = new TileTextBlock();
                    tileTextBlock.Name = "__" + tileRectangle.Row + "x" + tileRectangle.Column;
                    tileTextBlock.Row = tileRectangle.Row;
                    tileTextBlock.Column = tileRectangle.Column;
                    int index = random.Next(intList.Count);
                    tileTextBlock.Text = "" + intList[index];
                    tileTextBlock.TextAlignment = TextAlignment.Center;
                    tileTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    tileTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    intList.RemoveAt(index);
                    Grid.SetRow(tileTextBlock, tileRectangle.Row);
                    Grid.SetColumn(tileTextBlock, tileRectangle.Column);
                    TileGrid.Children.Add(tileTextBlock);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oops! There was an error! Please tell the developer: " + ex.Message, "Error");
                return;
            }
        }

        private void TileGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //get tile to manipulate if user makes a valid drag movement
                Point clickPoint = e.GetPosition(TileGrid);
                int clickedRow = (int)Math.Floor(clickPoint.Y / 100);
                int clickedColumn = (int)Math.Floor(clickPoint.X / 100);
                m_RectangleToMove = TileGrid.Children.OfType<TileRectangle>().First(t => t.Row == clickedRow && t.Column == clickedColumn);
                m_TextBlockToMove = TileGrid.Children.OfType<TileTextBlock>().First(t => t.Row == clickedRow && t.Column == clickedColumn);
            }
            catch(Exception ex)
            {
                if(ex.Message.Contains("Sequence contains no matching element"))
                {
                    //user did not click on a tile
                    m_RectangleToMove = null;
                    m_TextBlockToMove = null;
                    return;
                }
                MessageBox.Show("Oops! There was an error! Please tell the developer: " + ex.Message, "Error");
                return;
            }
        }

        private void TileGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try { 
                if (m_RectangleToMove == null || m_TextBlockToMove==null) //user didn't start dragging a tile
                {
                    return;
                }
                Point clickedPoint = e.GetPosition(TileGrid);
                int clickedRow = (int)Math.Floor(clickedPoint.Y / 100);
                int clickedColumn = (int)Math.Floor(clickedPoint.X / 100);
                if (clickedRow == m_RectangleToMove.Row && clickedColumn == m_RectangleToMove.Column) //don't move tile if it the user didn't drag to a different space
                {
                    return;
                }
                int columnDistance = Math.Abs(clickedColumn - m_RectangleToMove.Column);
                int rowDistance = Math.Abs(clickedRow - m_RectangleToMove.Row);
                if (rowDistance==columnDistance || rowDistance>1 || columnDistance>1) //don't move if the tile is not adjacent
                {
                    return;
                }
                bool emptySpace = true;
                foreach(TileRectangle tileRectangle in TileGrid.Children.OfType<TileRectangle>())
                {
                    if(tileRectangle.Row==clickedRow && tileRectangle.Column == clickedColumn)
                    {
                        emptySpace = false; //don't move tile if there is a tile in the intended destination
                    }
                }
                if (!emptySpace)
                {
                    return;
                }
                Grid.SetRow(m_RectangleToMove, clickedRow);
                Grid.SetColumn(m_RectangleToMove, clickedColumn);
                m_RectangleToMove.Row = clickedRow;
                m_RectangleToMove.Column = clickedColumn;
                Grid.SetRow(m_TextBlockToMove, clickedRow);
                Grid.SetColumn(m_TextBlockToMove, clickedColumn);
                m_TextBlockToMove.Row = clickedRow;
                m_TextBlockToMove.Column = clickedColumn;
                if (m_TextBlockToMove.Row == m_Size - 1 && m_TextBlockToMove.Column == m_Size - 2) //the user placed the last item in the spot where the last item should go so check if everything is in order
                {
                    var enumer = TileGrid.Children.OfType<TileTextBlock>().OrderBy(t => int.Parse(t.Text));
                    int testRow = 0;
                    int testColumn = 0;
                    foreach(var textBlock in enumer)
                    {
                        if(textBlock.Row==testRow && textBlock.Column == testColumn)
                        {
                            if (testColumn + 1 == m_Size)
                            {
                                testColumn = 0;
                                ++testRow;
                            }
                            else
                            {
                                ++testColumn;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    //all tiles are in order, user won
                    TextBlock winningText = new TextBlock();
                    winningText.Text = "You Won!";
                    winningText.TextAlignment = TextAlignment.Center;
                    winningText.VerticalAlignment = VerticalAlignment.Center;
                    winningText.HorizontalAlignment = HorizontalAlignment.Center;
                    winningText.Foreground = new SolidColorBrush(Colors.White);
                    Grid.SetRow(winningText, m_Size - 1);
                    Grid.SetColumn(winningText, m_Size - 1);
                    TileGrid.Children.Add(winningText);
                }
                m_RectangleToMove = null;
                m_TextBlockToMove = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oops! There was an error! Please tell the developer: " + ex.Message, "Error");
                return;
            }
        }
    }
}
