using UnityEngine;
using UnityEngine.UI;


namespace VT.Audio.Analyzer
{
    public class GridDisplayModule : AudioAnalyzerDisplayModule
    {
        public GridDisplayModule(AudioAnalyzer audioAnalyzer, GameObject displayPrefab, Transform parent, int width, int height)
            : base(audioAnalyzer, displayPrefab, parent, width, height)
        {
        }

        protected override void Display()
        {
            for (int i = 0; i < width; i++)
            {
                int visibleBlocks = Mathf.CeilToInt(audioAnalyzer.SmoothAudioFrequency[i] * height);
                for (int j = 0; j < height; j++)
                {
                    image = displayBlocks[i, j].GetComponent<Image>();
                    image.color = Color.clear;

                    if (j < visibleBlocks)
                    {
                        if (UseColorLerp)
                        {
                            SetBlockLerpColor(j);
                        }
                        else
                        {
                            SetBlockSeparateColor(j);
                        }
                    }
                }
            }
        }

        protected override void SetBlockSeparateColor(int index)
        {
            if (index > height / 3f * 2f)
            {
                image.color = highRangeColor;
            }
            else if (index > height / 3f)
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
            image.color = gradient.Evaluate((float)index / height);
        }
    }
}