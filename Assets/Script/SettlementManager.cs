using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettlementManager : MonoBehaviour
{
    [SerializeField] public GameObject winPage;
    [SerializeField] public GameObject losePage;

    // Start is called before the first frame update
    void Start()
    {
        winPage.SetActive(false);
        losePage.SetActive(false);
    }

    public async void OnGameReStart(int setLevel)
    {
        FindObjectOfType<SceneTransition>().CallTransition();
        await Task.Delay(1000);
        PlayerPrefs.SetInt("ResetLevel", setLevel);
        SceneManager.LoadScene("SampleScene");
    }
}
