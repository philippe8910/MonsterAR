using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonsDetectedManager : MonoBehaviour
{
    [Header("檢測區域")]
    [SerializeField] private bool findDemons;
    [SerializeField] private int choseACard;
    [SerializeField] private int choseOCard;
    [SerializeField] private int choseRCard;

    [Header("控制UI物件")]
    [SerializeField] UIManager UIctrl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void FindTest()
    {
        Debug.Log("Find!!");
    }
}
