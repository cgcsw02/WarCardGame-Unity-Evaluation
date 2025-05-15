using UnityEngine;

namespace Game.Presentation
{
    public class SimpleSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource loopMusicSource;
        [SerializeField] private AudioClip cardFlip, win, lose, notCorrect, loopMusic;

        private void Awake()
        {
            // Validate assignments
            if (sfxSource == null)
                Debug.LogError("SFX AudioSource not assigned in SimpleSoundPlayer!");
            if (loopMusicSource == null)
                Debug.LogError("Loop Music AudioSource not assigned in SimpleSoundPlayer!");
            if (cardFlip == null)
                Debug.LogError("Card Flip AudioClip not assigned in SimpleSoundPlayer!");
            if (win == null)
                Debug.LogError("Win AudioClip not assigned in SimpleSoundPlayer!");
            if (lose == null)
                Debug.LogError("Lose AudioClip not assigned in SimpleSoundPlayer!");
            if (notCorrect == null)
                Debug.LogError("NotCorrect AudioClip not assigned in SimpleSoundPlayer!");
            if (loopMusic == null)
                Debug.LogError("Loop Music AudioClip not assigned in SimpleSoundPlayer!");
        }

        public void PlayCardFlip()
        {
            if (cardFlip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(cardFlip);
                Debug.Log("Playing card flip sound");
            }
            else
                Debug.LogWarning("Card flip sound not played due to null clip or source!");
        }

        public void PlayWin()
        {
            if (win != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(win);
                Debug.Log("Playing win sound");
            }
            else
                Debug.LogWarning("Win sound not played due to null clip or source!");
        }

        public void PlayLose()
        {
            if (lose != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(lose);
                Debug.Log("Playing lose sound");
            }
            else
                Debug.LogWarning("Lose sound not played due to null clip or source!");
        }

        public void PlayNotCorrect()
        {
            if (notCorrect != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(notCorrect);
                Debug.Log("Playing notCorrect sound");
            }
            else
                Debug.LogWarning("NotCorrect sound not played due to null clip or source!");
        }

        public void PlayloopMusic()
        {
            if (loopMusic != null && loopMusicSource != null)
            {
                loopMusicSource.clip = loopMusic;
                loopMusicSource.loop = true;
                loopMusicSource.Play();
                Debug.Log("Playing loop music");
            }
            else
                Debug.LogWarning("Loop music not played due to null clip or source!");
        }
    }
}