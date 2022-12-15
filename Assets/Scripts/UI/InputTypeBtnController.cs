using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class InputTypeBtnController : MonoBehaviour {

    private TextMeshProUGUI btnText;

    private Button button;

    private int currentInputType = 0;

    private string[] inputTypes = {"Keyboard", "PS5", "Xbox"};

    private void Start() {

        btnText = GetComponentInChildren<TextMeshProUGUI>();

        btnText.text = inputTypes[currentInputType];

        button = GetComponent<Button>();

        button.onClick.AddListener(changeInputType);

    }

    private void changeInputType() {
        currentInputType++;
        if(currentInputType > (inputTypes.Length - 1)) {
            currentInputType = 0;
        }
        btnText.text = inputTypes[currentInputType];
    }

}
