using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUIToOpenAI : MonoBehaviour
{
    public string prompt;
    public bool lookingForText = false;
    public string idRequest = "";

    public Text text;

    public OpenAI openAI;
    public OpenAIConfig config;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(lookingForText)
            LookForText();
    }

    [Sirenix.OdinInspector.Button]
    void TextRequest()
    {
        lookingForText = true;
        idRequest = openAI.RequestIDGenerator();
        OpenAI.OpenAIRequest request = new OpenAI.OpenAIRequest();
        request.id = idRequest;
        request.prompt = prompt;
        if(config != null)
            request.config = config;
        openAI.Request(request);
    }

    void LookForText()
    {
        foreach(OpenAI.OpenAIRequest request in openAI.backRequests)
        {
            if (request.id == idRequest)
            {
                lookingForText = false;
                text.text = request.content.choices[0].text;
                text.text = text.text.TrimStart('\r', '\n').TrimEnd('\r', '\n');
                return;
            }
        }
    }
}
