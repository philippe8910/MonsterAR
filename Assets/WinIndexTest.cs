using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinIndexTest : MonoBehaviour
{

    [SerializeField] public int winIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetIndex()
    {
        PlayerPrefs.SetInt("WinIndex", winIndex);
        Debug.Log($"Reset:{PlayerPrefs.GetInt("WinIndex")}");
    }
}
