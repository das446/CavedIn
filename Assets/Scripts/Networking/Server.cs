using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using System.Linq;
using Caved;

public class Server : MonoBehaviour
{

    public int port = 6321;

    List<ServerClient> clients;
    List<ServerClient> disconnectList;

    TcpListener server;
    bool ServerStarted;
    string Player1Name;

    public BoardData board;

    string CurrentPlayer;
    public bool[,] Monsters; 
    bool MonstersMade;

    TileData nextTile;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        Player1Name = PlayerPrefs.GetString("Player1Name");
        CurrentPlayer=Player1Name;
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            ServerStarted = true;
        }
        catch (Exception e)
        {
            GameManager.debug("Socket Error: " + e.Message);
        }
    }

    void Update()
    {
        if (!ServerStarted)
        {
            return;
        }

        for (int i = 0; i < clients.Count; i++)
        {
            ServerClient c = clients[i];
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    if (!String.IsNullOrEmpty(data))
                    {
                        OnIncominngData(c, data);
                    }
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            Broadcast("Disconnect|"+disconnectList[i],clients);
            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }
    void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        string allUsers = "";
        foreach (ServerClient SC in clients)
        {
            allUsers += SC.ClientName + '|';

        }
        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);
        clients[0].ClientName = Player1Name;
        StartListening();

        Broadcast("SWHO|" + clients[0].ClientName, clients[clients.Count - 1]);
    }

    bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            else { return false; }
        }
        catch
        {
            return false;
        }
    }

    void Broadcast(string data, List<ServerClient> cl)
    {
        foreach (ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                GameManager.debug("Write Error : " + e.Message);
            }
        }
    }

    void Broadcast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> { c };
        Broadcast(data, sc);
    }

    void OnIncominngData(ServerClient c, string data)
    {

        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "CWHO":
                c.ClientName = aData[1];
                Broadcast("SCNN|" + c.ClientName, clients);
                break;

            case "GetTilesStart":
                AddFirstTile();
                MakeHand(aData[1]);
                SendHand(aData[1]);
                break;

            case "Current?":
                Broadcast("Current|"+CurrentPlayer,clients);
                break;
            case "MakeMonsters":
                MakeMonsters();
                SendMonsters();
                break;
            case "Select":
                Broadcast("Selected|"+aData[1]+"|"+aData[2],ClientsExcept(aData[1]));
                break;
            case "SetGhost":
                Broadcast("SetGhost|"+aData[2]+"|"+aData[3],ClientsExcept(aData[1]));
                break;
            case "GhostRot":
                Broadcast("GhostRot",ClientsExcept(aData[1]));
                break;
            case "PlaceTile":
                UpdateBoard(data);
                break;

            default:
                break;
        }
    }

    void MakeHand(string Name)
    {
        MakePlayerHand(board.Player1Hand);
        MakePlayerHand(board.Player2Hand);

    }
    void SendHand(string Name)
    {
        string msg = "SetHandTiles|" + clients[0].ClientName;
        for (int i = 0; i < 3; i++)
        {
            TileData t = board.Player1Hand[i];

            msg = msg +"|"+ t.Up.name + "|" + t.Right.name + "|" + t.Down.name + "|" + t.Left.name;
        }
        Broadcast(msg,clients);
        msg="SetHandTiles|" + clients[1].ClientName;
        for (int i = 0; i < 3; i++)
        {
            TileData t = board.Player2Hand[i];
            msg = msg + "|" + t.Up.name + "|" + t.Right.name + "|" + t.Down.name + "|" + t.Left.name;
        }
        Broadcast(msg, clients);

    }

    void MakePlayerHand(TileData[] hand)
    {
        if (hand[2] != null) { return; }
        for (int i = 0; i < 3; i++)
        {
            TileData t = new TileData();
            t.RandomizeColors();
            hand[i] = t;
        }
    }

    void MakeMonsters(){
        if(MonstersMade){return;}
        int width=board.width;
        int height=board.height;
        Monsters=new bool[width,height];
        Monsters[width / 2, height / 2]=true;
        Monsters[width / 2 - 1, height / 2]=true;
        Monsters[width / 2, height / 2 - 1]=true;
        Monsters[width / 2 - 1, height / 2 - 1]=true;

        for (int i = 4; i <Board.Main.MonstersAmnt; i++)
        {
            int x = UnityEngine.Random.Range(1, width - 1);
            int y = UnityEngine.Random.Range(1, height - 1);
            if (Monsters[x, y]==false)
            {
               Monsters[x,y]=true;
            }
            else { i--; }
        }
        MonstersMade=true;
    }

    void SendMonsters(){
        string msg="MakeMonsters";
        for(int x=0;x<board.width;x++){
            for(int y=0;y<board.height;y++){
                if(Monsters[x,y]){
                    msg=msg+"|"+x+"|"+y;
                }
            }
        }
        Broadcast(msg,clients);
    }

    //PlaceTile|David|0|x|y|Blue|Red|Green|Blue
    void UpdateBoard(string data){

        TileData td=new TileData();
        td.RandomizeColors();
        //PlaceTile|David|0|x|y|Blue|Red|Green|Blue|NewUp|NewRight|NewDown|NewLeft
        data=data+"|"+td.Up.name+"|"+td.Right.name+"|"+td.Down.name+"|"+td.Left.name;
        Broadcast(data,clients);


    }

    void AddFirstTile(){
        TileData tileData = new TileData("Black", "Red", "Blue", "Green");
        board.AddTileToBoard(tileData, new Vector3(board.width / 2, board.height / 2));
    }

    ServerClient byName(string Name){
        for(int i=0;i<clients.Count;i++){
            if(clients[i].ClientName==Name){
                return clients[i];
            }
        }
        return null;
    }

    List<ServerClient> ClientsExcept(string c)
    {
        ServerClient sc=byName(c);
        return clients.Where(x => x != sc).ToList();
    }

    List<ServerClient> ClientsExcept(ServerClient c){
        return clients.Where(x=>x!=c).ToList();
    }
}



public class ServerClient
{
    public string ClientName;
    public TcpClient tcp;

    public ServerClient(TcpClient Tcp, string client)
    {
        tcp = Tcp;
        ClientName = client;
    }

    public ServerClient(TcpClient Tcp)
    {
        tcp = Tcp;
    }

}