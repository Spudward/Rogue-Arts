using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class christos_gun : MonoBehaviour
{
    public GameObject bullet_emitter;
    public GameObject bullet;
    public float bullet_speed;

    public ParticleSystem sprae;
    //ParticleSystem spraeAll;
    // Start is called before the first frame update
    void Start()
    {
        //spraeAll = Instantiate(sprae);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject tem_bullet_han;
            tem_bullet_han = Instantiate(bullet, bullet_emitter.transform.position, bullet_emitter.transform.rotation)as GameObject;
            tem_bullet_han.transform.Rotate(Vector3.left * 90);
            Rigidbody tem_Rigidbody;
            tem_Rigidbody = tem_bullet_han.GetComponent<Rigidbody>();
            tem_Rigidbody.velocity = transform.TransformDirection(new Vector3(0, 0, bullet_speed));
            Destroy(tem_bullet_han, 5f);
            sprae.Play();
        }
    }
}
