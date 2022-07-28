using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "OpenAIAuth", menuName = "OpenAI/OpenAIAuth", order = 1)]
public class OpenAIAuth : ScriptableObject
{
    // The OpenAI API key.
    public string apiKey = "";
    // The OpenAI organization ID.
    public string orgId = "";
}
