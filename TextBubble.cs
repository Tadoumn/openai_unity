using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBubble : MonoBehaviour
{
    public Text textBubble;
    private TextRequestManager textRequestManager;

    void Start()
    {
        textRequestManager = Camera.main.GetComponent<TextRequestManager>();
    }

    public void CallTextBubble(TextRequestManager.GTag[] tags)
    {
        textRequestManager.CallTextBubble(tags, textBubble);
    }
}
