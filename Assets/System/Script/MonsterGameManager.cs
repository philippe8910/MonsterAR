using UnityEngine;
using UnityEngine.Events;
using Vuforia;
using System.Collections;
using System.Collections.Generic;

public class MonsterGameManager : MonoBehaviour
{
    public List<EmotionCardData> emotionCards;
    public List<SituationCardData> situationCards;
    public List<MonsterCardData> monsterCards;

    public UnityEvent OnRecognitionStarted;
    public UnityEvent OnRecognitionCompleted;
    public UnityEvent OnRecognitionSuccess;
    public UnityEvent OnRecognitionFailed;

    private List<GameObject> spawnedTargets = new List<GameObject>(); // 存放生成的 Image Target
    [SerializeField] private List<GameObject> detectedGameObjects = new List<GameObject>(); // 存放偵測到的 EmotionType
    private bool isRecognitionActive = false;

    void Start()
    {
        if (OnRecognitionStarted == null) OnRecognitionStarted = new UnityEvent();
        if (OnRecognitionCompleted == null) OnRecognitionCompleted = new UnityEvent();
        if (OnRecognitionSuccess == null) OnRecognitionSuccess = new UnityEvent();
        if (OnRecognitionFailed == null) OnRecognitionFailed = new UnityEvent();

        ClearPreviousTargets();
    }

    public void StartRecognition()
    {
        if (isRecognitionActive) return;
        ClearPreviousTargets();
        StartCoroutine(RecognitionDelay());
    }

    private IEnumerator RecognitionDelay()
    {
        OnRecognitionStarted.Invoke();
        Debug.Log("🔍 辨識將在 3 秒後開始...");
        yield return new WaitForSeconds(3f);
        
        isRecognitionActive = true;
        Debug.Log("✅ 辨識開始！");
        SpawnImageTargets(emotionCards);
        SpawnImageTargets(situationCards);
        SpawnImageTargets(monsterCards);
    }

    private void SpawnImageTargets<T>(List<T> cardList) where T : ScriptableObject
    {
        HashSet<EmotionType> spawnedTypes = new HashSet<EmotionType>();

        foreach (var card in cardList)
        {
            if (card == null) continue;

            GameObject prefab = null;
            EmotionType cardType = EmotionType.Happy;

            if (card is EmotionCardData emotionCard)
            {
                prefab = emotionCard.imageTargetPrefab;
                cardType = emotionCard.emotionType;
            }
            else if (card is SituationCardData situationCard)
            {
                prefab = situationCard.imageTargetPrefab;
                cardType = situationCard.associatedEmotion;
            }
            else if (card is MonsterCardData monsterCard)
            {
                prefab = monsterCard.imageTargetPrefab;
                cardType = monsterCard.associatedEmotion;
            }

            if (prefab != null && !spawnedTypes.Contains(cardType))
            {
                GameObject instance = Instantiate(prefab);
                instance.name = prefab.name; // **🔴 確保名字一致，避免比對錯誤**
                spawnedTargets.Add(instance);
                spawnedTypes.Add(cardType);

                var observer = instance.GetComponent<ObserverBehaviour>();
                if (observer)
                {
                    observer.OnTargetStatusChanged += OnTrackingChanged;
                }
            }
        }
    }

