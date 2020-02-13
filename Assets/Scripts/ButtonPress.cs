using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    public Text Txt_TestMsg;

    public void DisplayMessage() {
        Txt_TestMsg.text = "CLICKED!!!";
    }
}
