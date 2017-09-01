using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caged
{
    public class GameController : MonoBehaviour
    {
		public static GameController main;
		public BoardData Data;
		public BoardDisplay Display;
        public Player[] players;
		public int PlayerIndex = 0;
		public int MonstersAmnt = 10;
		public Image MonsterPicture;
		public Text MonsterDescription;
		public int WinAmnt;
		public Player Winner;

		public GameObject WinScreen;

    }
}