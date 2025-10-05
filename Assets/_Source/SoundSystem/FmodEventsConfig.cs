using FMODUnity;
using UnityEngine;

namespace SoundSystem
{
    [CreateAssetMenu(fileName = "FMOD Events Config", menuName = "Core/FMOD Events Config")]
    public class FmodEventsConfig: ScriptableObject
    {
        [field: Header("----MUSIC----")] 
        [field: SerializeField] public EventReference MenuMusic {get; private set;}
        [field: SerializeField] public EventReference GameMusic {get; private set;}
    }
}