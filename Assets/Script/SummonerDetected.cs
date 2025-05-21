using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerDetected : MonoBehaviour
{
    [SerializeField] private GameObject[] theDemons;
    // Start is called before the first frame update
    private void Awake()
    {
        RestDemonsObject();
    }

    public void OnScanTarget()
    {
        var target = PlayerPrefs.GetInt("TargetNumber");
        theDemons[target].SetActive(true);
    }

    public void OnLostTarget()
    {
        RestDemonsObject();
    }

    public void RefreshDemonView()
    {
        RestDemonsObject();  // ��������
        OnScanTarget();      // �ھ� PlayerPrefs �A�}�����c�]
    }

    private void RestDemonsObject()
    {
        for (int i = 0; i < theDemons.Length; i++)
        {
            theDemons[i].SetActive(false);
        }
    }
}
