using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARTest : MonoBehaviour
{
    [SerializeField] public bool isFound = false;

    public void OnObjectFound()
    {
        isFound = true;
        Debug.Log("���F");
    }

    public void OnObjectLose()
    {
        isFound = false;
        Debug.Log("�򥢤F");
    }
}
