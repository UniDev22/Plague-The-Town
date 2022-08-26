using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public Rigidbody rig;
    public float forceMin;
    public float forceMax;

    float lifetime = 2.5f;
    float fadetime = 2;

    void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        rig.AddForce(transform.right * force);
        rig.AddTorque(Random.insideUnitSphere * force); 

        StartCoroutine(Fade());
    }

    IEnumerator Fade(){
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1/fadetime;
        Material mat = GetComponent<Renderer>().material;
        Color initalColor = mat.color;

        while(percent < 1){
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initalColor, Color.clear, percent);
            yield return null;

        }
        Destroy(gameObject);
    }
}
