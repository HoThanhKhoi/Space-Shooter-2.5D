using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText, _bestScoreText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _restartText;
    [SerializeField] private Image _gameOverImage;
    [SerializeField] private Sprite[] _liveSprites;

    [SerializeField] private Image _livesImg;

    private GameManager _gameManager;
    private int _score = 0;
    private int _bestScore;
    private Player _player;

    void Start()
    {
        _bestScore = PlayerPrefs.GetInt("HighScore", 0);
        Debug.Log("Loaded Best Score: " + _bestScore);
        _scoreText.text = "Score: " + _score;
        _bestScoreText.text = "Best Score: " + _bestScore;
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _gameOverImage.enabled = false;

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null )
        {
            Debug.LogError("Game Manager is NULL");
        }



    }

    public void UpdateScore(int playerScore)
    {
        _score = playerScore;
        _scoreText.text = "Score: " + _score;

        CheckForBestScore();
    }

    public void CheckForBestScore()
    {
        if(_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScore", _bestScore);
            PlayerPrefs.Save();
            _bestScoreText.text = "Best Score: " + _bestScore;
        }
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
