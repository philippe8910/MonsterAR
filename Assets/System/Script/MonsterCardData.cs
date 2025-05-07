using UnityEngine;

[CreateAssetMenu(fileName = "NewSituationCard", menuName = "AR/Situation Card")]
public class SituationCardData : ScriptableObject
{
    public EmotionType associatedEmotion;  // 直接使用 EmotionType
    public GameObject imageTargetPrefab;
}
