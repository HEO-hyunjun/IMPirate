using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSystem : MonoBehaviour
{
    #region �����۰��� Ŭ����
    [System.Serializable]
    public class ItemSlot
    {
        [MMFReadOnly]
        public Item item;
        [Tooltip("�������� ä���� UI 1�� 2��")]
        public Image EmptyImage;
        [Tooltip("������϶� Image�� ������ �ǵ��")]
        public MMF_Player usingItemFeedback; //�����Ÿ��°� + Ŀ�����۾�����
    }
    [System.Serializable]
    public class ItemDataSource
    {
        public Item item;
        [Tooltip("�������� ��������� ����������� ����� �ǵ��")]
        public MMF_Player feedback; // ȹ��������̸� �ֺ��� �׳� ���� ������, ���������̶�� ������ �������� ����� �ǵ�� + ����
    }
    #endregion

    [Tooltip("�ִ� 2���� ���Ը� �ִٰ� ������")]
    public List<ItemSlot> itemSlot;
    [Tooltip("������ ������϶� itemSlot�� ������ �ǵ��")]
    public MMF_Player itemUsingCoolDownFeedback; // �����۽����ʿ� ȸ�� ������ �������̾����
    [Tooltip("�������� ȹ�������� ����� �ǵ��")]
    public MMF_Player itemGetFeedback; // �÷��̾�ȭ�� ĵ������ ��� UI�� ����, �Ҹ����, ��ƼŬ���

    [Tooltip("�÷��̾������� ����� �ǵ���� ������ �������� ��Ƶδ� �����ͺ��̽�")]
    public List<ItemDataSource> ItemDataList;

    private PlayerStatSystem statSystem;

    #region ������ ȹ�� ~ �������� �Լ���
    /// <summary>
    /// �÷��̾ �������� �Ծ����� ���۵Ǵ� �Լ�
    /// </summary>
    /// <param name="itemID"></param>
    public void GetItem(int itemID)
    {
        //������ ī�װ� 1000�ڸ�����
        // 1: ȹ�������
        // 2: ��������
        int itemCategory = itemID / 1000;

        switch(itemCategory)
        {
            case 1: // ȹ�� ������ -> �ٷ� ȿ������
                UseItem(itemID);
                break;
            case 2: // ��� ������ -> �����۽��Կ� ����
                PushItem(itemID);
                break;
        }
    }

    /// <summary>
    /// �����۽��Կ� �������� �о�ִ� �Լ�
    /// </summary>
    /// <param name="itemID"></param>
    private void PushItem(int itemID)
    {
        int findIDX = -1;
        for(int i=0; i < 2; i++)
        {
            if (itemSlot[i].item.itemID == 0)
            {
                findIDX = i;
                break;
            }
        }
        //������ ������ �� á�ٸ� ����
        if(findIDX == - 1)
            return;

        itemGetFeedback.PlayFeedbacks();

        itemSlot[findIDX].item = FindItemByID(itemID);
        itemSlot[findIDX].EmptyImage.sprite = FindImageByID(itemID);

    }
    

    /// <summary>
    /// Ư���ε����� �����۽��� �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="index"></param>
    public void UseItemSlot(int index)
    {
        //�����۽��Կ��� ������ ���̵� �����ͼ� �ش� �Լ� ���
        if (itemSlot[index].item.itemID == 0)
            return;
        if (!UseItem(itemSlot[index].item.itemID))
            return;
        SetFeedbackDuration<MMF_ScaleShake>(itemSlot[index].usingItemFeedback, itemSlot[index].item.duration);
        SetFeedbackDuration<MMF_Pause>(itemUsingCoolDownFeedback, itemSlot[index].item.duration);
    }

    /// <summary>
    /// ������ ���̵𿡵��� �ش��ϴ� ���� �˻��� �ش� �Լ� ���
    /// �����ۺ��� �Լ��� ���� ���
    /// ������ ����� ���õƴٸ� False ��ȯ
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>

    private bool UseItem(int itemID)
    {
        Item item = FindItemByID(itemID);
        if(item != null)
            return false;
        itemID /= 10;

        if (item.itemID / 100 == 1)
        {
            itemGetFeedback.PlayFeedbacks();
            switch (itemID)
            {
                // ȹ�������
                case 101:
                    statSystem.RemainBullet += (int)item.changeStatAmount;
                    return true;
                case 102:
                    statSystem.score += (int)item.changeStatAmount;
                    return true;
                case 103:
                    statSystem.addSpeedLevel((int)item.changeStatAmount);
                    return true;
                case 104:
                    statSystem.Attack += item.changeStatAmount;
                    return true;
                case 105:
                    if (item.owner != statSystem.PlayerID)
                        statSystem.Damage(item.changeStatAmount);
                    return true;
            }
        }
        if (statSystem.isUsingItem != 0) // �ٸ� ������ �����
            return false;
        statSystem.score += 5;
        statSystem.TempModifyItemUsing(item.duration, item.itemID);
        switch (itemID)
        {
            // ��������
            case 201:
                statSystem.TempModifyRelativeSpeed(item.duration,(int)item.changeStatAmount);
                return true;
            case 202:
                statSystem.TempHPAdd(item.duration, item.changeStatAmount);
                return true;
            case 203:
                statSystem.TempMultiplyAttack(item.duration,item.changeStatAmount);
                return true;
            case 204: //����
                return true;
            case 205:
                statSystem.TempMultiplyAttackInterval(itemID, item.changeStatAmount);
                return true;
            case 206:
                //���� �ǵ�� -> ����ȹ�������(105) �ν��Ͻ�
                return true;
        }

        return false;
    }
    #endregion

    #region ���������� ���� �����Լ���
    private void SetFeedbackDuration<T>(MMF_Player feedback, float duration) where T : MMF_Feedback
    {
        //Ư�� �ð� ����, �ǵ�� ���
        MMF_Feedback pause = feedback.GetFeedbackOfType<T>(MMF_Player.AccessMethods.First, 0);
        pause.FeedbackDuration = duration;
        itemUsingCoolDownFeedback.PlayFeedbacks();
    }
    private Item FindItemByID(int itemID)
    {
        foreach (var i in ItemDataList)
        {
            if (i.item.itemID == itemID)
                return i.item;
        }
        return null;
    }
    private Sprite FindImageByID(int itemID)
    {
        foreach (var i in ItemDataList)
        {
            if (i.item.itemID == itemID)
                return i.item.image;
        }
        return null;
    }

    private MMF_Player FindFeedbackByID(int itemID)
    {
        foreach (var i in ItemDataList)
        {
            if (i.item.itemID == itemID)
                return i.feedback;
        }
        return null;
    }
    #endregion
}
