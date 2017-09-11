using System.Collections.Generic;
using UnityEngine;
using System.Collections;
namespace Caved
{
    public class MoveCamera : MonoBehaviour
    {

        public float speed;
        public float viewWidth, viewHeight;
        public float UIWidth;
        public float maxZ, minZ;
        public Material Background;
        float xOff=0;
        float yOff=0;
        void Update()
        {
            //if(!Player.Current.Human){return;}
            viewWidth = Camera.main.orthographicSize - 10;
            viewHeight = Camera.main.orthographicSize + 2;
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < Board.Main.TotalWidth() - viewWidth / 2)
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                xOff-=Time.deltaTime/5  ;
                Background.mainTextureOffset=new Vector2(xOff,yOff);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > viewWidth / 2 + UIWidth)
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                xOff += Time.deltaTime/5;
                Background.mainTextureOffset= new Vector2(xOff, yOff);
            }
            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < Board.Main.TotalHeight() - viewHeight / 2)
            {
                yOff-=Time.deltaTime/10;
                Background.mainTextureOffset = new Vector2(xOff, yOff);
                transform.Translate(Vector3.up * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > viewHeight / 2)
            {
                transform.Translate(Vector3.down * Time.deltaTime * speed);
                yOff+=Time.deltaTime/10;
                Background.mainTextureOffset = new Vector2(xOff, yOff);
            }
        }
    }
}