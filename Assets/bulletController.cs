using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float lifetime = 5;

    // Start is called before the first frame update
    void Start()
    {
        lifetime += Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
