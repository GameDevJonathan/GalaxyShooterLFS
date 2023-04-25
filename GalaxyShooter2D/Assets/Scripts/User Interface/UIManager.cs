using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private TMP_Text _ammoText;
    
    [SerializeField]
    private Image _livesImage;
    
    [SerializeField]
    private Sprite[] _livesSprite;
    
    [SerializeField]
    private TMP_Text _gameOverText;

    [SerializeField]
    private TMP_Text _restartText;

    [SerializeField]
    private GameManager _gameManger;

    [SerializeField]
    private Image _thrusterBarFill;
    
    void Start()
    {
        _gameManger = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        NullChecks();
        _scoreText.text = $"Score: {0}";       
        _gameOverText.gameObject.SetActive(false);
        _ammoText.text = $"Ammo: {20}/{20}";

    }

    private void NullChecks()
    {
        if (!_gameManger)
        {
            Debug.LogError("GameManager is NULL");
        }

        if (!_thrusterBarFill)
        {
            Debug.LogError("Thruster Bar is null");
        }

        if (!_scoreText)
        {
            Debug.LogError("Score Text is NULL");
        }
        if (!_gameOverText)
        {
            Debug.LogError("Game Over Text is NULL");
        }

        if (!_ammoText)
        {
            Debug.LogError("Ammo Text is NULL");
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void UpdateLives(int currentLives)
    {
        //display img sprite
        //give it a new one based on the current lives index
        _livesImage.sprite = _livesSprite[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateThrusterBar(float bar)
    {
        _thrusterBarFill.fillAmount = bar;
    }

    public void UpdateAmmo(int curAmmo, int maxAmmo) 
    {
        _ammoText.text = $"Ammo: {curAmmo}/{maxAmmo}";
    }

    public void GameOverSequence(string text = "GameOver")
    {
        _gameOverText.text = text;
        _gameManger?.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverRoutine());

    }

    IEnumerator GameOverRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
