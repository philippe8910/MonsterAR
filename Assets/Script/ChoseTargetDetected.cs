using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        OnCrossChoseObject();
    }

    public void OnCrossChoseObject()
    {
        choseCardObject[nowType].SetActive(false);
    }
}
