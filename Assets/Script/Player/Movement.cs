using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public float right;
    public float rightTimer;
    public float left;
    public float leftTimer;
    public float front;
    public float back;
    public float frontTimer;
    public bool keepGoing = false;

    public GameObject player;
    public float dirInterval = 0.5f;
    private PlayerStatSystem stat;

    Rigidbody rb;
    // 바퀴리스트
    [Tooltip("순서대로 FL,FR,BL,BR")]
    public WheelCollider[] wheels = new WheelCollider[4];

    private void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, 0, 0);
        if (player == null)
            return;
        stat = player.GetComponent<PlayerStatSystem>();
        frontTimer = dirInterval;
        leftTimer = dirInterval;
        rightTimer = dirInterval;
    }

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (!stat.isControlable || stat.isDead)
            return;

        left = Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
        right = Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        back = Input.GetKey(KeyCode.DownArrow) ? 2 : 0;

        if (InputManager.instance != null && InputManager.instance.isPoseDetect)
        {
            left = InputManager.instance.leftArrowVal;
            right = InputManager.instance.rightArrowVal;
            back = InputManager.instance.isCancle ? 2: 0;
        }

        front = Mathf.Max(left, right);

        if(stat.animatorController != null)
        {
            float gap = Mathf.Abs(left - right);
            if (frontTimer <= 0 && front != 0 && gap < 0.2f && !keepGoing)
            { 
                stat.animatorController.TriggerFront();
                frontTimer = dirInterval;
                leftTimer = dirInterval;
                rightTimer = dirInterval;
                keepGoing = true;
            }
            if (leftTimer <= 0 && left > right + 0.3f && !keepGoing)
            { 
                stat.animatorController.TriggerLeft();
                frontTimer = dirInterval;
                leftTimer = dirInterval;
                rightTimer = dirInterval;
                keepGoing = true;
            }
            if(rightTimer <= 0 && right > left + 0.3f && !keepGoing)
            {
                stat.animatorController.TriggerRight();
                frontTimer = dirInterval;
                leftTimer = dirInterval;
                rightTimer = dirInterval;
                keepGoing = true;
            }

            if (back != 0 && !keepGoing)
            { 
                stat.animatorController.TriggerNo();
                keepGoing = true;
            }

            if(back != 0)
            {
                stat.isAttackable = false;
            }

            if (gap < 0.4f)
                frontTimer -= Time.deltaTime;
            if (left > 0.5f && gap > 0.4f)
                leftTimer -= Time.deltaTime;
            if (right > 0.5f && gap > 0.4f)
                rightTimer -= Time.deltaTime;

            if(left < 0.5f && right < 0.5f && gap < 0.4f)
            {
                frontTimer = dirInterval;
                leftTimer = dirInterval;
                rightTimer = dirInterval;
            }
            if(keepGoing && frontTimer + leftTimer + rightTimer  == dirInterval * 3 && back == 0)
                keepGoing = false;
        }

        rb.AddForce(transform.rotation * new Vector3(0, 0, front * stat.playerSpeed.Accel));
        for (int i = 2; i < 4; i++)
        {
            wheels[i].motorTorque = stat.playerSpeed.Torque * (front - back);
        }

        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = (left - right) * stat.playerSpeed.Rot;
        }
    }
}
