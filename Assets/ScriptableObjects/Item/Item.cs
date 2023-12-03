using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using JetBrains.Annotations;
using System;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item", order = int.MaxValue)]
public class Item :ScriptableObject
{
    //������ ī�װ��� 1000�ڸ�����
    // 1: ȹ�������
    // 2: ��������
    public int itemID;
    public string itemName;
    [Tooltip("������ ����")]
    [SerializeField]
    private string descript;
    public string Descript
    {
        get { return getDescription(); }
        set { descript = value; }
    }
    public Sprite image;
    public float duration = 0;
    public float changeStatAmount = 0;
    [MMFHidden]
    public string owner;

    private string getDescription()
    {
        string ret = descript;
        ret.Replace("a",duration.ToString());
        ret.Replace("b",changeStatAmount.ToString());
        return ret;
    }
}
