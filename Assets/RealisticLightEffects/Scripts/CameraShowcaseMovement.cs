using UnityEngine;
using System.Collections;


namespace RLE
{

    public class CameraShowcaseMovement : MonoBehaviour
    {


        public float speed;


        void Update()
        {
            float x = Input.GetAxis("Horizontal");

            transform.position += x * Vector3.right * speed * Time.deltaTime;
        }


        void OnGUI()
        {
            GUILayout.Label("<color=white>Press  'A'  And  'D'  To Move</color>");
        }

    }

}