using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class impostorScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;
    public KMHighlightable ModHL;
    public KMSelectable SelectableComp;

    public GameObject[] Prefabs;
    public GameObject BG;
    public GameObject SL;
    private GameObject ChosenPrefab;
    private ImposterMod chosenScript;
        
    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private int chosenMod;

    void Awake () {
        moduleId = moduleIdCounter++;
    }

    // Use this for initialization
    private void Start () 
    {
        GetMod();
        GetScript();
        GetSelectables();
    }
    private void GetMod()
    {
        BG.SetActive(false);
        chosenMod = UnityEngine.Random.Range(0, Prefabs.Length);
        //chosenMod = 4;
        ChosenPrefab = Instantiate(Prefabs[chosenMod], Vector3.zero, Quaternion.identity, this.transform);
        ChosenPrefab.transform.localPosition = Vector3.zero;
        Debug.LogFormat("[The Impostor #{0}] I may look like {1}, but do not be fooled...", moduleId, ChosenPrefab.name);

    }
    private void GetScript()
    {
        chosenScript = ChosenPrefab.GetComponent<ImposterMod>();
        chosenScript.moduleId = this.moduleId;
        chosenScript.Audio = Audio;
        chosenScript.Module = Module;
        chosenScript.solve += delegate () { Solve(); };
        SL.transform.localPosition = SLP.StatusPositions[chosenScript.SLPos];
    }
    private void GetSelectables()
    {
        KMSelectable[] btns = chosenScript.buttons;
        SelectableComp.Children = new KMSelectable[btns.Length];
        for (int i = 0; i < btns.Length; i++)
            SelectableComp.Children[i] = btns[i];
        SelectableComp.UpdateChildren();
    }
    private void Solve()
    {        
        Module.HandlePass();
        Audio.PlaySoundAtTransform("willSolve", transform);
        ChosenPrefab.SetActive(false);
        SL.transform.localPosition = SLP.StatusPositions[SLPositions.TR];
        BG.SetActive(true);
        for (int i = 0; i < chosenScript.buttons.Length; i++)
            chosenScript.buttons[i] = null;
    }
    
}
