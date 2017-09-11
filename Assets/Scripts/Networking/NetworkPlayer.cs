using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Caged
{
    public class NetworkPlayer : Player
    {

        public Client client;

        public bool local;
        bool alreadySetHand = false;

        public string CurrentPlayerName;

        // Use this for initialization
        void Start()
        {
            Tiles = new Tile[HandSize];
            setName(Name);
            CreateStartHand();
        }

       public void setCurrentPlayer(string Name)
        {
            NetworkPlayer player=picked(Name);
            Current=player;
            Current.Tiles[0].GetComponent<InHandTile>().Select();
            for(int i=0;i<Board.Main.players.Length;i++){
                if(Board.Main.players[i]==Current){
                    Board.Main.PlayerIndex=i;
                }
            }
        }

        public void Send(string msg){
            client.Send(msg);
        }

        public NetworkPlayer picked(string Name){
            if(Name==this.Name){
                return this;
            }
            else{
                return client.Opponent.player;
            }
        }


        new public void CreateStartHand()
        {

            for (int i = 0; i < HandSize; i++)
            {
                Tiles[i] = Instantiate(prefabTile, new Vector2(mid + 5 * i, inHandY), Quaternion.identity);
                InHandTile tile=Tiles[i].GetComponent<InHandTile>();
                tile.enabled = true;
                tile.Controller = this;
                tile.i=i;
            }
        }

    }
}