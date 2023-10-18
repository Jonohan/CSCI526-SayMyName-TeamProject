using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BackgroundColorChange : MonoBehaviour
{
    [Header("Camera")]
    private Camera mainCam;

    [Header("Color Properties")]
    [Range(0,1)] public float saturationReduction = 0.25f;
    [Range(0,1)] public float valueReduction = 0.3f;
    [SerializeField] private Color currentObjCol;
    [SerializeField] private Color backgroundCol;
    
   
    void Start()
    {
        mainCam = Camera.main;
        EventCenter.GetInstance().AddEventListener("PossessionSequence", ChangeBGColor);
        EventCenter.GetInstance().AddEventListener("ReturnToOgBody", ChangeBGColorBack);
        // Change the background color to deepened player's color first 
        ChangeBGColorBack(null);
    }

    /// <summary>
    /// Get material main color from enemy and apply a deeper shade to camera background
    /// </summary>
    /// <param name="info">Possession pair passed to event center.</param>
    private void ChangeBGColor(object info)
    {
        // Get enemy possessed
        List<GameObject> objList = info as List<GameObject>;
        GameObject enemy = objList[1];
        // Get its color
        Renderer enemyRend = enemy.GetComponent<Renderer>();
        Material enemyMat = enemyRend.material;
        Color enemyColor = enemyMat.color;
        currentObjCol = enemyColor;
        // Convert to a deeper shade
        backgroundCol = ToDeeperShade(enemyColor);
        // Set camera background color
        mainCam.backgroundColor = backgroundCol;
    }
    
    /// <summary>
    /// Change the background color of the camera back to the main character's color. 
    /// </summary>
    /// <param name="info">PlayerControllerPossessed component passed to event center. Not relevant and not used in this function.</param>
    private void ChangeBGColorBack(object info)
    {
        // Get original player
        PossessionManager pm = GameObject.Find("MonoPossessionManager").GetComponent<PossessionManager>();
        GameObject originalPlayer = pm.originalPlayer;
        // Get its color
        Renderer ogPlayerRend = originalPlayer.GetComponent<Renderer>();
        Material ogPlayerMat = ogPlayerRend.material;
        Color ogPlayerColor = ogPlayerMat.color;
        // Convert to a deeper shade
        currentObjCol = ogPlayerColor;
        backgroundCol = ToDeeperShade(ogPlayerColor);
        // Set camera background color
        mainCam.backgroundColor = backgroundCol;
    }
    
    
    /// <summary>
    /// Convert input color to a deeper one with lower saturation and lower value. Set the reduction value in the inspector.
    /// </summary>
    /// <param name="col">Input color. </param>
    /// <returns>The deeper color.</returns>
    private Color ToDeeperShade(Color col)
    {
        // Convert to HSV for Saturation & Value manipulation 
        Color.RGBToHSV(col, out float H, out float S, out float V);
        S = Mathf.Clamp01(S - saturationReduction);
        V = Mathf.Clamp01(S - valueReduction);
        // Convert back to RGB space
        Color backgroundCol = Color.HSVToRGB(H, S, V);
        return backgroundCol;
    }
    
    /// <summary>
    /// Getter function for CharacterHealth.cs to know which color to change the health bar into.
    /// </summary>
    /// <returns>The color of the currently possessed enemy (the color that the health bar needs to change to.)</returns>
    public Color GetCurrentObjColor()
    {
        return currentObjCol;
    }
}
