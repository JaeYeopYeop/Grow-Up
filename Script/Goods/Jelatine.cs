using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jelatine : MonoBehaviour
{
    private const float UpSpeedTypeOne = 100f;
    private const float UpSpeedTypeTwo = 1000f;
    private const float UpSpeedTypeThree = 10000f;
    public const int MaxJelatineValue = 99999999;
    [SerializeField] private Text _JelatineText;

    private float UpSpeed = UpSpeedTypeOne;
    

    private int _jelatineValue;
    public int JelatineValue
    {
        get
        {
            return _jelatineValue = PlayerPrefs.GetInt("Jelatine", 0);
        }
        set
        {
            _jelatineValue = value;
            PlayerPrefs.SetInt("Jelatine", _jelatineValue);
        }
    }

    private void Start()
    {
        _JelatineText.text=JelatineValue.ToString("n0");
        
    }

    /// <summary>
    /// value값은 현재 jelatine에서 더하거나 뺄 값
    /// </summary>
    /// <param name="value"></param>
    public void SetJelatine(int value)
    {
        StartCoroutine(NumCount(value));
    }

    IEnumerator NumCount(int targetNum)
    {
        float count = JelatineValue;
        int temp = JelatineValue;
        JelatineValue = targetNum + temp;
        if (targetNum > 0)
        {
            while (count <= (float)(targetNum + temp))
            {
                count += Time.deltaTime * UpSpeed;
                _JelatineText.text = count.ToString("n0");
                yield return null;
            }
        }
        else if(targetNum < 0)
        {
            while (count > (float)(temp + targetNum))
            {
                count -= Time.deltaTime * UpSpeed;
                _JelatineText.text=count.ToString("n0");
                yield return null;
            }
            count++;
        }
        
        _JelatineText.text = JelatineValue.ToString("n0");
    }

}
