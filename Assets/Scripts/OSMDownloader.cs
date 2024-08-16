using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System;
using System.Collections.Generic;

[ExecuteInEditMode]
public class OSMDownloader : MonoBehaviour
{
    [SerializeField] private OverpassData _data;

    private Coroutine requestRoutine = null;

    private const string BASE_URL = "https://overpass-api.de/api/interpreter";
    private readonly string tmpQuery = "[out:json][timeout:25][bbox:55.61550419876211,12.565162797015171,55.62021785058105,12.573412619121147];\r\n(\r\n  way[\"highway\"];\r\n);\r\nout body;\r\n>;\r\nout skel qt;";
    private readonly string smallQuery = "[out:json][timeout:25][bbox:55.61913728608837,12.570140265536201,55.62013606325383,12.572097626594125];\r\n(\r\n  way[\"highway\"];\r\n);\r\nout body;\r\n>;\r\nout skel qt;";

    public void MakeRequest()
    {
        StopAllCoroutines();
        _data = null;
        requestRoutine = null;
        requestRoutine = StartCoroutine(RequestData());
    }

    private IEnumerator RequestData()
    {
        var url = string.Concat(BASE_URL, "?data=", smallQuery);
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var guid = Guid.NewGuid();
            var fileName = string.Concat(guid, ".json");
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, fileName), request.downloadHandler.text);
            _data = JsonConvert.DeserializeObject<OverpassData>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.error);
        }
        Debug.Log("request done");
    }
}


