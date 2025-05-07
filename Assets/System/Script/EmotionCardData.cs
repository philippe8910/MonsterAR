using UnityEngine;

public enum EmotionType
{
    Happy,
    Sad,
    Angry,
    Fear,
    Surprise,
    None
}


[CreateAssetMenu(fileName = "NewEmotionCard", menuName = "AR/Emotion Card")]
public class EmotionCardData : ScriptableObject
{
    public EmotionType emotionType;
    public GameObject imageTargetPrefab;
}
