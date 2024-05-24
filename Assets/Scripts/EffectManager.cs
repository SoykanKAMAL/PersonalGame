using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public ParticleSystem matchEffect;
    private Queue<ParticleSystem> _matchEffectPool = new Queue<ParticleSystem>();

    private void OnEnable()
    {
        Card.onCardMatched += PlayMatchEffect;
    }

    private void OnDisable()
    {
        Card.onCardMatched -= PlayMatchEffect;
    }

    public void PlayMatchEffect(Card card)
    {
        ParticleSystem effect = GetMatchEffect();
        var pos = Camera.main.ScreenToWorldPoint(card.transform.position);
        pos.z = 0;
        effect.transform.position = pos;
        effect.Play();
    }

    public ParticleSystem GetMatchEffect()
    {
        if (_matchEffectPool.Count == 0)
        {
            ParticleSystem effect = Instantiate(matchEffect, transform);
            effect.gameObject.SetActive(false);
            _matchEffectPool.Enqueue(effect);
        }
        ParticleSystem pooledEffect = _matchEffectPool.Dequeue();
        pooledEffect.gameObject.SetActive(true);
        StartCoroutine(ReturnMatchEffect(pooledEffect));
        return pooledEffect;
    }

    public IEnumerator ReturnMatchEffect(ParticleSystem effect)
    {
        yield return new WaitForSeconds(matchEffect.main.startLifetime.constantMax);
        effect.gameObject.SetActive(false);
        _matchEffectPool.Enqueue(effect);
    }
}