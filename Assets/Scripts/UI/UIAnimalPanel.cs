using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimalPanel : MonoBehaviour
{
    [SerializeField] List<UIZoneSelector> selectors;
    [SerializeField] Image portraitImage;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text statusText;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        gameManager.OnSelectedAnimalChanged.AddListener(_ => UpdateAnimalData());
    }

    private void Start()
    {
        foreach (var selector in selectors)
        {
            selector.Toggle.onValueChanged.AddListener(value =>
            {
                if (gameManager.SelectedAnimal != null)
                {
                    gameManager.SelectedAnimal.TamingArea = (value ? selector.tamingArea : gameManager.StartArea);
                }
                UpdateSelectors();
            });
        }
        UpdateAnimalData();
    }

    void UpdateAnimalData()
    {
        var animal = gameManager.SelectedAnimal;
        if (animal == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        portraitImage.sprite = animal.Portrait;
        nameText.text = animal.GetName();
        statusText.text = animal.GetStatus();
        UpdateSelectors();
    }

    void UpdateSelectors()
    {
        foreach (var selector in selectors)
        {
            selector.Toggle.SetIsOnWithoutNotify(gameManager.SelectedAnimal?.TamingArea == selector.tamingArea);
        }
    }
}
