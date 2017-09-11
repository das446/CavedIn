using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    [System.Serializable]
    public class TileData
    {
        public Color Right, Down, Left, Up;
        public int x, y;

        public TileData(Color up,Color right,Color down,Color left){
            Set(up,right,down,left);
        }

        public TileData(Color up, Color right, Color down, Color left,int x,int y)
        {
            Set(up, right, down, left,x,y);
        }
        public TileData(string up, string right, string down, string left)
        {
            Set(Color.FromName(up), Color.FromName(right), Color.FromName(down),Color.FromName(left));
        }
        public TileData(TileData t){
            Set(t);
        }
        public TileData(TileData t,int x,int y)
        {
            Set(t);
        }
        public TileData()
        {
        }

        public string toString(){
            return "Up=" + Up.name + ", Right=" + Right.name + "\nDown=" + Down.name + ", Left=" + Left.name;
        }

        public void TestSet(){
            Debug.Log("Test set");
            Right = Color.Random();
            Down = Color.Random();
            Left = Color.Random();
            Up = Color.Random();
        }

        public void RandomizeColors()
        {
            Right = Color.Random();
            Down = Color.Random();
            Left = Color.Random();
            Up = Color.Random();
        }

        public void Rotate()
        {
            Color t = Up;
            Up = Left;
            Left = Down;
            Down = Right;
            Right = t;
        }

        public void SetAll(Color c)
        {
            Up = c;
            Down = c;
            Right = c;
            Left = c;
        }
        public void Set(TileData t)
        {
            if (t == null) { Debug.Log("Tried to copy null tile"); return; }
            Right = t.Right;
            Left = t.Left;
            Up = t.Up;
            Down = t.Down;
        }

        public void Set(TileData t,int x,int y)
        {
            if (t == null) { Debug.Log("Tried to copy null tile"); return; }
            Right = t.Right;
            Left = t.Left;
            Up = t.Up;
            Down = t.Down;
            this.x=x;
            this.y=y;
            Debug.Log("Tile x="+this.x+" y="+this.y);
        }

        public void Set(Color up, Color right, Color down, Color left)
        {
            Up = up;
            Down = down;
            Right = right;
            Left = left;
        }
        public void Set(Color up, Color right, Color down, Color left,int x,int y)
        {
            Up = up;
            Down = down;
            Right = right;
            Left = left;
            this.x=x;
            this.y=y;
        }

        public TileData Above()
        {
            return Board.Main.Above(x, y);
        }
        public TileData Below()
        {
            return Board.Main.Below(x, y);
        }
        public TileData ToRight()
        {
            return Board.Main.ToRight(x, y);
        }
        public TileData ToLeft()
        {
            return Board.Main.ToLeft(x, y);
        }

        public TileData Above(Board board)
        {
            return board.Above(x, y);
        }
        public TileData Below(Board board)
        {
            return board.Below(x, y);
        }
        public TileData ToRight(Board board)
        {
            return board.ToRight(x, y);
        }
        public TileData ToLeft(Board board)
        {
            return board.ToLeft(x, y);
        }


    }
}