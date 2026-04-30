using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class RankingManager : MonoBehaviour {
    private string filePath;
    public RankingData ranking;

    void Awake() {
        filePath = Application.persistentDataPath + "/ranking.json";
        LoadRanking();
    }

    public void AddScore(string name, float points) {
        ranking.scores.Add(new PlayerScore { playerName = name, score = points });
        // Ordenar de mayor a menor y quedarse con los 10 mejores
        ranking.scores = ranking.scores.OrderByDescending(s => s.score).Take(10).ToList();
        SaveRanking();
    }

    private void SaveRanking() {
        string json = JsonUtility.ToJson(ranking);
        File.WriteAllText(filePath, json);
    }

    private void LoadRanking() {
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            ranking = JsonUtility.FromJson<RankingData>(json);
        } else {
            ranking = new RankingData();
        }
    }
}