using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace RLE
{

    public class Blink : LightEffect
    {



        [Space(10)]


        public float Smoothness, smoothness;

        public float DarkRate, darkRate;

        public float MinLength, minLength;

        float deltaSum, targetTime, step, initialIntensity, brightRate;


        bool goingDark;


        public override void Awake()
        {

            base.Awake();


            type = EffectType.Blink;

            smoothness = (100 - Smoothness) * 0.01f;

            darkRate = DarkRate * 0.01f;
            brightRate = 1 - darkRate;

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
                m = darkRate / speed;
                min = m * 0.2f <= minLength ? minLength : m * 0.2f;
                step = -source.intensity * smoothness;

                targetTime = Random.Range(min, 2.4f * m <= min ? min : 2.4f * m);
            }
            else
            {
                m = brightRate / speed;
                min = m * 0.2f <= minLength ? minLength : m * 0.2f;
                step = (initialIntensity - source.intensity) * smoothness;

                targetTime = Random.Range(min, 2.4f * m <= min ? min : 2.4f * m);
            }
        }


    }



#if UNITY_EDITOR
    [CustomEditor(typeof(Blink))]
    public class BlinkEditor : Editor
    {

        Blink e;

        SerializedObject serializedObj;
        SerializedProperty prop;

        GUIContent g;

        void OnEnable()
        {
            e = (Blink)target;

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

            prop = serializedObj.FindProperty("DarkRate");
            g.text = "Dark Rate";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0.00001f, 99.99999f);


            prop = serializedObj.FindProperty("MinLength");
            g.text = "Minimum Length Of A Turn";
            EditorGUILayout.PropertyField(prop, g);

            serializedObj.ApplyModifiedProperties();

        }

    }


#endif
}

