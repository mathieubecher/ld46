using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float value;
    public List<Fixable> fixables;
    

    public CinemachineVirtualCamera cam;
    public Player player;
    public Text score;
    public ParticleSystem ash;
    
    public float minCooldownBroke = 10;
    public float maxCooldownBroke = 50;
    private float _cooldownBroke;
    public float timerStun = 0.2f;
    private float _stun;
    
    private float _cooldownStun = 2;

    private bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        fixables = new List<Fixable>();
        foreach(Fixable fix in FindObjectsOfType<Fixable>())
        {
            fixables.Add(fix);
           
        }

        _cooldownBroke = maxCooldownBroke;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            start = true;
            Broke(fixables[Random.Range(0,fixables.Count)]);
        }
        _cooldownBroke -= Time.deltaTime;
        if (_cooldownBroke < 0)
        {
            maxCooldownBroke = Mathf.Max(minCooldownBroke, maxCooldownBroke - 5);
            Broke(fixables[Random.Range(0,fixables.Count)]);
            _cooldownBroke = Random.Range(minCooldownBroke,maxCooldownBroke);
        }
        if (_stun > 0)
        {
            _stun -= Time.deltaTime;
            if (_stun <= 0) _stun = 0;
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =
                _stun / timerStun * 2;
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                _stun / timerStun * 2;
        }

        value = 0;
        foreach (Fixable fix in fixables)
        {
            value += fix.time/fix.fixTime;
        }

        value /= fixables.Count;
        score.text = (int)Mathf.Floor(value*100) + "%";
        if (value < 0) SceneManager.LoadScene(0);

    }

    public void Stun()
    {
        _cooldownStun = 2;
        _stun = timerStun;
        player.Stun(_stun);
        ash.Play();
    }

    public void Broke(Fixable fix)
    {
        fix.Broke();
    }
}
