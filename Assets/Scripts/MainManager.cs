using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text playerHighScore;
    public int curretHighScore;
    public string currentHighScoreName;
    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {

        LoadData(); 
        if (string.IsNullOrEmpty(currentHighScoreName)) // if there is no record the current highscore will be the actual player
        {
            currentHighScoreName = UIDataManager.instance.playerName;
        }
        playerHighScore.text = "Highscore = " + currentHighScoreName + ": " + curretHighScore;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                LoadData();
            }
        }
        ChangePlayerScore();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        SaveData();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void ChangePlayerScore()
    {
        if(m_Points > curretHighScore)
        {
            curretHighScore = m_Points;
            currentHighScoreName = UIDataManager.instance.playerName;
            playerHighScore.text = "Highscore = " + currentHighScoreName + ": " + curretHighScore;
            
        }
    }

    [System.Serializable]
    class Data
    {
        public int j_currentHighScore;
        public string j_currentHighscoreName;
    }

    public void SaveData()
    {
        Data data = new Data();
        data.j_currentHighScore = curretHighScore;
        data.j_currentHighscoreName = currentHighScoreName;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefiles", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefiles";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data data = JsonUtility.FromJson<Data>(json);
            curretHighScore = data.j_currentHighScore;
            currentHighScoreName = data.j_currentHighscoreName;

        }
    }




}
