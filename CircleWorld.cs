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
    public class CircleWorld : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    
            circleWolrd(46613, 66327);

        }

        public void circleWolrd(int startTime, int endTime) {
            var beatDuration = Beatmap.GetTimingPointAt((int)startTime).BeatDuration;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();
            camera.PositionX
                .Add(startTime, 20);
            camera.PositionY
                .Add(startTime, 20);
            camera.PositionZ.Add(startTime, 250);
            
            camera.NearClip.Add(startTime, 50); //ближе к камере начинает появляется
            camera.NearFade.Add(startTime, 100); //момент где уже полностью спрайт появился

            camera.FarFade.Add(startTime, 2000); //спрайт начинает исчезать в далеке
            camera.FarClip.Add(startTime, 3300); //спрайт полностью исчезнет в далеке

            var parent = scene.Root;

            // var font = LoadFont("sb/lyrics/circleLyrics", new FontDescription()
            // {
            //     FontPath = "Exo 2 Light",
            //     FontSize = 100,
            //     FontStyle = FontStyle.Regular,
            //     Color = Color4.White,
                
            // });

            for(int i = 0; i < 30; i ++)
            {
                List<Sprite3d> circle1 = circle3dCreate(startTime, endTime, new Vector3(-30+i, 30-i, -(i * 20)), new Vector3(0, 0, 800 - i * 20));
               
                for(int j = 0; j < circle1.Count; j ++)
                    parent.Add(circle1[j]);
            }
            /* var circle = GetLayer("Background").CreateSprite("sb/circle.png", OsbOrigin.Centre);
            
            double step = 0;
            for(int i = 0; i < 26; i ++)
            {
                circle.Move(startTime + i * 200, startTime + ((i + 1) * 200), 320 + 100 * Math.Cos(step), 240 + 100 * Math.Sin(step), 320 + 100 * Math.Cos(step + circleStep), 240 + 100 * Math.Sin(step + circleStep));
                circle.Scale(startTime, .1f);
                step += circleStep;
            } */
            scene.Generate(camera, GetLayer("Background"), startTime, endTime, beatDuration / 8);
        }


        public List<Sprite3d> circle3dCreate(int startTime, int endTime, Vector3 startPos, Vector3 endPos)
        {
            double circleStep = Math.PI / 12;
            int iter = 17;
            double timeStep = (endTime - startTime) / iter;
            double step = 0;
            List<Sprite3d> elements = new List<Sprite3d>();
            // var texture = font.GetTexture(line);
            for(double i = 0; i < Math.PI * 2; i += circleStep)
            {
                var circle = new Sprite3d()
                {
                    SpritePath = "sb/star.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed,
                };
                circle.ConfigureGenerators(g =>
                {
                    g.PositionDecimals = 2;
                    g.ScaleDecimals = 3;
                    g.ColorTolerance = 10;
                });
                //var circle = GetLayer("Background").CreateSprite("sb/circle.png", OsbOrigin.Centre);

                for(int j = 0; j < iter; j += 1)
                {
                    circle.PositionX
                        .Add(startTime + (j * timeStep), (float)(startPos.X + 100 * Math.Cos(step)));
                    circle.PositionX
                        .Add(startTime + ((j + 1) * timeStep), (float)(startPos.X + 100 * Math.Cos(step + circleStep / 2)));

                    circle.PositionY
                        .Add(startTime + (j * timeStep), (float)(startPos.Y + 100 * Math.Sin(step)));
                    circle.PositionY
                        .Add(startTime + ((j + 1) * timeStep), (float)(startPos.Y + 100 * Math.Sin(step + circleStep / 2)));
                    
                    step += circleStep / 2;

                }
                circle.PositionZ
                        .Add(startTime, startPos.Z)
                        .Add(endTime, endPos.Z);

                circle.ScaleX.Add(startTime, .1f);
                circle.ScaleY.Add(startTime, .1f);

                elements.Add(circle);
            }
            
            return elements;
        }
    }
}
