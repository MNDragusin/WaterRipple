using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour {

    
    public float magnitudeDivider;
    public float speedWaveSpread;
    public float dimPersantege = 0.98f;
    public float distanceReduction = 1.5f;

    public Vector2[] impactPos;
    public float[] distance;
    public float[] waveAmplitude;

    private Renderer rend;
    private Mesh mesh;
    private int waveNumber;
    private float distanceX, distanceZ;



    void Start () {

        mesh = GetComponent<MeshFilter>().mesh;

        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Custom/WaterRippleShader");

        waveNumber = 0;
    }

    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            waveAmplitude[i] = rend.material.GetFloat("_WaveAmplitude" + (i + 1));

            if (waveAmplitude[i] > 0)
            {
                distance[i] += speedWaveSpread / distanceReduction;
                rend.material.SetFloat("_Distance" + (i + 1), distance[i]);
                rend.material.SetFloat("_WaveAmplitude" + (i + 1), waveAmplitude[i] * dimPersantege);
            }
            if (waveAmplitude[i] < 0.01)
            {
                rend.material.SetFloat("_WaveAmplitude" + (i + 1), 0);
                distance[i] = 0;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
        waveNumber++;
        if (waveNumber == 9)
        {
            waveNumber = 1;
        }
        waveAmplitude[waveNumber - 1] = 0;
            distance[waveNumber - 1] = 0;

        distanceX = this.transform.position.x - collision.gameObject.transform.position.x;
        distanceZ = this.transform.position.z - collision.gameObject.transform.position.z;
            impactPos[waveNumber - 1].x = collision.transform.position.x;
            impactPos[waveNumber - 1].y = collision.transform.position.z;

            rend.material.SetFloat("_xImpact" + waveNumber, collision.transform.position.x);
            rend.material.SetFloat("_zImpact" + waveNumber, collision.transform.position.z);

            rend.material.SetFloat("_OffsetX" + waveNumber, distanceX / mesh.bounds.size.x * 2.5f);
            rend.material.SetFloat("_OffsetZ" + waveNumber, distanceZ / mesh.bounds.size.z * 2.5f);

            rend.material.SetFloat("_WaveAmplitude" + waveNumber, collision.rigidbody.velocity.magnitude * magnitudeDivider);
        }
    }
}
