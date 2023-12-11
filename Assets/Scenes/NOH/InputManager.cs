using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public UDPReceive udpReceive;
    public bool isPoseDetect
    {
        get { return udpReceive != null; }
    }
    public float narrowingFraction = 10;
    public static InputManager instance = null;

    public GameObject[] Body = new GameObject[33];
    public GameObject ifEmpty;

    public float leftArrowVal = 0;
    public float rightArrowVal = 0;
    public bool isCancle = false;
    public bool isOK = false;
    public bool[] item = new bool[2];


    private Vector3[] coordinates = new Vector3[33];
    private float left_elbow_angle;
    private float right_elbow_angle;
    public float left_shoulder_angle;
    public float right_shoulder_angle;
    private void classifyPose(Vector3[] coordinates)
    {
        /*for (int i = 0; i < coordinates.Length; i++) {
            print(string.Join(", ", coordinates));
        }*/

        Vector3 leftHand = coordinates[20]; Vector3 rightHand = coordinates[19];
        if(leftHand.x > rightHand.x) // 취소 -> X표 포즈 
        {
            leftArrowVal = 0;
            rightArrowVal = 0;
            isCancle = true;
            isOK = false;
            item[0] = false; item[1] = false;
        }
        else if (leftHand.x < rightHand.x && rightHand.x - leftHand.x  < 15) // 공격 -> 박수
        {
            leftArrowVal = 0;
            rightArrowVal = 0;
            isCancle = false;
            isOK = true;
            item[0] = false; item[1] = false;
        }
        else
        {
            if (left_shoulder_angle > 110) 
                item[0] = true;
            if (left_shoulder_angle > 50)
                leftArrowVal = Mathf.Min(left_shoulder_angle / 90, 1f);
            else
                leftArrowVal = 0;

            if (right_shoulder_angle > 110)
                item[1] = true;
            if (right_shoulder_angle > 50)
                rightArrowVal = Mathf.Min(right_shoulder_angle / 90, 1f);
            else
                rightArrowVal = 0;

            isCancle = false;
            isOK = false;
        }

    }

    private float calculateAngle(Vector3 landmark1, Vector3 landmark2, Vector3 landmark3)
    {
        float x1 = landmark1.x, y1 = landmark1.y, _1 = landmark1.z;
        float x2 = landmark2.x, y2 = landmark2.y, _2 = landmark2.z;
        float x3 = landmark3.x, y3 = landmark3.y, _3 = landmark3.z;

        float angle = Mathf.Rad2Deg * (Mathf.Atan2(y3 - y2, x3 - x2) - Mathf.Atan2(y1 - y2, x1 - x2));

        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }
    

    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            //DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }

        if (udpReceive == null)
            udpReceive = GetComponent<UDPReceive>();

        for (int i = 0; i < 32; i++)
        {
            if (Body[i] == null)
            {
                Body[i] = Instantiate(ifEmpty);
                Body[i].transform.parent = transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        
        string[] points = data.Split(',');

        for (int i = 0; i < 32; i++)
        {
            float x = float.Parse(points[i * 3]);
            float y = float.Parse(points[i * 3 + 1]);
            float z = float.Parse(points[i * 3 + 2]); // 좌표 값 동기화
            Body[i].transform.position = new Vector3(x, y, z)/narrowingFraction;
            coordinates[i] = new Vector3(x, y, z) / narrowingFraction;
        }


        classifyPose(coordinates);

        left_elbow_angle = calculateAngle(coordinates[15], coordinates[13], coordinates[11]);
        right_elbow_angle = calculateAngle(coordinates[12], coordinates[14], coordinates[16]);
        left_shoulder_angle = calculateAngle(coordinates[23], coordinates[11], coordinates[13]);
        right_shoulder_angle = calculateAngle(coordinates[14], coordinates[12], coordinates[24]);

        /*
        Debug.Log($"왼 어깨의 각도는 {left_shoulder_angle} 입니다."); // 왼 팔꿈치, 왼 어깨, 왼쪽 허리라인
        Debug.Log($"오른 어깨의 각도는 {right_shoulder_angle} 입니다."); // 오른 팔꿈치, 오른 어깨, 오른쪽 허리라인
        Debug.Log($"왼 팔꿈치의 각도는 {left_elbow_angle} 입니다."); // 왼 어깨, 왼 팔꿈치, 왼 손목
        Debug.Log($"오른 팔꿈치의 각도는 {right_elbow_angle} 입니다."); // 오른어깨, 오른 팔꿈치, 오른손목
        */
       
    }
}
