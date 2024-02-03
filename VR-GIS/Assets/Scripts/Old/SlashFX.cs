using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashFX : MonoBehaviour
{
    [SerializeField] Vector3 initialScale, changeScale;
    static Vector3 vect3;

    [SerializeField] Transform trfm;

    private void Start()
    {
        trfm.localScale = initialScale;
        vect3.z = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vect3.x = trfm.localScale.x + changeScale.x;
        vect3.y = trfm.localScale.y * changeScale.y;
        trfm.localScale = vect3;

        if (trfm.localScale.x < .01f)
        {
            Destroy(gameObject);
        }
    }
}
