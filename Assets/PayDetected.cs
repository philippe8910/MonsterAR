using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayDetected : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"PayDetected Start {PlayerPrefs.GetInt("isPay")}");
        if (PlayerPrefs.GetInt("isPay") == 1)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
