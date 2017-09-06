using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Caged
{
    public class GameController : MonoBehaviour
    {
        public Board board;
        public Text localPlayerLabel;
        public Text remotePlayerLabel;
        public Text gameStateLabel;
        public MatchController matchController;

		void Start(){
			this.AddObserver(OnPlace, Board.OnSetTile);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args">String in format Up</param>
		public void OnPlace(object sender, object args){
			TileData s=(Tile)args;

		}

		public void MakeNewTile(object sender, object args)
        {
            string s = (string)args;

        }


       

    }
}