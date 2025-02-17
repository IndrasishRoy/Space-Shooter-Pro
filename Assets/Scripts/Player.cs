using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    private Spawn_Manager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldsActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;

    public int _score, _bestScore;
    private UIManager _uiManager;
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //_bestScore = PlayerPrefs.GetInt("HighScore", 0);
        /* //take the current position = new position(0, 0, 0);
         transform.position = new Vector3(0, 0, 0);*/

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();

       // _gameManager = GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Player is NULL!");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL!");
        }
        else
        {
           _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
            CalculateMovement();
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
    }
    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //new Vector3(1, 0, 0) * userInput(0) * 3.5f * real time
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 directions = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(directions * _speed * Time.deltaTime);

        //if player position on the y is > 0
        //y position = 0
        //else if position on the y is < -3.8f
        //y position = -3.8f

        //if (transform.position.y > 0)
        //{
        //  transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y < -3.8f)
        //{
        //  transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        //if position on the x is > 11.3f
        //x position = -11.3f
        //else if position on the x is < -11.3f
        //x position = 11.3f

        //else if (transform.position.x > 11.3f)
        //{
        //  transform.position = new Vector3(-11.3f, transform.position.y, 0);
        //}
        //else if (transform.position.x < -11.3f)
        //{
        //  transform.position = new Vector3(11.3f, transform.position.y, 0);
        //}

        //Using MAthf.Clamp method
        //Mathf.Clamp(min, max)
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        if (transform.position.x > 11.3f)
        {
          transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
          transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        //if I hit the space key
        //spawn game object
        //if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        //{
            _canFire = Time.deltaTime + _fireRate;
        //offset = transform.position + Vector3(0, 0.8f, 0);
        if (_isTripleShotActive == true) {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }
    public void Damage()
    {
        if(_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
        if(_lives == 0)
        {
            AddBest(_bestScore);
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }
    public void AddScore(int points)
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
    public void AddBest(int bPoints)
    {
        if(_score > _bestScore)
        {
            Debug.Log("High Score Detcted!");
            _bestScore = _score;
           // PlayerPrefs.SetInt("HighScore", _bestScore);
        }
        
        _uiManager.UpdateBestScore(_bestScore);
    }
}
