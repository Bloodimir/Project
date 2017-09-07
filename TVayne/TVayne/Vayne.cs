using EloBuddy;
using EloBuddy.Networking;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
namespace TVayne
{
    public class Vayne:General
    {
        public Vayne()
        {
            Q = new Spell.Active(SpellSlot.Q, 300);
            E = new Spell.Targeted(SpellSlot.E, 590);
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
        }

        void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            
            if (sender.IsMe && Orbwalker.IsAutoAttacking == args.SData.IsAutoAttack())
            {
                if (args.Target is AIHeroClient)
                {
                    var t = args.Target as AIHeroClient;
                    if (t.IsValid && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && Q.IsReady())
                    {
                        Orbwalker.ResetAutoAttack();
                        Q.Cast(Game.CursorPos);
                    }
                }
            }
        }


        void Game_OnUpdate(System.EventArgs args)
        {
            if (MyHero.IsDead)
            {
                return;
            }
            if (Target != null && Target.IsValid && (Target is AIHeroClient))
            {
                 var t = Target as AIHeroClient;
                 if (E.IsReady())
                 {
                     for (int i = 0; i < 8; i++)
                     {
                         var pos = Vector3.Normalize(t.ServerPosition - MyHero.Position) * i * 50;
                         if (((t.IsValidTarget(E.Range)) && pos.IsZero))
                         {
                             Orbwalker.ResetAutoAttack();
                             E.Cast(t);
                         }
                     }
                 }
                 if (Q.IsReady())
                 {
                     if (t.Health < MyHero.GetSpellDamage(t,SpellSlot.Q))
                     {
                         Orbwalker.ResetAutoAttack();
                         Orbwalker.ForcedTarget = t;
                     }
                 }
            }

        }

        void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (MyHero.IsDead)
            {
                return;
            }
            if (Target != null && Target.IsValid && (Target is AIHeroClient))
            {
                var t = Target as AIHeroClient;
                if (t.Health > MyHero.GetAutoAttackDamage(t)*2)
                {
                    args.Process = false;
                }
            }
        }
    }
}
