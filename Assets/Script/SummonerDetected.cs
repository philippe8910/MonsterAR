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
        OnScanTargetFX();
        
        // 新增：惡魔出現時設定為暗黑狀態
        SetDarkState(target);
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
        if (index >= 0 && index < theDemons.Length && demonMats[index] != null)
        {
            isInDarkState[index] = true;
            
            // 設定暗黑效果
            float darkFactor = 0.2f; // 暗化程度
            Color darkColor = originalColors[index] * darkFactor;
            demonMats[index].color = darkColor;
            
            // 關閉發光效果
            demonMats[index].SetColor("_EmissionColor", Color.black);
            
            Debug.Log($"惡魔 {index} 進入暗黑狀態");
        }
    }

    // 新增：讓第 x 個 demon 恢復正常狀態（配合煙霧特效）
    public void RestoreNormalState(int index)
    {
        if (index >= 0 && index < theDemons.Length && demonMats[index] != null && isInDarkState[index])
        {
            // 播放煙霧特效做為障眼法
            OnScanTargetFX();
            
            // 延遲恢復正常狀態，讓煙霧有時間遮蔽
            StartCoroutine(RestoreAfterDelay(index, 0.3f));
        }
    }

    // 新增：外部接口 - 讓當前惡魔現身
    public void ShowCurrentDemon()
    {
        int target = PlayerPrefs.GetInt("TargetNumber", 0);
        RestoreNormalState(target);
        Debug.Log($"惡魔 {target} 現身");
    }

    // 延遲恢復正常狀態的協程
    private IEnumerator RestoreAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (index >= 0 && index < theDemons.Length && demonMats[index] != null)
        {
            isInDarkState[index] = false;
            
            // 恢復原始顏色
            demonMats[index].color = originalColors[index];
            
            // 關閉發光效果
            demonMats[index].SetColor("_EmissionColor", Color.black);
            
            Debug.Log($"惡魔 {index} 恢復正常狀態");
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
