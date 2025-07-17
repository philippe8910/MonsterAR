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
    private Color[] originalColors; // 存儲原始顏色
    private bool[] isInDarkState;   // 追蹤是否處於暗黑狀態

    [SerializeField] private float rotationSpeed = 100f;
    private Vector3 lastMousePos;

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

        //  修正：從子物件中抓 SkinnedMeshRenderer 並複製 Element 1 (DeadFX)
        demonMats = new Material[theDemons.Length];
        originalColors = new Color[theDemons.Length];
        isInDarkState = new bool[theDemons.Length];
        
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
                    originalColors[i] = mats[1].color; // 存儲原始顏色
                    isInDarkState[i] = false;        // 初始狀態
                    renderer.materials = mats;       // 套回 renderer
                }
            }
        }
    }

    private void Update()
    {
        RotateDemons();
    }

    private void OnDestroy()
    {
        if (observer != null)
        {
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void RotateDemons()
    {
        float horizontal = 0f;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition; // 開始拖曳時記錄位置
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = Input.mousePosition;
            horizontal = (currentMousePos.x - lastMousePos.x) * 0.1f;
            lastMousePos = currentMousePos;
        }
#else
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
    {
        horizontal = Input.GetTouch(0).deltaPosition.x * 0.1f;
    }
#endif

        if (Mathf.Abs(horizontal) > 0.01f)
        {
            foreach (var demon in theDemons)
            {
                if (demon != null && demon.activeInHierarchy)
                {
                    demon.transform.Rotate(Vector3.up, -horizontal * rotationSpeed * Time.deltaTime);
                    Debug.Log("轉動中：" + horizontal);
                }
            }
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
        
        // 新增：惡魔出現時立刻設定為暗黑狀態（在煙霧前）
        SetDarkState(target);
        
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

    // 新增：讓第 x 個 demon 進入暗黑狀態
    public void SetDarkState(int index)
    {
        Debug.Log($"SetDarkState 被調用，index: {index}");
        
        if (index >= 0 && index < theDemons.Length && demonMats[index] != null)
        {
            isInDarkState[index] = true;
            
            // 參考 FadeMaterialToWhite 的邏輯，但改成黑色
            Material mat = demonMats[index];
            Color original = originalColors[index];
            
            // 混合到黑色（黑炭等級！）
            // Color targetDark = Color.Lerp(original, Color.black, 0.95f);  // 如果純黑太極端可以用這個
            
            // 直接設成純黑炭！
            Color targetDark = Color.black;
            
            // 設定顏色
            mat.color = targetDark;
            
            // 關閉發光效果
            mat.SetColor("_EmissionColor", Color.black);
            
            Debug.Log($"惡魔 {index} 進入暗黑狀態：原始 {original} → 暗黑 {targetDark}");
        }
        else
        {
            Debug.LogError($"SetDarkState 失敗：index={index}, 範圍={theDemons.Length}, mat存在={demonMats[index] != null}");
        }
    }

    // 新增：讓第 x 個 demon 恢復正常狀態（配合煙霧特效）
    public void RestoreNormalState(int index)
    {
        if (index >= 0 && index < theDemons.Length && demonMats[index] != null && isInDarkState[index])
        {
            // 立刻恢復正常狀態（在煙霧開始時）
            isInDarkState[index] = false;
            
            // 恢復原始顏色
            demonMats[index].color = originalColors[index];
            demonMats[index].SetColor("_EmissionColor", Color.black);
            
            // 播放煙霧特效做為障眼法
            OnScanTargetFX();
            
            Debug.Log($"惡魔 {index} 立刻恢復正常狀態，煙霧開始");
        }
    }

    // 新增：外部接口 - 讓當前惡魔現身
    public void ShowCurrentDemon()
    {
        int target = PlayerPrefs.GetInt("TargetNumber", 0);
        RestoreNormalState(target);
        Debug.Log($"惡魔 {target} 現身");
    }

    // 新增：對外接口 - 直接設定當前惡魔為黑色狀態（可綁定到AR外掛）
    public void SetCurrentDemonToDark()
    {
        // 檢查是否已經現身成功，如果已成功則不要變黑
        var demonsManager = FindObjectOfType<DemonsDetectedManager>();
        if (demonsManager != null && demonsManager.IsDemonShown())
        {
            Debug.Log("惡魔已現身成功，忽略變黑請求");
            return;
        }
        
        int target = PlayerPrefs.GetInt("TargetNumber", 0);
        SetDarkState(target);
        Debug.Log($"對外接口：惡魔 {target} 設定為暗黑狀態");
    }

    // 已移除：延遲恢復正常狀態的協程（改為立即恢復）

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
