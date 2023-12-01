using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField]
    private Item item;

    [SerializeField]
    [MMFReadOnly]
    private ItemSystem target;
    [Tooltip("아이템을 획득했을때 아이템측에서 실행될 피드백")]
    public MMF_Player whenCollide;

    private void OnTriggerEnter(Collider other)
    {
        target = other.GetComponent<ItemSystem>();
        if (target == null)
            return;
        whenCollide.PlayFeedbacks();
        target.GetItem(item.itemID);
    }
}
