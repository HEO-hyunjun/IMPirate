using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using MoreMountains;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    public MMF_Player leftFeedback;
    public MMF_Player rightFeedback;
    public MMF_Player forwardFeedback;
    [Space(10)]
    public MMF_Player noFeedback;
    public MMF_Player okFeedback;
    public MMF_Player useItemFeedback;
    public MMF_Player getItemFeedback;
    public MMF_Player hitFeedback;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TriggerFront()
    {
        forwardFeedback?.PlayFeedbacks();
        animator.SetTrigger("pointing");
    }
    public void TriggerLeft()
    {
        leftFeedback?.PlayFeedbacks();
        animator.SetTrigger("pointing");
    }
    public void TriggerRight()
    {
        rightFeedback?.PlayFeedbacks();
        animator.SetTrigger("pointing");
    }

    public void TriggerHit()
    {
        hitFeedback?.PlayFeedbacks();
        animator.SetTrigger("hit");
    }

    public void TriggerOk()
    {
        okFeedback?.PlayFeedbacks();
        animator.SetTrigger("yes");
    }

    public void TriggerNo()
    {
        noFeedback?.PlayFeedbacks();
        animator.SetTrigger("no");
    }

    public void TriggerUseItem()
    {
        useItemFeedback?.PlayFeedbacks();
        animator.SetTrigger("useItem");
    }
    
    public void TriggerGetItem()
    {
        getItemFeedback?.PlayFeedbacks();
        animator.SetTrigger("victory");
    }

    public void TriggerVictory()
    {
        getItemFeedback?.PlayFeedbacks();
        animator.SetTrigger("victory");
    }


}
