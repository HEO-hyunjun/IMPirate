using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStat : MonoBehaviour
{
    public PlayerStatObject source;
    public bool isDead = false;
    public int isUsingItem = 0;
    [SerializeField]
    private string playerID;
    public string PlayerID { 
        get { return playerID; } 
    }

    public int score = 0;

    private float max_hp;
    public float Max_hp
    {
        get { return max_hp; }
        set
        {
            float tmp = hp / max_hp;
            max_hp = value;
            hp *= tmp;
        }
    }
    [SerializeField]
    private float hp;
    public float Hp { 
        set
        {
            if (value < 0)
            {
                isDead = true;
                hp = 0;
            }
            else if (value >= max_hp)
                hp = max_hp;
            else
                hp = value;
        }
        get { return hp; } 
    }

    public int max_attack;
    [SerializeField]
    private int attack;
    public int Attack
    {
        set
        {
            if (value < 0)
                attack = 0;
            else if (value > max_attack)
                attack = max_attack;
            else
                attack = value;
        }
    }

    public int max_speed_level;
    [SerializeField]
    private int speed_level;
    public int Speed_level
    {
        get { return speed_level; }
        set
        {
            if (value < 0)
                speed_level = 0;
            else if (value > max_speed_level)
                speed_level = max_speed_level;
            else
                speed_level = value;
        }
    }
    /// <summary>
    /// 사용하기 전에 반드시 speedlevel설정을 해줄것
    /// </summary>
    public PlayerSpeed playerSpeed;

    public float attackInterval;
    public void SetPlayerID(string id)
    {
        playerID = id;
    }

    public void LoadStatObject()
    {
        max_hp = source.max_hp;
        hp = source.max_hp;

        max_attack = source.max_attack;
        attack = source.attack;

        max_speed_level = source.max_speed_level;
        speed_level= source.speed_level;
        playerSpeed = new PlayerSpeed();
        InitSpeedLevel();

        attackInterval = source.attackInterval;
    }

    /// <summary>
    /// 스피드 레벨에 맞게 스텟을 초기화합니다.
    /// </summary>
    public void InitSpeedLevel()
    {
        playerSpeed.setSpeedLevel(Speed_level);
    }
}

public class PlayerSpeed
{
    [SerializeField]
    private float accel;
    public float Accel { get { return accel; } }
    [SerializeField]
    private float torque;
    public float Torque { get { return torque; } }
    [SerializeField]
    private float rot;
    public float Rot { get { return rot; } }

    /// <summary>
    /// level 1~4중 악셀 토크 로테이션값을 설정해줍니다.
    /// </summary>
    /// <param name="level"></param>
    public void setSpeedLevel(int level)
    {
        switch (level)
        {
            case 1:
                accel = 1000f;
                torque = 400f;
                rot = 45f;
                break;
            case 2:
                accel = 1500f;
                torque = 450;
                rot = 50f;
                break;
            case 3:
                accel = 2000f;
                torque = 500f;
                rot = 55f;
                break;
            case 4:
                accel = 2500f;
                torque = 550f;
                rot = 60f;
                break;
            default:
                break;
        }
    }
}