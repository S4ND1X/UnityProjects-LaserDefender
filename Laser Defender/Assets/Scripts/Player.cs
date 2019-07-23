﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config param
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 0.8f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 100f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;

    private Coroutine firingCoroutine;

    private float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        MoveX();
        Fire();

    }

    IEnumerator FireWhilePressed()
    {
            while (true) {
            //Aplicar rotacion que ya tiene si no tiene no habra: Quaternion
            GameObject laser = Instantiate(laserPrefab,
                                             transform.position,
                                             Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }


    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Se iguala la corrutina
            firingCoroutine =  StartCoroutine(FireWhilePressed());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            //Si se deja de presionar la corruttina se detiene
            StopCoroutine(firingCoroutine);
        }
    }

    private void MoveX()
    {
        //Se usa var porque automaticamente sabe que tipo de dato es
        //Se multiplica por Delta time para frames parejos y con la velocidad
        //Se le suma la posicion a la que se dirije + en la que esta
        var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, minY, maxY);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        //Se da el minimo que se puede mover en el eje de las x relativo a la camara
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX= gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        //Se usa para tener la coordenadas en Y en las que es el minimo y maximo
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }
}
