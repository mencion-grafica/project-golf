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
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2.0f);
        LevelManager.Instance.NextLevel();
    }
}