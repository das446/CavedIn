using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class Tile : MonoBehaviour
    {
        public TileData Data;
        public TileDisplay Display;

        void Initialize()
        {
            Data = new TileData();
            Display = GetComponent<TileDisplay>();
            Display.tile = this;
            Display.Data = Data;
        }

        public string toString()
        {
            return Data.toString();
        }

        public IEnumerator Rotate()
        {
            yield return StartCoroutine(Display.Rotate());
        }

        public void AdjustDisplay()
        {
            Display.AdjustDisplay();
        }

        public void RandomizeColors()
        {
            Data.RandomizeColors();
            Display.AdjustDisplay();
        }
        public void SetAll(Color c)
        {
            Data.SetAll(c);
            Display.AdjustDisplay();
        }
        public void Set(TileData t)
        {
            Data.Set(t);
            Display.AdjustDisplay();
        }
        public void Set(Color up, Color right, Color down, Color left)
        {
            Data.Set(up, right, down, left);
            Display.AdjustDisplay();
        }

        public void Set(string up, string right, string down, string left)
        {
            Data.Set(Color.FromName(up), Color.FromName(right), Color.FromName(down), Color.FromName(left));
            Display.AdjustDisplay();
        }

        public static implicit operator TileData(Tile t)
        {
            return t.Data;
        }
        public static implicit operator TileDisplay(Tile t)
        {
            return t.Display;
        }

    }
}