using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotActivator : MonoBehaviour
{
    [SerializeField] private GameObject Object;
    [SerializeField] private float ActivationDelay = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Object.SetActive(true);
            StartCoroutine(DeactivateAfterDelay(ActivationDelay));
        }
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Object.SetActive(false);
    }
}
