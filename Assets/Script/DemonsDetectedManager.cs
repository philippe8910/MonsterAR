using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonsDetectedManager : MonoBehaviour
{
    [Header("�˴��ϰ�")]
    [SerializeField] private bool findDemons;
    [SerializeField] private int choseACard;
    [SerializeField] private int choseOCard;
    [SerializeField] private int choseRCard;

    [Header("����UI����")]
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
