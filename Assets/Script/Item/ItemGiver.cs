using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    [SerializeField]
    [MMFReadOnly]
    private Item item;

    [SerializeField]
    [MMFReadOnly]
    private ItemSystem target;
    [Tooltip("�������� ȹ�������� ������������ ����� �ǵ��")]
    public MMF_Player whenCollide;
    // Start is called before the first frame update
    void Start()
    {
        item = GetComponent<Item>();
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.GetComponent<ItemSystem>();
        if (target == null)
            return;
        whenCollide.PlayFeedbacks();
        target.GetItem(item.itemID);
    }
}
