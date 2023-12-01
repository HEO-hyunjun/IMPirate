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
    [Tooltip("�������� ȹ�������� ������������ ����� �ǵ��")]
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
