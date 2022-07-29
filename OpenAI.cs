using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class OpenAI : MonoBehaviour
{
    public OpenAIAuth auth;
    public OpenAIConfig defaultConfig;

    public List<OpenAIRequest> backRequests = new List<OpenAIRequest>();

    public string RequestIDGenerator()
    {
        return System.Guid.NewGuid().ToString();
    }

    public void Request(OpenAIRequest requestForm)
    {
        if (requestForm.config == null)
        {
            requestForm.config = defaultConfig;
        }
        StartCoroutine(RequestOpenAI(requestForm));
    }

    [System.Serializable]
    public class OpenAIRequest
    {
        public string id;
        public string prompt;
        public OpenAIConfig config;
        public RequestContent content;
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

    public void GetRequestBack(UnityWebRequest request, OpenAIRequest requestForm)
    {
        
        string content = request.downloadHandler.text;
        RequestContent requestContent = JsonUtility.FromJson<RequestContent>(content);
        requestForm.content = requestContent;
        backRequests.Add(requestForm);
    }

    IEnumerator RequestOpenAI(OpenAIRequest requestForm)
    {
        string url = "https://api.openai.com/v1/completions";
        using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + auth.apiKey);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(
                "{\"model\": \"" + requestForm.config.modelName + "\", \"prompt\": \"" + requestForm.prompt + "\", \"temperature\": " + requestForm.config.temperature + ", \"max_tokens\": " + requestForm.config.maxTokenPerRequest + "}"
            ));
            //Get error if any
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                GetRequestBack(request, requestForm);
            }
        }
    }
}