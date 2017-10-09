using UnityEngine;


namespace RLE
{

    public class Overvoltage : LightEffect
    {

        [Space(10)]


        [Range(0.0001f, 100)]
        public float fadeSpeedMultiplier;

        [Range(1, 100)]
        public float peakLevel;

        public Color peakColor;

        Color initialColor;

        float initialIntensity, initialRange, intensityPeak, rangePeak, intensityStep, rangeStep;
        float colorLerpTimer;

        bool pastPeak;


        void ResetVariables()
        {
            pastPeak = false;

            colorLerpTimer = 0;

            source.intensity = initialIntensity;
            source.range = initialRange;
            source.color = initialColor;

            intensityPeak = (8 - source.intensity) * peakLevel * 0.01f + source.intensity;
            rangePeak = source.range * (peakLevel * 0.01f + 1);

            intensityStep = (intensityPeak - source.intensity) * speed * 0.1f;
            rangeStep = (rangePeak - source.range) * speed * 0.1f;
        }


        public override void Awake()
        {

            base.Awake();


            type = EffectType.Overvoltage;


            initialIntensity = source.intensity;
            initialRange = source.range;
            initialColor = source.color;

            if (playAtStart)
            {
                Play();
            }

        }


        public override void Update()
        {

            base.Update();

        }


        public new void Play()
        {
            shouldPlay = true;

            ResetVariables();
        }

        public new void Stop()
        {
            shouldPlay = false;
        }


        public override void LightFunction()
        {
            if (pastPeak)
            {
                source.intensity -= intensityStep * Time.deltaTime;
                source.range -= rangeStep * Time.deltaTime;


                if (source.intensity <= 0.01f)
                {
                    if (loop)
                    {
                        Play();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
            else
            {
                source.color = Color.Lerp(initialColor, peakColor, colorLerpTimer);
                colorLerpTimer += Time.deltaTime * speed * 0.1f;

                source.range += rangeStep * Time.deltaTime;
                source.intensity += intensityStep * Time.deltaTime;

                if (source.range >= rangePeak)
                {
                    intensityStep *= fadeSpeedMultiplier;
                    rangeStep *= fadeSpeedMultiplier;
                    pastPeak = true;
                }
            }
        }
    }
}
