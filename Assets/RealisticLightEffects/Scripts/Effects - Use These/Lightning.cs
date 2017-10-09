using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace RLE
{

    public class Lightning : LightEffect
    {



        [Space(10)]


        public float Smoothness, smoothness;

        public float frequency;

        public float lightningLengthMultiplier;

        public float MinLength, minLength;

        float deltaSum, targetTime, step, initialIntensity, period;


        int state;


        bool goingDark;


        public override void Awake()
        {

            base.Awake();


            type = EffectType.Blink;

            smoothness = (100 - Smoothness) * 0.01f;

            period = 1 / frequency;

            lightningLengthMultiplier *= 0.1f;

            minLength = MinLength;

            initialIntensity = source.intensity;

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

            deltaSum = 0;
            goingDark = source.intensity == 0;

            state = 0;

            SetNewTargets();
        }

        public new void Stop()
        {
            shouldPlay = false;
        }


        public override void LightFunction()
        {
            deltaSum += Time.deltaTime;

            if (deltaSum >= targetTime)
            {
                deltaSum = 0;

                goingDark = !goingDark;

                SetNewTargets();
            }

            source.intensity += step;

            if (source.intensity > initialIntensity)
                source.intensity = initialIntensity;


        }

        void SetNewTargets()
        {
            float m, min;

            if (goingDark)
            {
                m = period / speed;

                step = -source.intensity * smoothness;

                if (state == 1)
                {
                    min = m * 0.1f;
                    targetTime = Random.Range(min, 0.6f * m);
                }
                else
                {
                    min = m * 1f;
                    targetTime = Random.Range(min, 6.5f * m);
                }


            }
            else
            {
                m = lightningLengthMultiplier / speed;

                if (state == 0)
                {
                    if (Random.Range(0, 4) % 4 == 0)
                    {
                        min = m * 0.2f;
                        targetTime = Random.Range(min, .6f * m);
                    }
                    else
                    {
                        min = m * 0.1f;
                        targetTime = Random.Range(min, .3f * m);
                        state = 1;
                    }
                }
                else
                {
                    min = m * .11f;
                    targetTime = Random.Range(min, .35f * m);
                    state = 0;
                }

                step = (initialIntensity - source.intensity) * smoothness;

            }
        }


    }



#if UNITY_EDITOR
    [CustomEditor(typeof(Lightning))]
    public class LightningEditor : Editor
    {

        Lightning e;

        SerializedObject serializedObj;
        SerializedProperty prop;

        GUIContent g;

        void OnEnable()
        {
            e = (Lightning)target;

            serializedObj = new SerializedObject(e);
            g = new GUIContent();
        }

        public override void OnInspectorGUI()
        {
            serializedObj.Update();

            prop = serializedObj.FindProperty("playAtStart");
            g.text = "Play At Start";
            EditorGUILayout.PropertyField(prop, g);

            prop = serializedObj.FindProperty("speed");
            g.text = "Speed";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0.0001f, 1000);


            EditorGUILayout.Space();


            prop = serializedObj.FindProperty("Smoothness");
            g.text = "Smoothness";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0.01f, 99.99f);


            prop = serializedObj.FindProperty("frequency");
            g.text = "Frequency";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0.00001f, 1000f);

            prop = serializedObj.FindProperty("lightningLengthMultiplier");
            g.text = "Lightning Length Multiplier";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0.00001f, 1000f);

            serializedObj.ApplyModifiedProperties();

        }

    }


#endif
}

