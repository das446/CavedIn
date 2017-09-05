using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
namespace Caged
{
    public class AIPlayer : MonoBehaviour
    {
        public struct Move
        {
            public Vector2 pos;
            public int tile;
            public int rotations;
            public bool willCapture;

            public Move(Vector2 p, int i, int r, bool c)
            {
                pos = p;
                tile = i;
                rotations = r;
                willCapture = c;
            }

            public String toString()
            {
                return "Move Pos=" + pos.x + "," + pos.y + "\nTile n=" + tile + " Rotations=" + rotations;
            }
        }
        public Player player;

        public float WaitTime=1;
        public bool PlayingPiece = false;
        // Use this for initialization
        void Start()
        {
            player = GetComponent<Player>();
            player.Human = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log(ValidPositions(Board.Main.Data).Count);
            }
            if (Player.Current == player && !PlayingPiece)
            {
                StartCoroutine(TakeTurn());
            }
        }

        IEnumerator TakeTurn()
        {
            PlayingPiece=true;
            //GhostTile.main.Ca
            Move move = DecideMove();
            Debug.Log(move.toString());
            yield return StartCoroutine(MoveTile(move));
            PlayingPiece=false;
        }
        Move DecideMove()
        {
            System.Random randomizer = new System.Random();
            Move[] moves = ValidMoves().ToArray();
            return moves[randomizer.Next(moves.Length)];
        }
        IEnumerator MoveTile(Move move)
        {
            //SetGhost
            Tile tile = player.Tiles[move.tile];
            tile.GetComponent<InHandTile>().Select();

            //MoveGhost
            yield return MoveGhostTileToPos(move);
            for (int i = 0; i < move.rotations; i++)
            {
                yield return StartCoroutine(GhostTile.main.ghostTile.Rotate());
                yield return new WaitForSeconds(WaitTime);
            }
            GhostTile.main.Place();

        }

        IEnumerator MoveGhostTileToPos(Move move)
        {
            GhostTile.main.transform.position=Vector2.zero;
            float scale = Board.Main.scale;
            Vector2 targetPos = move.pos*scale;
            Vector2 currentPos = GhostTile.main.transform.position;
            while (targetPos!=currentPos)
            {
                if(currentPos.x<targetPos.x){
                    currentPos.x+=scale;
                }
                else if(currentPos.x>targetPos.x){
                    currentPos.x-=scale;
                }

                if(Board.Main.Data[currentPos]==null){
                    GhostTile.main.transform.position=currentPos;
                    yield return new WaitForSeconds(WaitTime);
                }

                if (currentPos.y < targetPos.y)
                {
                    currentPos.y+=scale;
                }
                else if (currentPos.y > targetPos.y)
                {
                    currentPos.y-=scale;
                }

                if (Board.Main.Data[currentPos] == null)
                {
                    GhostTile.main.transform.position = currentPos;
                    yield return new WaitForSeconds(WaitTime);
                }
                
            }
        }

        public HashSet<Move> ValidMoves()
        {
            HashSet<Vector2> Positions = ValidPositions(Board.Main.Data);
            HashSet<Move> Moves = new HashSet<Move>();
            Board board = Board.Main;
            foreach (Vector2 v in Positions)
            {
                int x = (int)v.x;
                int y = (int)v.y;
                int c = 0;
                int height=board.Data.height;
                int width=board.Data.width;
                foreach (TileData t in player.Tiles)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        if (board.CanPlayTile(t, x, y)&&x>=0&&y>=0&&x<width&&y<height)
                        {
                            Moves.Add(new Move(v, c, r, true));
                        }
                        t.Rotate();
                    }
                    c++;
                }
            }
            return Moves;

        }

        public HashSet<Vector2> ValidPositions(BoardData board)
        {
            HashSet<Vector2> Positions = new HashSet<Vector2>();
            foreach (Vector2 v in board.FilledPositions)
            {
                int x = (int)v.x;
                int y = (int)v.y;
                TileData t;
                if(x<0||y<0||x>=board.width||y>=board.height){Debug.Log("Skip outOfBounds");continue;}
                else {t = board.Tiles[x, y];}
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