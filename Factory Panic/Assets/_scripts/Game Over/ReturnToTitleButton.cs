using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitleButton : MonoBehaviour {

	public void ReturnToTitle()
    {
        ScoreHandler sh = FindObjectOfType<ScoreHandler>();
        if (sh)
        {
            sh.ToTitleScreen();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    //private void Update()
    //{
    //    if (Input.GetButtonDown("Submit"))
    //    {
    //        ReturnToTitle();
    //    }
    //}
}
