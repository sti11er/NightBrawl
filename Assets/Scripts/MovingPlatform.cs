using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //public GameObject Platform;
    public float speed = 0.001f;

    void Update() 
    {

        Debug.Log(Time.deltaTime);
        transform.position = new Vector2(transform.position.x + speed, transform.position.y);
    }
}
