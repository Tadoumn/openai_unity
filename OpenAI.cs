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
        if(requestForm.id == null)
        {
            requestForm.id = RequestIDGenerator();
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

        public string toString()
        {
            return "prompt_tokens: " + prompt_tokens + " completion_tokens: " + completion_tokens + " total_tokens: " + total_tokens;
        }
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
        string url = auth.openAIapiTextCompletionURL;
        using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
        {
            string uploadHandler = "{\"model\": \"" + requestForm.config.modelName + "\", \"prompt\": \"" + requestForm.prompt.Replace("\"","'").Replace("\n",@"\n") + "\", \"temperature\": " + requestForm.config.temperature.ToString().Replace(",",".") + ", \"max_tokens\": " + requestForm.config.maxTokenPerRequest + "}";
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + auth.apiKey);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(
                uploadHandler
            ));
            //Get error if any
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.Success)
            {
                GetRequestBack(request, requestForm);
            }
            else
            {
                Debug.Log("Error: " + request.error + "\n" + request.downloadHandler.text);
                Debug.Log(uploadHandler);
            }
        }
    }
}