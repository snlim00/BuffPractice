using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Stat
{
    public static class StatName
    {
        public const int DefaultSpeed = 0;
        public const int ExtraSpeed = 1;
        public const int SpeedMultiplier = 2;
        public const int FinalSpeed = 3;

        public const int Count = 4;
    }

    public static class StatDataKey
    {
        public const int StatusName = 0;
        public const int MinValue = 1;
        public const int MaxValue = 2;

        public const int Count = 3;

        public static string GetName(int value)
        {
            switch(value)
            {
                case StatusName:
                    return nameof(StatName);

                case MinValue:
                    return nameof(MinValue);

                case MaxValue:
                    return nameof(MaxValue);

                default:
                    return value + "에 해당하는 값이 없습니다. (" + nameof(StatDataKey) + ".GetName)";
            }
        }
    }

    public float value { get; private set; }

    [HideInInspector]
    public static float[] statusData = new float[StatDataKey.Count];

    private Stat()
    {

    }

    public Stat(float minValue, float maxValue)
    {
        statusData[StatDataKey.MinValue] = minValue;
        statusData[StatDataKey.MaxValue] = maxValue;
    }

    public void SetStatus(float value)
    {
        if (value > statusData[StatDataKey.MaxValue])
        {
            value = statusData[StatDataKey.MaxValue];
        }
        else if (value < statusData[StatDataKey.MinValue])
        {
            value = statusData[StatDataKey.MinValue];
        }

        this.value = value;
    }

    public static Stat[] statusTemplate;
    public static void SetStatusTemplate()
    {
        List<Dictionary<string, object>> statusData = CSVReader.Read("StatusData", "StatusData.CSV");

        statusTemplate = new Stat[statusData.Count];

        for (int i = 0; i < statusData.Count; ++i)
        {
            for(int j = 1; j < Stat.StatDataKey.Count; ++j)
            {
                Stat.statusData[i] = Convert.ToSingle(Convert.ToDouble(statusData[i][Stat.StatDataKey.GetName(j)]));
                Debug.Log(Stat.StatDataKey.GetName(j));
            }
            //Stat.statusData[i] = statusData[i]
        }
    }
}

public enum StackType2 { Add, Cover }

public class Buff2
{
    public string name;

    public int statusIndex;
    public float power;
    public float duration;
    public StackType2 stackType;
}

public class EnemyRemake : MonoBehaviour
{
    public Stat[] status;

    void Awake()
    {
        Stat.SetStatusTemplate();

        status = Stat.statusTemplate;
    }
}
