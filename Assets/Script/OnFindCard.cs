using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFindCard : MonoBehaviour
{
    [SerializeField] public int theIndex;

    public void OnFindCardIndex()
    {
        var targetCard = FindAnyObjectByType<ChoseTargetDetected>();
        targetCard.OnButtonLockoff(theIndex);
    }
}
