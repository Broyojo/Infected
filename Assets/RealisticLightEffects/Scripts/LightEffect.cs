using UnityEngine;


namespace RLE
{

    [RequireComponent(typeof(Light))]
    public class LightEffect : MonoBehaviour
    {

        public enum EffectType
        {
            Overvoltage,
            Emergency,
            Flicker,
            MultiColor,
            Blink,
            Toggle
        }

        [HideInInspector]
        public EffectType type;

        [Tooltip("Should the effect play automatically when game starts?")]
        public bool playAtStart;


        [Tooltip("Should the effect start over when finished?")]
        public bool loop;

        [Space(4)]

        [Range(0.0001f, 75f)]
        public float speed;

        [HideInInspector]
        public Light source;

        [HideInInspector]
        public bool shouldPlay;


        public virtual void Awake()
        {
            source = GetComponent<Light>();
        }



        public virtual void Update()
        {
            if (shouldPlay)
                LightFunction();
        }


        public virtual void LightFunction()
        {
            //Different in each effect
        }


        public void Play()
        {
            switch (type)
            {
                case EffectType.Overvoltage:
                    ((Overvoltage)this).Play();
                    break;

                case EffectType.Emergency:
                    ((Emergency)this).Play();
                    break;

                case EffectType.Flicker:
                    ((Flicker)this).Play();
                    break;

                case EffectType.Blink:
                    ((Blink)this).Play();
                    break;

                case EffectType.MultiColor:
                    ((MultiColor)this).Play();
                    break;

                case EffectType.Toggle:
                    ((Toggle)this).Play();
                    break;
            }

        }

        public void Stop()
        {
            switch (type)
            {
                case EffectType.Overvoltage:
                    ((Overvoltage)this).Stop();
                    break;

                case EffectType.Emergency:
                    ((Emergency)this).Stop();
                    break;

                case EffectType.Flicker:
                    ((Flicker)this).Stop();
                    break;

                case EffectType.Blink:
                    ((Blink)this).Stop();
                    break;

                case EffectType.MultiColor:
                    ((MultiColor)this).Stop();
                    break;

                case EffectType.Toggle:
                    ((Toggle)this).Stop();
                    break;
            }
        }

    }
}
