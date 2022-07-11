using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    Collider2D collider;

     bool isFriend { get; set; }
     int damage { get; set; }
     Vector2 spdDirection { get; set; }
     float speed { get; set; }
     int health { get; set; }

    public void Init(Sprite sprite, int direction, int damage, int speedLvl, float spdModifier, int pierce, bool isFriend)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        //sprite size for fast speed bullets is too big
        if (speedLvl > 1)
        {
            transform.localScale = new Vector3(1, 1, 1) * .38f;
        }

        //1 is up, 2 is right, 3 is down, 4 is left
        switch (direction)
        {
            case 1:
                transform.Rotate(0, 0, 90);
                spdDirection = new Vector2(0, 1);
                break;
            case int i when i==2 || i==0:
                spdDirection = new Vector2(1, 0);
                break;
            case 3: 
                transform.Rotate(0, 0, -90);
                spdDirection = new Vector2(0, -1);
                break;
            case 4:
                transform.Rotate(0, 0, 180);
                spdDirection = new Vector2(-1, 0);
                break;

        }

      
        this.damage = damage;
        health = pierce;

        speed = 2*speedLvl*spdModifier;      

    }
    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(transform.position.x + spdDirection.x,
            transform.position.y + spdDirection.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
    }
}
