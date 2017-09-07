using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;


namespace Bloodimir_Thresh
{
    class Program
    {
        public const string Hero = "Thresh";
        public static Spell.Skillshot Q, E;
        public static Spell.Active R;
        public static Item Talisman, Zhonia, Randuin;
        public static int[] AbilitySequence;
        public static int QOff = 0, WOff = 0, EOff = 0, ROff = 0;
        
        public static Menu ThreshMenu,
           ComboMenu,
           DrawMenu,
           SkinMenu,
           MiscMenu,
           QMenu,
           AutoCastMenu;

        public static AIHeroClient Me = ObjectManager.Player;
        public static HitChance QHitChance;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoaded;
        }
        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

         private static void OnLoaded(EventArgs args)
        {
            if (Player.Instance.ChampionName != Hero)
                return;
            Bootstrap.Init(null);
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 1200, 80);
            E = new Spell.Skillshot(SpellSlot.E, 750, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R, 620);
            
            AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };

            ThreshMenu = MainMenu.AddMenu("Bloodimir Thresh", "bthresh");
            ThreshMenu.AddGroupLabel("Bloodimir Morgana");
            ThreshMenu.AddSeparator();
            ThreshMenu.AddLabel("Bloodimir Morgana v2.1.0.0");

            ComboMenu = ThreshMenu.AddSubMenu("Combo", "sbtw");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddSeparator();
            ComboMenu.Add("usecomboq", new CheckBox("Use Q"));

            AutoCastMenu = ThreshMenu.AddSubMenu("Auto Cast", "ac");
            AutoCastMenu.AddGroupLabel("Auto Cast");
            AutoCastMenu.AddSeparator();
            AutoCastMenu.Add("qd", new CheckBox("Auto Q Dashing"));
            AutoCastMenu.Add("qi", new CheckBox("Auto Q Immobile"));
            AutoCastMenu.Add("ar", new CheckBox("Auto R"));
            AutoCastMenu.Add("rslider", new Slider("Minimum people for Auto R", 2, 0, 5));

            QMenu = ThreshMenu.AddSubMenu("Q Settings", "qsettings");
            QMenu.AddGroupLabel("Q Settings");
            QMenu.AddSeparator();
            QMenu.Add("qmin", new Slider("Min Range", 150, 0, (int)Q.Range));
            QMenu.Add("qmax", new Slider("Max Range", (int)Q.Range, 0, (int)Q.Range));
            QMenu.AddSeparator();
            foreach (var obj in ObjectManager.Get<AIHeroClient>().Where(obj => obj.Team != Me.Team))		
            {		
              QMenu.Add("hook" + obj.ChampionName.ToLower(), new CheckBox("Hook " + obj.ChampionName));		
           }
            QMenu.AddSeparator();
            QMenu.Add("mediumpred", new CheckBox("MEDIUM Bind Hitchance Prediction", false));
            QMenu.AddSeparator();
            QMenu.Add("intq", new CheckBox("Q to Interrupt"));

            SkinMenu = ThreshMenu.AddSubMenu("Skin Changer", "skin");
            SkinMenu.AddGroupLabel("Choose the desired skin");

            var skinchange = SkinMenu.Add("sID", new Slider("Skin", 3, 0, 5));
            var sid = new[] { "Default", "Exiled", "Sinful Succulence", "Blade Mistress", "Blackthorn", "dasd"};
            skinchange.DisplayName = sid[skinchange.CurrentValue];
            skinchange.OnValueChange +=
                delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = sid[changeArgs.NewValue];
                };

            MiscMenu = ThreshMenu.AddSubMenu("Misc", "misc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.AddSeparator();
            MiscMenu.Add("antigapcloser", new CheckBox("Anti Gapcloser"));
            MiscMenu.Add("lvlup", new CheckBox("Auto Level Up Spells", false));
            MiscMenu.AddSeparator();
            MiscMenu.Add("support", new CheckBox("Support Mode", false));

            DrawMenu = ThreshMenu.AddSubMenu("Drawings", "drawings");
            DrawMenu.AddGroupLabel("Drawings");
            DrawMenu.AddSeparator();
            DrawMenu.Add("drawq", new CheckBox("Draw Q"));
            DrawMenu.Add("drawe", new CheckBox("Draw E"));
            DrawMenu.Add("drawr", new CheckBox("Draw R"));
            DrawMenu.Add("drawaa", new CheckBox("Draw AA"));
            DrawMenu.Add("predictions", new CheckBox("Visualize Q Prediction"));

            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Drawing.OnDraw += delegate
            {
                if (!Me.IsDead)
                {
                    if (DrawMenu["drawr"].Cast<CheckBox>().CurrentValue && R.IsLearned)
                    {
                        Circle.Draw(SharpDX.Color.Red, R.Range, Player.Instance.Position);
                    }
                    if (DrawMenu["drawe"].Cast<CheckBox>().CurrentValue && E.IsLearned)
                    {
                        Circle.Draw(SharpDX.Color.Green, E.Range, Player.Instance.Position);
                    }
                    if (DrawMenu["drawaa"].Cast<CheckBox>().CurrentValue)
                    {
                        Circle.Draw(SharpDX.Color.Blue, Q.Range, Player.Instance.Position);
                    }
                    var predictedPositions = new Dictionary<int, Tuple<int, PredictionResult>>();
                    var predictions = DrawMenu["predictions"].Cast<CheckBox>().CurrentValue;
                    var qRange = DrawMenu["drawq"].Cast<CheckBox>().CurrentValue;

                    foreach (
                        var enemy in
                            EntityManager.Heroes.Enemies.Where(
                                enemy => QMenu["hook" + enemy.ChampionName].Cast<CheckBox>().CurrentValue &&
                                         enemy.IsValidTarget(Q.Range + 150) &&
                                         !enemy.HasBuffOfType(BuffType.SpellShield)))
                    {
                        var predictionsq = Q.GetPrediction(enemy);
                        predictedPositions[enemy.NetworkId] = new Tuple<int, PredictionResult>(Environment.TickCount,
                            predictionsq);
                        if (qRange && Q.IsLearned)
                        {
                            Circle.Draw(Q.IsReady() ? SharpDX.Color.Blue : SharpDX.Color.Red, Q.Range,
                                Player.Instance.Position);
                        }

                        if (!predictions)
                        {
                            return;
                        }

                        foreach (var prediction in predictedPositions.ToArray())
                        {
                            if (Environment.TickCount - prediction.Value.Item1 > 2000)
                            {
                                predictedPositions.Remove(prediction.Key);
                                continue;
                            }

                            Circle.Draw(SharpDX.Color.Red, 75, prediction.Value.Item2.CastPosition);
                            Line.DrawLine(Color.GreenYellow, Player.Instance.Position,
                                prediction.Value.Item2.CastPosition);
                            Line.DrawLine(Color.CornflowerBlue,
                                EntityManager.Heroes.Enemies.Find(o => o.NetworkId == prediction.Key).Position,
                                prediction.Value.Item2.CastPosition);
                            Drawing.DrawText(prediction.Value.Item2.CastPosition.WorldToScreen() + new Vector2(0, -20),
                                System.Drawing.Color.LimeGreen,
                                string.Format("Hitchance: {0}%", Math.Ceiling(prediction.Value.Item2.HitChancePercent)),
                                10);
                        }
                    }
                    ;
                }
                ;
            };
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs args)
        {
            if (Q.IsReady() && sender.IsValidTarget(Q.Range) && MiscMenu["intq"].Cast<CheckBox>().CurrentValue)
                Q.Cast(sender);
        }

        

        private static void LevelUpSpells()
        {
            var qL = Me.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
            var wL = Me.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
            var eL = Me.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
            var rL = Me.Spellbook.GetSpell(SpellSlot.R).Level + ROff;
            if (qL + wL + eL + rL < ObjectManager.Player.Level)
            {
                int[] level = { 0, 0, 0, 0 };
                for (var i = 0; i < ObjectManager.Player.Level; i++)
                {
                    level[AbilitySequence[i] - 1] = level[AbilitySequence[i] - 1] + 1;
                }
                if (qL < level[0]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.Q);
                if (wL < level[1]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.W);
                if (eL < level[2]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.E);
                if (rL < level[3]) ObjectManager.Player.Spellbook.LevelSpell(SpellSlot.R);
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            QHitChance = QMenu["mediumpred"].Cast<CheckBox>().CurrentValue ? HitChance.Medium : HitChance.High;
            SkinChange();
            if (MiscMenu["lvlup"].Cast<CheckBox>().CurrentValue) LevelUpSpells();
            AutoCast(immobile: AutoCastMenu["qi"].Cast<CheckBox>().CurrentValue,
                dashing: AutoCastMenu["qd"].Cast<CheckBox>().CurrentValue);
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    Combo(ComboMenu["usecomboq"].Cast<CheckBox>().CurrentValue);
            }

            }
        

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) ||
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)))
            {
                var t = target as Obj_AI_Minion;
                if (t != null)
                {
                    {
                        if (MiscMenu["support"].Cast<CheckBox>().CurrentValue)
                            args.Process = false;
                    }
                }
            }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs a)
        {
            var agapcloser = MiscMenu["antigapcloser"].Cast<CheckBox>().CurrentValue;
            var antigapc = E.IsReady() && agapcloser;
            if (antigapc)
            {
                if (sender.IsMe)
                {
                    var gap = a.Sender;
                    if (gap.IsValidTarget(350))
                    {
                        E.Cast(sender);
                    }
                }
            }
        }

        public static Obj_AI_Base GetEnemy(float range, GameObjectType t)
        {
            switch (t)
            {
                default:
                    return EntityManager.Heroes.Enemies.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
            }
        }

        private static void SkinChange()
        {
            var style = SkinMenu["sID"].DisplayName;
            switch (style)
            {
                case "Default":
                    Player.SetSkinId(0);
                    break;
                case "Exiled":
                    Player.SetSkinId(1);
                    break;
                case "Sinful Succulence":
                    Player.SetSkinId(2);
                    break;
                case "Blade Mistress":
                    Player.SetSkinId(3);
                    break;
                case "Blackthorn":
                    Player.SetSkinId(4);
                    break;
                case "dasd":
                    Player.SetSkinId(5);
                    break;
            }
        }

        private static bool Immobile(Obj_AI_Base unit)
        {
            return unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Stun) ||
                   unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Snare) ||
                   unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Suppression);
        }

        private static void AutoCast(bool dashing, bool immobile)
        {
            if (Q.IsReady())
            {
                foreach (var itarget in EntityManager.Heroes.Enemies.Where(h => h.IsValidTarget(Q.Range)))
                {
                    if (immobile && Immobile(itarget) && Q.GetPrediction(itarget).HitChance >= QHitChance)
                    {
                        Q.Cast(itarget);
                    }

                    if (dashing && itarget.Distance(Me.ServerPosition) <= 400f &&
                        Q.GetPrediction(itarget).HitChance >= HitChance.Dashing)
                    {
                        Q.Cast(itarget);
                    }
                }
            }
            if (R.IsReady())
            {
                if (AutoCastMenu["ar"].Cast<CheckBox>().CurrentValue &&
                    Me.CountEnemiesInRange(R.Range) >= AutoCastMenu["rslider"].Cast<Slider>().CurrentValue)
                {
                    R.Cast();

                }
            }
        }

        private static void Combo(bool useQ)
        {
            if (useQ && Q.IsReady())
            {
                var bindTarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (bindTarget.IsValidTarget(Q.Range))
                {
                    if (Q.GetPrediction(bindTarget).HitChance >= QHitChance)
                    {
                        if (bindTarget.Distance(Me.ServerPosition) > QMenu["qmin"].Cast<Slider>().CurrentValue && bindTarget.Distance(Me.ServerPosition) < QMenu["qmax"].Cast<Slider>().CurrentValue)
                        {
                            if (QMenu["hook" + bindTarget.ChampionName].Cast<CheckBox>().CurrentValue)
                            {
                                Q.Cast(bindTarget);
                            }
                        }
                    }
                }
            }
        }

            }
        }

