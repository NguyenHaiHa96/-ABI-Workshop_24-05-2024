using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using Random = UnityEngine.Random;

public static class Helper
{
    #region String

    public static List<int> StringToIntList(string inputString, string splitter)
    {
        if (String.IsNullOrEmpty(inputString)) return new List<int>();
        List<int> intList = new List<int>();
        string[] stringArray = inputString.Split(splitter);
        foreach (string element in stringArray)
        {
            if (int.TryParse(element, out var number))
            {
                intList.Add(number);
            }
        }
        return intList;
    }

    public static string RemoveString(string origin, string removedString)
    {
        if (string.IsNullOrEmpty(origin)) return "";
        return string.IsNullOrEmpty(removedString) ? origin : origin.Replace(removedString, "");
    }
    
    public static TEnum ConvertStringToEnum<TEnum>(string value, TEnum defaultValue = default) where TEnum : struct, IConvertible
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        try
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true); 
        }
        catch (ArgumentException)
        {
            return defaultValue;
        }
    }
    
    #endregion
    
    #region List

    public static T GetRandomElementFromList<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static List<T> GetNewListExcludeIndex<T>(List<T> original, int index)
    {
        if (original is not { Count: > 0 }) return new List<T>();
        List<T> newList = new List<T>();
        for (int i = 0; i < original.Count; i++)
        {
            if (i != index) continue;
            newList.Add(original[i]);
        }
        return newList;
    }

    #endregion
    
    #region Utils

    private static readonly Dictionary<float, WaitForSeconds> WfsDictionary = new();

    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if (WfsDictionary.TryGetValue(time, out var wait)) return wait;

        WfsDictionary[time] = new WaitForSeconds(time);
        return WfsDictionary[time];
    }

    #endregion
    
    #region Vector2

    private static readonly Vector2 ScaleSize09 = new Vector2(0.9f, 0.9f);

    #endregion

    #region Time

    private static readonly float TimeDuration01 = 0.1f;

    #endregion

    #region Enum

    public static string ConvertEnumToStringPascalCase<T>(T enumValue) where T : Enum
    {
        string enumString = enumValue.ToString();
        return Regex.Replace(enumString, "([a-z])([A-Z])", "$1 $2");
    }
    
    public static string ConvertEnumToStringUpperCase<T>(T enumValue) where T : Enum
    {
        string enumString = enumValue.ToString();
        return Regex.Replace(enumString, "([a-z])([A-Z])", "$1 $2").ToUpper();
    }
    
    public static TEnum GetRandomEnumInRange<TEnum>(int min, int max) where TEnum : struct
    {
        if (!Enum.IsDefined(typeof(TEnum), min) || !Enum.IsDefined(typeof(TEnum), max))
        {
            
        }
        int randomInt = Random.Range(min, max + 1); 
        return (TEnum)Enum.ToObject(typeof(TEnum), randomInt);
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
    
    #endregion
    
    #region Button

    public static void ButtonOnClickTween(this Button button, UnityAction action)
    {
        button.onClick.AddListener(() =>
        {
            button.interactable = false;
            button.transform.DOScale(ScaleSize09, TimeDuration01)
                .SetEase(Ease.InFlash)
                .SetLoops(2, LoopType.Yoyo)
                .From(Vector3.one)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    button.interactable = true;
                    action?.Invoke();
                });
        });
    }

    #endregion
}