using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRequestManager : MonoBehaviour
{
    public List<TextBubbleObject> idToCheck = new List<TextBubbleObject>();

    public OpenAI openAI;
    public OpenAIConfig config;

    [System.Serializable]
    public struct GTag
    {
        public string tag;
        public string type;
    }

    [System.Serializable]
    public struct TextBubbleObject
    {
        public string id;
        public Text text;
    }
    // Start is called before the first frame update
    void Start()
    {
        openAI = Camera.main.GetComponent<OpenAI>();
    }

    // Update is called once per frame
    void Update()
    {
        LookForText();
    }

    public void CallTextBubble(GTag[] tags, Text textBubble)
    {
        TextBubbleObject to;
        to.id = openAI.RequestIDGenerator();
        to.text = textBubble;
        OpenAI.OpenAIRequest request = new OpenAI.OpenAIRequest();
        request.id = to.id;
        request.prompt = PromptGenerator(tags);
        if(config != null)
            request.config = config;
        openAI.Request(request);
    }

    string PromptGenerator(GTag[] tags)
    {
        string prompt = "";
        foreach(GTag tag in tags)
        {
            prompt += "A robot saying that ";
            prompt += tag.type + " and " + tag.tag;
        }
        Debug.Log(prompt);
        return prompt;
    }
    void LookForText()
    {
        if(idToCheck.Count == 0)
            return;
        foreach(TextBubbleObject to in idToCheck)
        {
            foreach(OpenAI.OpenAIRequest request in openAI.backRequests)
            {
                if (request.id == to.id)
                {
                    to.text.text = request.content.choices[0].text;
                    to.text.text = to.text.text.TrimStart('\r', '\n').TrimEnd('\r', '\n');
                    idToCheck.Remove(to);
                    return;
                }
            }
        }
    }
}
