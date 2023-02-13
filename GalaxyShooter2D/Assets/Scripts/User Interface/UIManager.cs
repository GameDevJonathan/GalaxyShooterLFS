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
    private Image _livesImage;
    
    [SerializeField]
    private Sprite[] _livesSprite;
    
    [SerializeField]
    private TMP_Text _gameOverText;

    [SerializeField]
    private TMP_Text _restartText;

    [SerializeField]
    private TMP_Text _shieldsText;

    [SerializeField]
    private GameManager _gameManger;
    
    void Start()
    {
        _scoreText.text = $"Score: {0}";
        _shieldsText.text = $"Shield Health: {0}";
        _gameOverText.gameObject.SetActive(false);
        _gameManger = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (!_gameManger)
        {
            Debug.LogError("GameManager is NULL");
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

    public void UpdateShields(int _shields)
    {
        _shieldsText.text = $"Shield Health: {_shields}";
    }
  

    void GameOverSequence()
    {
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
