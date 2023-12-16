using Mirror;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerStatSystem))]

public class Attack : NetworkBehaviour
{
    public PlayerStatSystem Player;
    public GameObject AttackObject;
    private GameObject attack;
    private Rigidbody attackRb;
    public float attackPower = 100;
    public Transform AttackSpawnPosition;
    public MMF_Player feedback;


    
    [SerializeField]
    [MMFReadOnly]
    private float deltaTime;

    private void Awake()
    {
        if (Player == null)
            Player = GetComponent<PlayerStatSystem>();
        if (AttackSpawnPosition == null)
            AttackSpawnPosition = transform;
        deltaTime = 0;
        StartCoroutine(CalcTime());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            doAttack();
        if (InputManager.instance != null && InputManager.instance.isPoseDetect)
        {
            if (InputManager.instance.isOK)
                doAttack();
        }
    }

    private IEnumerator CalcTime()
    {
        while (true)
        {
            deltaTime += Time.deltaTime;
            yield return null;
            if (!Player.isAttackable && deltaTime > Player.attackInterval)
            {
                deltaTime = 0;
                Player.isAttackable = true;
            }
        }
    }
    [Command]
    private void doAttack()
    {
        if (!Player.isAttackable || !Player.isControlable || Player.RemainBullet == 0)
            return;
        if (Player.animatorController != null)
            Player.animatorController.TriggerOk();
        feedback.PlayFeedbacks();
        Player.uiSystem.playAttackUIFeedback();

        attack = Instantiate(AttackObject);
        attack.transform.position = AttackSpawnPosition.position;
        attack.transform.forward = transform.forward;

        attackRb = attack.GetComponent<Rigidbody>();
        attackRb.AddForce((attack.transform.forward + new Vector3(0, 1 * attack.transform.forward.y, 0)) * attackPower);
        NetworkServer.Spawn(attack);
        Player.RemainBullet--;

        OnCollideDamage onCollideDamage = attack.GetComponent<OnCollideDamage>();
        onCollideDamage.ownerStatSystem = Player;
        onCollideDamage.damage = Player.Attack;
        deltaTime = 0;
        Player.isAttackable = false;
    }
}
