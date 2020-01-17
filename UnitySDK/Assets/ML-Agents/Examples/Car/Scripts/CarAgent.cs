using MLAgents;
using UnityEngine;

public class CarAgent : Agent
{
    public TrackManager track;
    public CarController ctrl;
    public float raycastDistance = 4;
    public Transform[] raycasts;

    public float accel;
    public float steering;

    Vector3 startingPos;
    Quaternion startingRot;

    public float rewardOnCheckpoint = 1;

    private void Awake()
    {
        track = FindObjectOfType<TrackManager>();
        ctrl = GetComponent<CarController>();
        startingPos = transform.position;
        startingRot = transform.rotation;
    }

    public override void AgentReset()
    {
        ctrl.transform.position = startingPos;
        ctrl.transform.rotation = startingRot;
        track.Reset();
        ctrl.Reset();
    }

    public void OnReachCheckpoint()
    {
        AddReward(rewardOnCheckpoint);
    }

    public override void AgentAction(float[] vectorAction)
    {
        accel = vectorAction[0];
        accel = Mathf.Clamp(accel, 0, 1);
        if (accel > 0) accel = 1;
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
