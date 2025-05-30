using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum effectType
{
    typewriter = 0,
}

public class StoryText : MonoBehaviour
{
    public TMP_Text m_text;
    [Range(0, 1)] public float speed = 1;
    
    [Header("按钮控制")]
    public Button targetButton; // 要启用的按钮
    // 或者可以使用这个来自动查找Canvas下的Button
    public bool autoFindButton = true;

    private void Awake()
    {
        gameObject.TryGetComponent<TMP_Text>(out m_text);
        
        // 如果启用自动查找，寻找Canvas下的Button
        if (autoFindButton && targetButton == null)
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                targetButton = parentCanvas.GetComponentInChildren<Button>(true);
            }
        }
        
        // 初始时禁用按钮
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(TypeWriter());
    }
    
    private IEnumerator TypeWriter()
    {
        m_text.ForceMeshUpdate();
        TMP_TextInfo textInfo = m_text.textInfo;
        int total = textInfo.characterCount;
        int current = 0;
        bool complete = false;
        
        while (!complete)
        {
            if (current > total)
            {
                current = total;
                complete = true;
            }
            
            m_text.maxVisibleCharacters = current;
            current += 1;
            yield return new WaitForSecondsRealtime(speed);
        }
        
        // 文字显示完成后，等待1秒再启用按钮
        yield return new WaitForSecondsRealtime(1f);
        
        // 启用按钮
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(true);
            
            // 可选：添加按钮出现的动画效果
            if (targetButton.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
            {
                canvasGroup.alpha = 0f;
                StartCoroutine(FadeInButton(canvasGroup));
            }
        }
        
        yield return null;
    }
    
    // 按钮淡入动画
    private IEnumerator FadeInButton(CanvasGroup canvasGroup)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
}