    private void OnTrackingChanged(ObserverBehaviour target, TargetStatus status)
    {
        if (!isRecognitionActive) return;

        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            EmotionType detectedType = GetEmotionTypeFromTarget(target);
            if (!detectedGameObjects.Contains(target.gameObject)) // **避免重複加入**
            {
                detectedGameObjects.Add(target.gameObject);
                Debug.Log($"✅ 偵測到卡片: {detectedType}");
            }

            CheckCombination();
        }
    }

    private EmotionType GetEmotionTypeFromTarget(ObserverBehaviour target)
    {
        string targetName = target.gameObject.name; // **🔴 確保取得正確的物件名稱**
        Debug.Log($"🔍 嘗試查找 {targetName} 對應的 EmotionType");

        foreach (var card in emotionCards)
        {
            if (card.imageTargetPrefab.name == targetName) return card.emotionType;
        }
        foreach (var card in situationCards)
        {
            if (card.imageTargetPrefab.name == targetName) return card.associatedEmotion;
        }
        foreach (var card in monsterCards)
        {
            if (card.imageTargetPrefab.name == targetName) return card.associatedEmotion;
        }

        Debug.LogWarning($"⚠️ 未能找到 {targetName} 的 EmotionType");
        return EmotionType.Happy; // **預設回傳 Happy 避免錯誤**
    }

    private void CheckCombination()
{
    if (!isRecognitionActive) return;
    if (detectedGameObjects.Count < 3) return;

    Debug.Log("🔍 [檢查組合] 開始");
    isRecognitionActive = false;
    OnRecognitionCompleted.Invoke();

    EmotionType detectedEmotion = EmotionType.None;
    EmotionType detectedSituation = EmotionType.None;
    EmotionType detectedMonster = EmotionType.None;

    // 依照偵測到的物件名稱比對 EmotionType
    foreach (var detectedObj in detectedGameObjects)
    {
        string detectedName = detectedObj.name;
        Debug.Log($"🔍 [比對] 檢測到物件: {detectedName}");

        // 從情緒卡片比對
        foreach (var card in emotionCards)
        {
            if (card.imageTargetPrefab.name == detectedName)
            {
                detectedEmotion = card.emotionType;
                Debug.Log($"✅ 物件 {detectedName} 是 EmotionType: {detectedEmotion}");
            }
        }

        // 從情境卡片比對
        foreach (var card in situationCards)
        {
            if (card.imageTargetPrefab.name == detectedName)
            {
                detectedSituation = card.associatedEmotion;
                Debug.Log($"✅ 物件 {detectedName} 是 SituationType (對應情緒): {detectedSituation}");
            }
        }

        // 從怪獸卡片比對
        foreach (var card in monsterCards)
        {
            if (card.imageTargetPrefab.name == detectedName)
            {
                detectedMonster = card.associatedEmotion;
                Debug.Log($"✅ 物件 {detectedName} 是 MonsterType (對應情緒): {detectedMonster}");
            }
        }
    }

    Debug.Log("🔍 [檢查組合] 情緒: " + detectedEmotion);
    Debug.Log("🔍 [檢查組合] 情境: " + detectedSituation);
    Debug.Log("🔍 [檢查組合] 怪獸: " + detectedMonster);

    // 檢查是否三者匹配
    if (detectedEmotion != EmotionType.None &&
        detectedEmotion == detectedSituation &&
        detectedSituation == detectedMonster)
    {
        Debug.Log("🎯 [成功] 組合正確！消滅怪獸！");
        OnRecognitionSuccess.Invoke();
        StartCoroutine(DestroyMonster(spawnedTargets[2]));
    }
    else
    {
        Debug.Log("❌ [失敗] 組合錯誤！怪獸放大後消失！");
        OnRecognitionFailed.Invoke();
        StartCoroutine(EnlargeAndDisappear(spawnedTargets[2]));
    }
}


    private void ClearPreviousTargets()
    {
        foreach (GameObject target in spawnedTargets)
        {
            if (target != null) Destroy(target);
        }
        spawnedTargets.Clear();
        detectedGameObjects.Clear();
    }

    private IEnumerator DestroyMonster(GameObject monster)
    {
        if (monster)
        {
            yield return new WaitForSeconds(1);
            monster.SetActive(false);
        }
    }

    private IEnumerator EnlargeAndDisappear(GameObject monster)
    {
        if (monster)
        {
            Vector3 originalScale = monster.transform.localScale;
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                monster.transform.localScale = originalScale * (1 + t * 2);
                yield return null;
            }
            monster.SetActive(false);
            monster.transform.localScale = originalScale;
        }
    }
}
