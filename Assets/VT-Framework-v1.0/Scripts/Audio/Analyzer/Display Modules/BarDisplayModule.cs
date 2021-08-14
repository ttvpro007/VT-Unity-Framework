using UnityEngine;
using UnityEngine.UI;

namespace VT.Audio.Analyzer
{
    public class BarDisplayModule : AudioAnalyzerDisplayModule
    {
        public BarDisplayModule(AudioAnalyzer audioAnalyzer, GameObject displayPrefab, Transform parent, int width)
            : base(audioAnalyzer, displayPrefab, parent, width, 1)
        {
            maxScale = scaleMultiplier + minScale;
        }

        private Vector3 newScale;
        private float scaleMultiplier = 10f;
        private float minScale = 1f;
        private float maxScale;

        protected override void Display()
        {
            for (int i = 0; i < width; i++)
            {
                newScale = displayBlocks[i, 0].transform.localScale;
                newScale.y = AudioAnalyzer.Instance.SmoothAudioFrequency[i] * scaleMultiplier + minScale;
                displayBlocks[i, 0].transform.localScale = newScale;
                image = displayBlocks[i, 0].GetComponent<Image>();

                if (UseColorLerp)
                {
                    SetBlockLerpColor(i);
                }
                else
                {
                    SetBlockSeparateColor(i);
                }
            }
        }

        protected override void SetBlockSeparateColor(int index)
        {
            if (displayBlocks[index, 0].transform.localScale.y > maxScale / 3f * 2f)
            {
                image.color = highRangeColor;
            }
            else if (displayBlocks[index, 0].transform.localScale.y > maxScale / 3f)
            {
                image.color = midRangeColor;
            }
            else
            {
                image.color = lowRangeColor;
            }
        }

        protected override void SetBlockLerpColor(int index)
        {
            image.color = gradient.Evaluate(displayBlocks[index, 0].transform.localScale.y / maxScale);
        }
    }
}