using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Main Screen")]
    public RectTransform MainScreen;
    public Button StartBtn;
    public Button QuitBtn;
    private Vector3 mainScreenOriginalPos;

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

        if (StartBtn != null)
        {
            StartBtn.onClick.AddListener(StartGame);
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

    public void StartGame()
    {
        if (MainScreen != null)
        {
            MainScreen.DOAnchorPos(new Vector2(MainScreen.anchoredPosition.x + 1000, MainScreen.anchoredPosition.y), 0.5f)
                .SetEase(Ease.OutQuad);
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

    public void StartIntroduction()
    {
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
