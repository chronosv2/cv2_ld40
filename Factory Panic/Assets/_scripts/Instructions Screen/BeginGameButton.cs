using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void BeginGame()
    {
        SceneManager.LoadScene("game");
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Submit"))
        {
            BeginGame();
        }
	}
}
