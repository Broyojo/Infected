using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



namespace RLE
{

    public class Toggle : LightEffect
    {

        [HideInInspector]
        public float Smoothness, smoothness;

        

        float initialIntensity, step;

        bool on;

        public override void Awake()
        {

            base.Awake();


            type = EffectType.Toggle;

            smoothness = 1 - Smoothness * 0.01f;

            initialIntensity = source.intensity;

            on = initialIntensity != 0;

            step = initialIntensity * smoothness * (on ? 1 : -1);


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

            on = !on;
            step *= -1;
        }

        public new void Stop()
        {
            shouldPlay = false;
        }


        public override void LightFunction()
        {
            source.intensity += step;

            if (on)
            {
                if (source.intensity >= initialIntensity)
                {
                    source.intensity = initialIntensity;
                    shouldPlay = false;
                    
                    Play(); //Remove This Line To Prevent Looping
                }
            }
            else
            {
                if (source.intensity == 0)
                {
                    shouldPlay = false;
                    Play(); //Remove This Line To Prevent Looping
                }
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Toggle))]
    public class ToggleEditor : Editor
    {
        Toggle t;

        SerializedObject serializedObj;
        SerializedProperty prop;

        GUIContent g;

        void OnEnable()
        {
            t = (Toggle)target;
            serializedObj = new SerializedObject(target);
            g = new GUIContent();
        }

        public override void OnInspectorGUI()
        {

            serializedObj.Update();

            prop = serializedObj.FindProperty("playAtStart");
            g.text = "Play At Start";
            EditorGUILayout.PropertyField(prop, g);

            prop = serializedObj.FindProperty("loop");
            g.text = "Loop";
            EditorGUILayout.PropertyField(prop, g);

            EditorGUILayout.Space();

            prop = serializedObj.FindProperty("Smoothness");
            g.text = "Smoothness";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0, 99.99f);

            serializedObj.ApplyModifiedProperties();

        }
    }


#endif
}