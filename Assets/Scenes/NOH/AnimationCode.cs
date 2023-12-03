using System;
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
        Vector3[] coordinates = new Vector3[points.Length/3];
        
        for( int i =0; i<32; i++){
            float x = float.Parse(points[i * 3]);
            float y = float.Parse(points[i * 3 + 1]);
            float z = float.Parse(points[i * 3 + 2]); // 좌표 값 동기화
            
            coordinates[i] = new Vector3(x, y, x);
        } // 변환 전 좌표 계산 용도

        void classifyPose(Vector3[] coordinates){
            /*for (int i = 0; i < coordinates.Length; i++) {
                print(string.Join(", ", coordinates));
            }*/

            float left_elbow_angle = calculateAngle(coordinates[15], coordinates[13], coordinates[11]);
            float right_elbow_angle = calculateAngle(coordinates[12], coordinates[14], coordinates[16]);
            float left_shoulder_angle = calculateAngle(coordinates[23], coordinates[11], coordinates[13]);
            float right_shoulder_angle = calculateAngle(coordinates[14], coordinates[12], coordinates[24]);


            if (right_elbow_angle > 0 && right_elbow_angle <25){
                print("공격");
            }

        }

        classifyPose(coordinates);

        float calculateAngle(Vector3 landmark1, Vector3 landmark2, Vector3 landmark3){
            float x1 = landmark1.x, y1 = landmark1.y, _1 = landmark1.z;
            float x2 = landmark2.x, y2 = landmark2.y, _2 = landmark2.z;
            float x3 = landmark3.x, y3 = landmark3.y, _3 = landmark3.z;

            float angle = Mathf.Rad2Deg * (Mathf.Atan2(y3 - y2, x3 - x2) - Mathf.Atan2(y1 - y2, x1 - x2));
            
            if(angle<0){
                angle += 360 ;
            }
            
            return angle;    
        }

        float left_elbow_angle = calculateAngle(coordinates[15], coordinates[13], coordinates[11]);
        float right_elbow_angle = calculateAngle(coordinates[12], coordinates[14], coordinates[16]);
        float left_shoulder_angle = calculateAngle(coordinates[23], coordinates[11], coordinates[13]);
        float right_shoulder_angle = calculateAngle(coordinates[14], coordinates[12], coordinates[24]);

        
        Debug.Log($"왼 어깨의 각도는 {left_shoulder_angle} 입니다."); // 왼 팔꿈치, 왼 어깨, 왼쪽 허리라인
        Debug.Log($"오른 어깨의 각도는 {right_shoulder_angle} 입니다."); // 오른 팔꿈치, 오른 어깨, 오른쪽 허리라인
        Debug.Log($"왼 팔꿈치의 각도는 {left_elbow_angle} 입니다."); // 왼 어깨, 왼 팔꿈치, 왼 손목
        Debug.Log($"오른 팔꿈치의 각도는 {right_elbow_angle} 입니다."); // 오른어깨, 오른 팔꿈치, 오른손목

        for (int i=0; i<32; i++){
            
            float x = float.Parse(points[i * 3])/100;
            float y = float.Parse(points[i * 3 + 1])/100;
            float z = float.Parse(points[i * 3 + 2])/100; // 좌표 값 동기화
            Body[i].transform.localPosition = new Vector3(x,y,z);
        }
            
    }
}
