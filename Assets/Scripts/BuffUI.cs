using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUI : MonoBehaviour
{
    public Image icon;
    public Image progress;
    public TMP_Text stack;

    private bool isProgress = false;
    private Coroutine corProgress = null;

    private void Awake()
    {
        icon = GetComponent<Image>();
        progress = transform.GetChild(0).GetComponent<Image>();
        stack = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void PlayProgress(Buff buff, int stack)
    {
        corProgress = StartCoroutine(Progress(buff.duration - (Time.time - buff.startTime), stack));
    }

    public void StopProgress()
    {
        if(isProgress == true)
        {
            isProgress = false;
            StopCoroutine(corProgress);
        }
    }

    private IEnumerator Progress(float duration, int stack)
    {
        isProgress = true;

        this.stack.text = stack.ToString();

        float t = 0;

        while(t <= 1)
        {
            t += Time.deltaTime / duration;

            progress.fillAmount = t;

            yield return null;
        }

        BuffUIManager.S.RemoveItem(this);
        BuffUIManager.S.SetBuffUIPosition();
        Destroy(this.gameObject);

        isProgress = false;
    }
}
