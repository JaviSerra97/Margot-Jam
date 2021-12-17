namespace OWO
{
    public static partial class SensationsFactoryBuilder
    {
        private static OWOMuscle[] GetAllMuscles()
        {
            return new OWOMuscle[] { OWOMuscle.Abdominal_L,
                                     OWOMuscle.Abdominal_R,
                                     OWOMuscle.Arm_L,
                                     OWOMuscle.Arm_R,
                                     OWOMuscle.Dorsal_L,
                                     OWOMuscle.Dorsal_R,
                                     OWOMuscle.Lumbar_L,
                                     OWOMuscle.Lumbar_R,
                                     OWOMuscle.Pectoral_L,
                                     OWOMuscle.Pectoral_R};
        }

        private static OWOMuscle[] GetAllMusclesWithoutLumbars()
        {
            return new OWOMuscle[] { OWOMuscle.Abdominal_L,
                                     OWOMuscle.Abdominal_R,
                                     OWOMuscle.Arm_L,
                                     OWOMuscle.Arm_R,
                                     OWOMuscle.Dorsal_L,
                                     OWOMuscle.Dorsal_R,
                                     OWOMuscle.Pectoral_L,
                                     OWOMuscle.Pectoral_R };
        }

        private static OWOMuscle[] GetArms()
        {
            return new OWOMuscle[] {OWOMuscle.Arm_L,
                                    OWOMuscle.Arm_R };
        }

        private static OWOMuscle[] GetPectorals()
        {
            return new OWOMuscle[] {OWOMuscle.Pectoral_L,
                                    OWOMuscle.Pectoral_R};
        }

        private static OWOMuscle[] GetFrontMuscles()
        {
            return new OWOMuscle[] { OWOMuscle.Abdominal_L,
                                     OWOMuscle.Abdominal_R,
                                     OWOMuscle.Arm_L,
                                     OWOMuscle.Arm_R,
                                     OWOMuscle.Pectoral_L,
                                     OWOMuscle.Pectoral_R };
        }

        private static OWOMuscle[] GetBackMuscles()
        {
            return new OWOMuscle[] { OWOMuscle.Dorsal_L,
                                     OWOMuscle.Dorsal_R,
                                     OWOMuscle.Lumbar_L,
                                     OWOMuscle.Lumbar_R };
        }

        private static OWOMuscle[] GetDorsals()
        {
            return new OWOMuscle[] { OWOMuscle.Dorsal_L,
                                     OWOMuscle.Dorsal_R };
        }

        private static OWOMuscle[] GetPectoralsAndArms()
        {
            return new OWOMuscle[] { OWOMuscle.Arm_L,
                                     OWOMuscle.Arm_R,
                                     OWOMuscle.Pectoral_L,
                                     OWOMuscle.Pectoral_R };
        }

        private static OWOMuscle[] GetAbdominals()
        {
            return new OWOMuscle[] { OWOMuscle.Abdominal_L,
                                     OWOMuscle.Abdominal_R };
        }

        private static OWOMuscle[] GetLiftHeavyObjectMuscles()
        {
            return new OWOMuscle[] {OWOMuscle.Arm_R,
                                    OWOMuscle.Arm_L,
                                    OWOMuscle.Abdominal_L,
                                    OWOMuscle.Abdominal_R,
                                    OWOMuscle.Lumbar_R,
                                    OWOMuscle.Lumbar_L};
        }

        private static OWOMuscle[] GetIdleSpeedMuscles()
        {
            return new OWOMuscle[]{ OWOMuscle.Arm_L,
                                    OWOMuscle.Arm_R,
                                    OWOMuscle.Abdominal_R,
                                    OWOMuscle.Abdominal_L,
                                    OWOMuscle.Dorsal_L,
                                    OWOMuscle.Dorsal_R};
        }

        private static OWOMuscle[] GetHeartBeatMuscle()
        {
            return new OWOMuscle[] { OWOMuscle.Pectoral_L };
        }

        private static OWOMuscle[] GetHugMuscles()
        {
            return new OWOMuscle[] { OWOMuscle.Pectoral_L, OWOMuscle.Pectoral_R,
                                     OWOMuscle.Arm_L, OWOMuscle.Arm_R,
                                     OWOMuscle.Dorsal_L, OWOMuscle.Dorsal_R};
        }

        private static OWOMuscle[] GetPectoralsAndDorsals()
        {
            return new OWOMuscle[] { OWOMuscle.Pectoral_L, OWOMuscle.Pectoral_R,
                                     OWOMuscle.Dorsal_L, OWOMuscle.Dorsal_R};
        }
    }
}