using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;

public static class GCUtils
{
    public delegate bool BubbleSortCompare<T>(T a, T b);
    static System.Random rand = new System.Random();
    public static void BubbleSort<T>(List<T> list, BubbleSortCompare<T> isHigher)
    {
        if (list == null)
            return;
        for (int i = list.Count - 1; i > 0; i--)
        {
            for (int j = 0; j <= i - 1; j++)
            {
                if (isHigher(list[i], list[j]))
                {
                    var highValue = list[i];

                    list[i] = list[j];
                    list[j] = highValue;
                }
            }
        }
    }
    public static T InstantiateObject<T>(T prefab, Transform parent) where T : Component
    {
        if (prefab != null)
        {
            T go = GameObject.Instantiate<T>(prefab) as T;
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localPosition = Vector3.zero;
            go.gameObject.SetActive(false);
            return go;
        }
        return null;
    }
    public static List<int> ParseStringToListInt(string str, char splitChar)
    {
        var result = new List<int>();
        if (!string.IsNullOrEmpty(str))
        {
            var items = str.Split(splitChar);

            if (items != null)
            {
                foreach (var iter in items)
                {
                    if (string.IsNullOrEmpty(iter))
                        continue;

                    int id;
                    if (int.TryParse(iter, out id))
                        result.Add(id);
                }
            }
        }
        return result;
    }
    public static List<string> ParseStringToList(string str, char splitChar)
    {
        var result = new List<string>();
        if (!string.IsNullOrEmpty(str))
        {
            var items = str.Split(splitChar);
            if (items != null)
            {
                foreach (var iter in items)
                {
                    if (!string.IsNullOrEmpty(iter))
                        result.Add(iter);
                }
            }
        }
        return result;
    }
    public static int RandomInt(int min, int max)
    {
        return rand.Next(min, max);
    }
    public static Sprite ConvertToSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    //LOAD AUDIO
    public static AudioClip LoadAudioClip(string filePath)
    {
        var www = new WWW("file://" + filePath);

        while (!www.isDone) { }
        Debug.LogError(filePath);
        return www.GetAudioClip(false, false);
    }
}
