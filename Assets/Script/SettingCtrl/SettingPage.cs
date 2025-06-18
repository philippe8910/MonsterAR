using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SettingPage : MonoBehaviour
{
    [SerializeField] private bool isOpen_BGM;
    [SerializeField] private bool isOpen_SE;
    [SerializeField] private GameObject[] BGMButton;
    [SerializeField] private GameObject[] SEButton;
    [SerializeField] private AudioManager audioManager;
    // Start is called before the first frame update
    async void Start()
    {
        CheckAudio();
        Debug.Log($" BGM: { PlayerPrefs.GetInt("BGMSet")}");
        Debug.Log($" SE: {PlayerPrefs.GetInt("SESet")}");
        await Task.Delay(200);
        ClosePage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAudio()
    {
        if (PlayerPrefs.GetInt("BGMSet") == 1)
        {
            isOpen_BGM = true;
        }
        else
        {
            isOpen_BGM = false;
        }

        if (PlayerPrefs.GetInt("SESet") == 1)
        {
            isOpen_SE = true;
        }
        else
        {
            isOpen_SE = false;
        }

        SetBGM(isOpen_BGM);
        SetSE(isOpen_SE);
        LoadingPage();
    }

    public void ConfirmationChanges()
    {
        if (isOpen_BGM)
        {
            PlayerPrefs.SetInt("BGMSet", 1);
        }
        else
        {
            PlayerPrefs.SetInt("BGMSet", 2);
        }

        if (isOpen_SE)
        {
            PlayerPrefs.SetInt("SESet", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SESet", 2);
        }

        SetBGM(isOpen_BGM);
        SetSE(isOpen_SE);
        ClosePage();
    }

    public void ButtonUIDetected()
    {
        SetBGMBtn();
        SetSEBtn();
    }

    public void SetBGMBtn()
    {
        if (isOpen_BGM)
        {
            isOpen_BGM = false;
            BGMButton[0].SetActive(false);
            BGMButton[1].SetActive(true);
        }
        else
        {
            isOpen_BGM = true;
            BGMButton[0].SetActive(true);
            BGMButton[1].SetActive(false);
        }
    }

    public void SetSEBtn()
    {
        if (isOpen_SE)
        {
            isOpen_SE = false;
            SEButton[0].SetActive(false);
            SEButton[1].SetActive(true);
        }
        else
        {
            isOpen_SE = true;
            SEButton[0].SetActive(true);
            SEButton[1].SetActive(false);
        }
    }

    private void LoadingPage()
    {
        if (isOpen_BGM)
        {
            BGMButton[0].SetActive(true);
            BGMButton[1].SetActive(false);
        }
        else
        {
            BGMButton[0].SetActive(false);
            BGMButton[1].SetActive(true);
        }

        if (isOpen_SE)
        {
            SEButton[0].SetActive(true);
            SEButton[1].SetActive(false);
        }
        else
        {

            SEButton[0].SetActive(false);
            SEButton[1].SetActive(true);
        }
    }

    private void SetBGM(bool isOpen)
    {
        if (isOpen)
        {
            audioManager.bgm.volume = 0.08f;
        }
        else
        {
            audioManager.bgm.volume = 0;
        }
    }

    private void SetSE(bool isOpen)
    {
        if (isOpen)
        {
            audioManager.btnSE.volume = 0.05f;
            audioManager.Introduction.GetComponent<AudioSource>().volume = 0.45f;
            audioManager.prompts.GetComponent<AudioSource>().volume = 1f;
            audioManager.soundEffect.GetComponent<AudioSource>().volume = 0.20f;
        }
        else
        {
            audioManager.btnSE.volume = 0;
            audioManager.Introduction.GetComponent<AudioSource>().volume = 0;
            audioManager.prompts.GetComponent<AudioSource>().volume = 0;
            audioManager.soundEffect.GetComponent<AudioSource>().volume = 0;
        }
    }

    public void ClosePage()
    {
        this.gameObject.SetActive(false);
    }
}
