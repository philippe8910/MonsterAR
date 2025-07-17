using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastUnlockButtonV2 : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button unlockButton;
    [SerializeField] private Image buttonImage;
    
    [Header("Lock Sprites")]
    [SerializeField] private Sprite lockedSprite;    // 鎖住的圖片
    [SerializeField] private Sprite unlockedSprite;  // 解鎖的圖片
    
    [Header("Unlock State")]
    [SerializeField] private bool isUnlocked = false;
    
    private ChoseTargetDetected choseTargetDetected;
    
    private void Awake()
    {
        // 如果沒有手動指定按鈕，就使用當前物件的按鈕
        if (unlockButton == null)
            unlockButton = GetComponent<Button>();
            
        // 如果沒有手動指定圖片組件，就使用當前物件的圖片
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
            
        // 尋找場景中的 ChoseTargetDetected 組件
        choseTargetDetected = FindAnyObjectByType<ChoseTargetDetected>();
        
        // 調試資訊
        Debug.Log("FastUnlockButtonV2: 初始化完成");
        Debug.Log($"FastUnlockButtonV2: Button={unlockButton != null}, Image={buttonImage != null}");
        Debug.Log($"FastUnlockButtonV2: ChoseTargetDetected={choseTargetDetected != null}");
    }
    
    private void Start()
    {
        // 不移除原有的監聽器，直接添加新的
        if (unlockButton != null)
        {
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
            Debug.Log("FastUnlockButtonV2: 按鈕點擊事件已添加");
        }
        
        // 初始化按鈕狀態
        UpdateButtonState();
        Debug.Log($"FastUnlockButtonV2: 初始狀態 isUnlocked={isUnlocked}");
    }
    
    /// <summary>
    /// 快速解鎖按鈕點擊事件 - 這個方法也可以在 Unity Inspector 中手動設定
    /// </summary>
    public void OnUnlockButtonClicked()
    {
        Debug.Log("FastUnlockButtonV2: 按鈕被點擊");
        Debug.Log($"FastUnlockButtonV2: 點擊前狀態 isUnlocked={isUnlocked}");
        
        // 如果尚未解鎖，執行解鎖
        if (!isUnlocked)
        {
            // 設定為解鎖狀態
            SetUnlockState(true);
            
            Debug.Log($"FastUnlockButtonV2: 解鎖後狀態 isUnlocked={isUnlocked}");
            
            // 執行解鎖所有卡片
            if (choseTargetDetected != null)
            {
                Debug.Log("FastUnlockButtonV2: 執行 AllLockoff");
                choseTargetDetected.AllLockoff();
            }
        }
        else
        {
            Debug.Log("FastUnlockButtonV2: 已經解鎖，無需再次操作");
        }
    }
    
    /// <summary>
    /// 切換解鎖狀態 (已廢棄 - 改為一次性解鎖)
    /// </summary>
    [System.Obsolete("使用 SetUnlockState(true) 代替")]
    public void ToggleUnlockState()
    {
        // 只允許從鎖定變為解鎖
        if (!isUnlocked)
        {
            isUnlocked = true;
            Debug.Log($"FastUnlockButtonV2: 狀態切換到 isUnlocked={isUnlocked}");
            UpdateButtonState();
        }
    }
    
    /// <summary>
    /// 設定解鎖狀態
    /// </summary>
    /// <param name="unlocked">是否解鎖</param>
    public void SetUnlockState(bool unlocked)
    {
        isUnlocked = unlocked;
        UpdateButtonState();
    }
    
    /// <summary>
    /// 更新按鈕的視覺狀態
    /// </summary>
    private void UpdateButtonState()
    {
        Debug.Log($"FastUnlockButtonV2: 更新按鈕狀態 isUnlocked={isUnlocked}");
        
        if (buttonImage != null)
        {
            // 根據解鎖狀態更換圖片
            if (isUnlocked)
            {
                if (unlockedSprite != null)
                {
                    buttonImage.sprite = unlockedSprite;
                    Debug.Log("FastUnlockButtonV2: 設定為解鎖圖片");
                }
                else
                {
                    Debug.LogError("FastUnlockButtonV2: unlockedSprite 為 null");
                }
            }
            else
            {
                if (lockedSprite != null)
                {
                    buttonImage.sprite = lockedSprite;
                    Debug.Log("FastUnlockButtonV2: 設定為鎖定圖片");
                }
                else
                {
                    Debug.LogError("FastUnlockButtonV2: lockedSprite 為 null");
                }
            }
        }
        else
        {
            Debug.LogError("FastUnlockButtonV2: buttonImage 為 null");
        }
        
        // 可選：根據狀態調整按鈕的可點擊性
        if (unlockButton != null)
        {
            unlockButton.interactable = true; // 始終可點擊
        }
    }
    
    /// <summary>
    /// 重置為鎖定狀態
    /// </summary>
    public void ResetToLocked()
    {
        SetUnlockState(false);
    }
    
    /// <summary>
    /// 檢查當前是否為解鎖狀態
    /// </summary>
    /// <returns>是否已解鎖</returns>
    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}