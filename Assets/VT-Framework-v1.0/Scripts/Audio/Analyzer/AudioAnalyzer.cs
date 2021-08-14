using UnityEngine;
using VT.Utilities.Singleton;

namespace VT.Audio.Analyzer
{
    public class AudioAnalyzer : Singleton<AudioAnalyzer>
    {
        [Header("Audio Spectrum Analyzer Settings")]
        [Range(MIN_BANDS, MAX_BANDS)]
        [SerializeField] private int numberOfVisualizerBars = MIN_BANDS;
        [SerializeField] private FFTWindow spectrumAnalyzer;
        [SerializeField] private bool calculateSmoothFrequency;

        private const int MIN_BANDS = 2;
        private const int MAX_BANDS = 64;
        private const int SAMPLE_COUNTS = 128;
        private const float SMOOTH_DECREASE = 0.0001f;
        private const float SMOOTH_DECREASE_OVERTIME_MULTIPLIER = 2.71828f; //Euler's number... because nature!

        private float[] sampleLeft;
        private float[] sampleRight;
        private float[] frequencyBands;
        private float[] smoothFrequencyBands;
        private float[] bandBufferDecrease;
        private float[] currentHighestFrequencies;

        public float[] AudioFrequency { get; private set; }
        public float[] SmoothAudioFrequency { get; private set; }

        private void Start()
        {
            InitializeAudioAnalyzer();
        }

        private void Update()
        {
            GetAudioSourceSpectrum();
            MakeFrequencyBands();
            FindHighestFrequencies();
            CreateAudioFreqencyBands();
        }

        private void InitializeAudioAnalyzer()
        {
            sampleLeft = sampleRight = new float[SAMPLE_COUNTS];
            frequencyBands = new float[numberOfVisualizerBars];
            smoothFrequencyBands = new float[numberOfVisualizerBars];
            bandBufferDecrease = new float[numberOfVisualizerBars];
            currentHighestFrequencies = new float[numberOfVisualizerBars];
            AudioFrequency = new float[numberOfVisualizerBars];
            SmoothAudioFrequency = new float[numberOfVisualizerBars];
        }

        private void GetAudioSourceSpectrum()
        {
            AudioListener.GetSpectrumData(sampleLeft, 0, spectrumAnalyzer);
            AudioListener.GetSpectrumData(sampleRight, 1, spectrumAnalyzer);
        }

        private void MakeFrequencyBands()
        {
            int bandIndex = 0;
            for (int i = 0; i < SAMPLE_COUNTS; i++)
            {
                float averageLeft = 0f;
                float averageRight = 0f;

                int sampleCount = (int)Mathf.Lerp(MIN_BANDS, SAMPLE_COUNTS - 1, (float)i / (numberOfVisualizerBars - 1));

                for (int j = bandIndex; j < sampleCount; j++)
                {
                    averageLeft += sampleLeft[bandIndex] * (bandIndex + 1);
                    averageRight += sampleRight[bandIndex] * (bandIndex + 1);
                    bandIndex++;
                }

                averageLeft /= sampleCount;
                averageRight /= sampleCount;

                if (i < frequencyBands.Length)
                {
                    frequencyBands[i] = (averageLeft + averageRight) / 2f;

                    if (calculateSmoothFrequency)
                    {
                        CalculateSmoothFrequencyBand(i);
                    }
                }
            }
        }

        private void CalculateSmoothFrequencyBand(int index)
        {
            if (frequencyBands[index] > smoothFrequencyBands[index])
            {
                smoothFrequencyBands[index] = frequencyBands[index];
                bandBufferDecrease[index] = SMOOTH_DECREASE;
            }
            else
            {
                smoothFrequencyBands[index] -= bandBufferDecrease[index];
                smoothFrequencyBands[index] = Mathf.Max(frequencyBands[index], smoothFrequencyBands[index]);
                bandBufferDecrease[index] *= SMOOTH_DECREASE_OVERTIME_MULTIPLIER;
            }
        }

        private void FindHighestFrequencies()
        {
            for (int i = 0; i < numberOfVisualizerBars; i++)
            {
                if (currentHighestFrequencies[i] < frequencyBands[i])
                {
                    currentHighestFrequencies[i] = frequencyBands[i];
                }
            }
        }

        private void CreateAudioFreqencyBands()
        {
            for (int i = 0; i < numberOfVisualizerBars; i++)
            {
                if (currentHighestFrequencies[i] != 0)
                {
                    AudioFrequency[i] = frequencyBands[i] / currentHighestFrequencies[i];

                    if (calculateSmoothFrequency)
                    {
                        SmoothAudioFrequency[i] = smoothFrequencyBands[i] / currentHighestFrequencies[i];
                    }
                }
            }
        }
    }
}