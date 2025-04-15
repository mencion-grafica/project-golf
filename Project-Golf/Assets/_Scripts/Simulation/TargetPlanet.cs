using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlanet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Asteroid")) return;
        Confetti confetti = GameObject.FindWithTag("Confetti").GetComponent<Confetti>();
        confetti?.StartConfetti();
        StartCoroutine(LevelManager.Instance.IsLastLevel() ? StartCinematic() : NextLevel());
    }

    private IEnumerator StartCinematic()
    {
        LevelManager.Instance.SetIsCinematic(true);
        yield return new WaitForSeconds(2.5f);
        
        SoundManager.Instance.PlayMusic(AudioMusic.SadMusic, false);
        var finalAsteroid = Instantiate(Resources.Load<GameObject>("BigFinalAsteroid"));
        StartCoroutine(DropAsteroid(finalAsteroid, 10000.0f, 125.0f, 74.0f));
    }

    private IEnumerator DropAsteroid(GameObject asteroid, float startY, float endY, float duration)
    {
        Lights light = GameObject.FindWithTag("Alarm").GetComponent<Lights>();
        light?.AlarmLights();
        
        asteroid.transform.position = new Vector3(asteroid.transform.position.x, startY, asteroid.transform.position.z);
        
        Vector3 startPosition = asteroid.transform.position;
        Vector3 endPosition = new Vector3(asteroid.transform.position.x, endY, asteroid.transform.position.z);
        
        float time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            asteroid.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        
        asteroid.transform.position = endPosition;
        
        light?.NormalLights();
        Credits credits = GameObject.FindWithTag("CreditsRoom").GetComponent<Credits>();
        credits?.StartCredits();
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2.0f);
        LevelManager.Instance.NextLevel();
    }
}