using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_ADFGVX_SaveDecodedData : Button_ADFGVX
{
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        GameManager.verificationPart.StartDecodeVerification();
    }
}
