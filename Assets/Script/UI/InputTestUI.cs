using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputTestUI : MonoBehaviour
{
    public TMP_Text text;
    public UDPReceive source;

    private void Start()
    {
        source = InputManager.instance.udpReceive;
    }
    void Update()
    {
        text.text = "data : "+source.data;
    }
}
