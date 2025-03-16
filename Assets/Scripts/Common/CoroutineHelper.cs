using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ScoreData
{
    public int score;
}

public class CoroutineHelper : Singleton<CoroutineHelper>
{
    ScoreData scoreData = new ScoreData { score = 10 };
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
    
    public void MyStartCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void AddScore()
    {
        StartCoroutine(NetworkManager.Instance.AddScore((scoreData), () =>
        {
            Debug.Log("AddScore Complete");
        }, () => { }));
    }
}