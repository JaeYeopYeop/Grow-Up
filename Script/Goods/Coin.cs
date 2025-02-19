using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    [SerializeField] private Text _CoinText;

    private float UpSpeed = 100f;

    private int _coinValue;

    public int CoinValue
    {
        get
        {
            return _coinValue = PlayerPrefs.GetInt("Coin", 0);
        }
        set
        {
            _coinValue = value;
            PlayerPrefs.SetInt("Coin", value);
        }
    }

    private void Start()
    {
        _CoinText.text=CoinValue.ToString("n0");
    }

    /// <summary>
    /// value값은 현재 coin에서 더하거나 뺄 값
    /// </summary>
    /// <param name="value"></param>
    public void SetCoin(int value)
    {
        StartCoroutine(NumCount(value));
    }

    IEnumerator NumCount(int targetNum)
    {
        float count = (float)CoinValue;
        int temp = CoinValue;
        CoinValue = targetNum + temp;

        if (targetNum > 0)
        {
            while (count <= (float)(targetNum + temp))
            {
                count += Time.deltaTime * UpSpeed;
                _CoinText.text = count.ToString("n0");
                yield return null;
            }
        }
        else if (targetNum < 0)
        {
            while (count >= (float)(targetNum + temp))
            {
                count -= Time.deltaTime * UpSpeed;
                _CoinText.text = count.ToString("n0");
                yield return null;
            }
            count++;
        }

        _CoinText.text = CoinValue.ToString("n0");
    }

}
