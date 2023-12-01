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
        SetPlayerID("Base"); // ���� �����Ҷ� �� �÷��̾�� �ٸ��� �����ؾ���
        InitSpeedLevel();
    }

    void Awake()
    {
        Inititialize();
    }

    public void Damage(float damage)
    {
        // ������ �����Ҷ� ���� ������ �׶��� �ȴ�� ���ǹ� �߰��Ұ�
        Hp -= damage;
        if (isDead)
            Dead();
    }

    public void Dead()
    {
        Debug.Log(PlayerID + "is dead");
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
}
