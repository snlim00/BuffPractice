using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class STATUS_DATA_KEY
{
    public const int STATUS_NAME = 0;
    public const int MIN_VALUE = 1;
    public const int MAX_VALUE = 2;

    public const int COUNT = 3;
}

public class Status
{
    public float value { get; private set; }

    public static float[] statusData = new float[STATUS_DATA_KEY.COUNT];

    private Status()
    {

    }

    public Status(float minValue, float maxValue)
    {
        statusData[STATUS_DATA_KEY.MIN_VALUE] = minValue;
        statusData[STATUS_DATA_KEY.MAX_VALUE] = maxValue;
    }

    public void SetStatus(float value)
    {
        if (value > statusData[STATUS_DATA_KEY.MAX_VALUE])
        {
            value = statusData[STATUS_DATA_KEY.MAX_VALUE];
        }
        else if (value < statusData[STATUS_DATA_KEY.MIN_VALUE])
        {
            value = statusData[STATUS_DATA_KEY.MIN_VALUE];
        }

        this.value = value;
    }

    public static Status[] statusSample;
    public static void SetStatusSample()
    {
        List<Dictionary<string, object>> statusData = CSVReader.Read("StatusData", "StatusData");

        statusSample = new Status[statusData.Count];

        for (int i = 0; i < statusData.Count; ++i)
        {
            Status.statusData[i] = Convert.ToSingle(Convert.ToDouble(statusData[i]));
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
    public Status[] status;

    void Awake()
    {
        Status.SetStatusSample();

        status = Status.statusSample;
    }


}
