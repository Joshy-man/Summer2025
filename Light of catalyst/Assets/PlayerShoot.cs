using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        //Mouse Position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Position from us to the mouse
        Vector3 shootDirection = (mousePosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(shootDirection.x, shootDirection.y) * bulletSpeed;
        Destroy(bullet, 2f);
    }
}
