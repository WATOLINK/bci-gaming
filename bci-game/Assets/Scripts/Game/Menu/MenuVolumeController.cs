using UnityEngine;
using UnityEngine.UI;

public class MenuVolumeController : MonoBehaviour
{
    [Header("Volume Control")]
    [SerializeField] private Slider volumeSlider;    // Reference to the Slider UI element
    [SerializeField] private Button muteButton;      // Reference to the Mute Button UI element
    [SerializeField] private AudioSource[] audioSources;  // References to AudioSource(s) that control the volume
    [SerializeField] private Sprite muteIcon;         // Sprite for mute icon
    [SerializeField] private Sprite unmuteIcon;       // Sprite for unmute icon


    private bool isMuted = false; // Tracks the mute state
 
    private void Start()
    {
        // Initialize the volume slider and mute button
        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteButton.onClick.AddListener(ToggleMute);

        // Set initial volume based on the slider value
        SetVolume(volumeSlider.value);
        UpdateMuteIcon();
    }

    // Set the volume for all AudioSources
    private void SetVolume(float volume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }
    }

    // Toggle mute state
    private void ToggleMute()
    {
        isMuted = !isMuted;
        volumeSlider.interactable = !isMuted; // Disable slider if muted
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != null)
            {
                audioSource.mute = isMuted;
            }
        }
        UpdateMuteIcon();
    }

    // Update mute button icon based on mute state
    private void UpdateMuteIcon() 
    {
        Image buttonImage = muteButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? muteIcon : unmuteIcon;
        }
    }
}
