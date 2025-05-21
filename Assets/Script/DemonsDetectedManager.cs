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
    [SerializeField] Button startDetected;
    [SerializeField] Text detectedButtonTX;
    [SerializeField] SettlementManager settlement;

    [Header("���ܹϤ�����")]
    [SerializeField]  public Image hintImage;
    [SerializeField]  public Sprite successSprite;
    [SerializeField]  public Sprite failSprite;
    [SerializeField]  public float fadeDuration = 0.5f;

    [Header("���|����")]
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
            detectedButtonTX.text = "�����c�]!";
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
                ShowHintImage(false);//���Y�n�R
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

        // �p�G�O���� �� �� 3 ��A�H�X
        if (!isWin)
        {
            yield return new WaitForSeconds(3f);

            // �H�X
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
