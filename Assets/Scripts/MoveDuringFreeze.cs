using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDuringFreeze : MonoBehaviour
{
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }
}
