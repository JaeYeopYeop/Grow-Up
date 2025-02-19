using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class JellyData
{
    public float posX, posY;
    public int JellyType, JellyLevel;
    public long JellyExperience;
}
[Serializable]
public class JellyDataList
{
    public List<JellyData> JellyDatas = new List<JellyData>();
}

public class JellySaver : MonoBehaviour
{
    [SerializeField] private GameObject _JellyPrefab;
    [SerializeField] private Transform _Space;
    [SerializeField] private GamePlayManager _GamePlayManager;

    private string path;
    // Start is called before the first frame update
    void Start()
    {
        _GamePlayManager.FindRefrence();
        path = Application.persistentDataPath + "/JellyDatas.json";
        LoadJellyData();
    }

    public void SetReference(GamePlayManager manager)
    {
        _GamePlayManager = manager;
        _GamePlayManager.FindRefrence();


    }

    public void ClearJellyList()
    {
        _GamePlayManager._Jellys.Clear();
    }

    public void SaveJellyData()
    {
        JellyDataList _JellyDatas = new JellyDataList();
        foreach (Jelly jelly in _GamePlayManager._Jellys)
        {
            JellyData data = new JellyData()
            {
                posX = jelly.transform.position.x,
                posY = jelly.transform.position.y,
                JellyType = jelly.JellyType,
                JellyLevel = jelly.JellyLevel,
                JellyExperience = jelly.JellyExperience

            };
            _JellyDatas.JellyDatas.Add(data);
        }

        string json = JsonUtility.ToJson(_JellyDatas, true);
        File.WriteAllText(path, json);
        Debug.Log($"모든 오브젝트 저장 완료: {path}");
    }

    public void LoadJellyData()
    {
        
        
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            JellyDataList _JellyDataList=JsonUtility.FromJson<JellyDataList>(json);

            foreach(JellyData data in _JellyDataList.JellyDatas)
            {
                GameObject _Jelly = Instantiate(_JellyPrefab);

                
                _Jelly.transform.position=new Vector3(data.posX, data.posY,0f);
                _Jelly.transform.SetParent(_Space);
                _Jelly.GetComponent<Jelly>().JellyType = data.JellyType;
                _Jelly.GetComponent<Jelly>().JellyLevel = data.JellyLevel;
                _Jelly.GetComponent<Jelly>().JellyExperience = data.JellyExperience;
                //_Jelly.GetComponent<Jelly>().UpdateJellyStats();

                
            }

            Debug.Log("모든 저장 데이터 불러오기 완료");
        }
        else
        {
            Debug.Log("불러오기 실패");
        }
    }
}
