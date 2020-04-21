using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomizeMaterials : MonoBehaviour {

    public List<Material> listMat = new List<Material>();

    MeshRenderer meshRenderer;
    static bool canRandomize = false;

    /// <summary>
    /// Set the variable canRandomize to true or false
    /// </summary>
    /// <param name="state"></param>
    public void SetRandomizerOff() {
        canRandomize = false;
    }

    /// <summary>
    /// Start a coroutine to switch the material of the button
    /// </summary>
    public void StartRandomizer() {
        StartCoroutine(SwitchToRandomMaterial());
    }

    /// <summary>
    /// Randomly switch the material of the button over random short intervals
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchToRandomMaterial() {
        while (canRandomize) { 
            if (listMat.Count < 1) {
                yield break;
            }

            int indexMat = Random.Range(0, listMat.Count);
            float waitTime = Random.Range(0.5f, 1f);

            if (meshRenderer == null) {
                meshRenderer = GetComponentInChildren<MeshRenderer>();
            }

            meshRenderer.material = listMat.ElementAt(indexMat);
            yield return new WaitForSeconds(waitTime);
        }
    }

}
