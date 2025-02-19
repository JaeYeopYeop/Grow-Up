using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstClickPanel : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private GameUI _GameUI;

    private bool _isTouchOk = false;


    private void OnEnable()
    {
        StartCoroutine(IsTouchOk());
    }

    IEnumerator IsTouchOk()
    {
        _text.text = "준비중...";
        yield return new WaitForSeconds(3f);
        _isTouchOk = true;
        _text.text = "준비가 되셨다면\n아무곳이나 클릭하고 시작해주세요.";
    }
    // Update is called once per frame
    void Update()
    {
        if (_isTouchOk)
        {
            if (Input.GetMouseButton(0))
            {
                SoundManager.instance.PlaySfx(SoundManager.ESFX.SFX_Button);
                _GameUI.PlayTImeWalk=true;
                this.gameObject.SetActive(false);
                _isTouchOk=false;
            }
        }
    }
}
