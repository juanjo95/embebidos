using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO.Ports;
using System;
using System.Linq;

public class Cannon : MonoBehaviour {
    
	public GameObject cannonBullet;
	public Transform cannonTip;
	public Slider powerLevel;
	public AudioClip cannonShot;
	public int maxShot;
	public bool readyToShoot;
	public int shot;

	private int minLevel, maxLevel, timeDelay;
	private bool isMax, isCharging;
	private AudioSource audioSource;
	private float minRot, maxRot;
	private Vector3 currentRotation, touchPos;
	private int counter = 0;
	private Vector2 direction = Vector2.zero;
    
    SerialPort sp = new SerialPort("COM6", 9600);       

    void Awake(){
		audioSource = GetComponent<AudioSource> ();
	}

	void Start () {
        sp.Open();
        sp.ReadTimeout = 1;        

        readyToShoot = true;
		shot = maxShot;
		timeDelay = 50;
		minLevel = 0;
		maxLevel = 50;
		powerLevel.maxValue = maxLevel;
		powerLevel.value = minLevel;
		maxRot = 20;
		minRot = -25;
		Vector3 currentRotation = transform.rotation.eulerAngles;        
    }

// Update is called once per frame
void Update () {
		if (GameplayController.instance.gameInProgress) {
			if (readyToShoot) {

                if (sp.IsOpen)
                {
                    try
                    {
                        string valor = sp.ReadLine();
                        print(valor);
                        string[] valores = valor.Split(',');

                        if (shot != 0)
                        {
                            powerLevel.value = Convert.ToInt32(valores[1]);
                        }

                        if ((Convert.ToInt32(valores[0])) == 1)
                        {
                            LanzarBola();
                        }
                        DarAngulo(Convert.ToInt32(valores[2]));
                    }
                    catch
                    {

                    }
                }
                
			}

		}
	}	

    void LanzarBola()
    {
        if (shot != 0)
        {
            GameObject newCannonBullet = Instantiate(cannonBullet, cannonTip.position, Quaternion.identity) as GameObject;
            newCannonBullet.transform.GetComponent<Rigidbody2D>().AddForce(cannonTip.right * powerLevel.value, ForceMode2D.Impulse);
            if (GameController.instance != null && MusicController.instance != null)
            {
                if (GameController.instance.isMusicOn)
                {
                    audioSource.PlayOneShot(cannonShot);
                }
            }
            shot++;
            readyToShoot = false;
        }
    }

    void DispararCañon()
    {
      
        if (sp.ReadByte() == 1)
        {            
            if (shot != 0)
            {
                GameObject newCannonBullet = Instantiate(cannonBullet, cannonTip.position, Quaternion.identity) as GameObject;
                newCannonBullet.transform.GetComponent<Rigidbody2D>().AddForce(cannonTip.right * powerLevel.value, ForceMode2D.Impulse);
                if (GameController.instance != null && MusicController.instance != null)
                {
                    if (GameController.instance.isMusicOn)
                    {
                        audioSource.PlayOneShot(cannonShot);
                    }
                }
                shot++;
                powerLevel.value = 120;
                readyToShoot = false;
            }
        }

    }

    void DarAngulo(int rotacion)
    {

        currentRotation.z = rotacion-20;

        currentRotation.z = Mathf.Clamp(currentRotation.z, minRot, maxRot);

        transform.rotation = Quaternion.Euler(currentRotation);
    }

    void CannonMovement(){

		if (Input.GetKey (KeyCode.UpArrow)) {
			currentRotation.z += 50f * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			currentRotation.z -= 50f * Time.deltaTime;
		}
			
		currentRotation.z = Mathf.Clamp(currentRotation.z, minRot, maxRot);

		transform.rotation = Quaternion.Euler (currentRotation);
	}		
}
