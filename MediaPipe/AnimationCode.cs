using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCode : MonoBehaviour
{
    public UDPReceive udpReceive;
    public GameObject[] Body;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        
        string[] points = data.Split(',');
        print(points[0]); // 첫번째 좌표 가져온것임

        for (int i=0; i<33; i++){
            
            float x = float.Parse(points[i * 3])/100;
            float y = float.Parse(points[i * 3 + 1])/100;
            float z = float.Parse(points[i * 3 + 2])/100; // 좌표 값 동기화

            Body[i].transform.localPosition = new Vector3(x,y,z);
        
        }


    }
}
