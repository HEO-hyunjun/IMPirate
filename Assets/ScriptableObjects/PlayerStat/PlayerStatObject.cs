using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Object/PlayerStat", order = int.MaxValue)]
public class PlayerStatObject : ScriptableObject
{
    public float max_hp;

    public int max_attack;
    public int attack;

    public int max_speed_level;
    public int speed_level;

    public float attackInterval;
}