using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelIntroButton : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject levelIntroUI;

    public GameObject glowImage; // Assign the glow image here
    public AudioClip hoverSound; // Assign the hover sound here
    private AudioSource audioSource;
    private Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    private Button button;

    void Start()
    {
        if (glowImage != null)
        {
            glowImage.SetActive(false); // Initially hide the glow image
        }
        button = GetComponent<Button>();
        originalScale = transform.localScale;
        

        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (glowImage != null)
        {
            glowImage.SetActive(true); // Show the glow image
        }

        transform.localScale = hoverScale;

        // Play hover sound
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (glowImage != null)
        {
            glowImage.SetActive(false); // Hide the glow image
            
        }
        transform.localScale = originalScale;
    }

    public void ShowLevelIntroUI()
    {
        levelIntroUI.SetActive(true);
    }

    public void HideLevelIntroUI()
    {
        levelIntroUI.SetActive(false);
    }
}
