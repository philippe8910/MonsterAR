using UnityEngine;
using System.Collections.Generic;

public class MonsterAnimationController : MonoBehaviour
{
    public Animator monsterAnimator; // 怪獸的 Animator
    public List<string> animationList = new List<string>(); // 存儲動畫名稱的列表

    void Start()
    {
        if (monsterAnimator == null)
        {
            monsterAnimator = GetComponent<Animator>(); // 自動查找 Animator
        }

        if (animationList.Count == 0)
        {
            Debug.LogWarning("MonsterAnimationController: 動畫列表為空！");
        }
    }

    void OnMouseDown()
    {
        PlayRandomAnimation();
    }

    public void PlayRandomAnimation()
    {
        if (monsterAnimator != null && animationList.Count > 0)
        {
            string randomAnim = animationList[Random.Range(0, animationList.Count)];
            Debug.Log($"播放動畫: {randomAnim}");
            monsterAnimator.Play(randomAnim);
        }
        else
        {
            Debug.LogWarning("MonsterAnimationController: 沒有可播放的動畫！");
        }
    }
}