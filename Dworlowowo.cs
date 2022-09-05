using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding.CommandValues;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Dworlowowo : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 Color = Color4.White;
        [Configurable]
        public float ColorVariance = 0.6f;

        public Vector2 shift = new Vector2(100, -350);
        int StartTime = 184042;
        int EndTime = 203470;
        public override void Generate()
        {
            var scene = new Scene3d();
            var camera = new PerspectiveCamera();
            camera.PositionX
                .Add(StartTime, 0, EasingFunctions.ExpoOut);
            camera.PositionY
                .Add(StartTime, 0, EasingFunctions.ExpoOut);
            camera.PositionZ
                .Add(StartTime, 500, EasingFunctions.ExpoOut);
            
            
            camera.NearClip.Add(StartTime, 10); //ближе к камере начинает появляется
            camera.NearFade.Add(StartTime, 5); //момент где уже полностью спрайт появился

            camera.FarFade.Add(StartTime, 1010); //спрайт начинает исчезать в далеке
            camera.FarClip.Add(StartTime, 1060); //спрайт полностью исчезнет в далеке
            

            var parent = scene.Root;
            var beatDuration = Beatmap.GetTimingPointAt((int)StartTime).BeatDuration;

            
            string naziSprite = "";

            for(int i = 0; i < 250; i ++)
            {
                //рандомайзер спрайтов
                double RandomScale = Random(0.1, 0.3);
                int RandSprite = Random(1, 2);
                // int Rotate1 = Random(-2,2);
                switch(RandSprite)
            {
                case 1: naziSprite = "sb/heart.png";
                break;
                case 2: naziSprite = "sb/heart.png";
                break;

            }
                var nazi1 = new Sprite3d()
                {
                    
                    SpritePath = naziSprite,
                    UseDistanceFade = true,
                    StartRTime = -1,
                    RotationMode = RotationMode.Fixed,
                };

                nazi1.ConfigureGenerators(g =>
                {
                    g.PositionDecimals = 2;
                    g.ScaleDecimals = 3;
                    g.ColorTolerance = 10;
                });

                Vector2 randXY = new Vector2(Random(-880, 880), Random(-440, 40));
                nazi1.Coloring
                    .Add(StartTime, new Color4(	255, 255, 255, 255));
                nazi1.PositionX
                    .Add(StartTime, randXY.X);
                nazi1.PositionY
                    .Add(StartTime, randXY.Y);
                nazi1.PositionZ
                    .Add(StartTime, 1 + (i * 60))
                    .Add(EndTime, -5000 + (i * 60)); 
                nazi1.SpriteScale.Add(StartTime, 0.1f, 0.1f);
                var color = Color;
                        if (ColorVariance > 0)
                        {
                            ColorVariance = MathHelper.Clamp(ColorVariance, 0, 1);

                            var hsba = Color4.ToHsl(color);
                            var sMin = Math.Max(0, hsba.Y - ColorVariance * 0.5f);
                            var sMax = Math.Min(sMin + ColorVariance, 1);
                            var vMin = Math.Max(0, hsba.Z - ColorVariance * 0.5f);
                            var vMax = Math.Min(vMin + ColorVariance, 1);

                            color = Color4.FromHsl(new Vector4(
                                hsba.X,
                                (float)Random(sMin, sMax),
                                (float)Random(vMin, vMax),
                                hsba.W));
                        }
                nazi1.Coloring.Add(StartTime, color);
                parent.Add(nazi1);
            }

            scene.Generate(camera, GetLayer("Nazi"), StartTime, EndTime, beatDuration / 4);
        }
        
    }
}
