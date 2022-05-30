using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StackType { Add, Cover }

public class Buff
{
    public string name;

    public int status;
    public float power;
    public float duration;
    public StackType stackType;

    public bool isApplying = false;

    public Buff(string name, int status, float power, float duration, StackType stackType)
    {
        this.name = name;
        this.status = status;
        this.power = power;
        this.duration = duration;
        this.stackType = stackType;
    }
}

public struct BuffList
{
    public static readonly Buff TEST_EXTRA_SPEED 
        = new Buff(nameof(TEST_EXTRA_SPEED), STATUS.EXTRA_SPEED, 10, 3, StackType.Add);

    public static readonly Buff TEST_SPEED_MULT
        = new Buff(nameof(TEST_SPEED_MULT), STATUS.SPEED_MULTIPLIER, 0.5f, 3, StackType.Add);

    public static readonly Buff TEST_DAMAGE
        = new Buff(nameof(TEST_DAMAGE), STATUS.EXTRA_DAMAGE, 10, 2, StackType.Add);
}

public struct STATUS
{
    public const int DEFAULT_SPEED = 0;
    public const int EXTRA_SPEED = 1;
    public const int SPEED_MULTIPLIER = 2;

    public const int EXTRA_DAMAGE = 3;

    public const int COUNT = 4;
}

public class Entity : MonoBehaviour
{
    public List<Buff> buffList = new List<Buff>();

    public float[] status = new float[STATUS.COUNT];

    public float speed
    { 
        get 
        { 
            return (status[STATUS.DEFAULT_SPEED] + status[STATUS.EXTRA_SPEED]) * status[STATUS.SPEED_MULTIPLIER]; 
        } 
    }

    public float damage;

    private void Start()
    {
        
    }

    public void GivingBuff(Buff buff)
    {
        StartCoroutine(ApplyBuff(buff));
    }

    private IEnumerator ApplyBuff(Buff buff)
    {
        AddBuff(buff);

        yield return new WaitForSeconds(buff.duration);

        RemoveBuff(buff);
    }

    private void AddBuff(Buff buff)
    {
        buffList.Add(buff);
        SetStatus(buff.status, buff.power);

        buff.isApplying = true;
    }

    private void RemoveBuff(Buff buff)
    {
        if(buffList.Remove(buff) == true)
        {
            buff.isApplying = false;

            SetStatus(buff.status, -buff.power);
        }
    }

    private void SetStatus(int status, float power)
    {
        this.status[status] += power;
    }
}