using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float moveSpeed = 3f; // units per second
    public float turnSpeed = 30f; // degreees per second

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.right * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, Vector3.up);
    }
}
