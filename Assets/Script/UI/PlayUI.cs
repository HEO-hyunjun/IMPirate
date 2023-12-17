using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PlayUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nicknameInputField;
    [SerializeField]
    private TMP_InputField ipInputField;
    [SerializeField]
    private GameObject createRoomUI;

    public void OnClickCreateRoomButton()
    {
        if(nicknameInputField.text != "")
        {
            PlayerSettings.nickname = nicknameInputField.text;
            createRoomUI.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
        
    }

    public void OnClickEnterGameRoomButton()
    {
        if (nicknameInputField.text != "" && ipInputField.text != "")
        {
            PlayerSettings.nickname = nicknameInputField.text;
            var manager = RoomManager.singleton;
            manager.networkAddress = ipInputField.text;
            manager.StartClient();
        }
        if(nicknameInputField.text == "")
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
        if (ipInputField.text == "")
        {
            ipInputField.GetComponent<Animator>().SetTrigger("on");
        }
    }
}
