using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLight : MonoBehaviour
{
    [SerializeField] private GameObject idleIndicator;
    [SerializeField] private GameObject ActiveIndicator;

    private void Start()
    {
        
        if (idleIndicator != null)
        {
            idleIndicator.SetActive(true);
        }

        if(ActiveIndicator != null)
        {
            ActiveIndicator.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //Debug.Log("IndicatorLight triggered by Player");
            ActiveIndicator.SetActive(true);
            idleIndicator.SetActive(false);
            StartCoroutine(FlashColor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //Debug.Log("IndicatorLight off Player");
            ActiveIndicator.SetActive(false);
            idleIndicator.SetActive(true);
        }
    }

    private System.Collections.IEnumerator FlashColor()
    {
        if (ActiveIndicator != null)
        {
            //ActiveIndicator.SetActive(true);
            yield return new WaitForSeconds(.2f);
            ActiveIndicator.SetActive(false);
            idleIndicator.SetActive(true);
        }
    }
}
