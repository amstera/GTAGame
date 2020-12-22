using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Star;
    public GameObject Star2;
    public GameObject RedCopOverlay;
    public GameObject Police;
    public Text MoneyText;
    public int WantedLevel;
    public AudioSource PoliceSiren;


    private int _money;
    private float _wantedLevelAddedTime;
    private float _lastFlashRedTime;
    private bool _isFlashingRed;
    private float _lastPoliceSpawnTime;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        Star.SetActive(WantedLevel > 0);
        Star2.SetActive(WantedLevel > 1);

        if (WantedLevel > 0)
        {
            if (!PoliceSiren.isPlaying)
            {
                PoliceSiren.Play();
            }
            if (Time.time - _wantedLevelAddedTime >= 20)
            {
                _wantedLevelAddedTime = Time.time;
                WantedLevel = Mathf.Clamp(WantedLevel - 1, 0, 2);
            }

            if (Time.time - _lastFlashRedTime >= 1)
            {
                _lastFlashRedTime = Time.time;
                _isFlashingRed = !_isFlashingRed;
                RedCopOverlay.SetActive(_isFlashingRed);
            }

            int policeCount = FindObjectsOfType<Police>().Count(p => !p.IsDead);
            if (policeCount < WantedLevel)
            {
                for (int i = 0; i < (WantedLevel - policeCount); i++)
                {
                    if (_lastPoliceSpawnTime == 0 || (Time.time - _lastPoliceSpawnTime) >= 5)
                    {
                        Instantiate(Police, new Vector3((Camera.main.transform.position - Camera.main.transform.forward * Random.Range(2f, 5f)).x, 0, (Camera.main.transform.position - Camera.main.transform.right * Random.Range(3f, 10f)).z), Quaternion.identity);
                        _lastPoliceSpawnTime = Time.time;
                    }
                }
            }
        }
        else
        {
            RedCopOverlay.SetActive(false);
            PoliceSiren.Stop();
        }
    }

    public void AddWantedLevel()
    {
        WantedLevel = Mathf.Clamp(WantedLevel + 1, 0, 2);
        _wantedLevelAddedTime = Time.time;
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        MoneyText.text = $"${_money}";
    }
}
