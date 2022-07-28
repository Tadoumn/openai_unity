using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;


public class OpenAI : MonoBehaviour
{
    public OpenAIAuth auth;
    public OpenAIConfig defaultConfig;

    public List<RequestContent> backRequests = new List<RequestContent>();

    public string debugPrompt;

    [Button]
    public void DebugRequest()
    {
        Request(debugPrompt);
    }

    public void Request(string prompt, OpenAIConfig config = null)
    {
        if (config == null)
        {
            config = defaultConfig;
        }
        StartCoroutine(RequestOpenAI(prompt, config));
    }

    [System.Serializable]
    public class RequestContent
    {
        public string id;
        public string object_type;
        public int created;
        public string model;
        public List<Choice> choices;
        public Usage usage;
    }

    [System.Serializable]
    public class Choice
    {
        public string text;
        public int index;
        public float[] logprobs;
        public string finish_reason;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

    public void GetRequestBack(UnityWebRequest request)
    {
        
        string content = request.downloadHandler.text;
        RequestContent requestContent = JsonUtility.FromJson<RequestContent>(content);
        Debug.Log(requestContent.choices[0].text);

        backRequests.Add(requestContent);
    }

    IEnumerator RequestOpenAI(string prompt, OpenAIConfig config)
    {
        string url = "https://api.openai.com/v1/completions";
        using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + auth.apiKey);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(
                "{\"model\": \"" + config.modelName + "\", \"prompt\": \"" + prompt + "\", \"temperature\": " + config.temperature + ", \"max_tokens\": " + config.maxTokenPerRequest + "}"
            ));
            yield return request.SendWebRequest();
            GetRequestBack(request);
        }
    }
}