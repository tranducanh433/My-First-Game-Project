using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    /*public static CameraManager instance;

    public CinemachineVirtualCamera currentCameraController;
    public CinemachineConfiner cinemachineConfiner;
    public NoiseSettings noiseSettings;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void ShakeTheCamera()
    {
        StartCoroutine(CameraShakeCO());
    }

    private IEnumerator CameraShakeCO()
    {
        CinemachineBasicMultiChannelPerlin shakeSetting = currentCameraController.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shakeSetting.m_AmplitudeGain = 2;
        shakeSetting.m_NoiseProfile = noiseSettings;
        yield return new WaitForSeconds(0.25f);
        shakeSetting.m_AmplitudeGain = 0;
    }

    public void ChangeCameraView(PolygonCollider2D polygonCollider2D)
    {
        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
    }*/
}
