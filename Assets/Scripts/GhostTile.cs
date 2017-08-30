using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class GhostTile : MonoBehaviour
    {
        public Tile tile;
        public static GhostTile main;
        public float transparency;
        int uiWidth = 1000;
        int scale;
		
        // Use this for initialization
        void Start()
        {
            main = this;
            tile = GetComponent<Tile>();
            tile.transparency = transparency;
            tile.AdjustTransparency();
            scale = 3;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(tile.RotateClockwise());
            }

            Vector3 v3 = Input.mousePosition;
            if (v3.x > uiWidth) { v3.x = uiWidth; }
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            transform.position = NearestIntersection(v3);

            if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < uiWidth)
            {
                Tile t = Board.Main.AddTileToBoard(tile, transform.position / scale);
				if(t!=null){
					t=GetComponent<Tile>();
					t.RandomizeColors();
					t.AdjustColors();
					t.AdjustTransparency();
					InHandTile.selectedTile.Copy(t);
					InHandTile.selectedTile.AdjustColors();
                    Board.Main.SetNextPlayer();
				}
            }
        }
        Vector3 NearestIntersection(Vector3 v)
        {
            int x = Mathf.RoundToInt(v.x / scale) * scale;
            int y = Mathf.RoundToInt(v.y / scale) * scale;
            if (x < 0) { x = 0; }
            if (y < 0) { y = 0; }
            if (x > Board.Main.TotalWidth()) { x = (int)Board.Main.TotalWidth(); }
            if (y > Board.Main.TotalHeight()) { y = (int)Board.Main.TotalHeight(); }
            if (Board.Main.Tiles[x / scale, y / scale] != null) { return transform.position; }
            return new Vector3(x, y, 0);
        }
    }
}