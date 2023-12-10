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
        //SetPlayerID("Base"); // ���� �����Ҷ� �� �÷��̾�� �ٸ��� �����ؾ���
        //isControlable = true;// ���� �����Ҷ� �� �÷��̾�� �ٸ��� �����ؾ���

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
    }

    void Awake()
    {
        Inititialize();
    }
    public void Damage(float damage)
    {
        if (!isDead || (isUsingItem / 10) != 204) //���������� ������̰ų�, ���� �ʾҴٸ�,
            Hp -= damage;
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
    /// �ӽ÷� ���ǵ� �ܰ踦 �����ϴ� �Լ��Դϴ�.
    /// �÷��̾��� ���ǵ巹���� ��ȭ���� ��������
    /// ���ǵ� ������ ���� ������Ű���� addSpeedLevel�Լ��� ����Ұ�
    /// </summary>
    /// <param name="relativeLevel"></param>
    public void modifyRelativeSpeed(int relativeLevel)
    {
        playerSpeed.setSpeedLevel(Speed_level + relativeLevel);
        uiSystem.updateSpeedLevel(); // �ӽ÷� �ٲٴ� �Լ� �ڿ��� ������Ʈ�� ������մϴ�.
    }
    /// <summary>
    /// ���ǵ巹���� �ø������� ���� �Լ��Դϴ�.
    /// </summary>
    /// <param name="add"></param>
    public void addSpeedLevel(int add)
    {
        Speed_level += add;
        InitSpeedLevel();
    }

    #region ��� �����ۺ� �Լ�,�ڷ�ƾ
    public void TempModifyRelativeSpeed(float t, int relativeLevel)
    {
        StartCoroutine(CorTempModifyRelativeSpeed(t, relativeLevel));
        uiSystem.updateSpeedLevel(); // �ӽ÷� �ٲٴ� �Լ� �ڿ��� ������Ʈ�� ������մϴ�.
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
