using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caved
{
    public class Player : MonoBehaviour
    {

        public Tile[] Tiles;
        public Tile prefabTile;
        public int points;
        public string Name;
        public int HandSize;

        public Text PointsText;
        public float inHandY;
        public bool Human;

        public float TimeLeft = 60;
        public float MaxTime = 120;

        public float MaxRollover = 15;
        public float AddedTime = 15;
		public Text TimeText;

        public static Player Current;
        protected float mid = 20f;

        void Start()
        {
            Tiles = new Tile[HandSize];
            if(GameManager.Instance==null){CreateStartHand();
                Tiles[0].GetComponent<InHandTile>().Select();
            }
        }

        public void setName(string text)
        {
            Name=text;
            GetComponent<Text>().text = text;
        }

        void Update()
        {
            if (Current == null)
            {
                Current = this;
            }
            PointsText.text = Name + ":" + points;
            if (Player.Current == this)
            {
                TimeLeft -= Time.deltaTime;
				TimeText.text=toSeconds(TimeLeft);

                if (TimeLeft <= 0)
                {
					//Do a random valid move
                }
            }
        }

        public void GetTime()
        {
            float rollover = TimeLeft > MaxRollover ? MaxRollover : TimeLeft;
            float toAdd = rollover + AddedTime;
            TimeLeft += toAdd;
			TimeLeft=TimeLeft>MaxTime?MaxTime:TimeLeft;
        }

		public string toSeconds(float n){
			int min=(int)(n/60);
			int sec=(int)n%60;
			string s2=sec<10?"0"+sec:sec+"";
			string time=min+":"+s2;
			return time;
		}

        public void CreateStartHand(){
            for (int i = 0; i < HandSize; i++)
            {
                Tiles[i] = Instantiate(prefabTile, new Vector2(mid + 5 * i, inHandY), Quaternion.identity);
                Tiles[i].RandomizeColors();
                Tiles[i].Display.AdjustDisplay();
                Tiles[i].GetComponent<InHandTile>().enabled = true;
                Tiles[i].GetComponent<InHandTile>().Controller = this;
            }
        }
    }
}