using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float accel = 1000f;
    public float torque = 400f;
    public float rot = 45;
    float right;
    float left;
    float front;

    Rigidbody rb;
    // ¹ÙÄû
    [Tooltip("Â÷·Ê´ë·Î FL,FR,BL,BR")]
    public WheelCollider[] wheels = new WheelCollider[4];

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        left = Input.GetKey(KeyCode.LeftArrow)? 1 : 0;
        right = Input.GetKey(KeyCode.RightArrow) ? 1 : 0; 
        front = Mathf.Max(left,right);
        rb.AddForce(transform.rotation * new Vector3(0,0,front* accel));
        for(int i = 2; i < 4; i++)
        {
            wheels[i].motorTorque = torque * front;
        }

        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = (left-right) * rot;
        }
    }
}
