using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ImpactSoundPlayer : MonoBehaviour
{
    [Header("Sound Clips")]
    [SerializeField] public List<AudioClip> heavyPunchClips = new List<AudioClip>();
    [SerializeField] AudioClip lightPunchClip;

    [Header("Impulse Thresholds (raw impulse)")]
    public float lightThreshold = 50f;
    public float heavyThreshold = 150f;

    [Header("Audio Settings")]
    [Range(0f,1f)]
    public float volume = 1f;

    [Header("Tag Filtering")]
    [Tooltip("If non-empty, only collisions with this tag trigger sounds.")]
    public string punchTag = "Glove";

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        Debug.Log($"[ImpactSoundPlayer] Init: light≥{lightThreshold}, heavy≥{heavyThreshold}, punchTag='{punchTag}'");
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1. Only filter if a tag is specified
        if (!string.IsNullOrEmpty(punchTag))
        {
            // Log what collided and its tag
            Debug.Log($"[Impact] Collided with '{collision.collider.name}' (tag='{collision.collider.tag}')");
            if (!collision.collider.CompareTag(punchTag))
            {
                Debug.Log($"[Impact] Ignored: '{collision.collider.tag}' != '{punchTag}'");
                return;
            }
        }

        // 2. Measure the raw impulse
        float impulse = collision.impulse.magnitude;
        Debug.Log($"[Impact] Impulse magnitude = {impulse:F1}");

        AudioClip clipToPlay = null;

        // 3. Pick light or heavy
        if (impulse >= heavyThreshold )
        {
            int idx = Random.Range(0, heavyPunchClips.Count);
            clipToPlay = heavyPunchClips[idx];
            Debug.Log($"[Impact] Heavy hit! Playing heavy clip #{idx}: '{clipToPlay.name}'");
        }
        else if (impulse >= lightThreshold)
        {
            clipToPlay = lightPunchClip;
            Debug.Log($"[Impact] Light hit. Playing light clip: '{clipToPlay.name}'");
        }
        else
        {
            Debug.Log($"[Impact] No sound: impulse {impulse:F1} < lightThreshold {lightThreshold}");
        }

        // 4. Play
        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay, volume);
            Debug.Log("[Impact] AudioSource.PlayOneShot()");
        }
    }
}
