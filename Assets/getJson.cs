using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;

public class getJson : MonoBehaviour
{
    public string apiKey = "";

    public List<string> fields = new List<string>();
    private string result;

    string path = "Assets/result.txt";

    void Start()
    {
        StartCoroutine(GetCveData());
    }

    IEnumerator GetCveData()
    {
        result = "";
        for (int i = 0; i < fields.Count; i++)
        {
            string requestUrl = $"https://services.nvd.nist.gov/rest/json/cves/2.0?cpeName=cpe:2.3:o:microsoft:windows_10:{fields[i]}:*:*:*:*:*:*:*&resultsPerPage=1";
            using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
            {
                request.SetRequestHeader("X-Api-Key", apiKey);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    
                    var parsedJson = JSON.Parse(json);
                    int totalResults = parsedJson["totalResults"].AsInt;

                    Debug.Log($"Ver {fields[i]} = {totalResults}");
                    result += $"Ver {fields[i]} = {totalResults}\n";
                }
                else
                {
                    Debug.LogError($"API 요청 실패 (Ver{fields[i]}): {request.error}");
                }
            }
        }
        Debug.Log("all data print\n"+result);
        StreamWriter txt = new StreamWriter(path);
        txt.Write(result);
        txt.Flush();
        txt.Close();
    }
}
