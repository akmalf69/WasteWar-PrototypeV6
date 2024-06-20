using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    public GameObject highlightGO;

    public Button button;

    public void HandlerMouseEnter()
    {
        if(button.IsInteractable())
        {
            if(button.IsInteractable() )
            {
                highlightGO.SetActive(true);
            }
        }
    }
}
