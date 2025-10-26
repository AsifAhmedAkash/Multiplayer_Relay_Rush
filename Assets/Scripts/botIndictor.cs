using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botIndictor : MonoBehaviour
{
    
    [SerializeField] private GameObject ActiveGameobj;

    [SerializeField] private Animator botAnimator;

    private void Start()
    {

        

        if (ActiveGameobj != null)
        {
            ActiveGameobj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //Debug.Log("IndicatorLight triggered by Player");
            ActiveGameobj.SetActive(true);
            
            if(botAnimator != null)
            {
                //botAnimator.Play("active");
            }
            StartCoroutine(FlashColor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //Debug.Log("IndicatorLight off Player");
            ActiveGameobj.SetActive(false);
            
        }
    }

    private System.Collections.IEnumerator FlashColor()
    {
        if (ActiveGameobj != null)
        {
            //ActiveIndicator.SetActive(true);
            yield return new WaitForSeconds(.2f);
            ActiveGameobj.SetActive(false);
            
            if (botAnimator != null)
            {
                //botAnimator.SetBool("Wave", false);
            }
        }
    }
}
