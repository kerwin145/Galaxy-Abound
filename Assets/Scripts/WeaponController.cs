using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{

    [SerializeField] GameObject bullet;

    [SerializeField] int damageLvl = 1, speedLvl = 1, reloadLvl = 1, pierceLvl = 1;
    [SerializeField] Sprite[] bulletSpritesSpd1;
    [SerializeField] Sprite[] bulletSpritesSpd2;
    [SerializeField] Sprite[] bulletSpritesSpd3;
    [SerializeField] Sprite[] bulletSpritesSpd4;

    Sprite[][] bulletSprites;
    Sprite bulletSprite;


    [SerializeField] Sprite[] auraSprites = new Sprite[4];
    [SerializeField] GameObject bulletAura;


    int damage = 0;
    float reloadTime = 1f;
    int pierce;

    int playerDirection;
    float spdModifier = 1;
    float reloadCooldown;

    private void Awake()
    {
        damage += damageLvl;
        reloadTime = reloadTime - (reloadLvl) * .1f;
        pierce += pierceLvl;
        reloadCooldown = 0;

        bulletSprites = new Sprite[][] { bulletSpritesSpd1, bulletSpritesSpd2 , bulletSpritesSpd3, bulletSpritesSpd4};

    }
    public void shoot(bool boosted, int direction)
    {

        if (reloadCooldown <= 0)
        {
            bulletSprite = bulletSprites[speedLvl - 1][damageLvl - 1];
            Vector3 spawnPosition = transform.position;
            switch (direction)
            {
                case 1:
                    spawnPosition.y += bulletSprite.bounds.size.x / 6;
                    break;
                case int i when i == 2 || i == 0:
                    spawnPosition.x += bulletSprite.bounds.size.x / 4;
                    break;
                case 3:
                    spawnPosition.y -= bulletSprite.bounds.size.x / 6;
                    break;
                case 4:
                    spawnPosition.x -= bulletSprite.bounds.size.x / 4;
                    break;
            }

            if (boosted)
            {
                Debug.Log("Bullet Boosted");
                spdModifier = 1.5f;
            }

            else spdModifier = 1;

            GameObject BulletObj = Instantiate(bullet, spawnPosition, Quaternion.identity);
            Bullet BulletObjScript = BulletObj.GetComponent<Bullet>();
            BulletObjScript.Init(bulletSprite, direction, damage, speedLvl, spdModifier, pierce, true);

            if (boosted)
            {
                GameObject aura = Instantiate(bulletAura, spawnPosition, Quaternion.identity);
                aura.GetComponent<SpriteRenderer>().sprite = auraSprites[speedLvl - 1];
                aura.transform.localScale = BulletObj.transform.localScale * 1.5f;
                aura.transform.SetParent(BulletObj.transform);
                aura.transform.rotation = BulletObj.transform.rotation;
            }
            reloadCooldown = reloadTime;
        }

    }

    public void FixedUpdate()
    {
        if(reloadCooldown > 0)
            reloadCooldown -= Time.deltaTime;
    }

    public void setPlayerDirection(int playerDirection)
    {
        this.playerDirection = playerDirection;
    }


}
