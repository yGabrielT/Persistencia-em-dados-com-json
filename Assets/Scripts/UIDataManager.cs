using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIDataManager : MonoBehaviour
{
    public static UIDataManager instance { get; private set; }

    [SerializeField] private TextMeshProUGUI menuNameText;
    public string playerName;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        playerName = menuNameText.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
    #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
    #else
            Application.Quit();
    #endif
    }
}
