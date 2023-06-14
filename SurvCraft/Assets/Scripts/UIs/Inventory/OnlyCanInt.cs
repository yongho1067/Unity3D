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
        // �Էµ� ���ڰ� �������� Ȯ��
        if (char.IsDigit(addedChar))
        {
            // ������ ��� �Է� ���
            return addedChar;
        }
        else
        {
            // ���ڰ� �ƴ� ��� �Է� ����
            return '\0';
        }
    }
}
