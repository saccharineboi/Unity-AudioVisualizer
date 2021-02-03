using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
        public const int numSamples = 512;
        public const int numBands = 8;

        public float smoothness;

        [SerializeField] FFTWindow window;

        AudioSource audioSource;
        float[] samples;
        float[] bands;
        float[] buffers;
        float[] bufferHighest;
        float[] audioBands;

        void Start()
        {
                audioSource = GetComponent<AudioSource>();

                samples = new float[numSamples];
                bands = new float[numBands];
                buffers = new float[numBands];
                bufferHighest = new float[numBands];
                audioBands = new float[numBands];
        }

        void Update()
        {
                GenerateBands();
                GenerateDeltas();
                GenerateAudioBands();
        }

        void GenerateBands()
        {
                audioSource.GetSpectrumData(samples, 0, window);
                for (int i = 0, count = 0; i < numBands; ++i)
                {
                        int sampleCount = (int)Mathf.Pow(2, i + 1);

                        float sum = 0f;
                        for (int j = 0; j < sampleCount; ++j, ++count)
                                sum += samples[count] * sampleCount;

                        float average = sum / count;
                        bands[i] = average;
                }
        }

        void GenerateDeltas()
        {
                for (int i = 0; i < numBands; ++i)
                        buffers[i] = Mathf.Lerp(buffers[i], bands[i], Time.deltaTime * smoothness);
        }

        void GenerateAudioBands()
        {
                for (int i = 0; i < numBands; ++i)
                {
                        if (buffers[i] > bufferHighest[i])
                                bufferHighest[i] = buffers[i];
                        audioBands[i] = buffers[i] / bufferHighest[i];
                        if (float.IsNaN(audioBands[i]) || float.IsInfinity(audioBands[i]))
                                audioBands[i] = 0.5f;
                }
        }

        public float[] GetSamples() => samples;
        
        public float[] GetBands() => bands;

        public float[] GetBuffers() => buffers;

        public float[] GetAudioBands() => audioBands;
}