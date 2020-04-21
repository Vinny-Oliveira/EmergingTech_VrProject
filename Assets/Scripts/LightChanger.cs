using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : SingletonManager<LightChanger> {

    // References
    public Light roomLight;
    public int range = 50;
    public int changer = -10;

    // Starting colors
    float r_start, g_start, b_start;

    // Color control
    float max_r, max_g, max_b;
    float min_r, min_g, min_b;
    int r_changer, g_changer, b_changer;

    // Dimmer control
    bool canDimmer;

    private void Start() {
        r_start = roomLight.color.r;
        g_start = roomLight.color.g;
        b_start = roomLight.color.b;
    }

    /// <summary>
    /// Start the continuous change of color of the lights
    /// </summary>
    public void StartDimmer() {
        AssignColorRange(ref min_r, ref max_r, roomLight.color.r, range, ref r_changer, changer);
        AssignColorRange(ref min_g, ref max_g, roomLight.color.g, range, ref g_changer, changer);
        AssignColorRange(ref min_b, ref max_b, roomLight.color.b, range, ref b_changer, changer);

        canDimmer = true;
        StartCoroutine(ChangeColor());
    }

    /// <summary>
    /// Stop the change of colors of the lights
    /// </summary>
    public void StopDimmer() {
        canDimmer = false;
        roomLight.color = new Color(r_start, g_start, b_start);
    }

    /// <summary>
    /// Assign minumum and maximum values of a color parameter (r, g, or b)
    /// </summary>
    /// <param name="minColor"></param>
    /// <param name="maxColor"></param>
    /// <param name="maxValue"></param>
    /// <param name="range"></param>
    void AssignColorRange(ref float minColor, ref float maxColor, float maxValue, int range, ref int colorChanger, int valueCHanger) {
        maxColor = maxValue * 255;
        minColor = (maxColor - range < 0) ? (0) : (maxColor - range);
        colorChanger = valueCHanger;
    }

    /// <summary>
    /// CHange the value of a color parameter (r, g, or b)
    /// </summary>
    /// <param name="colorParam"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="changer"></param>
    /// <returns></returns>
    float ChangeColorParameter(float colorParam, float minValue, float maxValue, ref int changer) {
        float newParam = colorParam * 255;
        if (newParam < minValue + 1) {
            changer = Mathf.Abs(changer);
        } else if (newParam > maxValue - 1) {
            changer = -Mathf.Abs(changer);
        }

        newParam += changer;
        return newParam / 255;
    }

    /// <summary>
    /// Gradually change the color of the room light within a range
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColor() {
        while (canDimmer) { 
            float r = ChangeColorParameter(roomLight.color.r, min_r, max_r, ref r_changer);
            float g = ChangeColorParameter(roomLight.color.g, min_g, max_g, ref g_changer);
            float b = ChangeColorParameter(roomLight.color.b, min_b, max_b, ref b_changer);

            roomLight.color = new Color(r, g, b);
            yield return new WaitForSeconds(0.1f);
        }
    }


    //public void PlayLossEvent() { 
        
    //}
}
