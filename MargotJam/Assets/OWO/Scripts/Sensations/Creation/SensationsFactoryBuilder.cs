using System.Collections.Generic;

namespace OWO
{
    public static partial class SensationsFactoryBuilder
    {
        public static SensationsFactory WithDynamicSensations(this SensationsFactory _sensationFactory)
        {
            var dynamicSensations = new List<Sensation>();

            dynamicSensations.Add(CreateDynamicSensation(SensationId.Insects, GetAllMuscles()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.FreeFall, GetFrontMuscles()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.LiftObject, GetArms()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.FastDriving, GetPectoralsAndArms()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.InsectBites, GetPectorals()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.MachineGunRecoil, GetPectoralsAndArms()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.PushHeavyObject, GetArms()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.LiftHeavyObject, GetLiftHeavyObjectMuscles()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.IdleSpeed, GetIdleSpeedMuscles()));
            dynamicSensations.Add(CreateDynamicSensation(SensationId.Stress, GetPectorals()));

            _sensationFactory.AddSensations(dynamicSensations.ToArray());
            return _sensationFactory;
        }

        private static Sensation CreateDynamicSensation(SensationId _sensationId, OWOMuscle[] _muscles)
        {
            return new Sensation(SensationType.Dynamic, _sensationId, _muscles);
        }

        public static SensationsFactory WithStaticSensations(this SensationsFactory _sensationFactory)
        {
            var staticSensations = new List<Sensation>();

            staticSensations.Add(CreateStaticSensation(SensationId.Ball, GetAllMuscles()));
            staticSensations.Add(CreateStaticSensation(SensationId.Grip, GetArms()));
            staticSensations.Add(CreateStaticSensation(SensationId.Dart, GetAllMusclesWithoutLumbars()));
            staticSensations.Add(CreateStaticSensation(SensationId.Shot, GetAllMusclesWithoutLumbars()));
            staticSensations.Add(CreateStaticSensation(SensationId.Axe, GetPectoralsAndDorsals()));
            staticSensations.Add(CreateStaticSensation(SensationId.Punch, GetAllMusclesWithoutLumbars()));
            staticSensations.Add(CreateStaticSensation(SensationId.Dagger, GetAbdominals()));
            staticSensations.Add(CreateStaticSensation(SensationId.ShotWithExit, GetPectoralsAndDorsals()));
            staticSensations.Add(CreateStaticSensation(SensationId.AbdominalWound, GetAbdominals()));
            staticSensations.Add(CreateStaticSensation(SensationId.ChestWound, GetPectorals()));
            staticSensations.Add(CreateStaticSensation(SensationId.Oppression, GetDorsals()));
            staticSensations.Add(CreateStaticSensation(SensationId.StrangePresence, GetBackMuscles()));
            staticSensations.Add(CreateStaticSensation(SensationId.GunRecoil, GetArms()));
            staticSensations.Add(CreateStaticSensation(SensationId.HeartBeat, GetHeartBeatMuscle()));
            staticSensations.Add(CreateStaticSensation(SensationId.Hug, GetHugMuscles()));

            _sensationFactory.AddSensations(staticSensations.ToArray());
            return _sensationFactory;
        }

        private static Sensation CreateStaticSensation(SensationId _sensationId, OWOMuscle[] _muscles)
        {
            return new Sensation(SensationType.Static, _sensationId, _muscles);
        }
    }
}