using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineAxisSetting : MonoBehaviour
{
    void Awake()
    {
        CinemachineCore.GetInputAxis = noneControl;
    }

    public float noneControl(string axis)
    {
        return 0;
    }
}
