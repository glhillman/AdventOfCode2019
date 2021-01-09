using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    public class RecursiveGrid
    {
        public List<char[,]> _grids;
        private int srcIndex;
        private int dstIndex;
        public enum Selector
        {
            Source,
            Destination
        };

        public RecursiveGrid(int depth, char[,] initialGrid)
        {
            RecursiveGridInternal(initialGrid);
            Depth = depth;
        }

        public RecursiveGrid(int depth, RecursiveGrid innerGrid)
        {
            RecursiveGridInternal(null);
            InnerGrid = innerGrid;
            InnerGrid.OuterGrid = this;
            Depth = depth;
        }

        private void RecursiveGridInternal(char[,] initialGrid)
        {
            _grids = new List<char[,]>();

            _grids.Add(InitializeGrid(new char[5, 5]));
            _grids.Add(InitializeGrid(new char[5, 5]));

            if (initialGrid != null)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        _grids[0][x, y] = initialGrid[x, y];
                    }
                }
            }

            _grids[0][2, 2] = '?';
            _grids[1][2, 2] = '?';

            srcIndex = 0;
            dstIndex = 1;

            InnerGrid = null;
            OuterGrid = null;
        }

        public static char[,] InitializeGrid(char[,] grid)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    grid[x, y] = '.';
                }
            }
            grid[2, 2] = '?';

            return grid;
        }

        public bool ApplyRules()
        {
            bool needOuter = false;

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    int adjacentCount = 0;


                    if (InnerGrid != null)
                    {
                        if (x == 2)
                        {
                            if (y == 1)
                            {
                                adjacentCount += InnerGrid.TopEdgeCount(Selector.Source);
                            }
                            else if (y == 3)
                            {
                                adjacentCount += InnerGrid.BottomEdgeCount(Selector.Source);
                            }
                        }
                        if (y == 2)
                        {
                            if (x == 1)
                            {
                                adjacentCount += InnerGrid.LeftEdgeCount(Selector.Source);
                            }
                            else if (x == 3)
                            {
                                adjacentCount += InnerGrid.RightEdgeCount(Selector.Source);
                            }
                        }
                    }
                    if (OuterGrid != null)
                    {
                        if (x == 0)
                        {
                            adjacentCount += OuterGrid.CheckForBug(1, 2) ? 1 : 0;
                        }
                        if (x == 4)
                        {
                            adjacentCount += OuterGrid.CheckForBug(3, 2) ? 1 : 0;
                        }
                        if (y == 0)
                        {
                            adjacentCount += OuterGrid.CheckForBug(2, 1) ? 1 : 0;
                        }
                        if (y == 4)
                        {
                            adjacentCount += OuterGrid.CheckForBug(2, 3) ? 1 : 0;
                        }
                    }
                    if (x == 2 && y == 2 && InnerGrid != null)
                    {
                        // recurse in
                        InnerGrid.ApplyRules();
                    }
                    else
                    {
                        if (!(x == 2 && y == 2))
                        {
                            // count adjacent bugs
                            adjacentCount += (x > 0 && _grids[srcIndex][x - 1, y] == '#') ? 1 : 0;
                            adjacentCount += (x < 5 - 1 && _grids[srcIndex][x + 1, y] == '#') ? 1 : 0;
                            adjacentCount += (y > 0 && _grids[srcIndex][x, y - 1] == '#') ? 1 : 0;
                            adjacentCount += (y < 5 - 1 && _grids[srcIndex][x, y + 1] == '#') ? 1 : 0;
                        }
                    }

                    char newState = _grids[srcIndex][x, y];

                    if (_grids[srcIndex][x, y] == '#')
                    {
                        if (adjacentCount != 1)
                        {
                            newState = '.';
                        }
                    }
                    else
                    {
                        if (adjacentCount == 1 || adjacentCount == 2)
                        {
                            newState = '#';
                        }
                    }

                    _grids[dstIndex][x, y] = newState;
                }
            }

            if (CountAroundInnerSpot(_grids[dstIndex]) > 0 && InnerGrid == null)
            {
                // allocate a new inner grid
                InnerGrid = new RecursiveGrid(Depth + 1, InitializeGrid(new char[5, 5])); // dumb - need an empty constructor
                InnerGrid.OuterGrid = this;
            }

            // see if we need to grow outward - if so, return true & outer control will allocate a new one
            needOuter = EdgeCount(Selector.Destination) > 0;

            return needOuter && OuterGrid == null;
        }

        public int CountBugs()
        {
            int count = 0;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    count += _grids[srcIndex][x, y] == '#' ? 1 : 0;
                }
            }

            return count;
        }

        public bool CheckForBug(int x, int y)
        {
            return _grids[srcIndex][x, y] == '#';
        }

        public void SwitchSrcAndDst()
        {
            srcIndex = srcIndex == 1 ? 0 : 1;
            dstIndex = dstIndex == 1 ? 0 : 1;
        }

        public char[,] SourceGrid
        {
            get { return _grids[srcIndex]; }
        }

        public char[,] DestinationGrid
        {
            get { return _grids[dstIndex]; }
        }

        public int EdgeCount(Selector selectedIndex)
        {
            return TopEdgeCount(selectedIndex) + BottomEdgeCount(selectedIndex) + LeftEdgeCount(selectedIndex) + RightEdgeCount(selectedIndex);
        }

        public int TopEdgeCount(Selector selectedIndex)
        {
            return HorizontalEdgeCount(selectedIndex, 0);
        }

        public int BottomEdgeCount(Selector selectedIndex)
        {
            return HorizontalEdgeCount(selectedIndex, 4);
        }

        public int LeftEdgeCount(Selector selectedIndex)
        {
            return VerticalEdgeCount(selectedIndex, 0);
        }

        public int RightEdgeCount(Selector selectedIndex)
        {
            return VerticalEdgeCount(selectedIndex, 4);
        }

        private int HorizontalEdgeCount(Selector selectedIndex, int y)
        {
            int index = selectedIndex == Selector.Source ? srcIndex : dstIndex;

            int count = 0;

            for (int x = 0; x < 5; x++)
            {
                count += _grids[index][x, y] == '#' ? 1 : 0;
            }
            
            return count;
        }

        private int VerticalEdgeCount(Selector selectedIndex, int x)
        {
            int index = selectedIndex == Selector.Source ? srcIndex : dstIndex;

            int count = 0;

            for (int y = 0; y < 5; y++)
            {
                count += _grids[index][x, y] == '#' ? 1 : 0;
            }

            return count;
        }

        private int CountAroundInnerSpot(char[,] grid)
        {
            int count = 0;

            count += grid[2, 1] == '#' ? 1 : 0;
            count += grid[1, 2] == '#' ? 1 : 0;
            count += grid[3, 2] == '#' ? 1 : 0;
            count += grid[2, 3] == '#' ? 1 : 0;

            return count;
        }

        public override string ToString()
        {
            string inner = (InnerGrid == null) ? "null" : InnerGrid.Depth.ToString();
            string outer = (OuterGrid == null) ? "null" : OuterGrid.Depth.ToString();
            return string.Format("Depth: {0}, Inner: {1}, Outer: {2}", Depth, inner, outer);
        }

        public int Depth { get; private set; }
        public RecursiveGrid InnerGrid;
        public RecursiveGrid OuterGrid;
    }
}
