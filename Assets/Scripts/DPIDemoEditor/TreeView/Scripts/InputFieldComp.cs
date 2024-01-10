using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InputFieldComp : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField;

    [Header("Çå¿ÕBtn")]
    [SerializeField]
    Button ClearBtn;

    [Header("ËÑË÷Btn")]
    [SerializeField]
    Button searchBtn;

    public System.Action<string> OnClickSearchAction;

    private void Awake()
    {
        if (inputField == null)
            inputField = GetComponentInChildren<TMP_InputField>(true);

        inputField.onEndEdit.AddListener((x) =>
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnClickSearchAction?.Invoke(x);
            }
        });

        if (ClearBtn != null)
        {
            ClearBtn.onClick.AddListener(() =>
            {
                inputField.text = string.Empty;
                OnClickSearchAction?.Invoke(string.Empty);
            });
        }

        if (searchBtn != null)
        {
            searchBtn.onClick.AddListener(() =>
            {
                OnClickSearchAction?.Invoke(inputField.text);
            });
        }
    }


}
