using UnityEngine;

namespace AudioManager
{
    [CreateAssetMenu(menuName = "AudioManager/Sounds SO", fileName = "Sounds SO")]
    public class AudioSO : ScriptableObject
    {
        public SoundGroup[] sounds;
    }
}
