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
        
        [field: Header("----SFX----")] 
        [field: SerializeField] public EventReference JumpSound {get; private set;}
        [field: SerializeField] public EventReference StickHitSound {get; private set;}
        [field: SerializeField] public EventReference ItemCollectedSound {get; private set;}
        [field: SerializeField] public EventReference UISound {get; private set;}
        [field: SerializeField] public EventReference DeathSound {get; private set;}
    }
}