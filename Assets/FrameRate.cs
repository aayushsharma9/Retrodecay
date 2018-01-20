using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour
{
    float fps = 0;
    public Text fpsText;

    private void Update()
    {
        fps = 1 / Time.deltaTime;
        StartCoroutine(ShowFPS());
    }

    IEnumerator ShowFPS()
    {
        fpsText.text = "" + (int)fps;
        yield return new WaitForSecondsRealtime(1);
    }
}
