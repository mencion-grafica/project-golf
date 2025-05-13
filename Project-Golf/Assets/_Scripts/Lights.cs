using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    [SerializeField] private List<GameObject> lights = new List<GameObject>();
    [SerializeField] private List<Light> lightComponents = new List<Light>();

    [SerializeField] private Material lightMaterial;
    [SerializeField] private Material alarmMaterial;
    
    [SerializeField] private Color lightColor;
    [SerializeField] private Color alarmColor;

    [SerializeField] private float speed;
    
    [SerializeField] private AudioSource audioSource;
    
    private Coroutine _coroutine;
    private bool isAlarm;
    
    [ContextMenu("Normal Lights")]
    public void NormalLights()
    {
        isAlarm = false;
        foreach (GameObject lightObj in lights) lightObj.GetComponent<Renderer>().material = lightMaterial;
        if (_coroutine != null) StopCoroutine(_coroutine);
        SoundManager.Instance.StopFx(audioSource);
        foreach (Light lightComp in lightComponents)
        {
            lightComp.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            lightComp.color = lightColor;
        }
    }
    
    [ContextMenu("Alarm Lights")]
    public void AlarmLights()
    {
        isAlarm = true;
        foreach (GameObject lightObj in lights) lightObj.GetComponent<Renderer>().material = alarmMaterial;
        if (_coroutine != null) StopCoroutine(_coroutine);
        foreach (Light lightComp in lightComponents)
        {
            lightComp.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            lightComp.color = alarmColor;
        }
        SoundManager.Instance.PlayFxLoop(AudioFX.Alarm, audioSource);
        _coroutine = StartCoroutine(AlarmLightsCoroutine());
    }
    
    private IEnumerator AlarmLightsCoroutine()
    {
        while (isAlarm)
        {
            foreach (Light lightComp in lightComponents) lightComp.transform.Rotate(Vector3.up * (speed * Time.deltaTime));
            yield return null;
        }
        
        foreach (Light lightComp in lightComponents)
        {
            lightComp.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            lightComp.color = lightColor;
        }
    }
}
