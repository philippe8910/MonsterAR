using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonsDetectedManager : MonoBehaviour
{
    [Header("ÀË´ú°Ï°ì")]
    [SerializeField] private bool findDemons;
    [SerializeField] public int choseRCard;
    [SerializeField] public int choseOCard;
    [SerializeField] public int choseACard;

    [Header("±±¨îUIª«¥ó")]
    [SerializeField] UIManager UIctrl;
    [SerializeField] Button startDetected;
    [SerializeField] Text detectedButtonTX;

    // Start is called before the first frame update
    void Start()
    {
        startDetected.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void FindDemons()
    {
        findDemons = true;
        CheckScenPrivileges();
    }

    public void OnSyncIndex(int Rcard,int Ocard,int Acrad)
    {
        choseRCard = Rcard;
        choseOCard = Ocard;
        choseACard = Acrad;
        CheckScenPrivileges();
    }

    private void CheckScenPrivileges()
    {
        if (choseACard != 0 && choseOCard != 0 && choseRCard != 0 && findDemons)
        {
            startDetected.interactable = true;
            detectedButtonTX.text = "®·®»´cÅ]!";
        }
    }
}
