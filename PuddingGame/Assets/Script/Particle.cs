using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation; // ������ �ִϸ��̼� ������Ʈ
    public ParticleSystem particleSystem; // ����Ƽ ��ƼŬ �ý���

    // ��ƼŬ�� ����� �ִϸ��̼� �̸� ����Ʈ
    public List<string> animationNames = new List<string> { "1_1_30", "1_1_40", "1_2_30", "1_2_40", "1_3_30", "1_3_40" };

    void Start()
    {
        if (skeletonAnimation != null)
        {
            // ������ �ִϸ��̼��� �̺�Ʈ ������ ���
            skeletonAnimation.AnimationState.Event += HandleAnimationEvent;
        }
    }

    private void HandleAnimationEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (skeletonAnimation == null) return; // null üũ
        // �ִϸ��̼� �̸��� ������ ����Ʈ�� ���ԵǾ� �ְ�, �̺�Ʈ �̸��� 'FireParticle'�� ��
        if (animationNames.Contains(trackEntry.Animation.Name) && e.Data.Name == "skeleton")
        {
            PlayParticle();
            Debug.Log("particle");
        }
    }

    private void PlayParticle()
    {
        // ��ƼŬ �ý��� ���
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

    private void OnDestroy()
    {
        if (skeletonAnimation != null)
        {
            // �̺�Ʈ ������ ����
            skeletonAnimation.AnimationState.Event -= HandleAnimationEvent;
        }
    }
}
