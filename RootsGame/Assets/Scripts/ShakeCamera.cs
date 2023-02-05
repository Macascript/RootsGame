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

    public void ShakeCameraCorrect()
    {
        CinemachineBasicMultiChannelPerlin perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_FrequencyGain = correctFrequency;

        perlin.m_AmplitudeGain = correctAmplitude;

        startingAmplitude = correctAmplitude;
        shakeTimer = correctTime;
        shakeTimerTotal = correctTime;
    }

    public void ShakeCameraWrong()
    {
        CinemachineBasicMultiChannelPerlin perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_FrequencyGain = wrongFrequency;

        perlin.m_AmplitudeGain = wrongAmplitude;

        startingAmplitude = wrongAmplitude;
        shakeTimer = wrongTime;
        shakeTimerTotal = wrongTime;
    }

    private void Update()
    {
        shakeTimer -= Time.deltaTime;
        if(shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            perlin.m_AmplitudeGain = Mathf.Lerp(startingAmplitude, 0f, shakeTimer / shakeTimerTotal);
        }
    }
}
