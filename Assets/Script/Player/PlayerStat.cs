using Mirror;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStat : NetworkBehaviour
{
    public PlayerStatObject source;
    public StatUISystem uiSystem;
    #region 플레이어의 상태
    [SyncVar]
    public bool isDead = false;
    public bool isControlable { 
        get {
            return isOwned; 
        } 
    }
    
    public int isUsingItem = 0;
    [SerializeField]
    [MMFReadOnly]
    public bool isAttackable = true;
    #endregion
    #region 네트워크에 필요한 플레이어 정보
    [SerializeField]
    [SyncVar]
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
        }
    }
    #endregion
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
    [SyncVar]
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
        }
        get { return hp; }
    }
    #region 공격관련
    public float max_attack;
    [SerializeField]
    [SyncVar]
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
    #endregion
    #region 속도관련
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
    #endregion
    #region 초기화 메소드들
    public void SetPlayerID(string id)
    {
        CmdSetPlayerID(id);
    }

    [Command]
    public void CmdSetPlayerID(string id)
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
    #endregion
}
#region PlayerSpeed Class
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
        accel = 500f + (level - 1) * 150f;
        torque = 350f + (level - 1) * 50f;
        rot = 30f + Mathf.Min((level - 1), 6) * 5;
    }
    public int getSpeedLevel()
    {
        return (int)(((accel - 500f) / 150f) + 1);
    }
}
#endregion