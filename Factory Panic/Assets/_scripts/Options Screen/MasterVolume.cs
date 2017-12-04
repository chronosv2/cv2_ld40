
using UnityEngine;
using TMPro;

public class MasterVolume : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI textMesh;
    [SerializeField]
    float volumeStep = 0.05f;

    private void Start()
    {
        float val = AudioListener.volume;
        textMesh.text = val.ToString("0%");
    }


    void ChangeValue(float val)
    {
        AudioListener.volume = val;
        textMesh.text = val.ToString("0%");
    }

    public void PlusPush()
    {
        float v = AudioListener.volume;
        float w = Mathf.Clamp(v + volumeStep, 0, 1);
        ChangeValue(w);
    }

    public void MinusPush()
    {
        float v = AudioListener.volume;
        float w = Mathf.Clamp(v - volumeStep, 0, 1);
        ChangeValue(w);
    }

}
