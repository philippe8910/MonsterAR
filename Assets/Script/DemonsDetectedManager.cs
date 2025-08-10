using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DemonsDetectedManager : MonoBehaviour
{
    [Header("�˴��ϰ�")]
    [SerializeField] private bool findDemons;
    [SerializeField] public int choseRCard;
    [SerializeField] public int choseOCard;
    [SerializeField] public int choseACard;

    [Header("����UI����")]
    [SerializeField] UIManager UIctrl;
    [SerializeField] Button showDemonButton;  // 「現身」按鍵
    [SerializeField] Button startDetected;    // 「封印」按鍵
    [SerializeField] Text detectedButtonTX;
    [SerializeField] SettlementManager settlement;

    [Header("���ܹϤ�����")]
    [SerializeField] public Image hintImage;
    [SerializeField] public Sprite successSprite;
    [SerializeField] public Sprite failSprite;
    [SerializeField] public Image gameOverimg;
    [SerializeField] public float fadeDuration = 0.5f;
    [SerializeField] public GameObject[] HPObject;
    

    [Header("���y����")]
    [SerializeField] public GameObject[] scanObject;

    [Header("���|����")]
    [SerializeField] public int attemptsLeft;
    private bool isDemonShown = false;  // 追蹤惡魔是否已現身
    private bool detectionResult = false;  // 儲存偵測結果
    
    [Header("階段性結束時需要隱藏的UI元素")]
    [SerializeField] private GameObject[] uiElementsToHideOnStageEnd;  // 需要在階段性結束時隱藏的UI元素
    [SerializeField] private GameObject hpUI;  // 血量UI（需要保留）
    [SerializeField] private GameObject settingsUI;  // 設定UI（需要保留）

    // Start is called before the first frame update
    void Start()
    {
        // 初始狀態：現身按鍵不可用，封印按鍵不可用
        if (showDemonButton != null) showDemonButton.interactable = false;
        startDetected.interactable = false;
        isDemonShown = false;
        
        attemptsLeft = 3;
        gameOverimg.gameObject.SetActive(false);
        
        // 重置快速解鎖按鈕狀態（延遲執行以避免初始化問題）
        StartCoroutine(ResetFastUnlockButtonDelayed());
    }

    public void FindDemons()
    {
        findDemons = true;
        CheckScenPrivileges();
    }

    public void OnSyncIndex(int Rcard,int Ocard,int Acrad)
    {
        choseRCard = Rcard;
        choseOCard = Ocard;
        choseACard = Acrad;
        CheckScenPrivileges();
    }

    private void CheckScenPrivileges()
    {
        if (choseACard != 0 && choseOCard != 0 && choseRCard != 0 && findDemons)
        {
            // 如果惡魔已現身，啟用封印按鍵
            if (isDemonShown)
            {
                startDetected.interactable = true;
            }
            // 如果惡魔未現身，啟用現身按鍵
            else if (showDemonButton != null)
            {
                showDemonButton.interactable = true;
            }
            //detectedButtonTX.text = "�����c�]!";
            //OnStartDetected();
        }
    }

    // 新增：現身按鍵處理（包含偵測邏輯）
    public void OnShowDemon()
    {
        if (!isDemonShown)
        {
            // 執行偵測邏輯
            var isArrests = false;
            if (PlayerPrefs.GetInt("BossNumber") == PlayerPrefs.GetInt("TargetNumber"))
                switch (PlayerPrefs.GetInt("BossNumber"))
                {
                    case 1:
                        isArrests = IgnoreDetected();
                        break;
                    case 2:
                        isArrests = PrejudiceDetected();
                        break;
                    case 3:
                        isArrests = RejectionDetected();
                        break;
                    case 4:
                        isArrests = ShameDetected();
                        break;
                    case 5:
                        isArrests = OppressionDetected();
                        break;
                    case 6:
                        isArrests = HelplessnessDetected();
                        break;
                    case 7:
                        isArrests = BetrayalDetected();
                        break;
                    case 8:
                        isArrests = LonelinessDetected();
                        break;
                }
            else isArrests = false;
            
            if (isArrests)
            {
                // 偵測成功：顯示成功圖像，惡魔現身，啟用封印按鍵
                ShowHintImage(true);
                FindFirstObjectByType<AudioManager>().SetSEAudio(7);
                // 觸發惡魔現身
                var summonerDetected = FindObjectOfType<SummonerDetected>();
                if (summonerDetected != null)
                {
                    summonerDetected.ShowCurrentDemon();
                }
                
                // 鎖定所有卡片按鈕，防止玩家更改組合
                var choseTargetDetected = FindObjectOfType<ChoseTargetDetected>();
                if (choseTargetDetected != null)
                {
                    choseTargetDetected.LockAllCards();
                }
                
                // 更新狀態
                isDemonShown = true;
                detectionResult = isArrests;
                
                // 禁用現身按鍵，啟用封印按鍵
                if (showDemonButton != null) showDemonButton.interactable = false;
                CheckScenPrivileges(); // 重新檢查權限，啟用封印按鍵
                
                Debug.Log("現身成功：偵測正確，惡魔現身，封印按鍵已啟用");
            }
            else
            {
                // 偵測失敗：扣血，惡魔維持黑黑狀態
                FindFirstObjectByType<AudioManager>().SetSEAudio(6);
                FindFirstObjectByType<AudioManager>().HPSound();
                attemptsLeft--;
                SetHPAnimation(attemptsLeft);
                
                // 只在第1、2次失敗時顯示失敗提示文字
                if (attemptsLeft > 0)
                {
                    ShowHintImage(false);
                }
                
                // 重置現身按鍵為可用狀態，讓玩家可以重新嘗試
                if (showDemonButton != null) showDemonButton.interactable = false;
                CheckScenPrivileges(); // 重新檢查權限，可能再次啟用現身按鍵
                
                Debug.Log($"現身失敗：偵測錯誤，扣血，剩餘生命: {attemptsLeft}");
                
                // 如果生命用盡，遊戲結束
                if (attemptsLeft == 0)
                {
                    // 防呆措施：直接關閉提示文字圖片，避免動畫衝突
                    if (hintImage != null)
                    {
                        hintImage.gameObject.SetActive(false);
                        Debug.Log("第三次失敗：強制關閉提示文字圖片，避免動畫衝突");
                    }
                    
                    // 遊戲結束，隱藏UI元素
                    HideUIElementsOnStageEnd();
                    
                    // 顯示血量歸零的提示圖片（移動位置）
                    ShowGameOverHint();
                    
                    gameOverimg.gameObject.SetActive(true);
                    var audio = FindAnyObjectByType<AudioManager>();
                    audio.SetSEAudio(1);
                    var demonsSummoner = FindObjectOfType<SummonerDetected>();
                    demonsSummoner.OnScanTargetFX();
                    
                    for (int i = 0; i < demonsSummoner.theDemons.Length; i++)
                    {
                        if (demonsSummoner.theDemons[i] != null)
                        {
                            demonsSummoner.theDemons[i].SetActive(false);
                        }
                    }
                    FadeInGameOverImage();
                    
                    StartCoroutine(GameOverSequence());
                }
            }
        }
    }

    public void OnStartDetected()
    {
        // 防止遊戲啟動時意外觸發
        if (choseACard == 0 || choseOCard == 0 || choseRCard == 0 || !findDemons || !isDemonShown)
        {
            Debug.LogWarning("OnStartDetected: 遊戲狀態不符合封印條件，忽略此次調用");
            return;
        }
        
        // 直接執行封印動作，使用之前在 OnShowDemon() 中儲存的偵測結果
        Debug.Log($"OnStartDetected: 執行封印，偵測結果: {detectionResult}");
        WinDetected(detectionResult);
    }

    // 新增：檢查惡魔是否已現身成功
    public bool IsDemonShown()
    {
        return isDemonShown;
    }

    private async void WinDetected(bool isWin)
    {
        var demonsSummoner = FindObjectOfType<SummonerDetected>();
        var audio = FindAnyObjectByType<AudioManager>();
        if (isWin)
        {
            // 成功的UI圖像已在 OnShowDemon() 中顯示，此處只執行封印動畫
            // 隱藏指定的UI元素，保留血量和設定按鈕
            HideUIElementsOnStageEnd();
            
            audio.SetSEAudio(2);
            demonsSummoner.theDemons[PlayerPrefs.GetInt("TargetNumber")].GetComponent<Animator>().Play("dead");
            await Task.Delay(1000);
            FindObjectOfType<SummonerDetected>().FadeToWhite(PlayerPrefs.GetInt("TargetNumber"));
            await Task.Delay(3000);
            demonsSummoner.OnWinFX();
            await Task.Delay(150);
            for (int i = 0; i < demonsSummoner.theDemons.Length; i++)
            {
                if (demonsSummoner.theDemons[i] != null)
                {
                    demonsSummoner.theDemons[i].SetActive(false);
                }
            }
            audio.SetSEAudio(3);
            await Task.Delay(1000);
            FindObjectOfType<SceneTransition>().CallTransition();
            await Task.Delay(1000);
            settlement.winPage.SetActive(true);
            audio.SetPromptsAudio(3);
            audio.SetSEAudio(0);
        }
        else
        {
            attemptsLeft--;
            SetHPAnimation(attemptsLeft);
            if (attemptsLeft != 0)
            {
                ShowHintImage(false);
                
                // 重置惡魔現身狀態，讓玩家可以重新嘗試
                isDemonShown = false;
                detectionResult = false;  // 重置偵測結果
                startDetected.interactable = false;
                
                // 解鎖所有卡片按鈕，讓玩家可以重新選擇組合
                var choseTargetDetected = FindObjectOfType<ChoseTargetDetected>();
                if (choseTargetDetected != null)
                {
                    choseTargetDetected.AllLockoff();
                }
                
                // 重新檢查權限，可能啟用現身按鍵
                CheckScenPrivileges();
            }
            else
            {
                // 防呆措施：直接關閉提示文字圖片，避免動畫衝突
                if (hintImage != null)
                {
                    hintImage.gameObject.SetActive(false);
                    Debug.Log("WinDetected失敗：強制關閉提示文字圖片，避免動畫衝突");
                }
                
                // 失敗且生命值為0，隱藏UI元素
                HideUIElementsOnStageEnd();
                
                // 顯示血量歸零的提示圖片（移動位置）
                ShowGameOverHint();
                
                gameOverimg.gameObject.SetActive(true);
                audio.SetSEAudio(1);
                demonsSummoner.OnScanTargetFX();
                //await Task.Delay(150);
                for (int i = 0; i < demonsSummoner.theDemons.Length; i++)
                {
                    if (demonsSummoner.theDemons[i] != null)
                    {
                        demonsSummoner.theDemons[i].SetActive(false);
                    }
                }
                FadeInGameOverImage();
                await Task.Delay(3000);
                FindObjectOfType<SceneTransition>().CallTransition();
                await Task.Delay(1000);
                settlement.losePage.SetActive(true);
            }
            
        }
    }

    public void ScanObjectCtrl(bool enable)
    {
        
        if (enable)
        {
            for (int i = 0; i < scanObject.Length; i++)
            {
                if (scanObject[i] != null)
                {
                   Instantiate(scanObject[i], transform);
                }
            }
        }
        else
        {
            GameObject[] arObjects = GameObject.FindGameObjectsWithTag("ARObject");
            foreach (var obj in arObjects)
            {
                Destroy(obj);
            }
        }
    }

    private bool IgnoreDetected()
    {
        if (choseOCard != 3) return false;
        if (choseRCard != 3 && choseRCard != 8) return false;
        if (choseACard != 3 && choseACard != 7) return false;
        return true;
    }

    private bool PrejudiceDetected()
    {
        if (choseOCard != 6) return false;
        if (choseRCard != 4 && choseRCard != 5) return false;
        if (choseACard != 2 && choseACard != 8) return false;
        return true;
    }

    private bool RejectionDetected()
    {
        if (choseOCard != 1) return false;
        if (choseRCard != 1 && choseRCard != 6) return false;
        if (choseACard != 1 && choseACard != 4) return false;
        return true;
    }

    private bool ShameDetected()
    {
        if (choseOCard != 7) return false;
        if (choseRCard != 2 && choseRCard != 4) return false;
        if (choseACard != 2 && choseACard != 6) return false;
        return true;
    }

    private bool OppressionDetected()
    {
        if (choseOCard != 8) return false;
        if (choseRCard != 5 && choseRCard != 7) return false;
        if (choseACard != 5 && choseACard != 8) return false;
        return true;
    }

    private bool HelplessnessDetected()
    {
        if (choseOCard != 4) return false;
        if (choseRCard != 7 && choseRCard != 8) return false;
        if (choseACard != 3 && choseACard != 5) return false;
        return true;
    }

    private bool BetrayalDetected()
    {
        if (choseOCard != 2) return false;
        if (choseRCard != 1 && choseRCard != 2) return false;
        if (choseACard != 1 && choseACard != 7) return false;
        return true;
    }

    private bool LonelinessDetected()
    {
        if (choseOCard != 5) return false;
        if (choseRCard != 3 && choseRCard != 6) return false;
        if (choseACard != 4 && choseACard != 6) return false;
        return true;
    }

    public void ShowHintImage(bool isWin)
    {
        StopAllCoroutines(); // ����W�@�����ʵe������
        StartCoroutine(HintSequence(isWin));
    }

    private IEnumerator HintSequence(bool isWin)
    {
        // �����Ϥ�
        hintImage.sprite = isWin ? successSprite : failSprite;

        // ��l�z����
        Color c = hintImage.color;
        c.a = 0f;
        hintImage.color = c;
        hintImage.gameObject.SetActive(true);

        // �H�J
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            hintImage.color = c;
            yield return null;
        }

        // 等待一段時間後淡出（勝利和失敗都要淡出）
        if (isWin)
        {
            // 勝利時等待較短時間後淡出
            yield return new WaitForSeconds(2f);
        }
        else
        {
            // 失敗時等待3秒後淡出
            yield return new WaitForSeconds(3f);
        }

        // 淡出動畫
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            hintImage.color = c;
            yield return null;
        }

        hintImage.gameObject.SetActive(false);
    }

    private async void SetHPAnimation(int playerHP)
    {
        HPObject[playerHP].SetActive(false);
        await Task.Delay(300);
        HPObject[playerHP].SetActive(true);
        await Task.Delay(300);
        HPObject[playerHP].SetActive(false);
        await Task.Delay(300);
        HPObject[playerHP].SetActive(true);
        await Task.Delay(300);
        HPObject[playerHP].SetActive(false);
    }

    public void FadeInGameOverImage()
    {
        StartCoroutine(FadeImageCoroutine());
    }



    IEnumerator FadeImageCoroutine()
    {
        float duration = 1f; // �H�X�һݬ���
        float currentTime = 0f;
        Color color = gameOverimg.color;
        color.a = 0f;
        gameOverimg.color = color;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            color.a = Mathf.Clamp01(currentTime / duration);
            gameOverimg.color = color;
            yield return null;
        }
    }
    
    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<SceneTransition>().CallTransition();
        yield return new WaitForSeconds(1f);
        settlement.losePage.SetActive(true);
    }
    
    private IEnumerator ResetFastUnlockButtonDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        
        // 查找 FastUnlockButton 組件
        var fastUnlockButton = FindAnyObjectByType<FastUnlockButton>();
        if (fastUnlockButton != null)
        {
            Debug.Log("DemonsDetectedManager: 找到 FastUnlockButton，重置為鎖定狀態");
            fastUnlockButton.ResetToLocked();
        }
        else
        {
            Debug.LogWarning("DemonsDetectedManager: 找不到 FastUnlockButton 組件");
        }
    }
    
    // 階段性結束時隱藏UI元素的方法
    private void HideUIElementsOnStageEnd()
    {
        // 停止提示文字的動畫協程並強制隱藏
        if (hintImage != null)
        {
            StopAllCoroutines();
            hintImage.gameObject.SetActive(false);
            Debug.Log("階段性結束：強制隱藏提示文字圖片");
        }
        
        // 隱藏指定的UI元素陣列中的所有元素
        if (uiElementsToHideOnStageEnd != null)
        {
            for (int i = 0; i < uiElementsToHideOnStageEnd.Length; i++)
            {
                if (uiElementsToHideOnStageEnd[i] != null)
                {
                    uiElementsToHideOnStageEnd[i].SetActive(false);
                    Debug.Log($"階段性結束：隱藏UI元素 - {uiElementsToHideOnStageEnd[i].name}");
                }
            }
        }
        
        // 確保血量UI和設定UI保持顯示（如果有設定的話）
        if (hpUI != null)
        {
            hpUI.SetActive(true);
            Debug.Log("階段性結束：確保血量UI保持顯示");
        }
        
        if (settingsUI != null)
        {
            settingsUI.SetActive(true);
            Debug.Log("階段性結束：確保設定UI保持顯示");
        }
        
        Debug.Log("階段性結束UI隱藏處理完成");
    }
    
    // 顯示血量歸零時的提示圖片
    private void ShowGameOverHint()
    {
        // 停止所有協程，防止動畫衝突
        StopAllCoroutines();
        
        // 確保提示文字圖片已關閉
        if (hintImage != null)
        {
            hintImage.gameObject.SetActive(false);
        }
        
        // 使用原本的 gameOverimg 來顯示血量歸零的提示
        // 這裡可以調整圖片的位置、大小等屬性
        if (gameOverimg != null)
        {
            // 調整提示圖片的位置（如果需要的話）
            RectTransform rectTransform = gameOverimg.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 這裡可以設定血量歸零提示圖片的位置
                // 例如：rectTransform.anchoredPosition = new Vector2(0, 100);
                Debug.Log("血量歸零提示圖片位置已調整");
            }
            
            Debug.Log("顯示血量歸零提示圖片，已停止所有動畫協程");
        }
        else
        {
            Debug.LogWarning("找不到 gameOverimg 組件");
        }
    }
}
