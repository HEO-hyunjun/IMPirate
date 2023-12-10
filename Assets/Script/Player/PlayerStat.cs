using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStat : MonoBehaviour
{
    public PlayerStatObject source;
    public StatUISystem uiSystem;

    public bool isDead = false;
    public bool isControlable = false;
    public int isUsingItem = 0;
    [SerializeField]
    protected string playerID;
    public string PlayerID
    {
        get { return playerID; }
    }

    private int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            uiSystem.updateScore();
        }
    }
    protected float max_hp;
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
    protected float hp;
    public float Hp
    {
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
            uiSystem.updateHP();
        }
        get { return hp; }
    }

    public float max_attack;
    [SerializeField]
    protected float attack;
    [MMHidden]
    public float tmpAttack = 0;
    public float Attack
    {
        get { return Mathf.Floor(attack); }
        set
        {
            if (value < 0)
                attack = 0;
            else if (value > max_attack)
                attack = max_attack;
            else
                attack = value;
            uiSystem.updateAttack();
        }
    }

    public float attackInterval;
    [SerializeField]
    protected int remainBullet = 15;
    public int RemainBullet
    {
        set
        {
            if (value < 0)
                remainBullet = 0;
            else
                remainBullet = value;
            uiSystem.updateRemainBullet();
        }
        get { return remainBullet; }
    }

    public int max_speed_level;
    [SerializeField]
    protected int speed_level;
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
            uiSystem.updateSpeedLevel();
        }
    }
    /// <summary>
    /// 사용하기 전에 반드시 speedlevel설정을 해줄것
    /// </summary>
    public PlayerSpeed playerSpeed;

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
        speed_level = source.speed_level;
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
    /// level에 맞게 악셀 토크 로테이션값을 설정해줍니다.
    /// </summary>
    /// <param name="level"></param>
    public void setSpeedLevel(int level)
    {
        accel = 1000f + (level - 1) * 500f;
        torque = 400f + (level - 1) * 50f;
        rot = 30f + Mathf.Min((level - 1), 6) * 5;
    }
    public int getSpeedLevel()
    {
        return (int)(((accel - 1000f) / 500f) + 1);
    }
}