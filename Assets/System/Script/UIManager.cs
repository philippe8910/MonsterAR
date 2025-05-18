using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("SetUI")]
    public Button[] HomeSetUI;

    [Header("Main Screen")]
    public RectTransform MainScreen;
    public Button[] StartBtn;
    public Button QuitBtn;
    private Vector3 mainScreenOriginalPos;
    public int bossIndex = -1;

    [Header("Introduction Screen")]
    public RectTransform IntroductionScreen;
    public Button IntroStartBtn;
    private Vector3 introScreenOriginalPos;

    [Header("ChoseTarget Screen")]
    public RectTransform ChoseTargetScreen;
    public GameObject[] ScreenObject;
    public Button[] TargetBtn;
    public Sprite[] TargetDemon;
    public Image theTargetImage;
    public Button GameStartBtn;
    public Button RechoseBtn;
    public int targetIndex = -1;

    [Header("InGame Screen")]
    public Button inGameRechoseBtn;

    void Start()
    {
        if (MainScreen != null)
        {
            mainScreenOriginalPos = MainScreen.anchoredPosition;
        }

        if (IntroductionScreen != null)
        {
            introScreenOriginalPos = IntroductionScreen.anchoredPosition;
        }

        if (StartBtn != null && StartBtn.Length > 0)
        {
            for (int i = 0; i < StartBtn.Length; i++)
            {
                int capturedIndex = i;

                StartBtn[i].onClick.RemoveAllListeners();

                StartBtn[i].onClick.AddListener(() =>
                {
                    bossIndex = ++capturedIndex;
                    StartGame();
                });
            }
        }

        if (TargetBtn != null && TargetBtn.Length > 0)
        {
            for (int i = 0; i < TargetBtn.Length; i++)
            {
                int capturedIndex = i;

                TargetBtn[i].onClick.RemoveAllListeners();

                TargetBtn[i].onClick.AddListener(() =>
                {
                    targetIndex = capturedIndex;
                    ChoseTargetDemons();
                });
            }
        }

        if (QuitBtn != null)
        {
            QuitBtn.onClick.AddListener(QuitGame);
        }

        if (IntroStartBtn != null)
        {
            IntroStartBtn.onClick.AddListener(StartIntroduction);
        }

        if (GameStartBtn != null)
        {
            GameStartBtn.onClick.AddListener(ConfirmationTargets);
        }

        if (RechoseBtn != null)
        {
            RechoseBtn.onClick.AddListener(ReChoseTarget);
        }

        if (inGameRechoseBtn != null)
        {
            inGameRechoseBtn.onClick.AddListener(InGameReChoseTarget);
        }

        DisplayUbtton(false);
        ResetDetected();
    }

    public async void StartGame()
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        if (MainScreen != null)
        {
            MainScreen.DOAnchorPos(
                new Vector2(MainScreen.anchoredPosition.x + 1000, MainScreen.anchoredPosition.y),
                0.2f
            ).SetEase(Ease.OutQuad);
        }
        PlayerPrefs.SetInt("BossNumber", bossIndex);
        Debug.Log($"啟動遊戲，Boss為：{SetBossName(bossIndex)},代碼為{PlayerPrefs.GetInt("BossNumber")}");
    }

    public void ResetDetected()
    {
        if (PlayerPrefs.GetInt("ResetLevel") == 1)
        {
            if (MainScreen != null)
            {
                MainScreen.DOAnchorPos(
                    new Vector2(MainScreen.anchoredPosition.x + 1000, MainScreen.anchoredPosition.y),
                    0.2f
                ).SetEase(Ease.OutQuad);
            }
        }
    }

    private async void ChoseTargetDemons()
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        ScreenObject[0].SetActive(false);
        PlayerPrefs.SetInt("TargetNumber", targetIndex);
        theTargetImage.sprite = TargetDemon[PlayerPrefs.GetInt("TargetNumber")];
        Debug.Log($"選擇目標為：{SetBossName(targetIndex)},代碼為{PlayerPrefs.GetInt("TargetNumber")}");
    }

    private async void ReChoseTarget()
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        ScreenObject[0].SetActive(true);
    }

    private async void InGameReChoseTarget()
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        ScreenObject[0].SetActive(true);
        ScreenObject[1].SetActive(true);
    }

    private async void ConfirmationTargets()
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        ScreenObject[1].SetActive(false);
    }

    private string SetBossName(int checkIndex)
    {
        switch (checkIndex)
        {
            case 1:
                return "忽視魔!";
            case 2:
                return "偏見魔!";
            case 3:
                return "拒絕魔!";
            case 4:
                return "羞恥魔!";
            case 5:
                return "壓迫魔!";
            case 6:
                return "無助魔!";
            case 7:
                return "背叛魔!";
            case 8:
                return "孤單魔!";
            default:
                return "未知魔!";
        }
    }

    public void ResetMainScreen()
    {
        if (MainScreen != null)
        {
            MainScreen.DOAnchorPos(mainScreenOriginalPos, 0.5f)
                .SetEase(Ease.OutQuad);
        }
    }

    public async void StartIntroduction()
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        if (IntroductionScreen != null)
        {
            IntroductionScreen.DOAnchorPos(new Vector2(IntroductionScreen.anchoredPosition.x + 1000, IntroductionScreen.anchoredPosition.y), 0.5f).SetEase(Ease.OutQuad);
        }
        DisplayUbtton(true);
    }

    public void ResetIntroductionScreen()
    {
        if (IntroductionScreen != null)
        {
            IntroductionScreen.DOAnchorPos(introScreenOriginalPos, 0.5f)
                .SetEase(Ease.OutQuad);
        }
    }

    public void QuitGame()
    {
        Debug.Log("退出遊戲");
        Application.Quit();
    }

    public void DisplayUbtton(bool isDisplay)
    {
        for (int i = 0; i < HomeSetUI.Length; i++)
        {
            HomeSetUI[i].gameObject.SetActive(isDisplay);
        }
    }
}
