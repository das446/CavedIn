using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class BoardDisplay : MonoBehaviour
    {
        public GridOverlay grid;
        public GameObject TilesParent;
        public GameObject MonsterParent;
        public Tile TilePrefab;
        public Monster MonsterPrefab;
        public float scale = 3;

        public int width, height;

        public Tile AddTileToBoard(Vector3 pos)
        {
            Tile tile = Instantiate(TilePrefab, pos * scale, Quaternion.identity, TilesParent.transform);
            tile.name = "Tile" + pos.x + "," + pos.y;
            return tile;
        }

        public Tile AddTileToBoard(TileData t, Vector3 pos)
        {
            Tile tile = Instantiate(TilePrefab, pos * scale, Quaternion.identity, TilesParent.transform);
            tile.Set(tile);
            tile.name = "Tile" + pos.x + "," + pos.y;
            return tile;
        }

        public void Initialize(Board board)
        {
            grid.stopX = width * scale;
            grid.stopY = height * scale;
            grid.Step = scale;
            grid.DrawGrid();
        }

        public Monster CreateMonster(int x, int y)
        {
            return Instantiate(MonsterPrefab, new Vector3(x * scale + scale / 2, y * scale + scale / 2, 0), Quaternion.identity);
        }

    }
}
