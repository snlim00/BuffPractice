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

    private void Awake()
    {
        icon = GetComponent<Image>();
        progress = transform.GetChild(0).GetComponent<Image>();
        stack = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void PlayProgress(float duration, int stack)
    {
        StartCoroutine(Progress(duration, stack));
    }

    private IEnumerator Progress(float duration, int stack)
    {
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

    }
}
