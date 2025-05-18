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
        OnReset();
    }

    private async void OnReset()
    {
        PlayerPrefs.SetInt("ResetLevel", 0);
        await Task.Delay(300);
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
