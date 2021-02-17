using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationWidget : MonoBehaviour
{
    [SerializeField] private GameObject explanationWidget;
    private CanvasGroup canvasGroup;

   
    void Start()
    {
        HideWidget();
    }

    public void ShowWidget()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideWidget()
    {
        canvasGroup = explanationWidget.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; //this makes everything transparent
        canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }
}
