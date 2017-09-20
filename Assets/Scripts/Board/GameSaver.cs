using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Caved
{
    public static class GameSaver
    {
        /*
        DavidVsTim.game
        
        David: pts currentBool Up0 Right0 Down0 Left0 Up1 Right1 Down1 Left1 Up2 Right2 Down2 Left2
        Tim: pts currentBool Up0 Right0 Down0 Left0 Up1 Right1 Down1 Left1 Up2 Right2 Down2 Left2

        5 5 Up Right Down Left
        ...
        
        
        
         */
        public static void SaveGame(BoardData board, Player player1, Player player2)
        {
            string gameName = player1.Name + "Vs" + player2.Name+" "+DateTime.Now.ToString("en-US")+".game";
            string data = "";
            data = player1.Name;
            data += " " + player1.points;
            for (int i = 0; i < 3; i++)
            {
                data += " " + player1.Tiles[i].Data.Up.name + " " + player1.Tiles[i].Data.Right.name + " " + player1.Tiles[i].Data.Down.name + " " + player1.Tiles[i].Data.Left.name;
            }
            data += "\n";
            data = player2.Name;
            data += " " + player2.points;
            for (int i = 0; i < 3; i++)
            {
                data += " " + player2.Tiles[i].Data.Up.name + " " + player2.Tiles[i].Data.Right.name + " " + player2.Tiles[i].Data.Down.name + " " + player2.Tiles[i].Data.Left.name;
            }
            data+="\n";

            for(int x=0;x<board.width;x++){
                for (int y=0;y<board.height;x++){
                    if(board[x,y]!=null){
                        TileData tile=board[x,y];
                        data+= x +" "+y+" "+tile.Up.name+" "+tile.Right.name+" "+tile.Down.name+" "+tile.Left.name+"\n";
                    }
                }
            }
            string path = Application.dataPath;
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                path += "/../../Games/";
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path += "/../Games/";
            }
            Debug.Log(path);
            if(!Directory.Exists(path)){
                Directory.CreateDirectory(path);
            }
            File.Create(path+gameName);
            File.WriteAllText(path+gameName,data);
        }

    }
}