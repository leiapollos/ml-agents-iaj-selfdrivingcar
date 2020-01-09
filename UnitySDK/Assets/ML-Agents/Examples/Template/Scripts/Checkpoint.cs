using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool verticle = true;
    Collider m_Collider;
    Vector3 m_Center;
    public Transform prefab;
    protected Transform obj;
    public bool visible = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<Collider>();
        m_Center = m_Collider.bounds.center;
    }

    public void Spawn()
    {
        Vector3 h = new Vector3(0,0.5f,0);
        if(verticle)
            obj = Instantiate(prefab, m_Center+h, Quaternion.identity);
        else
            obj = Instantiate(prefab, m_Center+h, Quaternion.AngleAxis(90.0f, Vector3.up));
        visible = true;
    }

    public void Destroy()
    {
        if(obj!=null)
            Destroy(obj.gameObject);
        visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
