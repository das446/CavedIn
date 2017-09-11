using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class InHandTile : MonoBehaviour
    {

        // Use this for initialization
        public Vector2 Default;
        public static Tile selectedTile;
        public Tile tile;
        public GameObject Highlight;
        public float z;
        public Player Controller;
        public int i;
        void Start()
        {
            Default = Camera.main.WorldToScreenPoint(transform.position);
            tile = GetComponent<Tile>();
            if(selectedTile==null){
                Debug.Log("Assigned selectedTile");
                selectedTile=tile;
            }
        }

        void Update()
        {
            
            Vector3 v = Camera.main.ScreenToWorldPoint(Default);
            v.z = z;
            transform.position = v;
            if (tile == selectedTile)
            {
                Highlight.SetActive(true);
            }
            else
            {
                Highlight.SetActive(false);
            }



        }

        void OnMouseDown()
        {
          if(Controller.Human){Select();}
        }

        public void Select(bool SendToServer=true)
        {
            if(Controller!=Player.Current){return;}
            bool already=selectedTile==this;
            selectedTile = tile;
            if (GhostTile.main == null)
            {
                GhostTile.main = GameObject.FindObjectOfType<GhostTile>();
                Debug.Log(GhostTile.main);
                GhostTile.main.ghostTile=GhostTile.main.GetComponent<Tile>();
            }
            if(selectedTile==null){
                selectedTile=GetComponent<Tile>();
            }
            GhostTile.main.ghostTile.Set(selectedTile);
            GhostTile.main.ghostTile.Display.AdjustDisplay();

            if(GameManager.Instance!=null&&!already&&SendToServer){
                ServerSelect();
            }
            GhostTile.main.i=i;
        }

        void ServerSelect(){
            NetworkPlayer np=(NetworkPlayer)Controller;
            np.client.Send("Select|"+np.Name+"|"+i);
        }
    }
}