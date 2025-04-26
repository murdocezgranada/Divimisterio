using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Fondo : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimineto;
    private Vector2 offset;
    private Material material;

    private Rigidbody2D detective_0;
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        detective_0 = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        offset = (detective_0.velocity.x * 0.1f) * velocidadMovimineto * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
