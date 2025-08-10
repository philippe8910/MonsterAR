using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// 付費驗證系統 - 從GitHub Pages讀取JSON配置來驗證用戶付費狀態
/// </summary>
public class PaymentValidator : MonoBehaviour
{
    [Header("GitHub Pages 設定")]
    [SerializeField] private string configUrl = "https://alals93vu6.github.io/Money-Return-Detector/payment-config.json";
    [SerializeField] private string apiUrl = "https://alals93vu6.github.io/Money-Return-Detector/api.html?api=true";
    
    [Header("UI 組件")]
    [SerializeField] private GameObject watermarkUI;           // 浮水印UI
    [SerializeField] private Text watermarkText;               // 浮水印文字
    [SerializeField] private GameObject overdueDialogUI;       // 逾期對話框UI
    [SerializeField] private Text overdueMessageText;          // 逾期訊息文字
    [SerializeField] private GameObject networkErrorUI;        // 網路錯誤UI
    [SerializeField] private Text networkErrorText;            // 網路錯誤文字
    
    [Header("驗證設定")]
    [SerializeField] private bool enableDebugMode = true;      // 除錯模式
    [SerializeField] private bool validateOnStart = true;      // 啟動時驗證
    
    // 私有變數
    private PaymentResponse currentResponse;
    private bool isValidating = false;
    
    // 簡化的JSON回應結構
    [System.Serializable]
    public class PaymentResponse
    {
        public int statusCode;
        public string message;
    }
    
    void Start()
    {
        // 隱藏所有UI
        HideAllUI();
        
        if (validateOnStart)
        {
            // 啟動時進行付費驗證
            StartCoroutine(ValidateOnStartup());
        }
        
        if (enableDebugMode)
        {
            Debug.Log("PaymentValidator: 遊戲啟動付費驗證開始");
        }
    }
    
    /// <summary>
    /// 啟動時驗證付費狀態
    /// </summary>
    private IEnumerator ValidateOnStartup()
    {
        if (isValidating) yield break;
        
        isValidating = true;
        
        if (enableDebugMode)
        {
            Debug.Log("PaymentValidator: 正在驗證付費狀態...");
        }
        
        // 下載付費狀態
        yield return StartCoroutine(DownloadPaymentStatus());
        
        if (currentResponse == null)
        {
            // 網路錯誤
            HandleStartupStatusCode(4);
        }
        else
        {
            // 根據回傳的statusCode處理
            HandleStartupStatusCode(currentResponse.statusCode);
        }
        
        isValidating = false;
    }
    
    /// <summary>
    /// 開始付費狀態驗證（保留給其他地方使用）
    /// </summary>
    public void StartValidation()
    {
        if (!isValidating)
        {
            StartCoroutine(ValidatePaymentStatus());
        }
    }
    
    /// <summary>
    /// 驗證付費狀態（一般用途）
    /// </summary>
    private IEnumerator ValidatePaymentStatus()
    {
        if (isValidating) yield break;
        
        isValidating = true;
        
        if (enableDebugMode)
        {
            Debug.Log("PaymentValidator: 開始驗證付費狀態");
        }
        
        // 下載JSON檔案
        yield return StartCoroutine(DownloadPaymentStatus());
        
        if (currentResponse == null)
        {
            HandleStatusCode(4); // 網路錯誤
        }
        else
        {
            // 根據statusCode處理
            HandleStatusCode(currentResponse.statusCode);
        }
        
        isValidating = false;
    }
    
    /// <summary>
    /// 下載付費狀態JSON
    /// </summary>
    private IEnumerator DownloadPaymentStatus()
    {
        int retryCount = 3;
        
        for (int i = 0; i < retryCount; i++)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(configUrl))
            {
                request.timeout = 10; // 10秒超時
                
                yield return request.SendWebRequest();
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        string jsonText = request.downloadHandler.text;
                        currentResponse = JsonUtility.FromJson<PaymentResponse>(jsonText);
                        
                        if (enableDebugMode)
                        {
                            Debug.Log($"PaymentValidator: JSON下載成功，statusCode: {currentResponse.statusCode}, message: {currentResponse.message}");
                        }
                        
                        yield break;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"PaymentValidator: JSON解析錯誤: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"PaymentValidator: 下載失敗 (第{i + 1}次): {request.error}");
                }
                
