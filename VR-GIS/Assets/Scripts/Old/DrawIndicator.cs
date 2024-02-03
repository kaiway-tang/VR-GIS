using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawIndicator : MonoBehaviour
{
    [SerializeField] Transform zoneTrfm;
    [SerializeField] float zoneRiseRate, successDrawHeight;
    [SerializeField] Sword swordScript;
    [SerializeField] TextManager textManager;

    int zoneTimer, attemptNo = 0;

    bool zoneRising, tierTextSet;

    private void Start()
    {
        zoneTimer = 999;
    }

    private void FixedUpdate()
    {
        if (zoneTimer > 0)
        {
            if (zoneTimer > 300 && swordScript.FirstGripped())
            {
                zoneTimer = zoneTimer = Random.Range(100, 250);
            }
            zoneTimer--;
        }
        else
        {
            zoneTimer = Random.Range(100, 250);
            if (!zoneRising && !swordScript.IsDrawn())
            {
                attemptNo++;
                SpawnDrawZone();
                if (zoneRiseRate > .0099f)
                {
                    zoneRiseRate -= .0025f;
                }
            }
        }

        if (swordScript.IsDrawn() && !tierTextSet)
        {
            switch (attemptNo)
            {
                case 1:
                    textManager.SetText("Tier: Mythic");
                    break;
                case 2:
                    textManager.SetText("Tier: Legendary");
                    break;
                case 3:
                    textManager.SetText("Tier: Epic");
                    break;
                case 4:
                    textManager.SetText("Tier: Rare");
                    break;
                case 5:
                    textManager.SetText("Tier: Uncommon");
                    break;
                case 6:
                    textManager.SetText("Tier: Common");
                    break;
                default:
                    textManager.SetText("Tier: Standard");
                    break;
            }

            tierTextSet = true;
        }

        if (zoneRising)
        {
            if (zoneTrfm.position.y > successDrawHeight)
            {
                zoneRising = false;
                zoneTrfm.position = new Vector3(zoneTrfm.position.x, 0.1f, zoneTrfm.position.z);
            }
            else
            {
                zoneTrfm.position += Vector3.up * zoneRiseRate;
            }
        }
    }

    void SpawnDrawZone()
    {
        zoneRising = true;
        //zoneTrfm.localScale = new Vector3(0.3f,zoneHeight,0.3f);
    }
}
