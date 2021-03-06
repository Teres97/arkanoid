﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
   private SpriteRenderer sr;
   public int HitPoints = 0;
   public ParticleSystem DestroyEffect;
   public static event Action<Brick> OnBreakDestruction;

    private void Awake() {
        this.sr = this.GetComponent<SpriteRenderer>(); 
    }
   private void OnCollisionEnter2D(Collision2D collision) {
       Ball ball = collision.gameObject.GetComponent<Ball>();
       ApplyCollisionLogic(ball);
   }

   private void ApplyCollisionLogic(Ball ball){
        this.HitPoints--;
        if(this.HitPoints<=0){
            BrickManager.Instance.RemainingBricks.Remove(this);
            OnBreakDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else{
            this.sr.sprite = BrickManager.Instance.Sprites[this.HitPoints - 1];
        }
    }

    private void SpawnDestroyEffect(){
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition =  new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.HitPoints = hitpoints; 
    }
}
