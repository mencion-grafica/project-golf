using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private TransitionAnimator SceneTransition;
    public void onClick()
    {
        StartCoroutine(TransitionSceneChange());
    }

    public IEnumerator TransitionSceneChange()
    {
        SceneTransition.StartTransition();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }

    public void agustinMeCagoEnTusMuertos()
    {
        SceneManager.LoadScene(1);
    }

}
