using System;
using UnityEngine;
using UnityEngine.Events;

public class Final : MonoBehaviour
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
            collider.gameObject.transform.parent.parent.GetComponent<CarAgent>().OnReachFinal();
        }

        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void Reset()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
