using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ScrollContentHandler : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    public void ResetVerticalScroll()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}