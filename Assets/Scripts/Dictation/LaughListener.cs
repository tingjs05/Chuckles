using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Dictation
{
    public class LaughListener : MonoBehaviour
    {
        public string[] primary_keywords = new string[] {
                "ha",
                "Ha",
                "HA",
                "hA",
                "he",
                "hE",
                "HE",
                "He",
                "ho",
                "hO",
                "HO",
                "Ho",
                "hah",
                "hAh",
                "har"
        };
        

        public event Action OnLaugh;
        public ConfidenceLevel confidence = ConfidenceLevel.Low;
        private KeywordRecognizer recognizer;
        
        
        public string[] GetKeywordVariations()
        {
            List<string> keywordVariations = new List<string>();
            foreach (string keyword in primary_keywords)
            {
                keywordVariations.Add(keyword);
                keywordVariations.Add(keyword + keyword);
                keywordVariations.Add(keyword + keyword + keyword);
            }
            return keywordVariations.ToArray();
        }
        
        private void Start()
        {
            recognizer = new KeywordRecognizer(GetKeywordVariations(), confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
        private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            Debug.Log($"Phrase {args.text} recognized with confidence: {args.confidence}");
        }

        private void OnDestroy()
        {
            if (recognizer != null && recognizer.IsRunning)
            {
                recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
                recognizer.Stop();
            }
        }
    }
}