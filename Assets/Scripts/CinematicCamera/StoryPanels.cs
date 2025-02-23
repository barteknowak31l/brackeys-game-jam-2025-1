using AudioManager;
using StateMachine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class StoryPanels : MonoBehaviour
{
    public TMP_Text displayText;
    public GameObject menuPanel;
    public GameObject storyPanel;
    public GameObject titlePanel;

    public Menu menuScript;
    public string[] texts;
    private int currentIndex = 0;
    private bool isMenuActive = false;
    private AudioSource _audio;
    private bool _sound = true;

    private MovementController _player;

    void Start()
    {
        StateMachineManager.instance.story = false;
        if (displayText != null)
        {
            displayText.text = texts[currentIndex];
        }
        
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentIndex++;

            if (currentIndex < texts.Length)
            {
                displayText.text = texts[currentIndex]; 
            }
            else
            {
                displayText.text = "";

                StartCoroutine(SlideDownTitle()); 
            }
        }
    }

    IEnumerator SlideDownTitle()
    {
        if (titlePanel != null && _sound)
        {
            _sound = false;
            titlePanel.gameObject.SetActive(true); 
           

            yield return new WaitForSeconds(2f);
            AudioManager.AudioManager.PlaySound(AudioClips.SharkHit);
            yield return new WaitForSeconds(3f); 
            
            ShowMenu();
            _player.PlayMusic();
        }
    }

    void ShowMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
        menuScript.enabled=true;
        displayText.gameObject.SetActive(false);
        titlePanel.gameObject.SetActive(false); 
        storyPanel.gameObject.SetActive(false);
        StateMachineManager.instance.story = true;


    }
}
