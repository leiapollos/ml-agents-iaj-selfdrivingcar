using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    public float speed = 10;
    private bool reset = false;
    protected float velocity;
    protected float drag = 0.95f;
    protected float maxspeed = 0.06f;
    protected float acc = 0.002f;
    bool leftCol = false, rightCol = false, frontCol = false, back = false;

    Rigidbody rBody;
    public CheckpointManager checkpointManager;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        velocity = 0.0f;
    }

    public Transform Target;
    public override void AgentReset()
    {
        if (reset)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.rBody.rotation = Quaternion.identity;
            this.transform.position = new Vector3(0, 0.5f, 0);
            velocity = 0.0f;
            // Move the target to a new spot
            checkpointManager.Reset();
            reset = false;
            frontCol = false;
            rightCol = false;
            leftCol = false;
            back = false;
        }

    }

    public override void AgentAction(float[] vectorAction)
    {
        if(vectorAction[1] == 0.0f)
            velocity *= drag;
        if (vectorAction[1] < 0.0f) vectorAction[1] = 0.0f;
        velocity += vectorAction[1]*acc;
        velocity = velocity > maxspeed ? maxspeed : velocity;
        //velocity = velocity < -maxspeed ? -maxspeed : velocity;
//         if (vectorAction[0] < 0.0f)
//         {
//             if (!back)
//             {
//                 AddReward(-0.01f);
//                 back = true;
//             }
//         }
        else back = false;
        rBody.position += velocity* rBody.transform.forward;
        rBody.rotation *= Quaternion.AngleAxis(vectorAction[0], Vector3.up);

        // Fell off platform
        if (this.transform.position.y < 0)
        {
            reset = true;
            Done();
        }
        

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Target"))
        {
            AddReward(10.0f);
            checkpointManager.Next();
        }

        if (col.gameObject.CompareTag("Walls"))
        {
            reset = true;
            Done();
        }
    }

    void FixedUpdate()
    {

        //Raycast walls
        int size = 3;
        float angle = 20.0f;
        int layerMask = 1 << 14;
        Vector3 left = Quaternion.AngleAxis(-angle, Vector3.up) * this.transform.forward;
        Vector3 right = Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;
        if (Physics.Raycast(this.transform.position, left, size, layerMask))
        {
            if (!leftCol)
                AddReward(-0.01f);
            leftCol = true;
        }
        else leftCol = false;
        if (Physics.Raycast(this.transform.position, right, size, layerMask))
        {
            if (!rightCol)
                AddReward(-0.01f);
            rightCol = true;
        }
        else rightCol = false;
        if (Physics.Raycast(this.transform.position, this.transform.forward, size, layerMask))
        {
            if (!frontCol)
                AddReward(-0.5f); //Idea to try to make it turn "faster"
            frontCol = true;
        }
        else frontCol = false;
        Debug.DrawRay(this.transform.position, left * size, Color.green);
        Debug.DrawRay(this.transform.position, right * size, Color.green);
        Debug.DrawRay(this.transform.position, this.transform.forward * size, Color.green);
    }


    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(checkpointManager.GetTargetPos());
        AddVectorObs(this.transform.position);

        // Agent velocity
        Vector3 v = velocity * rBody.transform.forward;
        AddVectorObs(v.x);
        AddVectorObs(v.z);
    }
}