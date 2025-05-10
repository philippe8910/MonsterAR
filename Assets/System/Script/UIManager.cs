using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    [Header("Main Screen")]
    public RectTransform MainScreen;
    public Button[] StartBtn;
    public Button QuitBtn;
    private Vector3 mainScreenOriginalPos;
    public int bossIndex = -1;

    [Header("Introduction Screen")]
    public RectTransform IntroductionScreen;
    public Button IntroStartBtn; // 介紹畫面的開始按鈕
    private Vector3 introScreenOriginalPos;

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

        if (QuitBtn != null)
        {
            QuitBtn.onClick.AddListener(QuitGame);
        }

        if (IntroStartBtn != null)
        {
            IntroStartBtn.onClick.AddListener(StartIntroduction);
        }
    }

    public async void StartGame()
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        if (MainScreen != null)
        {
            MainScreen.DOAnchorPos(
                new Vector2(MainScreen.anchoredPosition.x + 1000, MainScreen.anchoredPosition.y),
                0.5f
            ).SetEase(Ease.OutQuad);
        }

        Debug.Log($"🚀 啟動遊戲，Boss為：{ BossName()}");
    }

    private string BossName()
    {
        switch (bossIndex)
        {
            case 0:
                return "忽視魔!";
            case 1:
                return "偏見魔!";
            case 2:
                return "拒絕魔!";
            case 3:
                return "羞恥魔!";
            case 4:
                return "壓迫魔!";
            case 5:
                return "無助魔!";
            case 6:
                return "背叛魔!";
            case 7:
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
            IntroductionScreen.DOAnchorPos(new Vector2(IntroductionScreen.anchoredPosition.x + 1000, IntroductionScreen.anchoredPosition.y), 0.5f)
                .SetEase(Ease.OutQuad);
        }
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
}
