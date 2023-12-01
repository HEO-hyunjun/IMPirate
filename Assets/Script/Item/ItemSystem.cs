using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSystem : MonoBehaviour
{
    #region 아이템관련 클래스
    [System.Serializable]
    public class ItemSlot
    {
        [MMFReadOnly]
        public Item item;
        [Tooltip("아이템이 채워질 UI 1번 2번")]
        public Image EmptyImage;
        [Tooltip("사용중일때 Image에 적용할 피드백")]
        public MMF_Player usingItemFeedback; //깜빡거리는거 + 커졌다작아졌다
    }
    [System.Serializable]
    public class ItemDataSource
    {
        public Item item;
        [Tooltip("아이템을 사용했을때 사용자측에서 실행될 피드백")]
        public MMF_Player feedback; // 획득아이템이면 주변에 그냥 색깔별 오오라, 사용아이템이라면 아이템 종류별로 공통된 피드백 + 설명
    }
    #endregion

    [Tooltip("최대 2개의 슬롯만 있다고 가정함")]
    public List<ItemSlot> itemSlot;
    [Tooltip("아이템 사용중일때 itemSlot에 적용할 피드백")]
    public MMF_Player itemUsingCoolDownFeedback; // 아이템슬롯쪽에 회색 투명한 오버레이씌우기
    [Tooltip("아이템을 획득했을때 재생할 피드백")]
    public MMF_Player itemGetFeedback; // 플레이어화면 캔버스에 띄울 UI에 설명, 소리재생, 파티클재생

    [Tooltip("플레이어측에서 재생할 피드백을 아이템 종류별로 담아두는 데이터베이스")]
    public List<ItemDataSource> ItemDataList;

    private PlayerStatSystem statSystem;

    #region 아이템 획득 ~ 사용까지의 함수들
    /// <summary>
    /// 플레이어가 아이템을 먹었을때 시작되는 함수
    /// </summary>
    /// <param name="itemID"></param>
    public void GetItem(int itemID)
    {
        //아이템 카테고리 1000자리수가
        // 1: 획득아이템
        // 2: 사용아이템
        int itemCategory = itemID / 1000;

        switch(itemCategory)
        {
            case 1: // 획득 아이템 -> 바로 효과적용
                UseItem(itemID);
                break;
            case 2: // 사용 아이템 -> 아이템슬롯에 저장
                PushItem(itemID);
                break;
        }
    }

    /// <summary>
    /// 아이템슬롯에 아이템을 밀어넣는 함수
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
        //아이템 슬롯이 꽉 찼다면 무시
        if(findIDX == - 1)
            return;

        itemGetFeedback.PlayFeedbacks();

        itemSlot[findIDX].item = FindItemByID(itemID);
        itemSlot[findIDX].EmptyImage.sprite = FindImageByID(itemID);

    }
    

    /// <summary>
    /// 특정인덱스의 아이템슬롯 아이템을 사용하는 함수
    /// </summary>
    /// <param name="index"></param>
    public void UseItemSlot(int index)
    {
        //아이템슬롯에서 아이템 아이디를 가져와서 해당 함수 사용
        if (itemSlot[index].item.itemID == 0)
            return;
        if (!UseItem(itemSlot[index].item.itemID))
            return;
        SetFeedbackDuration<MMF_ScaleShake>(itemSlot[index].usingItemFeedback, itemSlot[index].item.duration);
        SetFeedbackDuration<MMF_Pause>(itemUsingCoolDownFeedback, itemSlot[index].item.duration);
    }

    /// <summary>
    /// 아이템 아이디에따라서 해당하는 조건 검사후 해당 함수 사용
    /// 아이템별로 함수는 따로 사용
    /// 아이템 사용이 무시됐다면 False 반환
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
                // 획득아이템
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
        if (statSystem.isUsingItem != 0) // 다른 아이템 사용중
            return false;
        statSystem.score += 5;
        statSystem.TempModifyItemUsing(item.duration, item.itemID);
        switch (itemID)
        {
            // 사용아이템
            case 201:
                statSystem.TempModifyRelativeSpeed(item.duration,(int)item.changeStatAmount);
                return true;
            case 202:
                statSystem.TempHPAdd(item.duration, item.changeStatAmount);
                return true;
            case 203:
                statSystem.TempMultiplyAttack(item.duration,item.changeStatAmount);
                return true;
            case 204: //무적
                return true;
            case 205:
                statSystem.TempMultiplyAttackInterval(itemID, item.changeStatAmount);
                return true;
            case 206:
                //지뢰 피드백 -> 지뢰획득아이템(105) 인스턴스
                return true;
        }

        return false;
    }
    #endregion

    #region 로직구현을 위한 보조함수들
    private void SetFeedbackDuration<T>(MMF_Player feedback, float duration) where T : MMF_Feedback
    {
        //특정 시간 동안, 피드백 재생
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
