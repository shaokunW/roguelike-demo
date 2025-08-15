using System.Collections.Generic;
using CatAndHuman.UI.select;
using UnityEngine;
using CatAndHuman.UI.select;

namespace CatAndHuman
{
    public class SelectCharacterApp : MonoBehaviour
    {
        public TopPanel topPanel;
        public PoolList bottomPanel;
        public SegmentSwitcher segmentSwitcher;
  
        private UIStateStore stateStore;

        void Render(UIState s)
        {
            RenderTopPanel(s);
            RenderBottomPanel(s);
        }

        void RenderTopPanel(UIState s)
        {
        }

        void RenderBottomPanel(UIState s)
        {
        }

        void RenderList(UIState s)
        {
            RenderList(s);
        }
    }

    public enum Node
    {
        Empty,
        Browsing,
        Selected,
        Locked
    }

    public sealed class FSM
    {
        public Node Node { get; private set; } = Node.Empty;

        public void Dispatch(Ev e, Payload p)
        {
            var s = UIStateStore.I.State;
            switch (Node)
            {
                case Node.Empty:
                    if (e == Ev.Load)
                    {
                        Node = Node.Browsing;
                        s.pool = Pool.Character;
                        Commit(s, "");
                    }

                    break;

                case Node.Browsing:
                    if (e == Ev.TogglePool)
                    {
                        s.pool = (s.pool == Pool.Character) ? Pool.Weapon : Pool.Character;
                        Commit(s, "segment_slide");
                    }
                    else if (e == Ev.Select && !p.locked)
                    {
                        if (p.isWeapon) s.selectedWeaponIds.Add(p.id);
                        else s.selectedCharacterId = p.id;
                        Node = Node.Selected;
                        Commit(s, "card_select|top_fade");
                    }
                    else if (e == Ev.Select && p.locked)
                    {
                        Node = Node.Locked;
                        // TransitionFacade.Play("locked_pulse", .1f); // 不改状态
                        Node = Node.Browsing;
                    }

                    break;

                case Node.Selected:
                    if (e == Ev.Select && !p.locked)
                    {
                        if (p.isWeapon) s.selectedWeaponIds.Add(p.id);
                        else s.selectedCharacterId = p.id;
                        Commit(s, "card_select|top_fade");
                    }
                    else if (e == Ev.TogglePool)
                    {
                        s.pool = (s.pool == Pool.Character) ? Pool.Weapon : Pool.Character;
                        Commit(s, "segment_slide");
                    }

                    break;
            }
        }

        void Commit(UIState s, string transitions)
        {
            UIStateStore.I.Mutate(_ => s);
            // TransitionFacade.PlayMany(transitions); // 可为空
            // 由订阅 RenderFrom 的各控件统一渲染
        }
    }

    public struct Payload
    {
        public int id;
        public bool isWeapon;
        public bool locked;
    }

    public enum Ev
    {
        Load,
        TogglePool,
        Select,
        Lock,
        Unlock
    }
}