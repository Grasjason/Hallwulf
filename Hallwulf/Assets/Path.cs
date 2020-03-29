using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Direction
{
    Down = 1,
    Left = 2,
    Right = 4
}

public class Path : List<Direction> {

    public Direction GetNewDirection(Direction allowed, System.Random rnd)
    {
        Direction newd;
        int maxd = Enum.GetValues(typeof(Direction)).Length;
        int[] vals = (int[])Enum.GetValues(typeof(Direction));
        do
        {
            var t = rnd.Next(0, maxd);
            newd = (Direction)vals[t];
        }
        while ((newd & allowed) == 0);
        return newd;
    }

    public Path GenerateRandomPath(int startx, int starty, int endx, int endy, double prob, int minXLimit, int maxXLimit)
    {
        Path newpath = new Path();
        System.Random rnd = new System.Random();

        int curx = startx; int cury = starty; Direction curd = Direction.Down;
        Direction newd = curd;

        while (!(curx == endx && cury == endy))
        {
            if (rnd.NextDouble() <= prob) // let's generate a turn
            {

                do
                {
                    if (cury == endy && curx < endx) newd = Direction.Right;
                    else if (cury == endy && curx > endx) newd = Direction.Left;
                    else if (curx <= minXLimit) newd = Direction.Right; //GetNewDirection(Direction.Right | Direction.Down, rnd);
                    else if (curx >= maxXLimit) newd = Direction.Left; //GetNewDirection(Direction.Left | Direction.Down, rnd);
                    else newd = GetNewDirection(Direction.Right | Direction.Down | Direction.Left, rnd);

                }
                while ((newd | curd) == (Direction.Left | Direction.Right)); // excluding going back

                newpath.Add(newd);
                curd = newd;
                switch (newd)
                {
                    case Direction.Left:
                        curx--;
                        break;
                    case Direction.Right:
                        curx++;
                        break;
                    case Direction.Down:
                        cury--;
                        break;
                }
            }
        }
        return newpath;
    }
}