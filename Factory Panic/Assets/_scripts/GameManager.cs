using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour {
    const float wantTimerSet = 30.0f;

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
    public int Score { private set; get; }
    public int BlueOverflow { private set; get; }
    public int GreenOverflow { private set; get; }
    public int RedOverflow { private set; get; }
    public int YellowOverflow { private set; get; }
    public bool GameActive { private set; get; }
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
    Player player;

    // Use this for initialization
    void Start () {
        SpawnSpeed = 8.0f;
        GameActive = true;
        IEnumerator coroutine = SpeedUpCoroutine();
        StartCoroutine(coroutine);
        lastOres = new Ore.OreType[4] { Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE, Ore.OreType.NONE };
        processor = FindObjectOfType<OreProcessor>();
        player = FindObjectOfType<Player>();
	}

    
    IEnumerator SpeedUpCoroutine()
    {
        while(GameActive)
        {
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
        lastScored = value;
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
        Score += amt;
    }

	// Update is called once per frame
	void Update () {
        if (!GameActive) { return; }
        BlueCount = Mathf.Clamp(Mathf.FloorToInt(blueBox.CapPercent * 100),0,100);
        GreenCount = Mathf.Clamp(Mathf.FloorToInt(greenBox.CapPercent * 100), 0, 100);
        RedCount = Mathf.Clamp(Mathf.FloorToInt(redBox.CapPercent * 100), 0, 100);
        YellowCount = Mathf.Clamp(Mathf.FloorToInt(yellowBox.CapPercent * 100), 0, 100);
        if (blueBox.Overflow >= 12 && greenBox.Overflow >= 12 && redBox.Overflow >= 12 && yellowBox.Overflow >= 12)
        {
            TickFailTimer();
        }
        TickNewWantTimer();
        scoreText.text = "Score: "+Score.ToString("00000000");
        lastScoreText.text = lastScored.ToString("0000")+" PTS.";
        boxStates.text = "Box Status\n" + "1: " + BlueCount.ToString("000") + "%\n" + "2: " + GreenCount.ToString("000") + "%\n" + "3: " + RedCount.ToString("000") + "%\n" + "4: " + YellowCount.ToString("000") + "%";
        refreshTimer.text = "Refresh in " + wantTimer.ToString("00.0") + "s";
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
        }
    }

    private void TickFailTimer()
    {
        failTimer -= Time.deltaTime;
        if (failTimer <= 0)
        {
            GameActive = false;
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
