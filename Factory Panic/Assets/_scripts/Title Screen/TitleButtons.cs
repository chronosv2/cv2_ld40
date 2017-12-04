using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour {

    public void ToInstructions()
    {
        SceneManager.LoadScene(1);
    }

    public void ToGame()
    {
        SceneManager.LoadScene(2);
    }
    public void ToOptions()
    {
        SceneManager.LoadScene(4);
    }
}
