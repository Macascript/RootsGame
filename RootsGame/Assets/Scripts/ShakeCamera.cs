using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField]
    private float wrongAmplitude;

    [SerializeField]
    private float wrongFrequency;

    [SerializeField]
    private float wrongTime;

    [SerializeField]
    private float correctAmplitude;

    [SerializeField]
    private float correctFrequency;

    [SerializeField]
    private float correctTime;

    private float shakeTimer = 0;
    private float shakeTimerTotal = 0;
    private float startingAmplitude = 0;

    [SerializeField] private GestureDetector gestures;

    public void ShakeCameraCorrect(bool ok = true)
    {
        CinemachineBasicMultiChannelPerlin perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_FrequencyGain = correctFrequency;

        perlin.m_AmplitudeGain = correctAmplitude;

        //startingAmplitude = correctAmplitude;
        //shakeTimer = correctTime;
        //shakeTimerTotal = correctTime;
        gestures.enabled = false;
        if (ok)
            Invoke("StopShaking", correctTime);
    }

    public void ShakeCameraWrong(bool ok = true)
    {
        CinemachineBasicMultiChannelPerlin perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_FrequencyGain = wrongFrequency;

        perlin.m_AmplitudeGain = wrongAmplitude;

        //startingAmplitude = wrongAmplitude;
        //shakeTimer = wrongTime;
        //shakeTimerTotal = wrongTime;
        gestures.enabled = false;
        if (ok)
            Invoke("StopShaking", wrongTime);
    }

    public void StopShaking()
    {
        CinemachineBasicMultiChannelPerlin perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0f;
        perlin.m_FrequencyGain = 0f;
        gestures.enabled = true;
    }

    //private void Update()
    //{
    //    shakeTimer -= Time.deltaTime;
    //    if(shakeTimer <= 0f)
    //    {
    //        CinemachineBasicMultiChannelPerlin perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    //        perlin.m_AmplitudeGain = Mathf.Lerp(startingAmplitude, 0f, shakeTimer / shakeTimerTotal);
    //    }
    //}
}
