using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

[RequireComponent(typeof(PlayerStatSystem))]
public class ItemSystem : MonoBehaviour
{
    #region �����۰��� Ŭ����
    [System.Serializable]
    public class ItemSlot
    {
        [MMFHidden]
        public bool isFull = false;
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
    [Tooltip("�������� ȹ�������� ����� ��ƼŬ �ǵ��")]
    public MMF_Player itemGetFeedback; // �÷��̾�ȭ�� ĵ������ ��� UI�� ����
    [Tooltip("������ ����UI ����� �ǵ��")]
    public MMF_Player itemUIFeedback; // �÷��̾�ȭ�� ĵ������ ��� UI�� ����
    public Image itemDescriptImage;


    [Tooltip("�÷��̾������� ����� �ǵ���� ������ �������� ��Ƶδ� �����ͺ��̽�")]
    public List<ItemDataSource> ItemDataList;
    public GameObject minePrefab;
    private PlayerStatSystem statSystem;

    private void Awake()
    {
        statSystem = GetComponent<PlayerStatSystem>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            UseItemSlot(0);
        if (Input.GetKeyDown(KeyCode.W))
            UseItemSlot(1);
    }

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
        itemGetFeedback.PlayFeedbacks();

        SetDescriptText(itemUIFeedback, FindItemByID(itemID), true);

        switch (itemCategory)
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
        for (int i = 0; i < 2; i++)
        {
            if (!itemSlot[i].isFull)
            {
                findIDX = i;
                break;
            }
        }
        //������ ������ �� á�ٸ� ����
        if (findIDX == -1)
            return;
        itemSlot[findIDX].isFull = true;
        itemUIFeedback?.PlayFeedbacks();
        itemSlot[findIDX].item = FindItemByID(itemID);
        itemSlot[findIDX].EmptyImage.sprite = FindImageByID(itemID);
        itemSlot[findIDX].EmptyImage.color = new Color(255, 255, 255, 255);
    }


    /// <summary>
    /// Ư���ε����� �����۽��� �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="index"></param>
    public void UseItemSlot(int index)
    {
        //�����۽��Կ��� ������ ���̵� �����ͼ� �ش� �Լ� ���
        if (itemSlot[index].item == null)
            return;

        if (!UseItem(itemSlot[index].item.itemID))
            return;

        SetDescriptText(itemUIFeedback, itemSlot[index].item, false);
        itemUIFeedback.PlayFeedbacks();

        SetLooperDuration(itemSlot[index].usingItemFeedback, itemSlot[index].item.duration); ;
        itemSlot[index].usingItemFeedback.PlayFeedbacks();

        TempUsingSlot(itemSlot[index].item.duration, index);
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
        if (item == null)
            return false;

        itemID /= 10;

        if (itemID / 100 == 1)
        {
            itemUIFeedback.PlayFeedbacks();
            FindFeedbackByID(item.itemID)?.PlayFeedbacks();
            switch (itemID)
            {
                // ȹ�������
                case 101:
                    statSystem.RemainBullet += (int)item.changeStatAmount;
                    return true;
                case 102:
                    statSystem.Score += (int)item.changeStatAmount;
                    return true;
                case 103:
                    statSystem.addSpeedLevel((int)item.changeStatAmount);
                    return true;
                case 104:
                    if ((statSystem.isUsingItem / 10) == 203) //�����۹����޴����̶��,
                        statSystem.tmpAttack += item.changeStatAmount;
                    statSystem.Attack += item.changeStatAmount;
                    return true;
                case 105:
                    statSystem.Damage(item.changeStatAmount);
                    return true;
            }
        }
        if (statSystem.isUsingItem != 0) // �ٸ� ������ �����
            return false;
        statSystem.Score += 5;
        statSystem.TempModifyItemUsing(item.duration, item.itemID);

        SetHoldPauseDuration(itemUsingCoolDownFeedback, item.duration);
        itemUsingCoolDownFeedback.PlayFeedbacks();

        MMF_Player tmp = FindFeedbackByID(item.itemID);
        if (tmp != null)
        {
            SetHoldPauseDuration(tmp, item.duration);
            tmp.PlayFeedbacks();
        }

        switch (itemID)
        {
            // ��������
            case 201:
                statSystem.TempModifyRelativeSpeed(item.duration, (int)item.changeStatAmount);
                return true;
            case 202:
                statSystem.TempHPAdd(item.duration, item.changeStatAmount);
                return true;
            case 203:
                statSystem.TempMultiplyAttack(item.duration, item.changeStatAmount);
                return true;
            case 204: //����
                return true;
            case 205:
                statSystem.TempMultiplyAttackInterval(itemID, item.changeStatAmount);
                return true;
            case 206:
                //����ȹ�������(105) ��ġ
                GameObject mine = Instantiate(minePrefab);
                mine.transform.position = transform.position;
                ItemGiver mineItemGiver = mine.GetComponent<ItemGiver>();
                mineItemGiver.installer = statSystem.PlayerID;
                return true;
        }

        return false;
    }
    #endregion

    #region ���������� ���� �����Լ���
    private void SetPauseDuration(MMF_Player feedback, float duration)
    {
        MMF_Pause pause = feedback.GetFeedbackOfType<MMF_Pause>(MMF_Player.AccessMethods.First, 0);
        pause.FeedbackDuration = duration;
    }

    private void SetHoldPauseDuration(MMF_Player feedback, float duration)
    {
        MMF_HoldingPause pause = feedback.GetFeedbackOfType<MMF_HoldingPause>(MMF_Player.AccessMethods.First, 0);
        pause.FeedbackDuration = duration;
    }

    private void SetLooperDuration(MMF_Player feedback, float duration)
    {
        MMF_Looper looper = feedback.GetFeedbackOfType<MMF_Looper>(MMF_Player.AccessMethods.First, 0);
        looper.NumberOfLoops = (int)(duration * 2);
    }

    private void SetDescriptText(MMF_Player feedback, Item item, bool isGet)
    {
        itemDescriptImage.sprite = item.image;

        List<MMF_TMPText> texts = feedback.GetFeedbacksOfType<MMF_TMPText>();
        if (isGet)
            texts[0].NewText = "ȹ��: " + item.itemName;
        else
            texts[0].NewText = "���: " + item.itemName;

        texts[1].NewText = item.Descript;
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

    private void TempUsingSlot(float t, int index)
    {
        if (!itemSlot[index].isFull)
            return;
        StartCoroutine(CorTempUsingSlot(t, index));
    }
    private IEnumerator CorTempUsingSlot(float t, int index)
    {
        yield return new WaitForSeconds(t);
        itemSlot[index].isFull = false;
    }
    #endregion
}
