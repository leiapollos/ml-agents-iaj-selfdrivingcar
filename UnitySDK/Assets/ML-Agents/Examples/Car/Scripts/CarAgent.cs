using MLAgents;
using UnityEngine;

public class CarAgent : Agent
{
    public TrackManager track;
    public CarController ctrl;
    public float raycastDistance = 10;
    public Transform[] raycasts;

    public float accel;
    public float steering;

    Vector3 startingPos;
    Quaternion startingRot;

    public float rewardOnCheckpoint = 1;

    float time;

    private void Awake()
    {
        track = FindObjectOfType<TrackManager>();
        ctrl = GetComponent<CarController>();
        startingPos = transform.position;
        startingRot = transform.rotation;
        time = Time.time;
    }

    public override void AgentReset()
    {
        ctrl.transform.position = startingPos;
        ctrl.transform.rotation = startingRot;
        track.Reset();
        ctrl.Reset();
        time = Time.time;
    }

    public void OnReachCheckpoint()
    {
        AddReward(rewardOnCheckpoint);
    }

    public void OnReachFinal()
    {
        float fTime = Time.time-time;
        fTime = 1 / fTime;
        Debug.Log("hh"+fTime);
        AddReward(fTime*1000);
    }

    public override void AgentAction(float[] vectorAction)
    {
        accel = vectorAction[0];
        if (accel <= 0.0f)
            accel = -0.025f;
        else
        {
            accel = Mathf.Clamp(accel, 0, 1);
            if (accel > 0) accel = 1;
        }
        steering = vectorAction[1];

        AddReward(ctrl.LocalSpeed * .001f);

        ctrl.UpdateControls(accel, steering);
    }

    public override void CollectObservations()
    {
        AddVectorObs(ctrl.LocalSpeed);

        for (int i = 0; i < raycasts.Length; ++i)
        {
            AddRaycastVectorObs(raycasts[i]);
        }
    }

    void AddRaycastVectorObs(Transform ray)
    {
        RaycastHit hitInfo = new RaycastHit();
        var hit = Physics.Raycast(ray.position, ray.forward, out hitInfo, raycastDistance);
        var distance = hitInfo.distance;
        if (!hit) distance = raycastDistance;
        var obs = distance / raycastDistance;
        AddVectorObs(obs);

        if (distance < 1f)
        {
            //AddReward(-1.0f);
            Done();
            AgentReset();
        } 

        Debug.DrawRay(ray.position, ray.forward * distance, Color.Lerp(Color.red, Color.green, obs), Time.deltaTime * 2f);
    }

    public override float[] Heuristic()
    {
        float[] input = { Input.GetAxis("Vertical"), Input.GetAxis("Horizontal") };
        return input;
    }
}
