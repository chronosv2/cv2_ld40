using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreHandler : MonoBehaviour {

    int Score;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);	
	}
	
    public void SetScore(int val)
    {
        Score = val;
        SceneManager.LoadScene("gameover");
    }

	public int GetScore()
    {
        return Score;
    }

    public void ToTitleScreen()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
}
