using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZoneSelector : MonoBehaviour
{
    [SerializeField] public TamingArea tamingArea;

    public Toggle Toggle { get; private set; }

    void Awake()
    {
        Toggle = GetComponent<Toggle>();
    }
}
