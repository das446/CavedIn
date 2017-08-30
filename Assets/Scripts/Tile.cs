using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Caged
{
    public class Tile : MonoBehaviour
    {

        public Color Right, Down, Left, Up;
        public int x,y;
        public float rotateSpeed;
        bool rotating = false;
        public float transparency;

        public GameObject right, rightI, down, downI, left, leftI, up, upI;


        // Use this for initialization
        void Start()
        {
        
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("t");
                AdjustTransparency();
            }
        }

        public void RandomizeColors()
        {
            Right = Color.Random();
            Down = Color.Random();
            Left = Color.Random();
            Up = Color.Random();
        }

        public void AdjustColors()
        {
            right.GetComponent<Renderer>().material.color = Right;
            rightI.GetComponent<Renderer>().material.color = Right;
            left.GetComponent<Renderer>().material.color = Left;
            leftI.GetComponent<Renderer>().material.color = Left;
            up.GetComponent<Renderer>().material.color = Up;
            upI.GetComponent<Renderer>().material.color = Up;
            down.GetComponent<Renderer>().material.color = Down;
            downI.GetComponent<Renderer>().material.color = Down;
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
        

        void RotateColors()
        {
            Color t = Up;
            Up = Left;
            Left = Down;
            Down = Right;
            Right = t;
        }

        public IEnumerator RotateClockwise()
        {
            if(rotating){yield break;}
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
            RotateColors();
            AdjustColors();
            AdjustTransparency();
        }

        public void Copy(Tile t){
            if(t==null){Debug.Log("Tried to copy null tile"); return;}
            Right=t.Right;
            Left=t.Left;
            Up=t.Up;
            Down=t.Down;
            AdjustColors();
        }

        public void SetAll(Color c){
            Up=c;
            Down=c;
            Right=c;
            Left=c;
        }

        public Tile Above()
        {
            return Board.Main.Above(x,y);
        }
        public Tile Below()
        {
            return Board.Main.Below(x,y);
        }
        public Tile ToRight()
        {
            return Board.Main.ToRight(x,y);
        }
        public Tile ToLeft()
        {
            return Board.Main.ToLeft(x,y);
        }

    }
}