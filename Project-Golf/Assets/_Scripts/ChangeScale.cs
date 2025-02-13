using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeScale : MonoBehaviour
{
    private Vector3 initialScale;
    [SerializeField]
    private List<float> magnitudes;
    private List<float> means;
    private float mass;

    private void Awake()
    {
        initialScale = transform.localScale;
        mass = this.GetComponent<Rigidbody>().mass;

        means = new List<float>();

        for(int i = 0; i < magnitudes.Count; i++)
        {
            means.Add(((magnitudes[i] * initialScale.x) + initialScale.x) / 2);
            Debug.Log(means[i]);
        }
    }

    public void Rescale()
    {
        if(transform.localScale.x < means[0])
        {
            transform.localScale = initialScale * magnitudes[0];
        }
        else if(transform.localScale.x < means[1])
        {
            transform.localScale = initialScale * magnitudes[1];
        }
        else
        {
            transform.localScale = initialScale * magnitudes[2];
        }

        Debug.Log(transform.localScale);
    }
}
