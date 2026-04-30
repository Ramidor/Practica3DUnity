using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore {
    public string playerName;
    public int score;
}

[System.Serializable]
public class RankingData {
    public List<PlayerScore> scores = new List<PlayerScore>();
}