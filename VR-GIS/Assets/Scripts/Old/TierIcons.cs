using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierIcons : MonoBehaviour
{
    [SerializeField] Sword swordScript;
    [SerializeField] Transform trfm, selectorTrfm;
    [SerializeField] float hoverHeight, travelRate;
    [SerializeField] int drawnSwordID;
    [SerializeField] Transform[] swordTrfms;
    [SerializeField] ParticleSystem swordEffects;
    [SerializeField] TextManager textManager;
    bool onDrawn;
    // Start is called before the first frame update
    void Start()
    {
        selectorTrfm.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (swordScript.IsGripped())
        {
            if (trfm.position.y < hoverHeight)
            {
                trfm.position += Vector3.up * (hoverHeight - trfm.position.y) * travelRate;
            }
        }
        else
        {
            if (trfm.position.y > -.4f)
            {
                trfm.position += Vector3.up * (-.4f - trfm.position.y) * travelRate;
            }
        }

        if (!swordScript.IsDrawn())
        {
            trfm.Rotate(Vector3.up * 3);
        }
        else if (!onDrawn)
        {
            onDrawn = true;

            int minID = 0;
            float min = Vector3.Distance(swordTrfms[0].position, selectorTrfm.position); //inefficient... will fix later...
            float currentDist;
            for (int i = 1; i < swordTrfms.Length; i++)
            {
                currentDist = Vector3.Distance(swordTrfms[i].position, selectorTrfm.position);
                if (currentDist < min)
                {
                    minID = i;
                    min = currentDist;
                }
            }
            swordTrfms[minID].parent = null;
            swordEffects.transform.position = swordTrfms[minID].position + Vector3.up * -1.518f;
            swordEffects.Play();

            if (minID == 0) { textManager.SetText("Tier: Mythic"); }
            else if (minID == 4 || minID == 6) { textManager.SetText("Tier: Legendary"); }
            else { textManager.SetText("Tier: Rare"); }
        }
    }
}
