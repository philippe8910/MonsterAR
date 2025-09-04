using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoseTargetDetected : MonoBehaviour
{
    [Header("�d�P")]
    [SerializeField] private ARButtonController[] targetCard;
    [SerializeField] public int[] Targetindex;
    [SerializeField] private int nowType;

    [Header("��P����")]
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
        FindFirstObjectByType<AudioManager>().cardSound();
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
        targetCard[offLockIndex].GetComponent<ARButtonController>().EnableButton();
        targetCard[offLockIndex].GetComponent<ARButtonController>().isFind = true;
        //FindAnyObjectByType<DemonsDetectedManager>().OnStartDetected();
    }

    public void AllLockoff()
    {
        for (int i = 0; i < targetCard.Length; i++)
        {
            targetCard[i].GetComponent<ARButtonController>().EnableButton();
        }
        
        // 注意：不在這裡更新快速解鎖按鈕狀態，因為這個方法是由按鈕自己呼叫的
        Debug.Log("ChoseTargetDetected: 所有卡片已解鎖");
    }

    public void LockAllCards()
    {
        for (int i = 0; i < targetCard.Length; i++)
        {
            targetCard[i].GetComponent<ARButtonController>().ButtonDetected();
        }
        Debug.Log("ChoseTargetDetected: 所有卡片已鎖定");
    }
}
