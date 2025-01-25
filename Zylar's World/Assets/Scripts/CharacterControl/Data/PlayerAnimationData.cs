using UnityEngine;
using System;

[Serializable]
public class PlayerAnimationData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movingParameterName = "Moving";
    [SerializeField] private string stoppingParameterName = "Stopping";
    [SerializeField] private string landingParameterName = "Landing";
    [SerializeField] private string airborneParameterName = "Airborne";

    [Header("Grounded Parameter Names")]
    [SerializeField] private string idleParameterName = "IsIdling";
    [SerializeField] private string dashParameterName = "IsDashing";
    [SerializeField] private string walkParameterName = "IsWalking";
    [SerializeField] private string runParameterName = "IsRunning";
    [SerializeField] private string sprintParameterName = "IsSprinting";
    [SerializeField] private string mediumStopParameterName = "IsMediumStopping";
    [SerializeField] private string hardStopParameterName = "IsHardStopping";
    [SerializeField] private string rollParameterName = "IsRolling";
    [SerializeField] private string hardLandParameterName = "IsHardLanding";

    [Header("Airborne Parameter Names")]
    [SerializeField] private string fallParameterName = "IsFalling";

    public int GroundedParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }
    public int StoppingParameterHash { get; private set; }
    public int LandingParameterHash { get; private set; }
    public int AirborneParameterHash { get; private set; }

    public int IdleParameterHash { get; private set; }
    public int DashParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int SprintParameterHash { get; private set; }
    public int MediumStopParameterHash { get; private set; }
    public int HardStopParameterHash { get; private set; }
    public int RollParameterHash { get; private set; }
    public int HardLandParameterHash { get; private set; }

    public int FallParameterHash { get; private set; }

    public void Initialize()
    {
        GroundedParameterHash = Animator.StringToHash(groundedParameterName);
        MovingParameterHash = Animator.StringToHash(movingParameterName);
        StoppingParameterHash = Animator.StringToHash(stoppingParameterName);
        LandingParameterHash = Animator.StringToHash(landingParameterName);
        AirborneParameterHash = Animator.StringToHash(airborneParameterName);

        IdleParameterHash = Animator.StringToHash(idleParameterName);
        DashParameterHash = Animator.StringToHash(dashParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        SprintParameterHash = Animator.StringToHash(sprintParameterName);
        MediumStopParameterHash = Animator.StringToHash(mediumStopParameterName);
        HardStopParameterHash = Animator.StringToHash(hardStopParameterName);
        RollParameterHash = Animator.StringToHash(rollParameterName);
        HardLandParameterHash = Animator.StringToHash(hardLandParameterName);

        FallParameterHash = Animator.StringToHash(fallParameterName);
    }
}
