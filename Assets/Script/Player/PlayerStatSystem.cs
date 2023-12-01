using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using Cinemachine;

// playerstatobject -> playerstat -> statsystem
// playerstatobject data only
// playerstat data + eventbase property + init data
// statsystem contain method, playerstat
public class PlayerStatSystem : PlayerStat
{
    public CinemachineVirtualCamera cam;
    private void Inititialize()
    {
        LoadStatObject();
        //SetPlayerID("Base"); // 서버 연결할때 각 플레이어별로 다르게 세팅해야함
        //isControlable = true;// 서버 연결할때 각 플레이어별로 다르게 세팅해야함
        if (isControlable)
        {
            cam.Priority = 11;
        }
        InitSpeedLevel();
    }

    void Awake()
    {
        Inititialize();
    }

    public void Damage(float damage)
    {
        if(!isDead || isUsingItem != 204) //무적아이템 사용중이거나, 죽지 않았다면,
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
    public void TempModifyItemUsing(float t, int itemID)
    {
        StartCoroutine(CorTempModifyItemUsing(t, itemID));
    }
    private IEnumerator CorTempModifyItemUsing(float t, int itemID)
    {
        isUsingItem = itemID;
        yield return new WaitForSeconds(t);
        isUsingItem = 0;
    }
    #region 사용 아이템별 함수,코루틴
    public void TempModifyRelativeSpeed(float t, int relativeLevel)
    {
        StartCoroutine(CorTempModifyRelativeSpeed(t, relativeLevel));
    }
    private IEnumerator CorTempModifyRelativeSpeed(float t, int relativeLevel)
    {
        modifyRelativeSpeed(relativeLevel);
        yield return new WaitForSeconds(t);
        InitSpeedLevel();
    }

    public void TempMultiplyAttack(float t, float multi)
    {
        StartCoroutine(CorTempMultiplyAttack(t, multi));
    }
    private IEnumerator CorTempMultiplyAttack(float t, float multi)
    {
        float tmp = attack;
        attack *= multi;
        yield return new WaitForSeconds(t);
        attack = tmp;
    }

    public void TempMultiplyAttackInterval(float t, float multi)
    {
        StartCoroutine(CorTempMultiplyAttackInterva(t, multi));
    }
    private IEnumerator CorTempMultiplyAttackInterva(float t, float multi)
    {
        float tmp = attackInterval;
        attackInterval *= (1/multi);
        yield return new WaitForSeconds(t);
        attackInterval = tmp;
    }


    public void TempHPAdd(float t, float amount)
    {
        StartCoroutine(CorTempHPAdd(t, amount));
    }
    private IEnumerator CorTempHPAdd(float t, float amount)
    {
        int destCnt = 5;
        for (int cnt = 0; cnt<destCnt; cnt++)
        {
            Hp += amount / destCnt;
            yield return new WaitForSeconds(t/destCnt);
        }
    }
    #endregion
}
