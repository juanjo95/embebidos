using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO.Ports;
using System;
using System.Linq;



public class player : MonoBehaviour {

    bool estado = false;
    int potencia = 20;


    SerialPort sp = new SerialPort("COM3", 9600);

    public float fuerza = 0;
	// Use this for initialization
	void Start () {
        sp.Open();
        sp.ReadTimeout = 1;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (sp.IsOpen)
        {
            try
            {
                String valor = sp.ReadLine();
                print(valor);
                string[] valores = valor.Split(',');

                potencia = Convert.ToInt32(valores[1]);

                if (Convert.ToInt32(valores[0]) == 1 && !estado)
                {
                    Lanzar();
                    estado = true;
                }
            }
            catch
            {

            }
        }
        /*
        if (Input.GetKey("up") && estado == false)
        {            
            Lanzar();
            estado = true;
            
        }        
        */
        if (Input.GetKey("right"))
        {
            Derecha();
        }

        if (Input.GetKey("left"))
        {
            Izquierda();
        }
        
    }

    void Lanzar() {
        
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, fuerza * Time.deltaTime));
        gameObject.GetComponent<Rigidbody>().velocity=transform.forward*potencia;
        //  transform.Translate(new Vector3(0, 0, 1) * 10 * T
    }

    void Derecha()
    {
        gameObject.GetComponent<Rigidbody>().transform.Translate(10f * Time.deltaTime, 0, 0);
            //AddForce(new Vector3(fuerza * Time.deltaTime, 0, 0));
    }

    void Izquierda()
    {
        gameObject.GetComponent<Rigidbody>().transform.Translate(-10f * Time.deltaTime, 0, 0);        
    }


}
