using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public int threeStarTime;
    public int TwoStarTime;
    public string latterMessage;
    public bool hasStartDialog;
    public DialogData dialogData;
}
