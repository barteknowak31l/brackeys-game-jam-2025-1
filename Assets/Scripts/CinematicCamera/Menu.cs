using System.Collections;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI pressEnterText;
    public TextMeshProUGUI pressCText;
    public TextMeshProUGUI titleText;
    public GameObject creditsPanel;
    public GameObject uiElements;
    public float bounceHeight = 10f;
    public float bounceSpeed = 1f;

    void Start()
    {
        StartCoroutine(BlinkText());
        StartCoroutine(BounceEffect());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCredits();
        }
    }

    void ToggleCredits()
    {
        bool isActive = creditsPanel.activeSelf;
        creditsPanel.SetActive(!isActive);
        uiElements.SetActive(isActive);
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            pressEnterText.enabled = !pressEnterText.enabled;
            pressCText.enabled = !pressCText.enabled;
            yield return new WaitForSeconds(0.4f);
        }
    }
    IEnumerator BounceEffect()
    {
        Vector3 startPos = titleText.rectTransform.anchoredPosition;

        while (true)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * bounceSpeed;
                float newY = Mathf.Sin(t * Mathf.PI) * bounceHeight; 
                titleText.rectTransform.anchoredPosition = startPos + new Vector3(0, newY, 0);
                yield return null;
            }
        }
    }
}
