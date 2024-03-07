using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private string serverUri = "https://65e8c59f4bb72f0a9c505e15.mockapi.io/buttons/";

    public void RefreshButtons(Action<List<ButtonData>> callback)
    {
        var request = UnityWebRequest.Get(serverUri);

        StartCoroutine(SendRequestCoroutine(request, () =>
        {
            var content = request.downloadHandler.text;
            content = "{\"buttons\":" + content + "}";
            callback(JsonUtility.FromJson<ButtonsData>(content).buttons);
        }));
    }

    public void RefreshButton(string id, Action<ButtonData> callback)
    {
        var request = UnityWebRequest.Get(serverUri + id);

        StartCoroutine(SendRequestCoroutine(request,
            () => callback(JsonUtility.FromJson<ButtonData>(request.downloadHandler.text))));
    }

    public void CreateButton(Action<ButtonData> callback)
    {
        var request = UnityWebRequest.Post(serverUri, new WWWForm());

        StartCoroutine(SendRequestCoroutine(request,
            () => callback(JsonUtility.FromJson<ButtonData>(request.downloadHandler.text))));
    }

    public void UpdateButton(string id, string name, Action<ButtonData> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("text", name);

        var request = UnityWebRequest.Put(serverUri + id, form.data);
        request.uploadHandler.contentType = "application/x-www-form-urlencoded";

        StartCoroutine(SendRequestCoroutine(request,
            () => callback(JsonUtility.FromJson<ButtonData>(request.downloadHandler.text))));
    }

    public void DeleteButton(string id, Action callback)
    {
        var request = UnityWebRequest.Delete(serverUri + id);

        StartCoroutine(SendRequestCoroutine(request, callback));
    }

    private IEnumerator SendRequestCoroutine(UnityWebRequest request, Action callback)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.ConnectionError) callback();
    }
}
