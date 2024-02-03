using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : Billboarder
{
    [SerializeField] TextMeshProUGUI tmpro;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        tmpcolor = tmpro.color;
    }

    void FixedUpdate()
    {
        HandleFading();
    }

    #region handle fading
    int fadeTmr;
    Color tmpcolor;
    string nextText;
    public void FadeText(string text)
    {
        fadeTmr = 100;
        nextText = text;
    }

    public void SetText(string text)
    {
        tmpro.text = text;
    }

    void HandleFading()
    {
        if (fadeTmr > 0)
        {
            fadeTmr--;
            
            if (fadeTmr > 49)
            {
                tmpcolor.a = (fadeTmr - 50f)*.02f;

                if (fadeTmr == 50)
                {
                    tmpro.text = nextText;
                }
            }
            else
            {
                tmpcolor.a = (51f - fadeTmr)*.02f;
            }

            tmpro.color = tmpcolor;
        }
    }

    #endregion
}
