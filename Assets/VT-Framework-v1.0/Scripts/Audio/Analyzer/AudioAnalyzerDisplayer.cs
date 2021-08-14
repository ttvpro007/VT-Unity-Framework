using UnityEngine;

namespace VT.Audio.Analyzer
{
    public class AudioAnalyzerDisplayer : MonoBehaviour
    {
        public enum Type
        {
            Bar,
            Grid
        }

        [SerializeField] private GameObject metricBoxPrefab;
        [SerializeField] private Transform barDisplayTransform;
        [SerializeField] private Transform gridDisplayTransform;
        [SerializeField] private Type displayType = Type.Bar;
        [Range(2, 64)]
        [SerializeField] private int width = 8, height = 8;
        [SerializeField] private bool useColorLerp;
        [SerializeField] private Color lowRangeColor;
        [SerializeField] private Color midRangeColor;
        [SerializeField] private Color highRangeColor;

        private Type lastDisplayType;
        private bool lastUseColorLerp;
        GridDisplayModule gridDisplayModule;
        BarDisplayModule barDisplayModule;

        private void Awake()
        {
            InitializeDisplay();
        }

        private void Update()
        {
            if (displayType != lastDisplayType)
            {
                SetupDisplay();
            }

            if (useColorLerp != lastUseColorLerp)
            {
                SetupColor();
            }

            if (displayType == Type.Bar)
            {
                barDisplayModule.Update();
            }
            else if (displayType == Type.Grid)
            {
                gridDisplayModule.Update();
            }
        }

        private void InitializeDisplay()
        {
            barDisplayModule = new BarDisplayModule(AudioAnalyzer.Instance, metricBoxPrefab, barDisplayTransform, width);
            barDisplayModule.SetSpectrumsColor(lowRangeColor, midRangeColor, highRangeColor);

            gridDisplayModule = new GridDisplayModule(AudioAnalyzer.Instance, metricBoxPrefab, gridDisplayTransform, width, height);
            gridDisplayModule.SetSpectrumsColor(lowRangeColor, midRangeColor, highRangeColor);

            SetupColor();
            SetupDisplay();
        }

        private void SetupColor()
        {
            barDisplayModule.UseColorLerp = useColorLerp;
            gridDisplayModule.UseColorLerp = useColorLerp;
            lastUseColorLerp = useColorLerp;
        }

        private void SetupDisplay()
        {
            if (!barDisplayModule.HasSetup)
            {
                barDisplayModule.Setup();
            }

            if (!gridDisplayModule.HasSetup)
            {
                gridDisplayModule.Setup();
            }

            if (displayType == Type.Bar)
            {
                if (!barDisplayModule.IsActive)
                {
                    barDisplayModule.SetActive(true);
                }

                if (gridDisplayModule.IsActive)
                {
                    gridDisplayModule.SetActive(false);
                }
            }
            else if (displayType == Type.Grid)
            {
                if (barDisplayModule.IsActive)
                {
                    barDisplayModule.SetActive(false);
                }

                if (!gridDisplayModule.IsActive)
                {
                    gridDisplayModule.SetActive(true);
                }
            }

            lastDisplayType = displayType;
        }
    }
}