using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation; // 스파인 애니메이션 컴포넌트
    public ParticleSystem particleSystem; // 유니티 파티클 시스템

    // 파티클을 사용할 애니메이션 이름 리스트
    public List<string> animationNames = new List<string> { "1_1_30", "1_1_40", "1_2_30", "1_2_40", "1_3_30", "1_3_40" };

    void Start()
    {
        if (skeletonAnimation != null)
        {
            // 스파인 애니메이션의 이벤트 리스너 등록
            skeletonAnimation.AnimationState.Event += HandleAnimationEvent;
        }
    }

    private void HandleAnimationEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (skeletonAnimation == null) return; // null 체크
        // 애니메이션 이름이 지정한 리스트에 포함되어 있고, 이벤트 이름이 'FireParticle'일 때
        if (animationNames.Contains(trackEntry.Animation.Name) && e.Data.Name == "skeleton")
        {
            PlayParticle();
            Debug.Log("particle");
        }
    }

    private void PlayParticle()
    {
        // 파티클 시스템 재생
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }

    private void OnDestroy()
    {
        if (skeletonAnimation != null)
        {
            // 이벤트 리스너 제거
            skeletonAnimation.AnimationState.Event -= HandleAnimationEvent;
        }
    }
}
