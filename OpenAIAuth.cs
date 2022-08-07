using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "OpenAIAuth", menuName = "OpenAI/OpenAIAuth", order = 1)]
public class OpenAIAuth : ScriptableObject
{
    public string openAIapiTextCompletionURL = "https://api.openai.com/v1/completions";
    // The OpenAI API key.
    public string apiKey = "";
    // The OpenAI organization ID.
    public string orgId = "";
}
