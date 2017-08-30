using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Caged
{
    public class AIPlayer : MonoBehaviour
    {
		public struct Move{
			Vector2 pos;
			Tile tile;
			int rotations;
			bool willCapture;

			public Move(Vector2 p,Tile t,int r,bool c){
				pos=p;
				tile=t;
				rotations=r;
				willCapture=c;
			}
		}
        public Player player;
        // Use this for initialization
        void Start()
        {
            player = GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log(ValidPositions().Count);
            }
        }

		public HashSet<Move> ValidMoves(){
			HashSet<Vector2> Positions = ValidPositions();
			HashSet<Move> Moves=new HashSet<Move>();
			Board board = Board.Main;
			foreach(Vector2 v in Positions){
				int x=(int)v.x;
				int y=(int)v.y;
				foreach(Tile t in player.Tiles){
					for(int i=0;i<4;i++){
						if(board.CanPlayTile(t,x,y)){

							//Moves.Add(new Move(v,t,i));
						}
						t.RotateClockwise();
					}
				}
			}
			return Moves;

		}

        public HashSet<Vector2> ValidPositions()
        {
            Board board = Board.Main;
            HashSet<Vector2> Positions = new HashSet<Vector2>();
            foreach (Vector2 v in board.FilledPositions)
            {
                int x = (int)v.x;
                int y = (int)v.y;
                Tile t = board.Tiles[x, y];
                if (t.Above() == null)
                {
                    Positions.Add(new Vector2(v.x, v.y + 1));
                }
                if (t.Below() == null)
                {
                    Positions.Add(new Vector2(v.x, v.y - 1));
                }
                if (t.ToRight() == null)
                {
                    Positions.Add(new Vector2(v.x + 1, v.y));
                }
                if (t.ToLeft() == null)
                {
                    Positions.Add(new Vector2(v.x - 1, v.y));
                }
            }
            return Positions;
        }
    }
}