using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugThrow : MonoBehaviour
{
    public float force = 10;
    public void Throw()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * force);
    }
}
