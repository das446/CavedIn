using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Caved
{
    [System.Serializable]
    public class BoardData
    {
        public int width, height = 10;
        public TileData[,] Tiles;
        public List<Vector2> FilledPositions=new List<Vector2>();
        public Monster[,] Monsters;

        public TileData[] Player1Hand;
        public TileData[] Player2Hand;


        public BoardData(int w, int h)
        {
            width = w;
            height = h;
            Tiles=new TileData[w,h];
            Player1Hand=new TileData[3];
            Player2Hand=new TileData[3];
        }
        public BoardData(int w, int h, TileData[,] t, List<Vector2> filled, Monster[,] m)
        {
            width = w;
            height = h;
            Player1Hand = new TileData[3];
            Player2Hand = new TileData[3];
            Tiles = (TileData[,])t.Clone();
            FilledPositions = filled;
            Monsters = (Monster[,])m.Clone();
        }
        public void Initialize()
        {
            Tiles = new TileData[width + 1, height + 1];
            Monsters = new Monster[width, height];
        }
        public TileData AddTileToBoard(TileData t, Vector3 pos)
        {
            int x = (int)pos.x;
            int y = (int)pos.y;
            if (!CanPlayTile(t, x, y) && FilledPositions.Count>0)
            {
                return null;
            }
            TileData tile = new TileData(t,x,y);
            tile.x=x;
            tile.y=y;
            try{Tiles[x, y] = tile;}
            catch (IndexOutOfRangeException e){
                Debug.Log("x="+x+", y="+y);
            }
            FilledPositions.Add(pos);
            //CheckSurroundSpace(x, y);
            return tile;
        }

        public bool CanPlayTile(Tile t, int x, int y)
        {
            if (Neighbors(x, y) == 0||x<0||y<0||x>=width||y>=height) { return false; }
            if(Tiles[x,y]!=null){return false;}
            bool Avalid = true;
            if (Above(x, y) != null)
            {
                Avalid = Above(x, y).Down == t.Data.Up;
            }

            bool Bvalid = true;
            if (Below(x, y) != null)
            {
                Bvalid = Below(x, y).Up == t.Data.Down;
            }

            bool Rvalid = true;
            if (ToRight(x, y) != null)
            {
                Rvalid = ToRight(x, y).Left == t.Data.Right;
            }

            bool Lvalid = true;
            if (ToLeft(x, y) != null)
            {
                Lvalid = ToLeft(x, y).Right == t.Data.Left;
            }
            return Avalid && Bvalid && Rvalid && Lvalid;
        }

        public int Neighbors(int x, int y)
        {
            int i = 0;
            if (Above(x, y) != null) { i++; }
            if (Below(x, y) != null) { i++; }
            if (ToRight(x, y) != null) { i++; }
            if (ToLeft(x, y) != null) { i++; }
            return i;
        }

        public bool CheckSurrounded(int x, int y)
        {
            if(x<0||x>=width||y<0||y>=height){return false;}
            bool Surrounded = Tiles[x, y] != null && Tiles[x + 1, y] != null && Tiles[x, y + 1] != null && Tiles[x + 1, y + 1] != null;
            return Surrounded;
        }

        public bool CheckSurrounded(TileData t)
        {
            return CheckSurrounded(t.x, t.y);
        }
        public void CheckCaptureMonster(int x, int y, bool DoCapture = true)
        {
            if (CheckSurrounded(x, y))
            {
                if (Monsters[x, y] != null)
                {
                    Monsters[x, y].GetCaptured();
                    Monsters[x, y] = null;
                }
                else
                {
                    Player.Current.points++;
                }
            }

            if (CheckSurrounded(x - 1, y))
            {
                if (Monsters[x - 1, y] != null)
                {
                    Monsters[x - 1, y].GetCaptured();
                    Monsters[x - 1, y] = null;
                }
                else
                {
                    Player.Current.points++;
                }
            }
            if (CheckSurrounded(x, y - 1))
            {
                if (Monsters[x, y - 1] != null)
                {
                    Monsters[x, y - 1].GetCaptured();
                    Monsters[x, y - 1] = null;
                }
                else
                {
                    Player.Current.points++;
                }
            }
            if (CheckSurrounded(x - 1, y - 1))
            {
                if (Monsters[x - 1, y - 1] != null)
                {
                    Monsters[x - 1, y - 1].GetCaptured();
                    Monsters[x - 1, y - 1] = null;
                }
                else
                {
                    Player.Current.points++;
                }
            }
        }

        public bool CanPlayTile(TileData t, int x, int y)
        {
            if (Neighbors(x, y) == 0) { return false; }

            bool Avalid = true;
            if (Above(x, y) != null)
            {
                Avalid = Above(x, y).Down == t.Up;
            }

            bool Bvalid = true;
            if (Below(x, y) != null)
            {
                Bvalid = Below(x, y).Up == t.Down;
            }

            bool Rvalid = true;
            if (ToRight(x, y) != null)
            {
                Rvalid = ToRight(x, y).Left == t.Right;
            }

            bool Lvalid = true;
            if (ToLeft(x, y) != null)
            {
                Lvalid = ToLeft(x, y).Right == t.Left;
            }
            return Avalid && Bvalid && Rvalid && Lvalid;
        }
        public TileData Above(int x, int y)
        {
            if (y >= height-1||x<0||x>=width) { return null; }
            return Tiles[x, y + 1];
        }
        public TileData Below(int x, int y)
        {
            if (y <= 0|| x < 0 || x >= width) { return null; }
            return Tiles[x, y - 1];
        }
        public TileData ToRight(int x, int y)
        {
            if (x >= width-1||y<0||y>=height) { return null; }
            return Tiles[x + 1, y];
        }
        public TileData ToLeft(int x, int y)
        {
            if(x<=0|| y < 0 || y >= height){return null;}
            return Tiles[x - 1, y];
        }

        public void CreateMonster(Monster m,int x,int y)
        {
            Monsters[x, y] = m;
        }

        public TileData this[int x,int y]{
            get{
                if (x >= width || x < 0 || y < 0 || y >= height) { return null; }
                return Tiles[x,y];}
        }

        public TileData this[float x, float y]
        {
            get { return Tiles[(int)x, (int)y]; }
        }

        public TileData this[Vector2 pos]
        {
            get {int x=(int)pos.x / 3;
                 int y= (int)pos.y / 3;
                 if(x>=width||x<0||y<0||y>=height){return null;}
                return Tiles[x,y]; }
        }

    }
}
