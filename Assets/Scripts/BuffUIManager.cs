using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    public static BuffUIManager S;

    [SerializeField] private GameObject buffUIPref;

    private List<BuffUI> buffUIList = new List<BuffUI>();

    private float positionOffset = 100;

    private void Awake()
    {
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public BuffUI InstantiateBuffUI(Buff buff, int stack)
    {
        BuffUI buffUI = Instantiate(buffUIPref).GetComponent<BuffUI>();

        buffUI.transform.SetParent(transform);

        buffUI.transform.localScale = Vector2.one;

        buffUI.PlayProgress(buff.duration, stack);

        buffUIList.Add(buffUI);

        SetBuffUIPosition();

        buff.buffUI = buffUI;

        return buffUI;
    }

    public void SetBuffUIPosition()
    {
        for(int i = 0; i < buffUIList.Count; ++i)
        {
            buffUIList[i].transform.position = new Vector2(positionOffset * (i + 1), 50);
        }
    }

    public void RemoveItem(BuffUI buffUI)
    {
        BuffUIManager.S.buffUIList.Remove(buffUI);
    }
}