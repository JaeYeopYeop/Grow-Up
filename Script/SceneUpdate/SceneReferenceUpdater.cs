using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReferenceUpdater : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 변경될 때 이벤트 등록
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LeftDownBtn sceneObject = FindObjectOfType<LeftDownBtn>(); // 새로운 씬의 오브젝트 찾기
        Jelly sceneObject2 = FindObjectOfType<Jelly>(); // 새로운 씬의 오브젝트 찾기
        JellyUI sceneObject3 = FindObjectOfType<JellyUI>(); // 새로운 씬의 오브젝트 찾기
        PlantUI sceneObject4 = FindObjectOfType<PlantUI>(); // 새로운 씬의 오브젝트 찾기
        JellySell sceneObject5 = FindObjectOfType<JellySell>(); // 새로운 씬의 오브젝트 찾기
        JellySaver sceneObject6 = FindObjectOfType<JellySaver>(); // 새로운 씬의 오브젝트 찾기
        if (sceneObject != null)
        {
            sceneObject.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad 객체를 다시 연결
        }
        if (sceneObject2 != null)
        {
            sceneObject2.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad 객체를 다시 연결
        }
        if (sceneObject3 != null)
        {
            sceneObject3.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad 객체를 다시 연결
        }
        if (sceneObject4 != null)
        {
            sceneObject4.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad 객체를 다시 연결
        }
        if (sceneObject5 != null)
        {
            sceneObject5.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad 객체를 다시 연결
        }
        if (sceneObject6 != null)
        {
            sceneObject6.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad 객체를 다시 연결
        }
    }
}
