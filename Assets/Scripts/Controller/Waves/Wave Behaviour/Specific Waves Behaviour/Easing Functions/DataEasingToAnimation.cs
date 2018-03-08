using UnityEngine;
using EaseEquations = EasingEquationsDouble.EasingEquations;

public class DataEasingToAnimation {

    public static AnimationCurve ConvertEaseEquation(EaseEquations e, AnimationCurve animationEaseCurve) {
        ConversionProperties cp = CreateConversionProperties();

        // All Ease conversions share the same conversion properties with exception of SmoothTangentMaxAngle
        switch (e) {
            case EaseEquations.BounceEaseIn:
            case EaseEquations.BounceEaseInOut:
            case EaseEquations.BounceEaseOut:
            case EaseEquations.BounceEaseOutIn:
                cp.m_SmoothTangentMaxAngle = 60f; break;
            default:
                cp.m_SmoothTangentMaxAngle = 180f; break;
        }

        animationEaseCurve = new AnimationCurve();
        EasingToAnimationCurve.ConvertEaseEquationToCurve(e, animationEaseCurve, cp);
        return animationEaseCurve;
    }

    private static ConversionProperties CreateConversionProperties() {
        ConversionProperties cp = new ConversionProperties();
        cp.m_FitError = 0.001f;
        cp.m_RdpError = 0.0035f;
        cp.m_PointDistance = 0.01f;
        cp.m_NumEquationSteps = 1000;
        cp.m_MaxStepsBetweenPoints = 1f;
        cp.m_UseCurveFit = false;
        cp.m_TangentMode = RuntimeAnimationUtility.TangentMode.Auto;
        cp.m_PreprocessMode = ePreprocessModes.RDP;
        cp.m_SmoothTangentMaxAngle = 60f;

        return cp;
    }
}
