using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// playerstatobject -> playerstat -> statsystem
// playerstatobject data only
// playerstat data + eventbase property + init data
// statsystem contain method, playerstat
public class PlayerStatSystem : PlayerStat
{
    private void Inititialize()
    {
        LoadStatObject();
        SetPlayerID("Base"); // 서버 연결할때 각 플레이어별로 다르게 세팅해야함
        InitSpeedLevel();
    }

    void Awake()
    {
        Inititialize();
    }

    public void Damage(float damage)
    {
        // 아이템 구현할때 무적 먹으면 그때는 안닳게 조건문 추가할것
        Hp -= damage;
        if (isDead)
            Dead();
    }

    public void Dead()
    {
        Debug.Log(PlayerID + "is dead");
    }

    
    /// <summary>
    /// 임시로 스피드 단계를 조정하는 함수입니다.
    /// 플레이어의 스피드레벨은 변화하지 않음으로
    /// 스피드 레벨을 바을 변동시키려면 addSpeedLevel함수를 사용할것
    /// </summary>
    /// <param name="relativeLevel"></param>
    public void modifyRelativeSpeed(int relativeLevel)
    {
        playerSpeed.setSpeedLevel(Speed_level + relativeLevel);
    }
    /// <summary>
    /// 스피드레벨을 올리기위해 만든 함수입니다.
    /// </summary>
    /// <param name="add"></param>
    public void addSpeedLevel(int add)
    {
        Speed_level += add;
        InitSpeedLevel();
    }
}
