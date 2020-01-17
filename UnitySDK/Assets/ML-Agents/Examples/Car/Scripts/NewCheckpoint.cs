using System;
using UnityEngine;
using UnityEngine.Events;

public class NewCheckpoint : MonoBehaviour
{
    [System.Serializable]
    public class CheckpointEvent : UnityEvent
    {

    }

    [SerializeField]
    public CheckpointEvent reachedCheckpoint;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.transform.parent.parent.tag == "Car")
        {
            collider.gameObject.transform.parent.parent.GetComponent<CarAgent>().OnReachCheckpoint();
        }

        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void Reset()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
