using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbo : Accessory
{
    private bool turboMode = false;
    public bool turboEnabled = true;
    public float turboForce;
    public float turboConsumption;
    public float turboRechargeRate;
    private float turboQuantity = 100;

    public float TurboQuantity { get { return turboQuantity; } set { if (value >= 0 && value <= 100) turboQuantity = value; } } // da intendere come percentuale, 100 = 100%

    //private Rigidbody rb;
    public GameObject turboParticles;

    IEnumerator mCooldown;
    IEnumerator mTurboOn;
    AudioSource audioSource = null;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Turbo;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        turboForce = float.Parse(fileManager.LoadAccessoriesValue(5, (int)type + 1));
        turboForce *= vehicleController.accessoriesForceMoltiplier;
        turboParticles = transform.GetChild(1).gameObject;
    }
    

    void FixedUpdate()
    {
		if (!isCooldown) 
		{
			if (accessoryPressed && isActive) 
			{
				// FMODUnity.RuntimeManager.PlayOneShot(TurboOffPath);
				isActive = false;

                transform.GetChild(1).gameObject.SetActive(false);
                StopCoroutine(mTurboOn);
				StartCoroutine (mCooldown = Cooldown ());
			} 
			else if (accessoryPressed && !isActive) 
			{
                transform.GetChild(1).gameObject.SetActive(true);
				//Debug.Log("isActive a true");
				isActive = true;
				accessoryPressed = false;
				PlaySound ();
				// FMODUnity.RuntimeManager.PlayOneShot(TurboOnPath);
				StartCoroutine (mTurboOn = TurboOn ());
				StartCoroutine (HUD.instance.SlotBarDecrease (slot, duration));

			}
		} 

        if (isActive)
        {
            vehicleController.rb.AddRelativeForce(Vector3.forward * turboForce * Time.deltaTime, ForceMode.Force);
            TurboQuantity -= duration * Time.deltaTime;
        }
    }    

    IEnumerator TurboOn()
    {
        isActive = true;
        float timer = duration;
        PlaySound();
        while (timer > 0)
        {
			timer -= Time.deltaTime;
			//Debug.Log ("DURATION: " + timer + "  TIME: " + Time.deltaTime);
			yield return null;

        }
        isActive = false;
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(mCooldown = Cooldown());

    }

    void PlaySound()
    {
        int index = isActive ? 6 : 6;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}

