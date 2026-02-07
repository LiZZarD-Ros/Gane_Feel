using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    private static Action OnDiscoBallHitEvent;

    [SerializeField] private float _discoBallPartyTime = 2f;
    [SerializeField] private float _discoGloabalLightIntensity = .2f;
    [SerializeField] private Light2D _globalLight;


    private Coroutine _discoCoroutine;
    private ColorSpotlight[] _allSpotLights;
    private float _defaultGloabalLightIntensity;

    private void Awake()
    {
        _defaultGloabalLightIntensity = _globalLight.intensity;
    }


    private void Start()
    { 
        _allSpotLights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
 
    }

    private void OnEnable()
    {
        OnDiscoBallHitEvent += DimTheLights;
    }

    private void OnDisable()
    {
        OnDiscoBallHitEvent -= DimTheLights;
    }

    public void DiscoParty()
    {
        if (_discoCoroutine != null) {return;}


        OnDiscoBallHitEvent?.Invoke();
    }


    private void DimTheLights()
    {
        foreach (ColorSpotlight spotLight in _allSpotLights)
        {
            StartCoroutine(spotLight.SpotLightDiscoParty(_discoBallPartyTime));
        }

        _discoCoroutine = StartCoroutine(GloabalLightResetRoutine());
    }

private IEnumerator GloabalLightResetRoutine()
    {
        _globalLight.intensity = _discoGloabalLightIntensity;
        yield return new WaitForSeconds(_discoBallPartyTime);
        _globalLight.intensity = _defaultGloabalLightIntensity;
        _discoCoroutine = null; 
    }

}
