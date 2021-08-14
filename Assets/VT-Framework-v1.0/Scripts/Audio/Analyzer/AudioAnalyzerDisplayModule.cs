using UnityEngine;
using UnityEngine.UI;

namespace VT.Audio.Analyzer
{
    public abstract class AudioAnalyzerDisplayModule
    {
        public bool HasSetup { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool UseColorLerp { get; set; }
        protected float offsetX;
        protected float offsetY;
        protected GameObject displayPrefab;
        protected Transform parent;
        protected Vector3 spawnPosition;
        protected int width;
        protected int height;
        protected GameObject[,] displayBlocks;
        protected AudioAnalyzer audioAnalyzer;
        protected Image image;
        protected Color inactiveColor = Color.clear;
        protected Color lowRangeColor = Color.white;
        protected Color midRangeColor = Color.white;
        protected Color highRangeColor = Color.white;

        public AudioAnalyzerDisplayModule(AudioAnalyzer audioAnalyzer, GameObject displayPrefab, Transform parent, int width, int height)
        {
            this.audioAnalyzer = audioAnalyzer;
            this.displayPrefab = displayPrefab;
            this.parent = parent;
            this.width = width;
            this.height = height;

            offsetX = displayPrefab.transform.lossyScale.x / 2f;
            offsetY = displayPrefab.transform.lossyScale.y / 2f;
        }

        public void Setup()
        {
            displayBlocks = new GameObject[width, height];

            //Make it so that center will be in the middle of the amount of spawned boxes horizontally and vertically
            float spawnPositionX = parent.position.x + offsetX / 2f * (1 - width);
            float spawnPositionY = parent.position.y + offsetY / 2f * (1 - height);

            spawnPosition.x = spawnPositionX;
            spawnPosition.y = spawnPositionY;

            for (int i = 0; i < width; i++)
            {
                spawnPosition.y = spawnPositionY;

                for (int j = 0; j < height; j++)
                {
                    displayBlocks[i, j] = Object.Instantiate(displayPrefab, spawnPosition, Quaternion.identity, parent);
                    spawnPosition.y += offsetY;
                }

                spawnPosition.x += offsetX;
            }

            SetActive(false);
            HasSetup = true;
        }

        public void SetActive(bool value)
        {
            parent.gameObject.SetActive(value);
            IsActive = value;
        }

        protected Gradient gradient;
        public void SetSpectrumsColor(Color lowRangeColor, Color midRangeColor, Color highRangeColor)
        {
            this.lowRangeColor = lowRangeColor;
            this.midRangeColor = midRangeColor;
            this.highRangeColor = highRangeColor;
            gradient = new Gradient();
            gradient.SetKeys
            (
                new GradientColorKey[]
                {
                    new GradientColorKey(lowRangeColor, 0f),
                    new GradientColorKey(midRangeColor, 0.5f),
                    new GradientColorKey(highRangeColor, 1f)
                },

                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            );
        }

        public void Update()
        {
            if (!HasSetup) return;
            if (!IsActive) return;

            Display();
        }

        protected abstract void Display();
        protected abstract void SetBlockSeparateColor(int index);
        protected abstract void SetBlockLerpColor(int index);
    }
}