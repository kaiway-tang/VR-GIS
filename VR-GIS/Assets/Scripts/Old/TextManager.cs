using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextManager : MonoBehaviour
{
    [SerializeField] TMP_Text tmpText;
    [SerializeField] Sword swordScript;
    [SerializeField] string grippedText;
    int lastMode;
    // Start is called before the first frame update
    void Start()
    {
        lastMode = swordScript.GetMode();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lastMode != swordScript.GetMode())
        {
            lastMode = swordScript.GetMode();
            if (lastMode == Sword.TRACKING)
            {
                tmpText.text = grippedText;
            } else if (lastMode == Sword.SHEATHED)
            {
                tmpText.text = "Grip the sword";
            }
        }
    }

    public void SetText(string text)
    {
        tmpText.text = text;
    }
}
