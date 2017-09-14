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
        public static MoveCamera main;
        float xOff=0;
        float yOff=0;

        void Start()
        {
            main=this;    
        }
        void Update()
        {
            //if(!Player.Current.Human){return;}
            viewWidth = Camera.main.orthographicSize - 10;
            viewHeight = Camera.main.orthographicSize + 2;
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < Board.Main.TotalWidth() - viewWidth / 2)
            {
               MoveRight();
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > viewWidth / 2 + UIWidth)
            {
               MoveLeft();
            }
            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < Board.Main.TotalHeight() - viewHeight / 2)
            {
                MoveUp();
            }
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > viewHeight / 2)
            {
                MoveDown();
            }
        }

        public void MoveTowards(Vector2 target){
            if(transform.position.x<target.x){
                MoveRight();
            }
            else if(transform.position.x>target.x){
                MoveLeft();
            }

            if(transform.position.y<target.y){
                MoveUp();
            }
            else if(transform.position.y>target.y){
                MoveDown();
            }

        }

        void MoveRight(){
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            xOff -= Time.deltaTime / 5;
            Background.mainTextureOffset = new Vector2(xOff, yOff);
        }
        void MoveLeft(){
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            xOff += Time.deltaTime / 5;
            Background.mainTextureOffset = new Vector2(xOff, yOff);
        }
        void MoveUp(){
            yOff -= Time.deltaTime / 10;
            Background.mainTextureOffset = new Vector2(xOff, yOff);
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        void MoveDown(){
            transform.Translate(Vector3.down * Time.deltaTime * speed);
            yOff += Time.deltaTime / 10;
            Background.mainTextureOffset = new Vector2(xOff, yOff);
        }
    }
}