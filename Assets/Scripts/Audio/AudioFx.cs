using System;
using System.Collections;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public struct AudioFx
    {
        [SerializeField]
        AudioClip clip;
        [SerializeField]
        float delaySeconds;
        
        public void Play(MonoBehaviour script, Vector3? position = null)
        {
            script.StartCoroutine(play_delayed(position));
        }

        IEnumerator play_delayed(Vector3? position)
        {
            yield return new WaitForSeconds(delaySeconds);
            AudioSource.PlayClipAtPoint(clip, position ?? Camera.main.transform.position);
        }
    }
}