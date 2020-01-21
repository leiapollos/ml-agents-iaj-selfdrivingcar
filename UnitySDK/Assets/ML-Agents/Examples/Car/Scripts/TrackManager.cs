using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Tooltip("All the checkpoints for the track.")]
    public List<NewCheckpoint> checkpoints = new List<NewCheckpoint>();
    public List<Final> final = new List<Final>();

    public void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Select(x => x.GetComponent<NewCheckpoint>()).ToList();
        final = GameObject.FindGameObjectsWithTag("Final").Select(x => x.GetComponent<Final>()).ToList();
    }

    public void Reset()
    {
        for (int i = 0; i < checkpoints.Count; ++i)
        {
            checkpoints[i].Reset();
        }
        for(int i = 0; i < final.Count; i++)
        {
            final[i].Reset();
        }
    }

    public void FixedUpdate()
    {
        bool active = false;
        foreach (NewCheckpoint c in checkpoints){
            if(c.enabled == true)
            {
                active = true;
            }
        }
        if (!active)
        {
            Reset();
        }
    }
}
