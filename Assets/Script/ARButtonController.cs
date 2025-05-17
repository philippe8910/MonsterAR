using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARButtonController : MonoBehaviour
{
    [Header("按鈕本體")]
    [SerializeField] private Button targetButton;
    [SerializeField] public int CardTypeIndex;

    [Header("點擊後要啟動的物件")]
    [SerializeField] private GameObject objectToActivate;

    [Header("可替換的圖片陣列")]
    [SerializeField] private Sprite[] spriteVariants;

    [Header("實際顯示圖片的元件")]
    [SerializeField] private Image targetImage;

    private void Awake()
    {
        if (targetButton == null)
            targetButton = GetComponent<Button>();

        // 預設為關閉互動
        DisableButton();

        // 設定按下的事件
        targetButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }

    //功能 1：開啟按鈕
    public void EnableButton()
    {
        targetButton.interactable = true;

    }

    //功能 2：關閉按鈕
    public void DisableButton()
    {
        targetButton.interactable = false;
        ChangeSprite(0);
    }

    //功能 4：更換圖片
    public void ChangeSprite(int index)
    {
        if (spriteVariants != null && index >= 0 && index < spriteVariants.Length)
        {
            if (targetImage != null)
            {
                targetImage.sprite = spriteVariants[index];
            }
        }
        else
        {
            Debug.LogWarning("更換圖片失敗：索引超出範圍！");
        }
    }
}
