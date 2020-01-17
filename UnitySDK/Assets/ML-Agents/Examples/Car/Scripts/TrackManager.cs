using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Tooltip("All the checkpoints for the track.")]
    public List<NewCheckpoint> checkpoints = new List<NewCheckpoint>();

    public void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Select(x => x.GetComponent<NewCheckpoint>()).ToList();
    }

    public void Reset()
    {
        for (int i = 0; i < checkpoints.Count; ++i)
        {
            checkpoints[i].Reset();
        }
    }
}
