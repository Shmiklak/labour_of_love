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
    public class Dword2 : StoryboardObjectGenerator
    {
        public Vector2 shift = new Vector2(100, -350);
        int StartTime = 11470;
        int EndTime = 139470;
        public override void Generate()
        {
            var scene = new Scene3d();
            var camera = new PerspectiveCamera();
            camera.PositionX.Add(StartTime, 0);
                // .Add(12042, 0, EasingFunctions.ExpoOut);
            camera.PositionY.Add(StartTime, 0);
                // .Add(12042, 0, EasingFunctions.ExpoOut);
            camera.PositionZ.Add(StartTime, 180);
                // .Add(12042, 180, EasingFunctions.ExpoOut);
            
            
            camera.NearClip.Add(StartTime, 10); //ближе к камере начинает появляется
            camera.NearFade.Add(StartTime, 5); //момент где уже полностью спрайт появился

            camera.FarFade.Add(StartTime, 600); //спрайт начинает исчезать в далеке
            camera.FarClip.Add(StartTime, 650); //спрайт полностью исчезнет в далеке
            

            var parent = scene.Root;
            var beatDuration = Beatmap.GetTimingPointAt((int)StartTime).BeatDuration;

            
            string naziSprite = "";

            for(int i = 0; i < 100; i ++)
            {
                //рандомайзер спрайтов
                double RandomScale = Random(0.1, 0.3);
                int RandSprite = Random(0, 0);
                int Rotate1 = Random(-2,2);
                switch(RandSprite)
            {
                case 0: naziSprite = "sb/etc/_line.png";
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

                Vector2 randXY = new Vector2(Random(0, 0), Random(90, 90));
                nazi1.Coloring
                    .Add(StartTime, new Color4(	30, 245, 253, 255));
                nazi1.PositionX
                    .Add(StartTime, randXY.X);
                nazi1.PositionY
                    .Add(StartTime, randXY.Y);
                nazi1.PositionZ
                    .Add(StartTime, -5000 + (i * 60))
                    .Add(EndTime, 1 + (i * 60));
                nazi1.SpriteScale.Add(StartTime, 1f, 1f);
                nazi1.Coloring
                    .Add(45756, new Color4(	30, 245, 253, 255));
                nazi1.Coloring
                    .Add(48042, new Color4(	248, 94, 242, 255));
                
                // nazi1.SpriteRotation.Add(StartTime, 0);
                // nazi1.SpriteRotation.Add(48042, 0.1);
                // nazi1.SpriteRotation;
                parent.Add(nazi1);
            }

            scene.Generate(camera, GetLayer("Nazi"), StartTime, EndTime, beatDuration / 4);
        }
        
    }
}
