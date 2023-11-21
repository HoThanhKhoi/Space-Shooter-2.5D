using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _restartText;
    [SerializeField] private Image _gameOverImage;
    [SerializeField] private Sprite[] _liveSprites;

    [SerializeField] private Image _livesImg;

    private GameManager _gameManager;
    private int _score = 0;

    private Player _player;
    private void Awake()
    {
        _scoreText.text = "Score: " + _score;
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _gameOverImage.enabled = false;
    }
    void Start()
    {
        
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null )
        {
            Debug.LogError("Game Manager is NULL");
        }

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void EndGame()
    {
        _gameOverText.enabled = true;
        _restartText.enabled = true;
        _gameOverImage.enabled = true;
        _gameManager.GameOver();
    }
}
