using AudioManager;
using UnityEngine;

namespace Water
{
    public class Water : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.AudioManager.PlaySound(AudioClips.SharkFall);
            }
        }
    }
}
