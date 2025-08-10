using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 遊戲場景中的浮水印控制器
/// 根據PaymentValidator儲存的付費狀態決定是否顯示浮水印
/// </summary>
public class InGameWatermarkController : MonoBehaviour
{
    [Header("浮水印UI組件")]
    [SerializeField] private GameObject watermarkUI;           // 浮水印UI
    [SerializeField] private Text watermarkText;               // 浮水印文字
    [SerializeField] private Image watermarkImage;             // 浮水印圖片（如果有的話）
    
    [Header("浮水印設定")]
    [SerializeField] private string defaultWatermarkText = "未付費版本";
    [SerializeField] private float watermarkOpacity = 0.7f;
    [SerializeField] private int watermarkFontSize = 20;
    [SerializeField] private Color watermarkColor = Color.white;
    [SerializeField] private bool enableDebugMode = true;
    
    void Start()
    {
        CheckAndShowWatermark();
    }
    
    /// <summary>
    /// 檢查付費狀態並決定是否顯示浮水印
    /// </summary>
    private void CheckAndShowWatermark()
    {
        // 從PlayerPrefs讀取付費狀態（只讀取statusCode）
        int paymentStatus = PlayerPrefs.GetInt("PaymentStatus", 2); // 預設為未付費
        
        if (enableDebugMode)
        {
            Debug.Log($"InGameWatermarkController: 讀取付費狀態 - StatusCode: {paymentStatus}");
        }
        
        switch (paymentStatus)
        {
            case 1:
                // 已付費 - 隱藏浮水印
                HideWatermark();
                if (enableDebugMode)
                {
                    Debug.Log("InGameWatermarkController: 已付費，隱藏浮水印");
                }
                break;
                
            case 2:
                // 未付費 - 顯示浮水印
                ShowWatermark(defaultWatermarkText);
                if (enableDebugMode)
                {
                    Debug.Log("InGameWatermarkController: 未付費，顯示浮水印");
                }
                break;
                
            default:
                // 其他狀態 - 不應該到達這裡，因為statusCode 3和4會在啟動時處理
                HideWatermark();
                if (enableDebugMode)
                {
                    Debug.Log($"InGameWatermarkController: 意外的狀態碼 {paymentStatus}，隱藏浮水印");
                }
                break;
        }
    }
    
    /// <summary>
    /// 顯示浮水印
    /// </summary>
    private void ShowWatermark(string message)
    {
        if (watermarkUI != null)
        {
            watermarkUI.SetActive(true);
            
            if (watermarkText != null)
            {
                watermarkText.text = message;
                watermarkText.fontSize = watermarkFontSize;
                
                // 設定顏色和透明度
                Color color = watermarkColor;
                color.a = watermarkOpacity;
                watermarkText.color = color;
            }
            
            // 如果有浮水印圖片也設定透明度
            if (watermarkImage != null)
            {
                Color imageColor = watermarkImage.color;
                imageColor.a = watermarkOpacity;
                watermarkImage.color = imageColor;
            }
        }
    }
    
    /// <summary>
    /// 隱藏浮水印
    /// </summary>
    private void HideWatermark()
    {
        if (watermarkUI != null)
        {
            watermarkUI.SetActive(false);
        }
    }
    
    /// <summary>
    /// 手動刷新浮水印狀態（可供外部調用）
    /// </summary>
    public void RefreshWatermark()
    {
        CheckAndShowWatermark();
    }
    
    /// <summary>
    /// 設定浮水印文字（可供外部調用）
    /// </summary>
    public void SetWatermarkText(string text)
    {
        if (watermarkText != null)
        {
            watermarkText.text = text;
        }
    }
    
    /// <summary>
    /// 強制顯示浮水印（可供測試使用）
    /// </summary>
    public void ForceShowWatermark(string message = null)
    {
        ShowWatermark(message ?? defaultWatermarkText);
    }
    
    /// <summary>
    /// 強制隱藏浮水印（可供測試使用）
    /// </summary>
    public void ForceHideWatermark()
    {
        HideWatermark();
    }
}