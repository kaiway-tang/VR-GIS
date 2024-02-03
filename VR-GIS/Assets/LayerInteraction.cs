using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerInteraction : MonoBehaviour
{
    [SerializeField] string hoverInfo;

    Transform trfm;
    Vector3 defaultScale, enlargedScale;

    [SerializeField] GameObject[] enables;
    [SerializeField] Transform[] trfms;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] TextDisplay LHandDisplay, RHandDisplay;

    public bool hoverLeft, hoverRight, isEnabled = true;

    bool inSeparationMode;
    
    // Start is called before the first frame update
    void Start()
    {
        isEnabled = true;
        trfm = transform;

        defaultScale = trfm.localScale;
        enlargedScale = trfm.localScale * 1.2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inSeparationMode != GestureDetector.inSeparationMode)
        {
            inSeparationMode = GestureDetector.inSeparationMode;

            UpdateLayerState();
        }

        if (AnyHover() && GestureDetector.inSeparationMode)
        {
            trfm.localScale += (enlargedScale - trfm.localScale) * .1f;
        }
        else
        {
            trfm.localScale += (defaultScale - trfm.localScale) * .1f;
        }
    }

    void UpdateLayerState()
    {
        if (inSeparationMode) //on enter separation mode
        {
            if (meshRenderer) { meshRenderer.enabled = true; }

            for (int i = 0; i < enables.Length; i++)
            {
                enables[i].SetActive(true);
            }
        }
        else //on exit separation mode
        {
            if (meshRenderer) { meshRenderer.enabled = isEnabled; }

            for (int i = 0; i < enables.Length; i++)
            {
                enables[i].SetActive(isEnabled);
            }
        }
    }
    
    public void OnPinch(bool isLeft)
    {
        if ((isLeft && hoverLeft) || (!isLeft && hoverRight))
        {
            isEnabled = !isEnabled;
        }
    }

    bool AnyHover()
    {
        return (hoverRight || hoverLeft) && GestureDetector.inSeparationMode;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GestureDetector.inSeparationMode) { return; }

        if (other.tag == "Left")
        {
            hoverLeft = true;
            if (GestureDetector.leftHandState == GestureDetector.HandState.point)
            {
                isEnabled = !isEnabled;
            }

            LHandDisplay.SetText(hoverInfo);
        }

        if (other.tag == "Right")
        {
            hoverRight = true;
            if (GestureDetector.rightHandState == GestureDetector.HandState.point)
            {
                isEnabled = !isEnabled;
            }

            RHandDisplay.SetText(hoverInfo);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Left")
        {
            hoverLeft = false;

            LHandDisplay.SetText("");
        }

        if (other.tag == "Right")
        {
            hoverRight = false;

            RHandDisplay.SetText("");
        }
    }
}
