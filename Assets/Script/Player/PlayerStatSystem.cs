using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using Cinemachine;
using Mirror;

// playerstatobject -> playerstat -> statsystem
// playerstatobject 데이터
// playerstat 데이터 + 이벤트기반 프로퍼티 + 데이터 로드
// statsystem 메소드
public class PlayerStatSystem : PlayerStat
{
    public AnimatorController animatorController;
    public CinemachineVirtualCamera cam;
    private void Inititialize()
    {
        //animatorController = Instantiate(animatorController);
        LoadStatObject();
        SetPlayerID(PlayerSettings.nickname); // 서버 연결할때 각 플레이어별로 다르게 세팅해야함

        //isControlable = true;// 서버 연결할때 각 플레이어별로 다르게 세팅해야함

        if (isControlable)
        {
            cam.Priority = 11;
        }
        else
        {
            cam.Priority = 10;
            cam.gameObject.SetActive(false);
        }
        InitSpeedLevel();
        uiSystem.updateSpeedLevel();
    }

    void Start()
    {
        Inititialize();
    }
    #region Stat 변경 함수들
    [Command]
    public void CmdDamage(float damage)
    {
        if (!isDead || (isUsingItem / 10) != 204) //무적아이템 사용중이거나, 죽지 않았다면,
        { 
            Hp -= damage;
        }
        if (isDead)
            Dead();
    }

    public void Damage(float damage)
    {
        //CmdDamage(damage);
        if (!isDead || (isUsingItem / 10) != 204) //무적아이템 사용중이거나, 죽지 않았다면,
        {
            Hp -= damage;
            if (animatorController != null && isControlable)
                animatorController.TriggerHit();
        }
        if (isDead)
            Dead();
    }

    public void Dead()
    {
        Debug.Log(PlayerID + "is dead");
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

    /// <summary>
    /// 임시로 스피드 단계를 조정하는 함수입니다.
    /// 플레이어의 스피드레벨은 변화하지 않음으로
    /// 스피드 레벨을 바을 변동시키려면 addSpeedLevel함수를 사용할것
    /// </summary>
    /// <param name="relativeLevel"></param>
    public void modifyRelativeSpeed(int relativeLevel)
    {
        playerSpeed.setSpeedLevel(Speed_level + relativeLevel);
        uiSystem.updateSpeedLevel(); // 임시로 바꾸는 함수 뒤에는 업데이트를 해줘야합니다.
    }
    /// <summary>
    /// 스피드레벨을 올리기위해 만든 함수입니다.
    /// </summary>
    /// <param name="add"></param>
    public void addSpeedLevel(int add)
    {
        Speed_level += add;
        InitSpeedLevel();
        uiSystem.updateSpeedLevel();
    }

    public void TempModifyRelativeSpeed(float t, int relativeLevel)
    {
        StartCoroutine(CorTempModifyRelativeSpeed(t, relativeLevel));
        uiSystem.updateSpeedLevel(); // 임시로 바꾸는 함수 뒤에는 업데이트를 해줘야합니다.
    }
    private IEnumerator CorTempModifyRelativeSpeed(float t, int relativeLevel)
    {
        modifyRelativeSpeed(relativeLevel);
        yield return new WaitForSeconds(t);
        InitSpeedLevel();
        uiSystem.updateSpeedLevel();
    }

    public void TempMultiplyAttack(float t, float multi)
    {
        StartCoroutine(CorTempMultiplyAttack(t, multi));
    }
    private IEnumerator CorTempMultiplyAttack(float t, float multi)
    {
        float tmp = Attack;
        Attack *= multi;
        yield return new WaitForSeconds(t);
        Attack = tmp;
        if (tmpAttack != 0)
        {
            Attack += tmpAttack;
            tmpAttack = 0;
        }
    }

    public void TempMultiplyAttackInterval(float t, float multi)
    {
        StartCoroutine(CorTempMultiplyAttackInterva(t, multi));
    }
    private IEnumerator CorTempMultiplyAttackInterva(float t, float multi)
    {
        float tmp = attackInterval;
        attackInterval *= (1 / multi);
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
        for (int cnt = 0; cnt < destCnt; cnt++)
        {
            Hp += amount / destCnt;
            yield return new WaitForSeconds(t / destCnt);
        }
    }
    #endregion
}
