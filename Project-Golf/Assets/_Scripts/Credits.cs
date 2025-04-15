using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private static readonly int Dilate = Shader.PropertyToID("_FaceDilate");
    
    [SerializeField] private GameObject teleportTo;
    [SerializeField] private TMP_Text credits;
    [SerializeField] private TMP_Text title;
    [SerializeField] private float duration = 60.0f;
    
    public void StartCredits()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        title.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1.0f);
        player.transform.position = teleportTo.transform.position;
        player.transform.rotation = teleportTo.transform.rotation;
        StartCoroutine(MoveCredits());
    }

    private IEnumerator MoveCredits()
    {
        Vector3 startPosition = credits.transform.position;
        Vector3 endPosition = credits.transform.position + new Vector3(0, 22, 0);

        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            credits.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        
        credits.transform.position = endPosition;
        
        float start = -1.0f;
        float end = -0.375f;
        duration = 2.5f;
        time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            title.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(start, end, t));
            yield return null;
        }
    }
}
