using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    public class Day08
    {
        private int[,,] _image;
        private string _imageRaw;

        private const int WIDTH = 25;
        private const int HEIGHT = 6;
        public Day08()
        {
            
            ReadImageData();
            
        }

        public void Part1()
        {
            int leastZeros = int.MaxValue;
            int leastZLayer = 0;

            for (int z = 0; z < Depth; z++)
            {
                int zeroCount = CountInLayer(z, 0);

                if (zeroCount < leastZeros)
                {
                    leastZeros = zeroCount;
                    leastZLayer = z;
                }
            }

            int rslt = CountInLayer(leastZLayer, 1) * CountInLayer(leastZLayer, 2);

            Console.WriteLine("Part 1: {0}", rslt);
        }

        public void Part2()
        {
            int[,] image = new int[WIDTH, HEIGHT];

            for (int x = 0; x < WIDTH; x++)
                for (int y = 0; y < HEIGHT; y++)
                {
                    // scan columns all the way down until a black or white is produced
                    int color = 2;
                    image[x, y] = 2;
                    for (int z = 0; z < Depth; z++)
                    {
                        color = _image[x, y, z];
                        if (color < 2)
                        {
                            image[x, y] = color;
                            break;
                        }
                    }

                }

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    char c = image[x, y] == 1 ? '*' : ' ';
                    Console.Write("{0}", c);
                }
                Console.WriteLine();
            }
        }

        private int CountInLayer(int z, int matchValue)
        {
            int matchCount = 0;
            for (int y = 0; y < HEIGHT; y++)
                for (int x = 0; x < WIDTH; x++)
                    matchCount += _image[x, y, z] == matchValue ? 1 : 0;

            return matchCount;
        }

        private void ReadImageData()
        {

            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\ImageData.txt";

            if (File.Exists(inputFile))
            {
                StreamReader file = new StreamReader(inputFile);
                _imageRaw = file.ReadLine();
                
                int layerSize = WIDTH * HEIGHT;
                if (_imageRaw.Length % layerSize == 0)
                {
                    Depth = _imageRaw.Length / layerSize;
                    _image = new int[WIDTH, HEIGHT, Depth];
                    int i = 0;
                    for (int z = 0; z < Depth; z++)
                    {
                        for (int y = 0; y < HEIGHT; y++)
                        {
                            for (int x = 0; x < WIDTH; x++)
                            {
                                _image[x, y, z] = _imageRaw[i++] - '0';
                            }
                        }
                    }
                }
            }
            
        }

        private int Depth { get; set; }

    }
}
