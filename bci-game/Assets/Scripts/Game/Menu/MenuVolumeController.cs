using UnityEngine;
using UnityEngine.UI;

public class MenuVolumeController : MonoBehaviour
{
    [Header("Volume Control")]
    [SerializeField] private Slider volumeSlider;    // Reference to the Slider UI element
    [SerializeField] private Button muteButton;      // Reference to the Mute Button UI element
    [SerializeField] private AudioSource[] audioSources;  // References to AudioSource(s) that control the volume

    private bool isMuted = false; // Tracks the mute state
 
    private void Start()
    {
        // Initialize the volume slider and mute button
        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteButton.onClick.AddListener(ToggleMute);

        // Set initial volume based on the slider value
        SetVolume(volumeSlider.value);
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
    }
}
