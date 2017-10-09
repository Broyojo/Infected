using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace RLE
{

    public class Emergency : LightEffect
    {

        [Space(10)]

        [HideInInspector]
        public Color firstColor, secondColor;

        [HideInInspector]
        public bool initialColorIsFirstColor;

        public float transitionSpeed;

        bool secondPhase;

        public override void Awake()
        {

            base.Awake();


            type = EffectType.Emergency;

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

            if (initialColorIsFirstColor)
                firstColor = source.color;

            source.color = firstColor;

            secondPhase = false;
        }

        public new void Stop()
        {
            shouldPlay = false;
        }


        public override void LightFunction()
        {
            if (secondPhase)
            {

                source.color = Color.Lerp(source.color, firstColor, speed * Time.deltaTime);

                if (CloseEnough(source.color, firstColor))
                {
                    if (loop)
                    {
                        secondPhase = false;
                    }
                    else
                    {
                        Stop();
                    }
                }


            }
            else
            {

                source.color = Color.Lerp(source.color, secondColor, speed * Time.deltaTime);

                if (CloseEnough(source.color, secondColor))
                {
                    secondPhase = true;
                }

            }
        }

        bool CloseEnough(Color a, Color b)
        {
            float rf = Mathf.Abs(a.r - b.r);
            float gf = Mathf.Abs(a.g - b.g);
            float bf = Mathf.Abs(a.b - b.b);

            if (rf < transitionSpeed && gf < transitionSpeed && bf < transitionSpeed)
                return true;

            return false;

        }

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Emergency))]
    public class EmergencyEditor : Editor
    {
        Emergency e;

        bool initialColorIsFirstColor;

        SerializedObject serializedObj;
        SerializedProperty prop;

        GUIContent g;

        void OnEnable()
        {
            e = (Emergency)target;

            serializedObj = new SerializedObject(e);
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


            prop = serializedObj.FindProperty("speed");
            g.text = "Speed";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0.0001f, 1000);


            EditorGUILayout.Space();

            prop = serializedObj.FindProperty("transitionSpeed");
            g.text = "Transition Speed";
            EditorGUILayout.PropertyField(prop, g);
            prop.floatValue = Mathf.Clamp(prop.floatValue, 0.00001f, 0.99999f);


            prop = serializedObj.FindProperty("initialColorIsFirstColor");
            g.text = "Initial Color Is First Color";
            EditorGUILayout.PropertyField(prop, g);


            if (!prop.boolValue)
            {
                prop = serializedObj.FindProperty("firstColor");
                g.text = "First Color";
                EditorGUILayout.PropertyField(prop, g);
            }
            else
            {
                prop.colorValue = e.GetComponent<Light>().color;
            }

            prop = serializedObj.FindProperty("secondColor");
            g.text = "Second Color";
            EditorGUILayout.PropertyField(prop, g);

            serializedObj.ApplyModifiedProperties();

        }

    }

#endif
}