using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Dictation
{
    public class SpeechToText : MonoBehaviour
    {
        public string[] keywords = new string[] {"ha"};
        public ConfidenceLevel confidence = ConfidenceLevel.Medium;
        private KeywordRecognizer recognizer;
        private void Start()
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
        private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            Debug.Log($"Phrase {args.text} recognized with confidence: {args.confidence}");
        }
        private void OnApplicationQuit()
        {
            if (recognizer != null && recognizer.IsRunning)
            {
                recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
                recognizer.Stop();
            }
        }
    }
}