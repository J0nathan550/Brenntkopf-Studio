using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectForce : MonoBehaviour
{
    public Rigidbody body;
    void Start()
    {
        body.AddForce(Vector3.up * Random.Range(15f,40f), ForceMode.Impulse);
    }
}
