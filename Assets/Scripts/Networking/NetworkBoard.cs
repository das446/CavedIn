using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class NetworkBoard : Board
    {


        void Start()
        {
			ClientInit();		

        }

		public void ClientInit(){
			Main = this;
            Data = new BoardData(width, height);
            Display = GetComponent<BoardDisplay>();
            GameManager.Instance.InitializeGame();
            Data.Initialize();
            Display.Initialize(this);
            players[0].Name = PlayerPrefs.GetString("Player1Name");
            players[0].setName(players[0].Name);
            players[1].setName(players[1].Name);
            PlayerIndex = 0;
            TileData tileData = new TileData(Color.Black, Color.Red, Color.Blue, Color.Green);
			Tile t = AddTileToBoard(tileData, new Vector3(width / 2, height / 2));
            t.Data = tileData;
            t.AdjustDisplay();
            CreateMonsters();
			GameManager.Instance.InitializeGame();
			//Player.Current.Tiles[0].GetComponent<InHandTile>().Select();
		}
    }
}