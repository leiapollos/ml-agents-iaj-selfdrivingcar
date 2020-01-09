using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    protected int current = 0;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    public void Next()
    {
        checkpoints[current].Destroy();
        current++;
        if (current > 2)
            Debug.Log("SIIIIMMMM CRL");
        if (current <= checkpoints.Count-1)
        {
            checkpoints[current].Spawn();
        }
    }

    public void Reset()
    {
        foreach(Checkpoint c in checkpoints)
        {
            c.Destroy();
        }
        checkpoints[0].Spawn();
        current = 0;
    }

    public Vector3 GetTargetPos()
    {
        return checkpoints[current].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
