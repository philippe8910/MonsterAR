using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SummonerDetected : MonoBehaviour
{
    [SerializeField] private GameObject[] theDemons;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject scanFxPrefab;
    [SerializeField] private GameObject winFxPrefab;

    private ObserverBehaviour observer; // 追蹤用
    private Status previousStatus = Status.NO_POSE;

    private void Awake()
    {
        RestDemonsObject();
    }

    private void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnDestroy()
    {
        if (observer != null)
        {
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        var current = targetStatus.Status;

        if ((current == Status.TRACKED || current == Status.EXTENDED_TRACKED)
            && (previousStatus != Status.TRACKED && previousStatus != Status.EXTENDED_TRACKED))
        {
            // 剛偵測到：觸發掃描行為
            OnScanTarget();
        }

        if (current == Status.NO_POSE && previousStatus != Status.NO_POSE)
        { 
            //掉追蹤：重置場景
            OnLostTarget();
        }

        previousStatus = current;
    }

    public void OnScanTarget()
    {
        int target = PlayerPrefs.GetInt("TargetNumber", 0);
        FindObjectOfType<DemonsDetectedManager>().FindDemons();
        theDemons[target].SetActive(true);
        OnScanTargetFX();
    }

    public void OnLostTarget()
    {
        Debug.Log("LOST");
        RestDemonsObject();
    }

    public void RefreshDemonView()
    {
        RestDemonsObject();
        OnScanTarget();
    }

    private void RestDemonsObject()
    {
        foreach (var demon in theDemons)
        {
            if (demon != null)
                demon.SetActive(false);
        }
    }

    public void OnScanTargetFX()
    {
        Debug.Log("FX");
        if (scanFxPrefab == null || spawnPoint == null) return;

        GameObject fx = Instantiate(scanFxPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        fx.transform.localPosition = Vector3.zero;
        fx.transform.localRotation = Quaternion.identity;
    }

    public void OnWinFX()
    {
        Debug.Log("FX");
        if (winFxPrefab == null || spawnPoint == null) return;

        GameObject fx = Instantiate(winFxPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        fx.transform.localPosition = Vector3.zero;
        fx.transform.localRotation = Quaternion.identity;
    }
}
