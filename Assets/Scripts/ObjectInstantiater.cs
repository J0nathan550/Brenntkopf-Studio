using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiater : MonoBehaviour
{
    public GameObject[] objects;
    private float time = 0.3f;
    public float TimeSet = 0.3f;
    public GameObject spawnPoint;

    private void Start()
    {
        time = TimeSet;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            int randomNumber = Random.Range(0, objects.Length);
            GameObject go = Instantiate(objects[randomNumber], spawnPoint.transform.position, Quaternion.Euler(Random.Range(20,120f), Random.Range(20, 120f), Random.Range(20, 120f)));
            time = TimeSet;
            Destroy(go,5f);
        }
    }
}
