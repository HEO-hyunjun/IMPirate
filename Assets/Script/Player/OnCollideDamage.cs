using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideDamage : MonoBehaviour
{
    public PlayerStatSystem ownerStatSystem;
    private PlayerStatSystem targetStatSystem;

    public MMF_Player whenStart;
    public MMF_Player whenCollide;
    public float damage = 10f;
    private void Awake()
    {
        whenStart.PlayFeedbacks();
    }
    private void OnTriggerEnter(Collider other)
    {
        targetStatSystem = other.gameObject.GetComponent<PlayerStatSystem>();
        whenCollide.PlayFeedbacks();
        if (targetStatSystem == null)
            return;
        if (targetStatSystem.PlayerID == ownerStatSystem.PlayerID)
            return;

        targetStatSystem.Damage(damage);
    }
}
