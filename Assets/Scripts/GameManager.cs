using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Star;
    public GameObject Star2;
    public GameObject RedCopOverlay;
    public Text MoneyText;
    public static GameManager Instance;

    private int _money;
    private int _wantedLevel;
    private float _wantedLevelAddedTime;
    private float _lastFlashRedTime;
    private bool _isFlashingRed;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        Star.SetActive(_wantedLevel > 0);
        Star2.SetActive(_wantedLevel > 1);

        if (_wantedLevel > 0)
        {
            if (Time.time - _wantedLevelAddedTime >= 20)
            {
                _wantedLevelAddedTime = Time.time;
                _wantedLevel = Mathf.Clamp(_wantedLevel - 1, 0, 2);
            }

            if (Time.time - _lastFlashRedTime >= 1)
            {
                _lastFlashRedTime = Time.time;
                _isFlashingRed = !_isFlashingRed;
                RedCopOverlay.SetActive(_isFlashingRed);
            }
        }
        else
        {
            RedCopOverlay.SetActive(false);
        }
    }

    public void AddWantedLevel()
    {
        _wantedLevel = Mathf.Clamp(_wantedLevel + 1, 0, 2);
        _wantedLevelAddedTime = Time.time;
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        MoneyText.text = $"${_money}";
    }
}
