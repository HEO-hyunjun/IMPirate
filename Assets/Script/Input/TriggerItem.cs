using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    public int itemNum;
    private Collider collider;
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCharacter"))
        {
            if (InputManager.instance != null && InputManager.instance.isPoseDetect)
            {
                InputManager.instance.item[(int)Mathf.Clamp(itemNum,0,1)] = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCharacter"))
        {
            if (InputManager.instance != null && InputManager.instance.isPoseDetect)
            {
                InputManager.instance.item[(int)Mathf.Clamp(itemNum, 0, 1)] = false;
            }
        }
    }
}
