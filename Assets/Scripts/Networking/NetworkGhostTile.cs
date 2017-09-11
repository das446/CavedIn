using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caved
{
    public class NetworkGhostTile : GhostTile
    {
        public override void Place()
        {
            TileData t = ghostTile.Data;
            bool canPlaceTile = Board.Main.CanPlayTile(ghostTile, transform.position / scale);
            if (canPlaceTile)
            {
				int x=(int)transform.position.x;
				int y=(int)transform.position.y;
                NetworkPlayer np = (NetworkPlayer)Player.Current;
				string msg="PlaceTile|" + np.Name + "|" + i + "|" + x + "|" + y + "|" +
                        t.Up.name + "|" + t.Right.name + "|" + t.Down.name + "|" + t.Left.name;
				Debug.Log(msg);
                np.Send(msg);

				//PlaceTile|David|0|x|y|Blue|Red|Green|Blue

                //PlacedTile.Set(ghostTile.Data);
                //PlacedTile.AdjustDisplay();
               
                //InHandTile.selectedTile.Data.RandomizeColors();
                //InHandTile.selectedTile.Display.AdjustDisplay();
                //ghostTile.Set(InHandTile.selectedTile);
                //ghostTile.AdjustDisplay();
                //Board.Main.SetNextPlayer();

            }

        }

    }
}