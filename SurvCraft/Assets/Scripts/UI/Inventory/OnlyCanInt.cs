using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnlyCanInt : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    { 
        inputField.onValidateInput += ValidateNumericInput;
    }

    private char ValidateNumericInput(string text, int charIndex, char addedChar)
    {
        // 입력된 문자가 숫자인지 확인
        if (char.IsDigit(addedChar))
        {
            // 숫자인 경우 입력 허용
            return addedChar;
        }
        else
        {
            // 숫자가 아닌 경우 입력 차단
            return '\0';
        }
    }
}
