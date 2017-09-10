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
            public bool capture;//TODO None-Empty-Monster

            public Move(Vector2 p, int i, int r, bool c)
            {
                pos = p;
                tile = i;
                rotations = r;
                capture = c;
            }

            public String toString()
            {
                return "Move Pos=" + pos.x + "," + pos.y + "\nTile n=" + tile + " Rotations=" + rotations;
            }
        }
        public Player player;

        public float WaitTime = 1;
        public bool PlayingPiece = false;

        public float startWait=1;
        void Start()
        {
            player = GetComponent<Player>();
            player.Human = false;
        }

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
            PlayingPiece = true;
            //GhostTile.main.Ca
            Move move = DecideMove();
            yield return StartCoroutine(MoveTile(move));
            PlayingPiece = false;
        }
        Move DecideMove()
        {
            System.Random randomizer = new System.Random();
            Move[] moves = ValidMoves(player).ToArray();
            Move[] capturingMoves = moves.Where(x => x.capture).ToArray();
            if(capturingMoves.Length>0){
                return capturingMoves[randomizer.Next(capturingMoves.Length)];    
            }
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
            GhostTile.main.transform.position = Vector2.zero;
            float scale = Board.Main.scale;
            Vector2 targetPos = move.pos * scale;
            Vector2 currentPos = GhostTile.main.transform.position;
            yield return new WaitForSeconds(startWait);
            while (targetPos != currentPos)
            {
                if (currentPos.x < targetPos.x)
                {
                    currentPos.x += scale;
                }
                else if (currentPos.x > targetPos.x)
                {
                    currentPos.x -= scale;
                }

                if (Board.Main.Data[currentPos] == null)
                {
                    GhostTile.main.transform.position = currentPos;
                    yield return new WaitForSeconds(WaitTime);
                }

                if (currentPos.y < targetPos.y)
                {
                    currentPos.y += scale;
                }
                else if (currentPos.y > targetPos.y)
                {
                    currentPos.y -= scale;
                }

                if (Board.Main.Data[currentPos] == null)
                {
                    GhostTile.main.transform.position = currentPos;
                    yield return new WaitForSeconds(WaitTime);
                }

            }
        }

        //Static so these can be reused by other classes
        //May seperate into another class
        public static HashSet<Move> ValidMoves(Player player)
        {
            HashSet<Vector2> Positions = ValidPositions(Board.Main.Data);
            HashSet<Move> Moves = new HashSet<Move>();
            Board board = Board.Main;
            foreach (Vector2 v in Positions)
            {
                int x = (int)v.x;
                int y = (int)v.y;
                int c = 0;
                int height = board.Data.height;
                int width = board.Data.width;
                foreach (TileData t in player.Tiles)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        if (board.CanPlayTile(t, x, y) && x >= 0 && y >= 0 && x < width && y < height)
                        {
                            Move m = new Move(v, c, r, false);
                            m.capture = WillCapture(m, board);
                            Moves.Add(m);
                        }
                        t.Rotate();
                    }
                    c++;
                }
            }
            return Moves;

        }

        public static bool WillCapture(Move move, Board board)
        {
            int x = (int)move.pos.x;
            int y = (int)move.pos.y;
            bool UR=board.Data[x, y + 1] != null && board.Data[x + 1, y] != null && board.Data[x + 1, y + 1] != null;
            bool DR = board.Data[x, y - 1] != null && board.Data[x + 1, y] != null && board.Data[x + 1, y - 1] != null;
            bool DL = board.Data[x-1, y] != null && board.Data[x, y-1] != null && board.Data[x - 1, y - 1] != null;
            bool UL = board.Data[x-1, y] != null && board.Data[x, y+1] != null && board.Data[x - 1, y + 1] != null;

            return UR || DR || DL || UL;
        }

        public static HashSet<Vector2> ValidPositions(BoardData board)
        {
            HashSet<Vector2> Positions = new HashSet<Vector2>();
            foreach (Vector2 v in board.FilledPositions)
            {
                int x = (int)v.x;
                int y = (int)v.y;
                TileData t;
                if (x < 0 || y < 0 || x >= board.width || y >= board.height) { Debug.Log("Skip outOfBounds"); continue; }
                else { t = board.Tiles[x, y]; }
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