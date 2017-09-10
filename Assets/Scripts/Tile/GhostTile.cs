using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class GhostTile : MonoBehaviour
    {
        public Tile ghostTile;
        public static GhostTile main;
        public float transparency;
        int uiWidth = 1000;
        int scale;
        public bool TestingFollow=false;

        // Use this for initialization
        void Start()
        {
            main = this;
            ghostTile = GetComponent<Tile>();
            ghostTile.Display.transparency = transparency;
            ghostTile.Display.AdjustDisplay();
            scale = 3;
            StartCoroutine(SendPositionToServer());
        }

        // Update is called once per frame
        void Update()
        {
            if (ShouldRotate())
            {
                StartCoroutine(ghostTile.Rotate());
                if(GameManager.Instance!=null){
                    NetworkPlayer np=(NetworkPlayer)Player.Current;
                    np.Send("GhostRot|"+np.Name);
                    Debug.Log("Sent Rotate Ghost");
                }
            }

            if (Player.Current.Human)
            {
                Vector3 v3 = Input.mousePosition;
                if (v3.x > uiWidth) { v3.x = uiWidth; }
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                transform.position = NearestIntersection(v3);

                if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < uiWidth)
                {
                    Place();
                }
            }
            else if(TestingFollow){
                Vector3 p= Camera.main.transform.position;
                p.x=transform.position.x+7.5f;
                p.y=transform.position.y;
                Camera.main.transform.position=Vector3.MoveTowards(Camera.main.transform.position,p,Time.deltaTime*15);
            }
        }

        public void Place()
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

        public bool ShouldRotate()
        {
            return (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X)) && !ghostTile.Display.rotating;
        }
        Vector3 NearestIntersection(Vector3 v)
        {
            int x = Mathf.RoundToInt(v.x / scale) * scale;
            int y = Mathf.RoundToInt(v.y / scale) * scale;
            if (x < 0) { x = 0; }
            if (y < 0) { y = 0; }
            if (x > Board.Main.TotalWidth()) { x = (int)Board.Main.TotalWidth(); }
            if (y > Board.Main.TotalHeight()) { y = (int)Board.Main.TotalHeight(); }
            if(Board.Main.Data.Tiles==null){Debug.Log("Board.Main is null");}

            if (Board.Main.Data.Tiles[x / scale, y / scale] != null) { return transform.position; }
            return new Vector3(x, y, 0);
        }

        IEnumerator SendPositionToServer()
        {
            while (true)
            {
                if (Player.Current==null){yield return new WaitForSeconds(1);}
                if (Player.Current.Human && GameManager.Instance != null)
                {
                    NetworkPlayer np = (NetworkPlayer)Player.Current;
                    int x = (int)transform.position.x;
                    int y = (int)transform.position.y;
                    np.client.Send("SetGhost|" + Player.Current.Name + "|" + x + "|" + y);
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}