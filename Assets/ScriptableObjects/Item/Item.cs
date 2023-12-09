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
    //아이템 분류
    // 1: 획득아이템
    // 2: 사용아이템
    public int itemID;
    public string itemName;
    [Tooltip("아이템 설명")]
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
        ret = ret.Replace("a",duration.ToString());
        ret = ret.Replace("b",changeStatAmount.ToString());
        return ret;
    }
}
