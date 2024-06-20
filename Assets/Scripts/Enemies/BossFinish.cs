using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinish : MonoBehaviour
{
    public Finish2 finishScript;

    private void OnDestroy()
    {
        if (finishScript != null)
        {
            finishScript.gameObject.SetActive(true);
        }
    }
}