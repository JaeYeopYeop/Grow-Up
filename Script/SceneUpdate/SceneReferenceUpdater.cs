using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReferenceUpdater : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // ���� ����� �� �̺�Ʈ ���
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LeftDownBtn sceneObject = FindObjectOfType<LeftDownBtn>(); // ���ο� ���� ������Ʈ ã��
        Jelly sceneObject2 = FindObjectOfType<Jelly>(); // ���ο� ���� ������Ʈ ã��
        JellyUI sceneObject3 = FindObjectOfType<JellyUI>(); // ���ο� ���� ������Ʈ ã��
        PlantUI sceneObject4 = FindObjectOfType<PlantUI>(); // ���ο� ���� ������Ʈ ã��
        JellySell sceneObject5 = FindObjectOfType<JellySell>(); // ���ο� ���� ������Ʈ ã��
        JellySaver sceneObject6 = FindObjectOfType<JellySaver>(); // ���ο� ���� ������Ʈ ã��
        if (sceneObject != null)
        {
            sceneObject.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad ��ü�� �ٽ� ����
        }
        if (sceneObject2 != null)
        {
            sceneObject2.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad ��ü�� �ٽ� ����
        }
        if (sceneObject3 != null)
        {
            sceneObject3.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad ��ü�� �ٽ� ����
        }
        if (sceneObject4 != null)
        {
            sceneObject4.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad ��ü�� �ٽ� ����
        }
        if (sceneObject5 != null)
        {
            sceneObject5.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad ��ü�� �ٽ� ����
        }
        if (sceneObject6 != null)
        {
            sceneObject6.SetReference(GamePlayManager.Instance); // DontDestroyOnLoad ��ü�� �ٽ� ����
        }
    }
}
