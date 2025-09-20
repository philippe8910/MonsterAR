using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReStartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //OnReset();
    }

    public async void OnReset()
    {
        SetAudio();
        PlayerPrefs.SetInt("ResetLevel", 0);
        await Task.Delay(500);
        SceneManager.LoadScene("SampleScene");
    }

    private void SetAudio()
    {
        //Debug.Log($" BGM: {PlayerPrefs.GetInt("BGMSet")}");
        //Debug.Log($" SE: {PlayerPrefs.GetInt("SESet")}");

        if (PlayerPrefs.GetInt("BGMSet") == 0)
        {
            PlayerPrefs.SetInt("BGMSet", 1);
        }

        if (PlayerPrefs.GetInt("SESet") == 0)
        {
            PlayerPrefs.SetInt("SESet", 1);
        }

        //Debug.Log($" BGM: {PlayerPrefs.GetInt("BGMSet")}");
        //Debug.Log($" SE: {PlayerPrefs.GetInt("SESet")}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
