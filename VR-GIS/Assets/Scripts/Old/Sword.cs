using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] Transform grabTrfm, trfm, zoneTrfm;
    [SerializeField] float leeway, returnRate, zoneHeight, successDrawHeight;

    [SerializeField] int mode;
    public const int RETURNING = 0, TRACKING = 1, SHEATHED = 2, DRAWN = 3;

    [SerializeField] GameMode gameMode;

    [SerializeField] ParticleSystem swordEffects;

    bool firstGripped;

    Vector3 trackVect3, initVect3; //caching vector3s to avoid using 'new' keyword

    enum GameMode { TRACKING, TIMING };

    private void Start()
    {
        trackVect3 = trfm.position;
        initVect3 = trfm.position;
    }

    private void FixedUpdate()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(grabTrfm.position.x - trfm.position.x, 2) + Mathf.Pow(grabTrfm.position.z - trfm.position.z, 2));

        if (mode == TRACKING)
        {
            if (!firstGripped) { firstGripped = true; }

            trackVect3.y = grabTrfm.position.y;
            trfm.position = trackVect3;

            if (distance > leeway)
            {
                mode = RETURNING;
            }

            if (gameMode == GameMode.TRACKING && (Mathf.Abs(grabTrfm.position.y - zoneTrfm.position.y) > zoneHeight && trfm.position.y > initVect3.y + .1f))
            {
                mode = RETURNING;
            }

            if (distance < .001f)
            {
                mode = RETURNING;
            }

            if (trfm.position.y > successDrawHeight - .05f)
            {
                mode = DRAWN;
                if (gameMode == GameMode.TRACKING)
                {
                    swordEffects.Play();
                }
            }
        }
        else if (mode == RETURNING)
        {
            if (trfm.position.y > initVect3.y)
            {
                trfm.position -= Vector3.up * returnRate;
            }
            else if (distance < leeway && Mathf.Abs(trfm.position.y - grabTrfm.position.y) < .01f)
            {
                mode = SHEATHED;
            }
        }
        else if (mode == SHEATHED)
        {
            if (distance < leeway && Mathf.Abs(trfm.position.y - grabTrfm.position.y) > .01f)
            {
                mode = TRACKING;
            }
        }

        if (mode == DRAWN)
        {
            trfm.Rotate(Vector3.up * 2);
        }
        else
        {
            grabTrfm.position = trfm.position;
            grabTrfm.rotation = Quaternion.identity;
        }
    }

    public bool IsGripped()
    {
        return mode == TRACKING;
    }

    public bool IsDrawn()
    {
        return mode == DRAWN;
    }

    public bool FirstGripped()
    {
        return firstGripped;
    }

    public int GetMode()
    {
        return mode;
    }
}
