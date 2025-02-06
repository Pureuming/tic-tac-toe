using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            // 경우에 따라 첫 Scene의 OnSceneLoaded가 호출이 안 되는 경우를 해결
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            
            // Scene 전환 시, 호출되는 Action Method 할당
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Destroy 후에는 OnSceneLoaded가 할당하지 않도록
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
}