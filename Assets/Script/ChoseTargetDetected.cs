using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoseTargetDetected : MonoBehaviour
{
    [Header("¥dµP")]
    [SerializeField] private ARButtonController[] targetCard;
    [SerializeField] private int[] Targetindex;
    [SerializeField] private int nowType;

    [Header("¿ïµP¤¶­±")]
    [SerializeField] private GameObject[] choseCardObject;

    // Start is called before the first frame update
    void Start()
    {
        for(int i =  0; i < choseCardObject.Length; i++)
        {
            choseCardObject[i].SetActive(false);
        }
    }

    public void OnOpenChoseCardPage(int cardType)
    {
        nowType = cardType;
        choseCardObject[nowType].SetActive(true);
    }

    public void OnChoseTargetCard(int cardIndex)
    {
        Targetindex[nowType] = cardIndex;
        targetCard[nowType].ChangeSprite(cardIndex);
        SyncIndex();
        OnCrossChoseObject();
    }

    public void OnCrossChoseObject()
    {
        choseCardObject[nowType].SetActive(false);
    }

    public void SyncIndex()
    {
        var Detected = FindAnyObjectByType<DemonsDetectedManager>();
        Detected.OnSyncIndex(Targetindex[1], Targetindex[2], Targetindex[3]);
    }

    public void OnButtonLockoff(int offLockIndex)
    {
        targetCard[offLockIndex].GetComponent<Button>().interactable = true;
    }

    public void AllLockoff()
    {
        for (int i = 0; i < targetCard.Length; i++)
        {
            targetCard[i].GetComponent<Button>().interactable = true;
        }
    }
}
