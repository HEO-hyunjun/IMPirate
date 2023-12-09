using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float right;
    float left;
    float front;

    public GameObject player;
    private PlayerStatSystem stat;
    public AnimationCode anime_a ;

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
    }

    private void Awake()
    {
        Initialize();
    }
    private void FixedUpdate()
    {
        if (!stat.isControlable)
            return;

        left = Input.GetKey(KeyCode.LeftArrow)? 1 : 0; // left = anime_a.state
        right = Input.GetKey(KeyCode.RightArrow) ? 1 : 0; 
        front = Mathf.Max(left,right);
        rb.AddForce(transform.rotation * new Vector3(0,0,front* stat.playerSpeed.Accel));
        for(int i = 2; i < 4; i++)
        {
            wheels[i].motorTorque = stat.playerSpeed.Torque * front;
        }

        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = (left-right) * stat.playerSpeed.Rot;
        }
    }
}
