using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level List", menuName = "Create Level List/Level List")]
public class LevelDataBase : ScriptableObject
{
    public List<CardLevelData> cardLevels = new ();
}

[System.Serializable]
public class CardLevelData
{
    public int level;
    public int rows;
    public int columns;
    public float timeLimitInSeconds;
}
