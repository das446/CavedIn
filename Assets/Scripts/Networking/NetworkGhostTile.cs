using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class NetworkGhostTile : GhostTile
    {
		new public void Place()
        {
            Tile PlacedTile = Board.Main.AddTileToBoard(ghostTile, transform.position / scale);
            if (PlacedTile != null)
            {
                PlacedTile.Set(ghostTile.Data);
                PlacedTile.AdjustDisplay();
                InHandTile.selectedTile.Data.RandomizeColors();
                InHandTile.selectedTile.Display.AdjustDisplay();
                ghostTile.Set(InHandTile.selectedTile);
                ghostTile.AdjustDisplay();
                Board.Main.SetNextPlayer();
            }

        }

    }
}