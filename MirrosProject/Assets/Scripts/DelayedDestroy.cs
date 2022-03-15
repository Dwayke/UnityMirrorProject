using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [SerializeField] private float delay = 1f;

    private float destroyTime = -1f;

    private void Awake()
    {
        destroyTime = Time.time + delay;
    }
    private void Update()
    {
        if (destroyTime != -1 && Time.time>= destroyTime)
        {
            destroyTime = -1f;
            Destroy(gameObject);
        }
    }
}
