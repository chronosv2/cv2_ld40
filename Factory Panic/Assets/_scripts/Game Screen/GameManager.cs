using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
    const float wantTimerSet = 45.0f;

    [SerializeField]
	public float SpawnSpeed { private set; get; }
    [SerializeField]
    OreBox blueBox;
    [SerializeField]
    OreBox greenBox;
    [SerializeField]
    OreBox redBox;
    [SerializeField]
    OreBox yellowBox;
    public int BlueCount { private set; get; }
    public int GreenCount { private set; get; }
    public int RedCount { private set; get; }
    public int YellowCount { private set; get; }
    public int BlueMax { private set; get; }
    public int GreenMax { private set; get; }
    public int RedMax { private set; get; }
    public int YellowMax { private set; get; }
    public int Score { private set; get; }
    //public int BlueOverflow { private set; get; }
    //public int GreenOverflow { private set; get; }
    //public int RedOverflow { private set; get; }
    //public int YellowOverflow { private set; get; }
    public bool GameActive { private set; get; }
    [SerializeField]
    float FailTimerStart = 8.0f;
    float failTimer = 8.0f;
    float wantTimer = 30.0f;
    [SerializeField]
    Sprite[] rockPileSprites;
    [SerializeField]
    Sprite[] overflowSprites;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI lastScoreText;
    [SerializeField]
    TextMeshProUGUI boxStates;
    [SerializeField]
    TextMeshProUGUI refreshTimer;
    [SerializeField]
    TextMeshProUGUI multiplierUI;
    [SerializeField]
    Image[] blips;
    [SerializeField]
    Material[] oreColors;
    Ore.OreType[] lastOres;
    int lastScored = 0;
    OreProcessor processor;
    [SerializeField]
    GameObject[] carrying;
    [SerializeField]
    GameObject hasUpgrade;
    [SerializeField]
    TextMeshProUGUI messageBox;
    [SerializeField]
    float messageLength = 2.0f;
    [SerializeField]
    Image pauseBG;
    [SerializeField]
    Image FailImage;
    Player player;
    float multiplier = 1.0f;
    float nextWholeSecond = 8.0f;
    AudioSource audioSource;
    [SerializeField]
    AudioClip klaxon;
    public static bool IsGamePaused { private set; get; }
    
    // Use this for initialization
    void Start () {
        SpawnSpeed = 8.0f;
        IsGamePaused = false;
        GameActive = true;
        IEnumerator coroutine = SpeedUpCoroutine();
        StartCoroutine(coroutine);
        lastOres = new Ore.OreType[4] { Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE };
        processor = FindObjectOfType<OreProcessor>();
        player = FindObjectOfType<Player>();
        wantTimer = wantTimerSet;
        failTimer = FailTimerStart;
        nextWholeSecond = FailTimerStart;
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator DisplayMessageCoroutine(string message)
    {
        messageBox.text = message;
        messageBox.enabled = true;
        yield return new WaitForSeconds(messageLength);
        messageBox.enabled = false;
    }

    public void DisplayMessage(string msg)
    {
        IEnumerator coroutine = DisplayMessageCoroutine(msg);
        StartCoroutine(coroutine);
    }

    IEnumerator SpeedUpCoroutine()
    {
        while(GameActive)
        {
            if (IsGamePaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(10.0f);
            SpawnSpeed = Mathf.Clamp(SpawnSpeed - 0.2f, 3f, 8);
        }
        yield return null;
    }

    public void SetLastPattern(Ore.OreType[] ores, int value)
    {
        Debug.Log("Received Scoring Data: " + ores.Length + " worth " + value);
        for (int i=0;i<ores.Length;i++)
        {
            lastOres[i] = ores[i];
            Debug.Log("Transferred " + ores[i].ToString() + " to " + lastOres[i].ToString());
        }
        if (lastScored >= 100)
        {
            lastScored = Mathf.FloorToInt(value * multiplier);
        } else
        {
            lastScored = value;
        }
    }

    public void IncreaseMultiplier()
    {
        multiplier += 0.2f;
    }

    public Sprite[] GetRockPileSprites()
    {
        return rockPileSprites;
    }

    public Sprite[] GetOverflowSprites()
    {
        return overflowSprites;
    }

    public void AddScore(int amt)
    {
        Score += Mathf.FloorToInt(amt * multiplier);
    }



	// Update is called once per frame
	void Update () {
        if (!GameActive) { return; }
        pauseBG.enabled = IsGamePaused;
        if (Input.GetButtonUp("Cancel"))
        {
            IsGamePaused = !IsGamePaused;
            if (!IsGamePaused)
            {
                messageBox.enabled = false;
            }
        }
        if (IsGamePaused)
        {
            if (!messageBox.enabled)
            {
                messageBox.enabled = true;
                messageBox.text = "Game Paused";
            }
            return;
        }
        BlueCount = blueBox.HeldOre;
        GreenCount = greenBox.HeldOre;
        RedCount = redBox.HeldOre;
        YellowCount = yellowBox.HeldOre;
        BlueMax = blueBox.Capacity;
        GreenMax = greenBox.Capacity;
        RedMax = redBox.Capacity;
        YellowMax = yellowBox.Capacity;
        int numOverflowed = 0;
        if (blueBox.Overflow >= 10) numOverflowed += 1;
        if (greenBox.Overflow >= 10) numOverflowed += 1;
        if (redBox.Overflow >= 10) numOverflowed += 1;
        if (yellowBox.Overflow >= 10) numOverflowed += 1;
        if (numOverflowed >= 2)
        {
            TickFailTimer();
        }
        TickNewWantTimer();
        scoreText.text = "Score: "+Score.ToString("000000");
        lastScoreText.text = lastScored.ToString("0000")+" PTS.";
        // Bins:  08/08   08/08   08/08   08/08
        boxStates.text = "Bins:  " + BlueCount.ToString("00") + "/" + BlueMax.ToString("00") + "   " + GreenCount.ToString("00") + "/" + GreenMax.ToString("00") + "   " + RedCount.ToString("00") + "/" + RedMax.ToString("00") + "   " + YellowCount.ToString("00") + "/" + YellowMax.ToString("00");
        refreshTimer.text = "Refresh in " + wantTimer.ToString("00.0") + "s";
        multiplierUI.text = "Multiplier: " + multiplier.ToString("#0.0")+"x";
        if (player.IsCarryingUpgrade)
        {
            hasUpgrade.SetActive(true);
            carrying[0].SetActive(false);
            carrying[1].SetActive(false);
        } else
        {
            hasUpgrade.SetActive(false);
            carrying[0].SetActive(true);
            carrying[1].SetActive(true);
            Ore.OreType[] whatHas = player.GetHeldOres();
            Image cImg1 = carrying[0].GetComponent<Image>();
            Image cImg2 = carrying[1].GetComponent<Image>();
            cImg1.color = OreToColor(whatHas[0]);
            cImg2.color = OreToColor(whatHas[1]);
        }
        for (int i=0;i<4;i++)
        {
            switch(lastOres[i])
            {
                case Ore.OreType.NONE:
                    blips[i].color = oreColors[0].color;
                    break;
                case Ore.OreType.BLUE:
                    blips[i].color = oreColors[1].color;
                    break;
                case Ore.OreType.GREEN:
                    blips[i].color = oreColors[2].color;
                    break;
                case Ore.OreType.RED:
                    blips[i].color = oreColors[3].color;
                    break;
                case Ore.OreType.YELLOW:
                    blips[i].color = oreColors[4].color;
                    break;
            }
        }
	}

    private void TickNewWantTimer()
    {
        wantTimer -= Time.deltaTime;
        if (wantTimer<=0)
        {
            wantTimer = wantTimerSet;
            processor.GenerateNewPatterns();
            multiplier = 1.0f;
        }
    }

    private void TickFailTimer()
    {
        failTimer -= Time.deltaTime;
        FailImage.fillAmount = failTimer / FailTimerStart;
        if (failTimer < nextWholeSecond && failTimer > 0)
        {
            nextWholeSecond = Mathf.Floor(failTimer-1);
            audioSource.PlayOneShot(klaxon, 0.6f);
        }
        if (failTimer <= 0)
        {
            GameActive = false;
            ScoreHandler sh = FindObjectOfType<ScoreHandler>();
            if (sh)
            {
                sh.SetScore(Score);
            }
        }
    }

    public Material[] GetOreColors()
    {
        return oreColors;
    }

    public Color OreToColor(Ore.OreType ore)
    {
        switch (ore)
        {
            case Ore.OreType.NONE:
                return oreColors[0].color;
            case Ore.OreType.BLUE:
                return oreColors[1].color;
            case Ore.OreType.GREEN:
                return oreColors[2].color;
            case Ore.OreType.RED:
                return oreColors[3].color;
            case Ore.OreType.YELLOW:
                return oreColors[4].color;
            default:
                return oreColors[0].color;
        }
    }
}
