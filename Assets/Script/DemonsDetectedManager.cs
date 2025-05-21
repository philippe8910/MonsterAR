using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DemonsDetectedManager : MonoBehaviour
{
    [Header("檢測區域")]
    [SerializeField] private bool findDemons;
    [SerializeField] public int choseRCard;
    [SerializeField] public int choseOCard;
    [SerializeField] public int choseACard;

    [Header("控制UI物件")]
    [SerializeField] UIManager UIctrl;
    [SerializeField] Button startDetected;
    [SerializeField] Text detectedButtonTX;
    [SerializeField] SettlementManager settlement;

    [Header("提示圖片相關")]
    [SerializeField]  public Image hintImage;
    [SerializeField]  public Sprite successSprite;
    [SerializeField]  public Sprite failSprite;
    [SerializeField]  public float fadeDuration = 0.5f;

    [Header("機會次數")]
    [SerializeField] public int attemptsLeft;

    // Start is called before the first frame update
    void Start()
    {
        startDetected.interactable = false;
        attemptsLeft = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            startDetected.interactable = true;
            detectedButtonTX.text = "捕捉惡魔!";
        }
    }

    public void OnStartDetected()
    {
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
        WinDetected(isArrests);
    }

    private async void WinDetected(bool isWin)
    {
        if (isWin)
        {
            ShowHintImage(true);
            await Task.Delay(4000);
            /*FindObjectOfType<SceneTransition>().CallTransition();
            await Task.Delay(1000);
            settlement.winPage.SetActive(true);*/
        }
        else
        {
            if (attemptsLeft != 0)
            {
                attemptsLeft--;
                ShowHintImage(false);
            }
            else
            {
                ShowHintImage(false);//測丸要刪
                /*FindObjectOfType<SceneTransition>().CallTransition();
                await Task.Delay(1000);
                settlement.losePage.SetActive(true);*/
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
        StopAllCoroutines(); // 防止上一次的動畫未結束
        StartCoroutine(HintSequence(isWin));
    }

    private IEnumerator HintSequence(bool isWin)
    {
        // 切換圖片
        hintImage.sprite = isWin ? successSprite : failSprite;

        // 初始透明度
        Color c = hintImage.color;
        c.a = 0f;
        hintImage.color = c;
        hintImage.gameObject.SetActive(true);

        // 淡入
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            hintImage.color = c;
            yield return null;
        }

        // 如果是失敗 → 停 3 秒再淡出
        if (!isWin)
        {
            yield return new WaitForSeconds(3f);

            // 淡出
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
    }
}
