using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RLE
{

    public class Flicker : LightEffect
    {


        [Space(10)]
        [HideInInspector]
        public int scale;
        [HideInInspector]
        public int maxChangeMultiplier;

        [HideInInspector]
        public float positionOffsetMultiplier;

        [HideInInspector]
        public bool sourceStatic;


        float initialIntensity, intensityDelta, intensityTarget, initialRange, rangeDelta, rangeTarget;
        float minIntensity, maxIntensity, minRange, maxRange, maxIntensityStep, maxRangeStep;
        float maxPositionReach;
        float tempFloat;

        Vector3 positionDelta, positionTarget;
        Vector3 tempVec;


        public override void Awake()
        {
            base.Awake();

            type = EffectType.Flicker;

            initialIntensity = source.intensity;
            initialRange = source.range;

            minIntensity = (1 - (float)scale * 0.01f) * initialIntensity;
            maxIntensity = (8 - initialIntensity) * (float)(scale * scale) * 0.01f * 0.01f + initialIntensity;

            minRange = (1 - (float)(scale * scale) * 0.01f * 0.01f) * initialRange;
            maxRange = initialRange * (1 + (float)scale * 0.01f);

            maxIntensityStep = (maxIntensity - minIntensity) * (float)maxChangeMultiplier * 0.01f;
            maxRangeStep = (maxRange - minRange) * (float)maxChangeMultiplier * 0.01f;

            positionOffsetMultiplier *= 0.0001f;
            maxPositionReach = (float)scale * positionOffsetMultiplier * 10f;


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

            rangeTarget = source.range;
            intensityTarget = source.intensity;
            positionTarget = source.transform.localPosition;
            SetNewTargets();

        }
        public new void Stop()
        {
            shouldPlay = false;
        }


        public override void LightFunction()
        {
            if ((intensityTarget - source.intensity) * intensityDelta <= 0)
            {
                SetNewTargets();
            }


            source.intensity += intensityDelta * Time.deltaTime * speed;
            source.range += rangeDelta * Time.deltaTime * speed;

            if (!sourceStatic)
            {
                source.transform.localPosition += positionDelta * Time.deltaTime * speed;
            }

        }


        void SetNewTargets()
        {
            tempFloat = Random.Range(-maxIntensityStep, maxIntensityStep);
            intensityTarget = (intensityTarget + tempFloat < minIntensity || intensityTarget + tempFloat > maxIntensity) ? intensityTarget - tempFloat * 0.2f : intensityTarget + tempFloat;
            intensityDelta = intensityTarget - source.intensity;


            tempFloat = Random.Range(-maxRangeStep, maxRangeStep);
            rangeTarget = (rangeTarget + tempFloat < minRange || rangeTarget + tempFloat > maxRange) ? rangeTarget - tempFloat : rangeTarget + tempFloat;
            rangeDelta = rangeTarget - source.range;


            if (!sourceStatic)
            {
                positionTarget = GetRandomVector3();
                positionDelta = positionTarget - source.transform.localPosition;
            }

        }



        Vector3 GetRandomVector3()
        {
            float x = positionTarget.x;
            float y = positionTarget.y;
            float z = positionTarget.z;

            tempFloat = Random.Range(-scale * positionOffsetMultiplier, scale * positionOffsetMultiplier);
            x = (x + tempFloat < -maxPositionReach || x + tempFloat > maxPositionReach) ? x - tempFloat : x + tempFloat;

            tempFloat = Random.Range(-scale * positionOffsetMultiplier, scale * positionOffsetMultiplier);
            y = (y + tempFloat < -maxPositionReach || y + tempFloat > maxPositionReach) ? y - tempFloat : y + tempFloat;

            tempFloat = Random.Range(-scale * positionOffsetMultiplier, scale * positionOffsetMultiplier);
            z = (z + tempFloat < -maxPositionReach || z + tempFloat > maxPositionReach) ? z - tempFloat : z + tempFloat;

            return new Vector3(x, y, z);

        }


    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Flicker))]
    public class FlickerEditor : Editor
    {
        Flicker f;

        SerializedObject serializedObj;
        SerializedProperty prop;

        GUIContent g;

        void OnEnable()
        {
            f = (Flicker)target;

            serializedObj = new SerializedObject(f);
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

            prop = serializedObj.FindProperty("sourceStatic");
            g.text = "Disable Source Movement";
            EditorGUILayout.PropertyField(prop, g);

            if (!prop.boolValue)
            {
                prop = serializedObj.FindProperty("positionOffsetMultiplier");
                g.text = "Position Offset Multiplier";
                EditorGUILayout.PropertyField(prop, g);
                prop.floatValue = Mathf.Clamp(prop.floatValue, 0.00001f, 100000);
            }

            EditorGUILayout.Space();


            prop = serializedObj.FindProperty("scale");
            g.text = "Scale";
            EditorGUILayout.IntSlider(prop, 1, 100, g);

            prop = serializedObj.FindProperty("maxChangeMultiplier");
            g.text = "Max Distance To New Target";
            EditorGUILayout.IntSlider(prop, 1, 100, g);


            serializedObj.ApplyModifiedProperties();

        }

    }


#endif
}