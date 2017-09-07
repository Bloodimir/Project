using System.Collections.Generic;

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
namespace TVayne
{
    public class General
    {
        public Spell.Active Q;
        public Spell.Targeted E;
        public Spell.Active R;
        public AIHeroClient MyHero { get { return ObjectManager.Player; } }
        public Obj_AI_Base Target
        {
            get { return TargetSelector.GetTarget(E.Range + Q.Range, DamageType.Physical); }
        }
        public AIHeroClient HeroTarget()
        {
            return Target as AIHeroClient;
        }
        public bool VayneStack(Obj_AI_Base target)
        {
            foreach (var item in target.Buffs)
            {
                if (item.Name == "vaynesilvereddebuff" && item.Count == 2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
