using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterCard", menuName = "AR/Monster Card")]
public class MonsterCardData : ScriptableObject
{
    public EmotionType associatedEmotion;  // 直接使用 EmotionType
    public GameObject imageTargetPrefab;
    public GameObject monsterModel; // 對應的怪獸模型
}
