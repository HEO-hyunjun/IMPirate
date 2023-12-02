using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField]
    public Item item;

    [SerializeField]
    [MMFReadOnly]
    private ItemSystem targetItemSystem;
    private PlayerStatSystem targetStatSystem;
    [Tooltip("�������� ȹ�������� ������������ ����� �ǵ��")]
    public MMF_Player whenCollide;
    public string installer = "developer";

    private void OnTriggerEnter(Collider other)
    {
        targetItemSystem = other.GetComponent<ItemSystem>();
        targetStatSystem = other.GetComponent<PlayerStatSystem>();

        if (targetItemSystem == null || targetStatSystem == null)
            return;
        if (targetStatSystem.PlayerID == installer)
            return;

        targetItemSystem.GetItem(item.itemID);
        whenCollide.PlayFeedbacks();
    }
}
