using UnityEngine;
using TMPro;

public class GameOverScoreUI : MonoBehaviour {
    ScoreHandler sh;
    int score;
    TextMeshProUGUI textMesh;
    // Use this for initialization
    private void Start()
    {
        sh = FindObjectOfType<ScoreHandler>();
        if (sh) score = sh.GetScore();
        textMesh = GetComponent<TextMeshProUGUI>();
        Debug.Log(sh);
    }

    void Update () {
        if (sh)
        {
            textMesh.text = "Final Score\n" + score.ToString("000000");
        } else
        {
            textMesh.text = "ScoreHandler missing!";
        }
    }	
}
