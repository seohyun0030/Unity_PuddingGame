using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Unity.EditorCoroutines.Editor;
public class Dialogue : EditorWindow
{

    [MenuItem("MyEditor/GoogleSheet")]
    public static void OpenPanel()
    {

        var window = CreateWindow<Dialogue>();
        window.Show();

    }

    private void OnEnable()
    {

        TextField documentIDField = new TextField("documentID");
        TextField gidField = new TextField("gid");

        Button getBtn = new Button(() =>
        {

            GoogleSheet.GetSheetData(documentIDField.value, gidField.value, this, (b, s) =>
            {

                if (b)
                {

                    Debug.Log(s);

                }
                else
                {

                    Debug.Log("½ÇÆÐ");

                }

            });

        });

        getBtn.style.height = new StyleLength(30);

        rootVisualElement.Add(documentIDField);
        rootVisualElement.Add(gidField);
        rootVisualElement.Add(getBtn);

    }

}
public static class GoogleSheet
{

    public static void GetSheetData(string documentID, string sheetID, object onwer, Action<bool, string> process = null)
    {

        EditorCoroutineUtility.StartCoroutine(GetSheetDataCo(documentID, sheetID, process), onwer);

    }

    private static IEnumerator GetSheetDataCo(string documentID, string sheetID, Action<bool, string> process = null)
    {

        string url = $"https://docs.google.com/spreadsheets/d/{documentID}/export?format=tsv&gid={sheetID}";

        UnityWebRequest req = UnityWebRequest.Get(url);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.responseCode != 200)
        {

            process?.Invoke(false, null);
            yield break;

        }

        process?.Invoke(true, req.downloadHandler.text);

    }

}