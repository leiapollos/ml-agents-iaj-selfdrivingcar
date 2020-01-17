using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration;

    public float speed;
    public float maxSpeed = 0.06f;

    public float steering;
    public float maxSteering = 45f;
    
    Rigidbody m_Rigidbody;
    public float LocalSpeed => speed / maxSpeed;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void UpdateControls(float accel, float angle)
    {
        acceleration = accel;
        steering = angle;
    }

    void FixedUpdate()
    {
        speed += speed * acceleration;
        speed = speed > maxSpeed ? maxSpeed : speed;

        m_Rigidbody.position += speed * transform.forward;
        m_Rigidbody.rotation *= Quaternion.AngleAxis(steering, Vector3.up);
    }

    public void Reset()
    {
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.velocity = Vector3.zero;
        acceleration = 0;
        steering = 0;
    }
}
