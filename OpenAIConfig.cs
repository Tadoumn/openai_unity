using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpenAIConfig", menuName = "OpenAI/OpenAIConfig", order = 1)]
public class OpenAIConfig : ScriptableObject
{
    //Open ai request paramaters
    public string modelName = "text-davinci-002";
    public float maxTokenPerRequest = 10;
    public float temperature = 0.1f;
}
