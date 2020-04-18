using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Fixable> fixables;
    

    public CinemachineVirtualCamera cam;
    public Player player;

    public float timerStun = 0.2f;
    private float _stun;

    private float _cooldownStun = 2;
    // Start is called before the first frame update
    void Start()
    {
        fixables = new List<Fixable>();
        foreach(Fixable fix in FindObjectsOfType<Fixable>())
        {
            fixables.Add(fix);
            fix.Broke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_stun > 0)
        {
            _stun -= Time.deltaTime;
            if (_stun <= 0) _stun = 0;
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =
                _stun / timerStun * 2;
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                _stun / timerStun * 2;
            

        }
    }

    public void Stun()
    {
        _cooldownStun = 2;
        _stun = timerStun;
        player.Stun(_stun);
    }
}
