using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileHandler {

    public static void SaveToJSON<T>(List<T> toSave, string filename, bool prettyPrint = false)
    {
        string content = JsonHelper.ToJson<T>(toSave.ToArray(), prettyPrint);
        WriteFile(GetPath(filename), content);
    }

    public static void SaveToJSON<T>(T toSave, string filename, bool prettyPrint = false)
    {
        string content = JsonUtility.ToJson(toSave, prettyPrint);
        WriteFile(GetPath(filename), content);
    }


    public static List<T> ReadListFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        //return JsonUtility.FromJson<List<T>>(content);
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }
        Debug.Log("---");
        Debug.Log(content);
        Debug.Log("---");

        var test = JsonHelper.FromJson<T>(content);
        Debug.Log(test);
        List<T> res = JsonHelper.FromJson<T>(content)?.ToList() ?? new List<T>();
        return res;
    }


    public static T ReadFromJSON<T> (string filename) {
        string content = ReadFile (GetPath (filename));

        if (string.IsNullOrEmpty (content) || content == "{}") {
            return default (T);
        }

        T res = JsonUtility.FromJson<T> (content);

        return res;

    }

    private static string GetPath (string filename) {
        
        // Verwende Application.dataPath, um den Assets-Ordner zu erhalten
        return Path.Combine(Application.dataPath, filename);
        
        //Der Application.persistentDataPath wird normalerweise für persistente Daten auf dem Gerät verwendet
        //return Application.persistentDataPath + "/" + filename;
    }

    private static void WriteFile (string path, string content) {
        FileStream fileStream = new FileStream (path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter (fileStream)) {
            writer.Write (content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                Debug.Log($"File content for {path}: {content}");
                return content;
            }
        }
        return "";
    }

}

public static class JsonHelper {
    public static T[] FromJson<T> (string json) {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T> (T[] array) {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson (wrapper);
    }

    public static string ToJson<T> (T[] array, bool prettyPrint) {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson (wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T> {
        public T[] Items;
    }
}