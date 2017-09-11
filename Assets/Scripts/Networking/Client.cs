using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;
using System.Linq;


namespace Caved
{
    public class Client : MonoBehaviour
    {

        bool socketReady;
        public string clientName;
        TcpClient socket;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        List<GameClient> Players = new List<GameClient>();
        public Caved.NetworkPlayer player;
        public bool Host;
        public Client Opponent;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (socketReady)
            {
                if (stream.DataAvailable)
                {
                    string data = reader.ReadLine();
                    if (data != null)
                    {
                        OnIncomingData(data);
                    }
                }
            }
        }

        public bool ConnectToServer(string host, int port)
        {
            if (socketReady) { return false; }

            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);

                socketReady = true;

            }
            catch (Exception e)
            {
                GameManager.debug("Socket Error : " + e.Message);
            }

            return socketReady;
        }

        void OnIncomingData(string data)
        {
            string[] aData = data.Split('|');

            switch (aData[0])
            {
                case "SWHO":
                    for (int i = 1; i < aData.Length; i++)
                    {
                        UserConnected(aData[i], false);
                    }
                    Send("CWHO|" + clientName);
                    break;

                case "SCNN":
                    UserConnected(aData[1], true);
                    break;

                case "SetHandTiles":
                    Debug.Log(clientName + " recieved " + data);
                    SetHandTiles(aData);
                    break;

                case "MakeMonsters":
                    ((NetworkBoard)(Board.Main)).CreateMonstersFromServer(aData);
                    break;

                case "Current":
                    player.setCurrentPlayer(aData[1]);
                    player.CurrentPlayerName = aData[1];
                    
                    break;

                case "Selected":
                    SelectFromServer(aData);
                    break;

                case "SetGhost":
                    int x = int.Parse(aData[1]);
                    int y = int.Parse(aData[2]);
                    GhostTile.main.transform.position = new Vector2(x, y);
                    break;

                case "GhostRot":
                    StartCoroutine(GhostTile.main.ghostTile.Rotate());
                    break;

                case "PlaceTile":
                    PlaceTileFromServer(aData);
                    break;

                default:
                    break;
            }
        }
        public void Send(string data)
        {
            if (!socketReady)
            {
                return;
            }
            writer.WriteLine(data);
            writer.Flush();
        }

        public void CloseSocket()
        {
            if (!socketReady)
            {
                return;
            }

            writer.Close();
            reader.Close();
            socket.Close();
            socketReady = false;
        }

        void OnIncominngData(ServerClient c, string data)
        {
            OnIncomingData(data);
        }

        public IEnumerator RequestUntil(Func<bool> condition, string msg)
        {
            while (!condition())
            {
                Send(msg);
                yield return new WaitForSeconds(1);
            }
        }

        public IEnumerator RequestWhile(Func<bool> condition, string msg)
        {
            while (condition())
            {
                Send(msg);
                yield return new WaitForSeconds(1);
            }
        }

        void UserConnected(string Name, bool Host)
        {
            if (Name == "" || Players.Any(x => x.name == Name)) { return; }
            GameClient c = new GameClient();
            c.name = Name;
            Players.Add(c);
            if (!GameManager.Instance.Clients.Any(x => x.clientName == Name))
            {
                Client C = Instantiate(GameManager.Instance.clientPrefab).GetComponent<Client>();
                C.clientName = Name;
                C.name = Name;
                GameManager.Instance.Clients.Add(C);
            }
            if (Players.Count == 2)
            {
                GameManager.Instance.StartGame();
            }
        }

        void OnApplicationQuit()
        {
            CloseSocket();
        }

        void OnDisable()
        {
            CloseSocket();
        }
        void SetHandTiles(string[] aData)
        {
            player.client = this;
            if (aData[1] == clientName)
            {
                player.Tiles[0].Set(aData[2], aData[3], aData[4], aData[5]);
                player.Tiles[1].Set(aData[6], aData[7], aData[8], aData[9]);
                player.Tiles[2].Set(aData[10], aData[11], aData[12], aData[13]);
            }
            else
            {
                Opponent.player.Tiles[0].Set(aData[2], aData[3], aData[4], aData[5]);
                Opponent.player.Tiles[1].Set(aData[6], aData[7], aData[8], aData[9]);
                Opponent.player.Tiles[2].Set(aData[10], aData[11], aData[12], aData[13]);
            }
        }

        void SelectFromServer(string[] data)
        {
            Caved.NetworkPlayer networkPlayer = player.picked(data[1]);
            if (networkPlayer != player)
            {
                networkPlayer.Tiles[int.Parse(data[2])].GetComponent<InHandTile>().Select(false);
            }
        }

        //PlaceTile|David|0|x|y|Blue|Red|Green|Blue|NewUp|NewRight|NewDown|NewLeft
        void PlaceTileFromServer(string[] data)
        {
            NetworkPlayer player = (NetworkPlayer)this.player.picked(data[1]);
            TileData tileData=new TileData(data[5],data[6],data[7],data[8]);
            Vector2 pos=new Vector2(int.Parse(data[3]),int.Parse(data[4]));

            int i=int.Parse(data[2]);
            Tile InHandTile=player.Tiles[i];

            Tile PlacedTile=Board.Main.AddTileToBoard(tileData,pos/Board.Main.scale);
            if(PlacedTile==null){return;}

            PlacedTile.Set(tileData);
            PlacedTile.AdjustDisplay();

            TileData newTile=new TileData(data[9], data[10], data[11], data[12]);

            InHandTile.Data.Set(newTile);
            InHandTile.Display.AdjustDisplay();
            GhostTile.main.ghostTile.Set(newTile);
            GhostTile.main.ghostTile.AdjustDisplay();
            Board.Main.SetNextPlayer();
            Debug.Log("CurrentPlayer="+Player.Current.Name);
            GhostTile.main.transform.position=Vector3.zero;
        }

    }

    public class GameClient
    {
        public string name;
        public bool isHost;
    }
}