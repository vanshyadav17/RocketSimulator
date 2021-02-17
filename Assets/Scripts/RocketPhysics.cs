using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class RocketPhysics : MonoBehaviour
{

    public float initialFuelWeight;
    public float initialEmptyRocketWeight;

    public double currentRocketWeight;
    public double currentFuelWeight;

    public double massFlowRate;
    public double exhaustVelocity;
    public double areaRatio;
    public double exitPressure;
    public double freeStreamPressure;

    public double thrust;

    public double rocketVelocity;

    public double acceleration;

    public double timeElapsed;

    private double OldVelocity;
    public double specificImpulse;

    public double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }
    public double Acceleration { get => acceleration; set => acceleration = value; }
    public double RocketVelocity { get => rocketVelocity; set => rocketVelocity = value; }
    public double Thrust { get => thrust; set => thrust = value; }
    public double FreeStreamPressure { get => freeStreamPressure; set => freeStreamPressure = value; }
    public double ExitPressure { get => exitPressure; set => exitPressure = value; }
    public double AreaRatio { get => areaRatio; set => areaRatio = value; }
    public double ExhaustVelocity { get => exhaustVelocity; set => exhaustVelocity = value; }
    public double MassFlowRate { get => massFlowRate; set => massFlowRate = value; }
    public double CurrentFuelWeight { get => currentFuelWeight; set => currentFuelWeight = value; }
    public double CurrentRocketWeight { get => currentRocketWeight; set => currentRocketWeight = value; }
    public float InitialFuelWeight { get => initialFuelWeight; set => initialFuelWeight = value; }
    public float InitialEmptyRocketWeight { get => initialEmptyRocketWeight; set => initialEmptyRocketWeight = value; }
    public double SpecificImpulse { get => specificImpulse; set => specificImpulse = value; }

    private bool RocketIsOn = false;

    [SerializeField] private InputField exhaustVelocityField;
    [SerializeField] private InputField massFlowRateField;
    [SerializeField] private InputField exitPressureField;
    [SerializeField] private InputField freeStreamPressureField;
    [SerializeField] private InputField areaRatioField;
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private ParticleSystem dustParticles;

    void Start()
    {
        InitialFuelWeight = 1000000; // kilograms
        InitialEmptyRocketWeight = 8000; // kilograms

        CurrentRocketWeight = InitialFuelWeight + InitialEmptyRocketWeight;
        CurrentFuelWeight = InitialFuelWeight;

        MassFlowRate = 2500; // kilograms per second
        ExhaustVelocity = 2200; // meters per second
        AreaRatio = 46;
        ExitPressure = 100000; // pascals
        FreeStreamPressure = 0; // pascals

        Thrust = CalculateThrust(MassFlowRate, ExhaustVelocity, ExitPressure, FreeStreamPressure, AreaRatio);
        RocketVelocity = 0; // meters per second
        OldVelocity = 0;

        TimeElapsed = 0;

        RocketIsOn = true;

        exhaustVelocityField.text = ExhaustVelocity + " " + exhaustVelocityField.GetComponent<DependantVariableEdit>().unit;
        massFlowRateField.text = MassFlowRate + " " + massFlowRateField.GetComponent<DependantVariableEdit>().unit;
        exitPressureField.text = ExitPressure + " " + exitPressureField.GetComponent<DependantVariableEdit>().unit;
        freeStreamPressureField.text = FreeStreamPressure + " " + freeStreamPressureField.GetComponent<DependantVariableEdit>().unit;
        areaRatioField.text = AreaRatio + " " + areaRatioField.GetComponent<DependantVariableEdit>().unit;


        InvokeRepeating("UpdateTime", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfZero();
        UpdateFuel();
        UpdateThrust();
        UpdateVelocity();
        UpdateAcceleration();
        FetchAndUpdateIndependentVariables();
        
    }


    public void LogStatusUpdate()
    {
       
    }

    public double CalculateThrust(double MassFlowRate, double ExhaustVelocity, double ExitPressure, double FreeStreamPressure, double AreaRatio)
    {
        return ((MassFlowRate * ExhaustVelocity) + ((ExitPressure - FreeStreamPressure) * AreaRatio));
    }

    public double CalculateVelocity(double InitialVelocity, double ExhaustVelocity, double InitialMass, double Mass)
    {
 
        return (InitialVelocity + (ExhaustVelocity * System.Math.Log((InitialMass / Mass))));
    }

    public void UpdateThrust()
    {
        Thrust = CalculateThrust(MassFlowRate, ExhaustVelocity, ExitPressure, FreeStreamPressure, AreaRatio);
    }

    public void UpdateVelocity()
    {
        OldVelocity = RocketVelocity;
        if (!RocketIsOn) return;
        RocketVelocity = CalculateVelocity(0, ExhaustVelocity, (InitialEmptyRocketWeight + InitialFuelWeight), CurrentRocketWeight);
    }

    public void UpdateFuel()
    {
        CurrentFuelWeight = CurrentFuelWeight - (MassFlowRate * Time.deltaTime);
        CurrentRocketWeight = InitialEmptyRocketWeight + CurrentFuelWeight;
    }
   
    public void CheckIfZero()
    {
        if((CurrentFuelWeight - (MassFlowRate * Time.deltaTime)) < 0)
        {
            SpecificImpulse = CalculateSpecificImpulse(Thrust, TimeElapsed, InitialFuelWeight, 9.80);
            MassFlowRate = 0;
            CurrentFuelWeight = 0;
            ExitPressure = 0;
            ExhaustVelocity = 0;
            RocketIsOn = false;
            flameParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            

        }
    }

    public void UpdateTime()
    {
        TimeElapsed = TimeElapsed + 1;
    }

    public void UpdateAcceleration()
    {
        Acceleration = (RocketVelocity - OldVelocity) / Time.deltaTime;
    }

    public double CalculateSpecificImpulse(double AverageForce, double BurnDuration, double PropellantMass, double GravitationalConstant)
    {
        return (AverageForce * BurnDuration) / (PropellantMass * (GravitationalConstant));
    }

    public double randomizeNumber(int minRange, int maxRange, double number)
    {
        System.Random r = new System.Random();
    
        double factor = r.Next(minRange, maxRange);
        return (number + factor);

    }

    public void FetchAndUpdateIndependentVariables()
    {
        ExhaustVelocity = getNumberFromUnitText(exhaustVelocityField);
        MassFlowRate = getNumberFromUnitText(massFlowRateField);
        ExitPressure = getNumberFromUnitText(exitPressureField);
        FreeStreamPressure = getNumberFromUnitText(freeStreamPressureField);
        AreaRatio = getNumberFromUnitText(areaRatioField);
    }

    public double getNumberFromUnitText(InputField inputField)
    {
        string newString = Regex.Replace(inputField.text, "[^.0-9]", "");
        if (newString == "") { return 0; }
        return double.Parse(newString);
    }


    public void makeConsistencyCheck()
    {
        if(massFlowRate <= 0)
        {
            exitPressure = 0;
            exhaustVelocity = 0;
        }
     
    }

}
