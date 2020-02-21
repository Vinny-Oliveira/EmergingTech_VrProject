using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    public Text Txt_TestMsg;
    private int num = 0;

    public void DisplayMessage() {
        Txt_TestMsg.text = "CLICKED -> " + num;
        num++;
    }
}
