using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Caved
{
    //TODO Seperate board from game controller
    public class Board : MonoBehaviour
    {
        public BoardData Data;
        public BoardDisplay Display;

        public static Board Main;

        public Player[] players;
        public int PlayerIndex = 0;
        public int MonstersAmnt = 10;

        public Monster MonsterPrefab;
        public Image Picture;
        public Text Description;
        public int WinAmnt;

        [SerializeField] protected int width;
        [SerializeField] protected int height;

        public Player Winner;
        public GameObject WinScreen;
        public bool MonstersSet;

        public GameObject PlaceTileParticles;
        public GameSettings settings;

        void Start()
        {
            Main = this;
            Data = new BoardData(width, height);
            Display = GetComponent<BoardDisplay>();
            Data.Initialize();
            Display.Initialize(this);
            players[0].Name=PlayerPrefs.GetString("Player1Name");
            players[0].setName(players[0].Name);
            players[1].Name = PlayerPrefs.GetString("Player2Name");
            players[1].setName(players[1].Name);
            Debug.Log(PlayerPrefs.GetString(MainMenu.EnemyType));
            if(PlayerPrefs.GetString(MainMenu.EnemyType)=="Com"){
                players[1].GetComponent<AIPlayer>().enabled=true;
                players[1].Human=false;
            }
            PlayerIndex = 0;
            Player.Current = players[0];
            Player.Current.Tiles[0].GetComponent<InHandTile>().Select();
            TileData tileData = new TileData(Color.Black, Color.Red, Color.Blue, Color.Green);
            Tile t = AddTileToBoard(tileData, new Vector3(width / 2, height / 2));
            t.Data=tileData;
            t.AdjustDisplay();
            CreateMonsters();

        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape)){
                Application.Quit();
            }
        }
        public Tile AddTileToBoard(TileData t, Vector3 pos)
        {
            TileData tile = Data.AddTileToBoard(t, pos);
            if (tile != null)
            {
                Tile T = Display.AddTileToBoard(tile, pos);
                if (Data.FilledPositions.Count <= 1) { return T; }
                bool captured=Data.CheckCaptureMonster(tile.x, tile.y);
                AddTileParticles(T);
                if (Player.Current.points >= WinAmnt) { EndGame(); }
                return T;
            }
            else
            {
                return null;
            }
        }

        void AddTileParticles(Tile T){
            GameObject particles = Instantiate(PlaceTileParticles, T.transform.position, Quaternion.identity);
            /*
            None of this works for now because it just changes them to black and I have no idea why,
            will try to fix later but it's not that big an issue for now

            ParticleSystem particleSystem=particles.GetComponent<ParticleSystem>();
            Gradient gradient= new Gradient();
            GradientColorKey[] gck=new GradientColorKey[4];
            GradientAlphaKey[] gak;
            gak = new GradientAlphaKey[4];
            gck[0].color=T.Data.Up.rgb;
            gck[0].time = 0;
            gck[1].color = T.Data.Right.rgb;
            gck[1].time = 0.3f;
            gck[2].color = T.Data.Down.rgb;
            gck[2].time = 0.6f;
            gck[3].color = T.Data.Left.rgb;
            gck[3].time = 1;

            gak[0].alpha = 1;
            gak[0].time = 0;
            gak[1].alpha = 1;
            gak[1].time = 0.1f;
            gak[2].alpha = 1;
            gak[2].time = 0.2f;
            gak[3].alpha = 1;
            gak[3].time = 1;

            gradient.SetKeys(gck,gak);
            ParticleSystem.ColorOverLifetimeModule col=particleSystem.colorOverLifetime;
            col.color=gradient;
            */
            Destroy(particles, 3);
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
            return Data.CanPlayTile(t, x, y);
        }
        public bool CanPlayTile(Tile t, Vector2 p)
        {
            return Data.CanPlayTile(t, (int)p.x, (int)p.y);
        }
        public TileData Above(int x, int y)
        {
            return Data.Above(x, y);
        }
        public TileData Below(int x, int y)
        {
            return Data.Below(x, y);
        }
        public TileData ToRight(int x, int y)
        {
            return Data.ToRight(x, y);
        }
        public TileData ToLeft(int x, int y)
        {
            return Data.ToLeft(x, y); ;
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
                if (Data.Monsters[x, y] == null)
                {
                    CreateMonster(x, y);
                }
                else { i--; }
            }
        }

        public void CreateMonster(int x, int y)
        {
            Monster m = Display.CreateMonster(x, y);
            Data.CreateMonster(m, x, y);
            m.Description = Description;
            m.Picture = Picture;
        }

        public bool CheckSurrounded(int x, int y)
        {
            return Data.CheckSurrounded(x, y);
        }

        public bool CanPlayTile(TileData t, int x, int y)
        {
            return Data.CanPlayTile(t, x, y);
        }

        public void SetNextPlayer()
        {
            Player.Current.GetTime();
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
            foreach(Player p in players){
                p.gameObject.SetActive(false);
            }
            Text text = WinScreen.transform.GetChild(0).GetComponent<Text>();
            text.text = Player.Current.Name + " wins.\n" + players[0].Name + " " + players[0].points + " pts" +
            "                   " + players[1].Name + " " + players[1].points;
        }


        public virtual void PlayAgain()
        {
            SceneManager.LoadScene(0);
        }

        public static implicit operator BoardData(Board b)
        {
            return b.Data;
        }

        public static implicit operator BoardDisplay(Board b)
        {
            return b.Display;
        }

        public float TotalWidth()
        {
            return width * Display.scale;
        }

        public float TotalHeight()
        {
            return height * Display.scale;
        }

        public float scale{
            get {return Display.scale;}
        }

    }
}