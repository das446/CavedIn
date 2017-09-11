using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class NetworkGhostTile : GhostTile
    {
        new public void Place()
        {
            TileData t = ghostTile.Data;
            bool canPlaceTile = Board.Main.CanPlayTile(ghostTile, transform.position / scale);
            if (canPlaceTile)
            {
				int x=(int)transform.position.x;
				int y=(int)transform.position.y;
                NetworkPlayer np = (NetworkPlayer)Player.Current;
                np.Send("PlaceTile|" + np.name + "|" + i + "|" + x + "|" + y + "|" +
                        t.Up + "|" + t.Right + "|" + t.Down + "|" + t.Left);

				//PlacedTile|David|0|x|y|Blue|Red|Green|Blue

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