                if (i < retryCount - 1)
                {
                    yield return new WaitForSeconds(2f); // 重試前等待2秒
                }
            }
        }
        
        currentResponse = null;
    }
    
    /// <summary>
    /// 處理啟動時的狀態代碼
    /// </summary>
    private void HandleStartupStatusCode(int statusCode)
    {
        // 儲存付費狀態到PlayerPrefs（只儲存statusCode）
        PlayerPrefs.SetInt("PaymentStatus", statusCode);
        PlayerPrefs.Save();
        
        if (enableDebugMode)
        {
            Debug.Log($"PaymentValidator: 付費狀態已儲存 - StatusCode: {statusCode}");
        }
        
        switch (statusCode)
        {
            case 1:
                // 1. 已付費 -> 直接開始遊戲
                PlayerPrefs.SetInt("isPay", 1);
                StartGame();
                break;
                
            case 2:
                // 2. 未付費 -> 記住參數，進入遊戲（遊戲場景會顯示浮水印）
                PlayerPrefs.SetInt("isPay", 0);
                StartGame();
                break;
                
            case 3:
                // 3. 嚴重拖欠 -> 提示付費，5秒後結束遊戲
                ShowPaymentRequiredDialog();
                break;
                
            case 4:
            default:
                // 4. 沒有網路 -> 提示檢查網路，要求重啟
                ShowNetworkErrorDialog();
                break;
        }
    }
    
    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void StartGame()
    {
        if (enableDebugMode)
        {
            Debug.Log("PaymentValidator: 付費驗證完成，允許進入遊戲");
        }
        
        // 尋找ReStartGame腳本來控制遊戲流程
        ReStartGame restartScript = FindObjectOfType<ReStartGame>();
        if (restartScript != null)
        {
            if (enableDebugMode)
            {
                Debug.Log("PaymentValidator: 找到ReStartGame腳本，交由其控制遊戲流程");
            }
            restartScript.OnReset(); // 呼叫ReStartGame的OnReset方法
            // 讓ReStartGame繼續其正常流程
            // ReStartGame會負責載入SampleScene
        }
        
        // 隱藏PaymentValidator的UI
        HideAllUI();
    }
    
    /// <summary>
    /// 載入主場景的備用方法
    /// </summary>
    private IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(0.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
    /// <summary>
    /// 顯示付費要求對話框
    /// </summary>
    async private void ShowPaymentRequiredDialog()
    {
        if (overdueDialogUI != null)
        {
            overdueDialogUI.SetActive(true);
            
            if (overdueMessageText != null)
            {
                overdueMessageText.text = "您已長期遲繳費用，完成付費後將解鎖正常使用功能";
            }
            
            // 5秒後自動結束遊戲
            await Task.Delay(5000);
            Application.Quit();

            if (enableDebugMode)
            {
                Debug.Log("PaymentValidator: 顯示付費要求對話框，5秒後退出");
            }
        }
    }

    /// <summary>
    /// 顯示網路錯誤對話框
    /// </summary>
    async private void ShowNetworkErrorDialog()
    {
        if (networkErrorUI != null)
        {
            networkErrorUI.SetActive(true);
            
            if (networkErrorText != null)
            {
                networkErrorText.text = "請檢查網路環境以及連線狀態後再重啟";
            }

            await Task.Delay(5000);
            Application.Quit();

            if (enableDebugMode)
            {
                Debug.Log("PaymentValidator: 顯示網路錯誤對話框");
            }
        }
    }
    
    /// <summary>
    /// 處理狀態代碼（舊版，保留給其他地方使用）
    /// </summary>
    private void HandleStatusCode(int statusCode)
    {
        HideAllUI();
        
        switch (statusCode)
        {
            case 1:
                // 已付費 - 正常遊戲，不做任何動作
                if (enableDebugMode)
                {
                    Debug.Log("PaymentValidator: statusCode 1 - 已付費，正常使用");
                }
                break;
                
            case 2:
                // 未付費 - 顯示浮水印
                ShowWatermark();
                break;
                
            case 3:
                // 長期拖欠 - 顯示警告並準備退出
                ShowOverdueDialog();
                break;
                
            case 4:
            default:
                // 網路錯誤或其他 - 顯示錯誤訊息
                ShowNetworkError();
                break;
        }
    }
    
    /// <summary>
    /// 顯示浮水印
    /// </summary>
    private void ShowWatermark()
    {
        if (watermarkUI != null)
        {
            watermarkUI.SetActive(true);
            
            if (watermarkText != null)
            {
                watermarkText.text = "";
                watermarkText.fontSize = 20;
                
                Color color = watermarkText.color;
                color.a = 0.7f;
                watermarkText.color = color;
            }
            
            if (enableDebugMode)
            {
                Debug.Log("PaymentValidator: 顯示未付費浮水印");
            }
        }
    }
    
    /// <summary>
    /// 顯示逾期對話框
    /// </summary>
    private void ShowOverdueDialog()
    {
        if (overdueDialogUI != null)
        {
            overdueDialogUI.SetActive(true);
            
            if (overdueMessageText != null && currentResponse != null)
            {
                overdueMessageText.text = currentResponse.message;
            }
            
            // 5秒後退出應用程式
            StartCoroutine(ExitGameAfterDelay(5));
            
            if (enableDebugMode)
            {
                Debug.Log("PaymentValidator: 顯示逾期對話框，準備退出");
            }
        }
    }
    
    /// <summary>
    /// 顯示網路錯誤
    /// </summary>
    private void ShowNetworkError()
    {
        if (networkErrorUI != null)
        {
            networkErrorUI.SetActive(true);
            
            if (networkErrorText != null)
            {
                if (currentResponse != null)
                {
                    networkErrorText.text = currentResponse.message;
                }
                else
                {
                    networkErrorText.text = "無法連接到伺服器，請檢查網路連線並重新啟動應用程式";
                }
            }
            
            if (enableDebugMode)
            {
                Debug.Log("PaymentValidator: 顯示網路錯誤訊息");
            }
        }
    }
    
    /// <summary>
    /// 延遲後退出遊戲
    /// </summary>
    private IEnumerator ExitGameAfterDelay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        if (enableDebugMode)
        {
            Debug.Log("PaymentValidator: 應用程式即將退出");
        }
        
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    /// <summary>
    /// 隱藏所有UI
    /// </summary>
    private void HideAllUI()
    {
        if (watermarkUI != null) watermarkUI.SetActive(false);
        if (overdueDialogUI != null) overdueDialogUI.SetActive(false);
        if (networkErrorUI != null) networkErrorUI.SetActive(false);
    }
    
    /// <summary>
    /// 重試按鈕點擊事件
    /// </summary>
    public void OnRetryButtonClicked()
    {
        StartValidation();
    }
    
    /// <summary>
    /// 退出按鈕點擊事件
    /// </summary>
    public void OnExitButtonClicked()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}