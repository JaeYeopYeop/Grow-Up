using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceText : MonoBehaviour
{
    private int _Value;
    public int Value
    {
        get => _Value;
        set => _Value = value;
    }
    private float _moveSpeed = 0.2f;
    private float _destroytime = 1.5f;
    public bool isClicked = false;
    public bool isGoods = false;
    private Color _color;
    private float _colorSpeed = 5f;

    private string[] _text = new string[] { "+Ex", "+Coin", "+Jel" };
    public int _textNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().text = string.Format($"{_text[_textNum]} {Value}");
        this.transform.localScale = this.transform.localScale * 0.0641f;
        if (isGoods) {
            _color = Color.green;
            _color.a = 255f;
        }
        else
        {
            _color = Color.white;
            _color.a = 255f;
        }
        
        if (isClicked) {
            _destroytime = 0.5f;
            _colorSpeed = 15.0f;
        }
        Destroy( this.gameObject,_destroytime );
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().Translate(new Vector3(0f, _moveSpeed * Time.deltaTime, 0f));
        
        _color.a= Mathf.Lerp(_color.a,0f, _colorSpeed*Time.deltaTime);
        // Text의 color.a는 0~1이기 때문에 사실은 0~1사이에 도달해야만 투명도에 영향을 준다.
        this.GetComponent<Text>().color= _color;
    }
}
