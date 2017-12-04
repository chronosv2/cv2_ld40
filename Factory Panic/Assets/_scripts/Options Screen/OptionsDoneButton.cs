using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsDoneButton : MonoBehaviour {
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
