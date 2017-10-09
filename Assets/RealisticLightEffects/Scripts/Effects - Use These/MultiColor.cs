using UnityEngine;

namespace RLE
{

    public class MultiColor : LightEffect
    {

        [Space(10)]

        [Tooltip("Add as many colors as you want")]
        public Color[] colors;


        int amount, current;

        float lerpTimer;

        public override void Awake()
        {

            base.Awake();


            type = EffectType.MultiColor;


            amount = colors.Length;

            lerpTimer = 0;

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
            if (colors.Length <= 0)
            {
                Debug.LogError("'Colors' property of 'MultiColor' effect must contain more than one color. It currently contains " + colors.Length + ".");
                return;
            }
            shouldPlay = true;
            lerpTimer = 0;
        }

        public new void Stop()
        {
            shouldPlay = false;
        }


        public override void LightFunction()
        {
            lerpTimer += Time.deltaTime * speed;

            source.color = Color.Lerp(colors[current], colors[(current + 1) % amount], lerpTimer);

            if (lerpTimer >= 1)
            {
                lerpTimer = 0;

                current++;

                if (current == amount)
                {
                    if (loop)
                        current -= amount;
                    else
                        Stop();
                }
            }

        }

    }
}


