using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPageDetected : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public List<GameObject> WinPage;
    [SerializeField] public Text IndexText;


    public void OnWinPageDetected(int index)
    {
        for (int i = 0; i < WinPage.Count; i++)
        {
            WinPage[i].SetActive(false);
        }

        Debug.Log($"WinPageOn:{index}");

        switch (index)
        {
            case 1:
                WinPage[0].SetActive(true);
                break;

            case 2:
                WinPage[1].SetActive(true);
                break;

            default:
                WinPage[2].SetActive(true);
                if (index >= 3)
                {
                    IndexText.text = $"{index}";
                }
                break;
        }

    }
}
