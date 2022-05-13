using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    public bool isUp = true;    
    public float upDistance = 70f;
    public float speed =  .5f;


    private void Update()
    {
        if (transform.position.y >= upDistance && isUp)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            isUp = false;
        }
        else
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }

}
