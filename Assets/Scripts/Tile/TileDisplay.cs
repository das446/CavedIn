using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caged
{
    public class TileDisplay : MonoBehaviour
    {
        public TileData Data;
        public Tile tile;
        public float rotateSpeed;
        public bool rotating = false;
        public float transparency;
        public GameObject right, rightI, down, downI, left, leftI, up, upI;


        public void AdjustDisplay(){
            Data=tile.Data;
            AdjustColors();
            AdjustTransparency();
        }
        public void AdjustColors()
        {
            right.GetComponent<Renderer>().material.color = Data.Right;
            rightI.GetComponent<Renderer>().material.color = Data.Right;
            left.GetComponent<Renderer>().material.color = Data.Left;
            leftI.GetComponent<Renderer>().material.color = Data.Left;
            up.GetComponent<Renderer>().material.color = Data.Up;
            upI.GetComponent<Renderer>().material.color = Data.Up;
            down.GetComponent<Renderer>().material.color = Data.Down;
            downI.GetComponent<Renderer>().material.color = Data.Down;
        }
        public void AdjustTransparency()
        {
            Renderer[] rens = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rens)
            {
                UnityEngine.Color c = r.material.color;
                c.a = transparency;
                r.material.color = c;
            }
            
        }

        public IEnumerator Rotate()
        {
            if (rotating) { yield break; }
            rotating = true;
            Vector3 target = transform.eulerAngles;
            Vector3 temp = target;
            target.z = target.z - 90;
            if (target.z < 0) { target.z += 360; }
            while (Mathf.Abs(transform.eulerAngles.z - target.z) > 3f)
            {
                temp.z -= Time.deltaTime * rotateSpeed;
                transform.eulerAngles = temp;
                yield return new WaitForEndOfFrame();
            }
            transform.eulerAngles = Vector3.zero;
            rotating = false;
            Data.Rotate();
            AdjustDisplay();
        }
    }
}