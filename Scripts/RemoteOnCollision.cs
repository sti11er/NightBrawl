using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteOnCollision : MonoBehaviour
{
    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", destroyTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {  
        Destroy(gameObject);
        if (!col.CompareTag("Player"))
            Destroy(col.gameObject);
    }

    void DestroyObject() 
    {
        Destroy(gameObject);
    }
}
