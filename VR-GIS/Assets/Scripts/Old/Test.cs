using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] Vector3 rightPos, leftPos;
    [SerializeField] Transform rightTrfm, leftTrfm;
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rightPos = rightTrfm.position;
        leftPos = leftTrfm.position;
        distance = Vector3.Distance(rightTrfm.position, leftTrfm.position);
    }
}
