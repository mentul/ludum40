using UnityEngine;


[DefaultExecutionOrder(100)]
public class FPSCounter : MonoBehaviour
{
    public UnityEngine.UI.Text fpsText, minFpsText, maxFpsText, avgFpsText;

    public float fps, avgfps, minfps = float.PositiveInfinity, maxfps = float.NegativeInfinity;
    float fpsSum, fpsCount;
    float deltaTime = 0.0f;

    bool isCounting = false;

    void Start()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 1000;
        StartCounter();
    }

    void Update()
    {
        if (isCounting)
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void LateUpdate()
    {
        if (isCounting)
        {
            fps = 1.0f / deltaTime;
            if (fps > maxfps) maxfps = fps;
            if (fps < minfps && fps > 0) minfps = fps;
            fpsCount++;
            fpsSum += fps;
            avgfps = fpsSum / fpsCount;
            UpdateText();
        }
    }

    public void StartCounter()
    {
        isCounting = true;
    }

    public void StopCounter()
    {
        isCounting = false;
    }

    public void ResetCounter()
    {
        isCounting = false;
        fps = 0;
        maxfps = float.NegativeInfinity;
        minfps = float.PositiveInfinity;
        avgfps = 0;
        fpsSum = 0;
        fpsCount = 0;
        UpdateText();
    }

    void UpdateText()
    {
        fpsText.text = fps.ToString();
        minFpsText.text = minfps.ToString();
        maxFpsText.text = maxfps.ToString();
        avgFpsText.text = avgfps.ToString();
    }
}
