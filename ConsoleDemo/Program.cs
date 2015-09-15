﻿using MazeGeneration;
using MazeGeneration.Generation;
using System;
using System.Threading;

namespace ConsoleDemo
{
    class Program
    {
        static Maze maze;
        static int mazeWidth = 25, mazeHeight = 10;
        static int offset = 3;
        static char[,] display;
        static Random r;
        static Array directions;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            r = new Random((int)DateTime.Now.Ticks);
            maze = new Maze(mazeWidth, mazeHeight);
            directions = Enum.GetValues(typeof(NodeWall));
            display = new char[mazeWidth * offset, mazeHeight * offset];

            Console.WriteLine("Choose a type of maze to generate:");
            Console.WriteLine();
            Console.WriteLine("1: Recursive Backtracker");
            Console.WriteLine("2: Randomised Prim's Algorithm");
            var mazeOption = Console.ReadKey().Key;

            IMazeGenerator generator;

            switch (mazeOption)
            {
                case ConsoleKey.D2:
                    generator = new PrimsAlgorithm(r);
                    break;
                case ConsoleKey.D1:
                default:
                    generator = new RecursiveBacktracker(r);
                    break;
            }
            Console.Clear();

            while (true)
            {
                maze.InitialiseNodes();
                generator.Generate(maze);
                for (int h = 0; h < mazeHeight; h++)
                {
                    for (int w = 0; w < mazeWidth; w++)
                    {
                        WriteNodeChar(w, h, maze[(uint)w, (uint)h].Walls);
                    }
                }

                Console.Clear();
                DrawDisplay();
                Console.WriteLine();
                Console.WriteLine("Press any key to regenerate, or Escape to exit....");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        private static void DrawDisplay()
        {
            for (int y = 0; y < mazeHeight * offset; y++)
            {
                for (int x = 0; x < mazeWidth * offset; x++)
                {
                    Console.Write(display[x, y]);
                }
                Console.WriteLine();
            }
        }

        private static void WriteNodeChar(int x, int y, ushort node)
        {
            int xOffset = x * offset;
            int yOffset = y * offset;
            for (int h = yOffset; h < yOffset + offset; h++)
            {
                for (int w = xOffset; w < xOffset + offset; w++)
                {
                    // Ugly drawing code
                    if (w == xOffset + 1 && h == yOffset + 1)
                    {
                        display[w, h] = ' ';
                    }
                    else if (w == xOffset + 1 && h == yOffset)
                    {
                        display[w, h] = (node & (short)NodeWall.North) == (short)NodeWall.North ? 'X' : ' ';
                    }
                    else if (w == xOffset && h == yOffset + 1)
                    {
                        display[w, h] = (node & (short)NodeWall.West) == (short)NodeWall.West ? 'X' : ' ';
                    }
                    else if (w == xOffset + 2 && h == yOffset + 1)
                    {
                        display[w, h] = (node & (short)NodeWall.East) == (short)NodeWall.East ? 'X' : ' ';
                    }
                    else if (w == xOffset + 1 && h == yOffset + 2)
                    {
                        display[w, h] = (node & (short)NodeWall.South) == (short)NodeWall.South ? 'X' : ' ';
                    }
                    else
                    {
                        display[w, h] = 'X';
                    }

                }
            }
        }
    }
}
