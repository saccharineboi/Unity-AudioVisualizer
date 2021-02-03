using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public int numCubes;
        public float radius;
        public float minScale;
        public float scaleMultiplier;

        [SerializeField] GameObject sampleCubePrefab;
        GameObject[] sampleCubes;
        AudioPeer audioPeer;

        void Start()
        {
                audioPeer = FindObjectOfType<AudioPeer>();
                sampleCubes = new GameObject[numCubes];

                float angle = 0f;
                for (int i = 0; i < numCubes; ++i)
                {
                        Vector3 position = new Vector3(Mathf.Cos(-angle * Mathf.Deg2Rad) * radius, transform.position.y, Mathf.Sin(-angle * Mathf.Deg2Rad) * radius);
                        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

                        GameObject cube = Instantiate(sampleCubePrefab, position, rotation);
                        cube.transform.parent = transform;
                        cube.name = $"Cube #{i}";

                        sampleCubes[i] = cube;
                        angle += 360f / numCubes;
                }
        }

        void Update()
        {
                float[] buffers = audioPeer.GetAudioBands();
                for (int i = 0; i < numCubes; ++i)
                {
                        Vector3 scale = new Vector3(transform.localScale.x, minScale + scaleMultiplier * buffers[i], transform.localScale.z);
                        sampleCubes[i].transform.localScale = scale;
                }
        }
}
