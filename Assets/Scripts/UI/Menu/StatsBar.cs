using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    public static StatsBar instance;

	public VehicleStatBar[] looms = new VehicleStatBar[3];

	public VehicleStatBar[] engines = new VehicleStatBar[5];

	public VehicleStatBar[] wheels = new VehicleStatBar[6];

	public VehicleStatBar[] Accessory1 = new VehicleStatBar[10];

	public VehicleStatBar[] Accessory2 = new VehicleStatBar[11];


    private enum ImageBars { SpeedNew, Speed, FuelConsumptionNew, FuelConsumption, AccelerationNew, Acceleration, AdherenceOneNew, AdherenceOne,
        AdherenceTwoNew, AdherenceTwo, AdherenceThreeNew, AdherenceThree,
        WeightNew, Weight, }

    private Image[] _StatsBars = new Image[14];

    private Color _Red = Color.red;
    private Color _Green = Color.green;
    private Color _DefaultColor;

    private Vector2 _GaragePos, _RecapPos;

    private int _DefaultLoomIndex = 0;
    private int _DefaultEngineIndex = 0;
    private int _DefaultWheelsIndex = 0;
    private int _DefaultAccessory1Index = 0;
    private int _DefaultAccessory2Index = 0;


    private void Start()
    {
        instance = this;

        _GaragePos = transform.position;
        _RecapPos = transform.Find("SecondPosition").position;

        ////////////ACTUAL STATS///////////
        _StatsBars[(int)ImageBars.Speed] = GetImagesComponent("NewSpeedBar");
        _StatsBars[(int)ImageBars.Acceleration] = GetImagesComponent("NewAccelerationBar");
        _StatsBars[(int)ImageBars.FuelConsumption] = GetImagesComponent("NewFuelConsumptionBar");
        _StatsBars[(int)ImageBars.AdherenceOne] = GetImagesComponent("NewAdhrence1Bar");
        _StatsBars[(int)ImageBars.AdherenceTwo] = GetImagesComponent("NewAdherence2Bar");
        _StatsBars[(int)ImageBars.AdherenceThree] = GetImagesComponent("NewAdherence3Bar");
        _StatsBars[(int)ImageBars.Weight] = GetImagesComponent("NewWeightBar");

        ////////////NEW STATS//////////////////
        _StatsBars[(int)ImageBars.SpeedNew] = GetFatherImage("NewSpeedBar");
        _StatsBars[(int)ImageBars.AccelerationNew] = GetFatherImage("NewAccelerationBar");
        _StatsBars[(int)ImageBars.FuelConsumptionNew] = GetFatherImage("NewFuelConsumptionBar");
        _StatsBars[(int)ImageBars.AdherenceOneNew] = GetFatherImage("NewAdhrence1Bar");
        _StatsBars[(int)ImageBars.AdherenceTwoNew] = GetFatherImage("NewAdherence2Bar");
        _StatsBars[(int)ImageBars.AdherenceThreeNew] = GetFatherImage("NewAdherence3Bar");
        _StatsBars[(int)ImageBars.WeightNew] = GetFatherImage("NewWeightBar");

        _DefaultColor = _StatsBars[(int)ImageBars.Speed].color;

        ///Show default stats
        ShowDefaultStats();
    }

    public void RefreshBars()
    {
        for (int i = 1; i < _StatsBars.Length;)
        {
            _StatsBars[i].fillAmount = _StatsBars[i - 1].fillAmount;
            i += 2;
        }
        ShowDefaultStats();

        _DefaultLoomIndex = Garage.instance.loomIndex;
        _DefaultEngineIndex = Garage.instance.engineIndex;
        _DefaultWheelsIndex = Garage.instance.wheelIndex;
        _DefaultAccessory1Index = Garage.instance.slot1Index;
        _DefaultAccessory2Index = Garage.instance.slot2Index;
    }

    public void ShowNewStats()
    {
        var _Loom = looms[Garage.instance.loomIndex];
        var _DefaultLoom = looms[_DefaultLoomIndex];

        var _Engine = engines[Garage.instance.engineIndex];
        var _DefaultEngine = engines[_DefaultEngineIndex];

        var _Wheels = wheels[Garage.instance.wheelIndex];
        var _DefaultWheels = wheels[_DefaultWheelsIndex];

        var _Accessory1 = Accessory1[Garage.instance.slot1Index];
        var _DefaultAccessory1 = Accessory1[_DefaultAccessory1Index];

        var _Accessory2 = Accessory2[Garage.instance.slot2Index];
        var _DefaultAccessory2 = Accessory2[_DefaultAccessory2Index];

        _StatsBars[(int)ImageBars.Speed].fillAmount = _DefaultLoom.speed + _DefaultEngine.speed + _DefaultWheels.speed + _DefaultAccessory1.speed + _DefaultAccessory2.speed;
        _StatsBars[(int)ImageBars.Acceleration].fillAmount = _DefaultLoom.acceleration + _DefaultEngine.acceleration + _DefaultWheels.acceleration + _DefaultAccessory1.acceleration + _DefaultAccessory2.acceleration;
        _StatsBars[(int)ImageBars.FuelConsumption].fillAmount = _DefaultLoom.fuelConsumption + _DefaultEngine.fuelConsumption + _DefaultWheels.fuelConsumption + _DefaultAccessory1.fuelConsumption + _DefaultAccessory2.fuelConsumption;
        _StatsBars[(int)ImageBars.AdherenceOne].fillAmount = _DefaultLoom.adherence1 + _DefaultEngine.adherence1 + _DefaultWheels.adherence1 + _DefaultAccessory1.adherence1 + _DefaultAccessory2.adherence1;
        _StatsBars[(int)ImageBars.AdherenceTwo].fillAmount = _DefaultLoom.adherence2 + _DefaultEngine.adherence2 + _DefaultWheels.adherence2 + _DefaultAccessory1.adherence2 + _DefaultAccessory2.adherence2;
        _StatsBars[(int)ImageBars.AdherenceThree].fillAmount = _DefaultLoom.adherence3 + _DefaultEngine.adherence3 + _DefaultWheels.adherence3 + _DefaultAccessory1.adherence1 + _DefaultAccessory2.adherence3;
        _StatsBars[(int)ImageBars.Weight].fillAmount = _DefaultLoom.weight + _DefaultEngine.weight + _DefaultWheels.weight + _DefaultAccessory1.weight + _DefaultAccessory2.weight;

        _StatsBars[(int)ImageBars.SpeedNew].fillAmount = _Loom.speed + _Engine.speed + _Wheels.speed + _Accessory1.speed + _Accessory2.speed;
        _StatsBars[(int)ImageBars.AccelerationNew].fillAmount = _Loom.acceleration + _Engine.acceleration + _Wheels.acceleration + _Accessory1.acceleration + _Accessory2.acceleration;
        _StatsBars[(int)ImageBars.FuelConsumptionNew].fillAmount = _Loom.fuelConsumption + _Engine.fuelConsumption + _Wheels.fuelConsumption + _Accessory1.fuelConsumption + _Accessory2.fuelConsumption;
        _StatsBars[(int)ImageBars.AdherenceOneNew].fillAmount = _Loom.adherence1 + _Engine.adherence1 + _Wheels.adherence1 + _Accessory1.adherence1 + _Accessory2.adherence1;
        _StatsBars[(int)ImageBars.AdherenceTwoNew].fillAmount = _Loom.adherence2 + _Engine.adherence2 + _Wheels.adherence2 + _Accessory1.adherence2 + _Accessory2.adherence2;
        _StatsBars[(int)ImageBars.AdherenceThreeNew].fillAmount = _Loom.adherence3 + _Engine.adherence3 + _Wheels.adherence3 + _Accessory1.adherence3 + _Accessory2.adherence3;
        _StatsBars[(int)ImageBars.WeightNew].fillAmount = _Loom.weight + _Engine.weight + _Wheels.weight + _Accessory1.weight + _Accessory2.weight;

        ///check difference between default value and new value
        BarChecks(_StatsBars[(int)ImageBars.SpeedNew], _StatsBars[(int)ImageBars.Speed]);
        BarChecks(_StatsBars[(int)ImageBars.AccelerationNew], _StatsBars[(int)ImageBars.Acceleration]);
        BarChecks(_StatsBars[(int)ImageBars.FuelConsumptionNew], _StatsBars[(int)ImageBars.FuelConsumption]);
        BarChecks(_StatsBars[(int)ImageBars.AdherenceOneNew], _StatsBars[(int)ImageBars.AdherenceOne]);
        BarChecks(_StatsBars[(int)ImageBars.AdherenceTwoNew], _StatsBars[(int)ImageBars.AdherenceTwo]);
        BarChecks(_StatsBars[(int)ImageBars.AdherenceThreeNew], _StatsBars[(int)ImageBars.AdherenceThree]);
        BarChecks(_StatsBars[(int)ImageBars.WeightNew], _StatsBars[(int)ImageBars.Weight]);


    }

    public void ChangeParent(string name)
    {
        if(name == "Garage")
        {
            this.transform.parent = MenuInizialeContainer.instance.interfaces[1].transform;
            transform.position = _GaragePos;
        }
        else if(name == "Recap&Go")
        {
            this.transform.parent = MenuInizialeContainer.instance.interfaces[2].transform;
            transform.position = _RecapPos;
        }
    }

    private void ShowDefaultStats()
    {
        
        var _Loom = looms[Garage.instance.loomIndex];
        var _Engine = engines[Garage.instance.engineIndex];
        var _Wheels = wheels[Garage.instance.wheelIndex];
        var _Accessory1 = Accessory1[Garage.instance.slot1Index];
        var _Accessory2 = Accessory2[Garage.instance.slot2Index];

        _StatsBars[(int)ImageBars.Speed].fillAmount = _Loom.speed +_Engine.speed + _Wheels.speed + _Accessory1.speed + _Accessory2.speed;
        _StatsBars[(int)ImageBars.Acceleration].fillAmount = _Loom.acceleration +_Engine.acceleration + _Wheels.acceleration + _Accessory1.acceleration + _Accessory2.acceleration; 
        _StatsBars[(int)ImageBars.FuelConsumption].fillAmount = _Loom.fuelConsumption +_Engine.fuelConsumption + _Wheels.fuelConsumption + _Accessory1.fuelConsumption + _Accessory2.fuelConsumption;
        _StatsBars[(int)ImageBars.AdherenceOne].fillAmount = _Loom.adherence1 +_Engine.adherence1 + _Wheels.adherence1 + _Accessory1.adherence1 + _Accessory2.adherence1;
        _StatsBars[(int)ImageBars.AdherenceTwo].fillAmount = _Loom.adherence2 +_Engine.adherence2 + _Wheels.adherence2 + _Accessory1.adherence2 + _Accessory2.adherence2;
        _StatsBars[(int)ImageBars.AdherenceThree].fillAmount = _Loom.adherence3 +_Engine.adherence3 + _Wheels.adherence3 + _Accessory1.adherence1 + _Accessory2.adherence3;
        _StatsBars[(int)ImageBars.Weight].fillAmount = _Loom.weight +_Engine.weight + _Wheels.weight + _Accessory1.weight + _Accessory2.weight;

        _StatsBars[(int)ImageBars.SpeedNew].fillAmount = _StatsBars[(int)ImageBars.Speed].fillAmount;
        _StatsBars[(int)ImageBars.AccelerationNew].fillAmount = _StatsBars[(int)ImageBars.Acceleration].fillAmount;
        _StatsBars[(int)ImageBars.FuelConsumptionNew].fillAmount = _StatsBars[(int)ImageBars.FuelConsumption].fillAmount;
        _StatsBars[(int)ImageBars.AdherenceOneNew].fillAmount = _StatsBars[(int)ImageBars.AdherenceOne].fillAmount;
        _StatsBars[(int)ImageBars.AdherenceTwoNew].fillAmount = _StatsBars[(int)ImageBars.AdherenceTwo].fillAmount;
        _StatsBars[(int)ImageBars.AdherenceThreeNew].fillAmount = _StatsBars[(int)ImageBars.AdherenceThree].fillAmount;
        _StatsBars[(int)ImageBars.WeightNew].fillAmount = _StatsBars[(int)ImageBars.Weight].fillAmount;

    }

    private void BarChecks( Image newSlot, Image oldSlot)
    { 

        if (newSlot.fillAmount > oldSlot.fillAmount)
        {
            IncreaseBar( newSlot);
        }

        else if (newSlot.fillAmount < oldSlot.fillAmount)
        {
            ReduceBar( newSlot,  oldSlot);
        }

        else
        {
            newSlot.fillAmount = 0;
        }
    }

    private void ReduceBar( Image newBar,  Image oldBar)
    {
        var temp = newBar.fillAmount;
        newBar.fillAmount = oldBar.fillAmount;
        oldBar.fillAmount = temp;
        newBar.color = _Red;
       

    }

    private void IncreaseBar( Image newBar)
    {
        newBar.color = _Green;
    }

    private Image GetImagesComponent(string name)
    {
        return transform.Find(name).GetChild(0).GetComponent<Image>();
    }

    private Image GetFatherImage(string name)
    {
        return transform.Find(name).GetComponent<Image>();
    }

}
