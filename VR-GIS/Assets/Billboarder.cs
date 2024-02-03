using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarder : MonoBehaviour
{
    [SerializeField] Transform cameraTrfm;
    protected Transform trfm;
    // Start is called before the first frame update
    protected void Start()
    {
        trfm = transform;
    }

    protected void Update()
    {
        trfm.forward = cameraTrfm.position - trfm.position;
    }
}
