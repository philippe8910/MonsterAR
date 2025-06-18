using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SummonerDetected : MonoBehaviour
{
    [SerializeField] public GameObject[] theDemons;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject scanFxPrefab;
    [SerializeField] private GameObject winFxPrefab;

    private ObserverBehaviour observer; // 追蹤用
    private Status previousStatus = Status.NO_POSE;

    [SerializeField] private Material[] demonMats;

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

        // ✅ 修正：從子物件中抓 SkinnedMeshRenderer 並複製 Element 1 (DeadFX)
        demonMats = new Material[theDemons.Length];
        for (int i = 0; i < theDemons.Length; i++)
        {
            if (theDemons[i] != null)
            {
                var renderer = theDemons[i].GetComponentInChildren<SkinnedMeshRenderer>();
                if (renderer != null && renderer.materials.Length > 1)
                {
                    Material[] mats = renderer.materials;
                    mats[1] = new Material(mats[1]); // clone Element 1
                    demonMats[i] = mats[1];          // 儲存以供外部淡白
                    renderer.materials = mats;       // 套回 renderer
                }
            }
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
            OnScanTarget();
        }

        if (current == Status.NO_POSE && previousStatus != Status.NO_POSE)
        {
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

    // 外部接口：讓第 x 個 demon 的 DeadFX 淡白
    public void FadeToWhite(int index)
    {
        if (index >= 0 && index < theDemons.Length && demonMats[index] != null)
        {
            StartCoroutine(FadeMaterialToWhite(demonMats[index]));
        }
    }

    // 淡入 Coroutine
    private IEnumerator FadeMaterialToWhite(Material mat)
    {
        float duration = 1.5f;
        float t = 0f;
        Color original = mat.color;
        float maxWhiteBlend = 0.4f;
        float maxEmission = 0.4f;

        Color targetWhite = Color.Lerp(original, Color.white, maxWhiteBlend);
        mat.EnableKeyword("_EMISSION");

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // 淡入至目標白度
            mat.color = Color.Lerp(original, targetWhite, t);

            // 柔和發光
            mat.SetColor("_EmissionColor", Color.white * (t * maxEmission));

            yield return null;
        }

        mat.color = targetWhite;
        mat.SetColor("_EmissionColor", Color.white * maxEmission);
    }

}
