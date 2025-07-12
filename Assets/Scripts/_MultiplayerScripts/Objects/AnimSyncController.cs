using Fusion;
using UnityEngine;

public class AnimSyncController : NetworkBehaviour
{
    [Networked] public bool AnimStart { get; set; } = false;
    private bool _playedAnim = false;
    [SerializeField] private Animation _anim;

    public override void Render()
    {
        ApplyChangeAnimKey();
    }

    public void ApplyChangeAnimKey()
    {
        var anim = GetComponent<Animation>();

        if (AnimStart && !_playedAnim)
        {
            _anim.Play("NetGotOutKey");
            _playedAnim = true;
        }

        // Quando a animação terminar, reseta o estado
        if (_playedAnim && !_anim.IsPlaying("NetGotOutKey"))
        {
            if (HasStateAuthority)
            {
                SetStartAnimKey(false);
            }
            else
            {
                RPC_SetStartAnimKey(false);
            }

            _playedAnim = false;
        }
    }

    public void SetStartAnimKey(bool change)
    {
        AnimStart = change;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetStartAnimKey(bool change)
    {
        SetStartAnimKey(change);
    }
}
