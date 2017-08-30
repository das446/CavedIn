using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caged
{
    //TODO Seperate board from game controller
    public class Board : MonoBehaviour
    {
        public static Board Main;
        public GridOverlay grid;

        public Tile TilePrefab;

        public GameObject TilesParent;

        public int width, height;//There will be one more edge than space
        public Tile[,] Tiles;
        public List<Vector2> FilledPositions;

        Monster[,] Monsters;

        public Player[] players;
        public int PlayerIndex = 0;

        public float scale = 3f;
        public int MonstersAmnt = 10;

        public Monster MonsterPrefab;
        public Image Picture;
        public Text Description;
        public int WinAmnt;

        public Player Winner;
        public GameObject WinScreen;

        void Start()
        {
            Main = this;
            Tiles = new Tile[width + 1, height + 1];
            Monsters = new Monster[width, height];
            grid.stopX = width * scale;
            grid.stopY = height * scale;
            grid.Step = scale;
            grid.DrawGrid();
            Tile t = AddTileToBoard(new Vector3(width / 2, height / 2));
            t.Up = Color.Black;
            t.Right = Color.Red;
            t.Down = Color.Blue;
            t.Left = Color.Green;
            t.AdjustColors();
            CreateMonsters();
            Player.Current = players[0];
            Player.Current.Tiles[0].GetComponent<InHandTile>().Select();

            //GenerateSampleGame();
        }

        void GenerateSampleGame()
        {
            for (int x = 0; x <= width; x++)
            {
                for (int y = 0; y <= height; y++)
                {
                    Tile tile = AddTileToBoard(new Vector2(x, y));
                    tile.RandomizeColors();
                    if (y < height && Tiles[x, y + 1] != null) { tile.Up = Tiles[x, y + 1].Down; }
                    if (y > 0 && Tiles[x, y - 1] != null) { tile.Down = Tiles[x, y - 1].Up; }
                    if (x > 0 && Tiles[x - 1, y] != null) { tile.Left = Tiles[x - 1, y].Right; }
                    if (x < width && Tiles[x + 1, y] != null) { tile.Right = Tiles[x + 1, y].Left; }
                    tile.AdjustColors();
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ForkParticleEffect[] effects = GameObject.FindObjectsOfType<ForkParticleEffect>();
                foreach (var f in effects)
                {
                    f.EnableEffect(true);
                    var F = Instantiate(f);
                    F.EnableEffect(true);
                }
            }

        }

        public Tile AddTileToBoard(Vector3 pos)
        {

            Tile tile = Instantiate(TilePrefab, pos * scale, Quaternion.identity, TilesParent.transform);
            Tiles[(int)pos.x, (int)pos.y] = tile;
            tile.name = "Tile" + pos.x + "," + pos.y;
            FilledPositions.Add(pos);
            tile.x = (int)pos.x;
            tile.y = (int)pos.y;
            return tile;
        }
        public Tile AddTileToBoard(Tile t, Vector3 pos)
        {
            int x = (int)pos.x;
            int y = (int)pos.y;
            if (!CanPlayTile(t, x, y))
            {
                return null;
            }
            Tile tile = Instantiate(TilePrefab, pos * scale, Quaternion.identity, TilesParent.transform);
            Tiles[x, y] = tile;
            tile.name = "Tile" + pos.x + "," + pos.y;
            FilledPositions.Add(pos);
            tile.Copy(t);
            tile.x = x;
            tile.y = y;
            //CheckSurroundSpace(x, y);
            CheckCaptureMonster(x, y);
            return tile;
        }

        public float TotalWidth()
        {
            return width * scale;
        }
        public float TotalHeight()
        {
            return height * scale;
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

        //are all Neighboring tiles valid or nonexistent
        public bool CanPlayTile(Tile t, int x, int y)
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
        public Tile Above(int x, int y)
        {
            return Tiles[x, y + 1];
        }
        public Tile Below(int x, int y)
        {
            return Tiles[x, y - 1];
        }
        public Tile ToRight(int x, int y)
        {
            return Tiles[x + 1, y];
        }
        public Tile ToLeft(int x, int y)
        {
            return Tiles[x - 1, y];
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

            if (CheckSurrounded(x-1, y))
            {
                if (Monsters[x-1, y] != null)
                {
                    Monsters[x-1, y].GetCaptured();
                    Monsters[x-1, y] = null;
                }
                else
                {
                    Player.Current.points++;
                }
            }
            if (CheckSurrounded(x, y-1))
            {
                if (Monsters[x, y-1] != null)
                {
                    Monsters[x, y-1].GetCaptured();
                    Monsters[x, y-1] = null;
                }
                else
                {
                    Player.Current.points++;
                }
            }
            if (CheckSurrounded(x-1, y - 1))
            {
                if (Monsters[x-1, y - 1] != null)
                {
                    Monsters[x-1, y - 1].GetCaptured();
                    Monsters[x-1, y - 1] = null;
                }
                else
                {
                    Player.Current.points++;
                }
            }
            if(Player.Current.points<=WinAmnt){EndGame();}
        }

        public void CreateMonsters()
        {
            CreateMonster(width / 2, height / 2);
            CreateMonster(width / 2 - 1, height / 2);
            CreateMonster(width / 2, height / 2 - 1);
            CreateMonster(width / 2 - 1, height / 2 - 1);

            for (int i = 4; i < MonstersAmnt; i++)
            {
                int x = Random.Range(1, width - 1);
                int y = Random.Range(1, height - 1);
                if (Monsters[x, y] == null)
                {
                    CreateMonster(x, y);
                }
                else { i--; }
            }
        }

        public void CreateMonster(int x, int y)
        {
            Monster m = Instantiate(MonsterPrefab, new Vector3(x * scale + scale / 2, y * scale + scale / 2, 0), Quaternion.identity);
            Monsters[x, y] = m;
            m.Description = Description;
            m.Picture = Picture;
        }

        public bool CheckSurrounded(int x, int y)
        {
            bool Surrounded = Tiles[x, y] != null && Tiles[x + 1, y] != null && Tiles[x, y + 1] != null && Tiles[x + 1, y + 1] != null;
            return Surrounded;
        }

        public void SetNextPlayer()
        {
            PlayerIndex++;
            if (PlayerIndex >= players.Length)
            {
                PlayerIndex = 0;
            }
            Player.Current = players[PlayerIndex];
            Player.Current.Tiles[0].GetComponent<InHandTile>().Select();
        }

        public void EndGame()
        {
            WinScreen.SetActive(true);
            Text text = WinScreen.transform.GetChild(0).GetComponent<Text>();
            text.text = Player.Current.Name + " wins.\n" + players[0].Name + " " + players[0].points + " pts" +
            "                   " + players[1].Name + " " + players[1].points;
        }

    }
}