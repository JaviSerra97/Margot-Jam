using UnityEngine;
using OWO;

public class SensationsSenderTest : MonoBehaviour
{
    [SerializeField] private string ip = string.Empty;
    [SerializeField] private OWOController owoController = null;

    [Header("Send sensation")]
    [SerializeField] private SensationId defaultSensation = SensationId.Ball;
    [SerializeField] private OWOMuscle muscle = OWOMuscle.Abdominal_L;

    [Header("Dynamic sensations")]
    [SerializeField] private DynamicSensation dynamicSensation = DynamicSensation.FastDriving;
    [SerializeField] private OWOMuscle dynamicSensationMuscle = OWOMuscle.Abdominal_L;
    [SerializeField] private int intensityPercentage = 0;

    private void Start()
    {
        OWOClient.OnSensationSent += debug => Debug.Log(debug);

        owoController = new OWOController();
        owoController.Connect(ip);
    }

    [ContextMenu("Send sensation")]
    public void SendSensation()
    {
        owoController.SendSensation(defaultSensation, muscle);
    }

    [ContextMenu("Continous sensation")]
    public void SendContinousSensation()
    {
        owoController.SendContinuosSensation(dynamicSensation, muscle);
    }

    [ContextMenu("Sensation percentage")]
    public void SendSensationWithPercentage()
    {
        owoController.SendSensationWithPercentage(defaultSensation, dynamicSensationMuscle, intensityPercentage);
    }

    [ContextMenu("Continuos sensation percentage")]
    public void SendContinuosSensationWithPercentage()
    {
        owoController.SendContinuosSensationWithPercentage(dynamicSensation,
                                                           dynamicSensationMuscle,
                                                           intensityPercentage);
    }
}