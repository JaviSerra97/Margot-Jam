namespace OWO
{
    public enum OWOMuscle
    {
        Pectoral_R, Pectoral_L,
        Abdominal_R, Abdominal_L,
        Arm_R, Arm_L,
        Dorsal_R, Dorsal_L,
        Lumbar_R, Lumbar_L
    };

    public enum SensationType { None, Static, Dynamic }

    public enum SensationId
    {
        Ball = 0, Dart = 1, Dagger = 2, ShotWithExit = 4, Axe = 6, Punch = 7, Grip = 8, Shot = 9,
        Insects = 10, FreeFall = 11, LiftObject = 12, LiftHeavyObject = 13, FastDriving = 14,
        IdleSpeed = 15, InsectBites = 16, MachineGunRecoil = 17, PushHeavyObject = 18, Stress = 19,
        AbdominalWound = 20, ChestWound = 21, Oppression = 22, StrangePresence = 23,
        GunRecoil = 28, HeartBeat = 29, Hug = 30
    }

    public enum DynamicSensation
    {
        Insects = 10, FreeFall = 11, LiftObject = 12, LiftHeavyObject = 13,
        FastDriving = 14, IdleSpeed = 15, InsectBites = 16, MachineGunRecoil = 17,
        PushHeavyObject = 18, Stress = 19, StrangePresence = 23
    }
}