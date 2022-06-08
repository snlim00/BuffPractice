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

    public bool haveBuffUI = false;
    public BuffUI buffUI;

    public float startTime;

    public Buff(string name, int status, float power, float duration, StackType stackType)
    {
        this.name = name;
        this.status = status;
        this.power = power;
        this.duration = duration;
        this.stackType = stackType;
    }

    public Buff(Buff bf)
    {
        this.name = bf.name;
        this.status = bf.status;
        this.power = bf.power;
        this.duration = bf.duration;
        this.stackType = bf.stackType;
    }

    public Buff(Buff bf, float power)
    {
        this.name = bf.name;
        this.status = bf.status;
        this.power = power;
        this.duration = bf.duration;
        this.stackType = bf.stackType;
    }

    public Buff(Buff bf, float power, float duration)
    {
        this.name = bf.name;
        this.status = bf.status;
        this.power = power;
        this.duration = duration;
        this.stackType = bf.stackType;
    }
}

public struct BuffList
{
    public static readonly Buff TEST_EXTRA_SPEED 
        = new Buff(nameof(TEST_EXTRA_SPEED), STATUS.EXTRA_SPEED, 10, 3, StackType.Cover);

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
        buff.startTime = Time.time;

        if (buff.stackType == StackType.Cover)
            CoverStackProcess(buff);

        else if (buff.stackType == StackType.Add)
            AbleBuff(buff);

        UIProcess(buff);
        //BuffUIManager.S.InstantiateBuffUI(buff.duration, 0);
    }

    private void RemoveBuff(Buff buff)
    {
        DisableBuff(buff);

        if (buffList.Remove(buff) == true)
        {
            if (buff.stackType == StackType.Cover)
                CoverStackProcess(buff);

            else if (buff.stackType == StackType.Add)
                DisableBuff(buff);

            UIProcess(buff);
        }
    }

    private void AbleBuff(Buff buff)
    {
        if (buff.isApplying == false)
        {
            buff.isApplying = true;
            SetStatus(buff);

            //Debug.Log("ABLE");
        }
    }

    private void DisableBuff(Buff buff)
    {
        if (buff.isApplying == true)
        {
            buff.isApplying = false;
            SetStatus(buff, false);

            //Debug.Log("DISABLE");
        }
    }

    private void UIProcess(Buff buff)
    {
        List<Buff> sameBuffList = FindSameBuff(buff);

        //if(buff.stackType == StackType.Cover)
        {
            if (sameBuffList.Count == 1)
            {
                BuffUIManager.S.InstantiateBuffUI(sameBuffList[0], sameBuffList.Count);
            }
            else
            {
                int mostPowerIndex = FindMostPowerIndex(sameBuffList);
                int haveBuffUIIndex = FindHaveBuffUIIndex(sameBuffList);

                if (haveBuffUIIndex == -1)
                {
                    BuffUIManager.S.InstantiateBuffUI(sameBuffList[mostPowerIndex], sameBuffList.Count);
                    Debug.Log("A");
                }
                else if (sameBuffList[mostPowerIndex] == buff)
                {
                    Debug.Log("B");
                }
                else if (mostPowerIndex == haveBuffUIIndex)
                {
                    sameBuffList[mostPowerIndex].buffUI.StopProgress();
                    sameBuffList[mostPowerIndex].buffUI.PlayProgress(sameBuffList[mostPowerIndex], sameBuffList.Count);
                    Debug.Log("C");
                }
                else
                {
                    sameBuffList[mostPowerIndex] = sameBuffList[haveBuffUIIndex];
                    sameBuffList[haveBuffUIIndex].haveBuffUI = false;
                    sameBuffList[mostPowerIndex].haveBuffUI = true;

                    sameBuffList[mostPowerIndex].buffUI.StopProgress();
                    sameBuffList[mostPowerIndex].buffUI.PlayProgress(sameBuffList[mostPowerIndex], sameBuffList.Count);
                    Debug.Log("D");
                }
            }
        }
        
    }

    private void SetStatus(Buff buff, bool isAble = true)
    {
        if (isAble == true)
        { 
            status[buff.status] += buff.power;
        }
        else
        {
            status[buff.status] -= buff.power;
        }
    }
    
    private void CoverStackProcess(Buff buff)
    {
        if(buff.stackType == StackType.Cover)
        {
            //동일한 이름의 버프 찾기
            List<Buff> sameBuffList = FindSameBuff(buff);

            //모든 버프 비활성화
            for (int i = 0; i < sameBuffList.Count; ++i)
            {
                DisableBuff(sameBuffList[i]);
            }

            //가장 높은 능력치의 버프만 활성화
            if (sameBuffList.Count > 0)
            {
                int mostPowerIndex = FindMostPowerIndex(sameBuffList);

                AbleBuff(sameBuffList[mostPowerIndex]);
            }
        }
    }

    private List<Buff> FindSameBuff(Buff buff)
    {
        List<Buff> sameBuffList = new List<Buff>();
        for (int i = 0; i < buffList.Count; ++i)
        {
            if (buffList[i].name == buff.name)
            {
                sameBuffList.Add(buffList[i]);
            }
        }

        return sameBuffList;
    }

    private int FindMostPowerIndex(List<Buff> buffList)
    {
        int mostPowerIndex = 0;
        for (int i = 1; i < buffList.Count; ++i)
        {
            if (buffList[mostPowerIndex].power < buffList[i].power)
            {
                mostPowerIndex = i;
            }
        }

        return mostPowerIndex;
    }

    private int FindHaveBuffUIIndex(List<Buff> buffList)
    {
        int haveBuffUIIndex = -1;

        for (int i = 0; i < buffList.Count; ++i)
        {
            if (buffList[i].haveBuffUI == true)
            {
                haveBuffUIIndex = i;
                break;
            }
        }

        return haveBuffUIIndex;
    }
}