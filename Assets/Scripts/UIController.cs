using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public Text TimeElapsed;
    public Text Thrust;
    public Text Velocity;
    public Text Acceleration;
    public Text Fuel;
    public Text FuelPercentage;
    public GameObject FuelBar;

    private float initialBarHeight = 0;

    void Start()
    {
        var rectTransform = FuelBar.transform as RectTransform;
        initialBarHeight = rectTransform.sizeDelta.y;
    }

    void Update()
    {
        UpdateTimeElapsed();
        UpdateThrust();
        UpdateAcceleration();
        UpdateVelocity();
        UpdateFuelBar();
        UpdateFuel();
        UpdateFuelPercentage();
    }

    void UpdateTimeElapsed()
    {
        double timeElapsed = gameObject.GetComponent<RocketPhysics>().timeElapsed;
        System.TimeSpan t = System.TimeSpan.FromSeconds(timeElapsed);
        string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds);
       

        TimeElapsed.text = "T+" + timeFormatted;
    }

    void UpdateThrust()
    {
        double thrust = gameObject.GetComponent<RocketPhysics>().thrust;
        string textFormatted = "THRUST         " + System.Math.Round(thrust, 2).ToString() + " N";
        Thrust.text = textFormatted;
    }

    void UpdateVelocity()
    {
        double velocity = gameObject.GetComponent<RocketPhysics>().rocketVelocity;
        string textFormatted = "VELOCITY       " + System.Math.Round(velocity, 2).ToString() + " m/s";
        Velocity.text = textFormatted;
    }

    void UpdateAcceleration()
    {
        double acceleration = gameObject.GetComponent<RocketPhysics>().acceleration;
        string textFormatted = "ACCELERATION   " + System.Math.Round(acceleration, 2).ToString() + " m/s²";
        Acceleration.text = textFormatted;
    }

    void UpdateFuelBar()
    {
        var rectTransform = FuelBar.transform as RectTransform;
        double initialFuel = gameObject.GetComponent<RocketPhysics>().initialFuelWeight;
        double currentFuel = gameObject.GetComponent<RocketPhysics>().currentFuelWeight;
        float currentFuelMultiplier = (float) (currentFuel / initialFuel);

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (currentFuelMultiplier * initialBarHeight));
    }

    void UpdateFuel()
    {
        double currentFuel = gameObject.GetComponent<RocketPhysics>().currentFuelWeight;
        string textFormatted = "PROPELLANT   " + System.Math.Round(currentFuel, 0).ToString() + " kg";
        Fuel.text = textFormatted;
    }

    void UpdateFuelPercentage()
    {
        double initialFuel = gameObject.GetComponent<RocketPhysics>().initialFuelWeight;
        double currentFuel = gameObject.GetComponent<RocketPhysics>().currentFuelWeight;
        double currentFuelPercentage = ((currentFuel / initialFuel) * 100);
        string textFormatted = System.Math.Round(currentFuelPercentage, 1).ToString() + "%";
     
        FuelPercentage.text = textFormatted;
    }
}
