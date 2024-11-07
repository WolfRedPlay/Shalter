using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public class DiscordBotHandler : MonoBehaviour
{
    private string serverUrl = "http://127.0.0.1:5000/";

    public void StartDiscrodBot()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        processStartInfo.FileName = "C:\\Users\\Sergei\\AppData\\Local\\Programs\\Python\\Python312\\python.exe";
        processStartInfo.Arguments = @"C:\Users\\Sergei\Python\Lessons\Lessons\Main.py";
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
        processStartInfo.UseShellExecute = false; // Use true if you want to use shell
        processStartInfo.CreateNoWindow = false; // Use false if you want a window


        using (Process process = new Process())
        {
            process.StartInfo = processStartInfo;
            process.Start();

            // Wait for the process to exit
            //process.WaitForExit();
        }
    }


    public void SendMessageToDiscord(string message)
    {
        StartCoroutine(SendPostRequest(message));
    }

    private IEnumerator SendPostRequest(string message)
    {
        UnityWebRequest request = new UnityWebRequest(serverUrl + "get_message", "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        string json = $"{{\"message\": \"{message}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Error: " + request.error);
        }
        else
        {
            UnityEngine.Debug.Log("Message sent: " + request.downloadHandler.text);
        }
    }

    public void GetMessageFromDiscord()
    {
        StartCoroutine(CheckForUpdates());
    }

    private IEnumerator CheckForUpdates()
    {
        bool inProgress = true;
        while (inProgress)
        {
            yield return new WaitForSeconds(1f); // Проверять каждые 5 секунд

            UnityWebRequest request = UnityWebRequest.Get(serverUrl + "send_message");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.LogError("Error: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                UnityEngine.Debug.Log("Received update: " + jsonResponse);
                inProgress = false;
                // Обработка полученного JSON, например:
                // var data = JsonUtility.FromJson<UpdateResponse>(jsonResponse);
            }
        }
    }
}
