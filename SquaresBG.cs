using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class SquaresBG : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 Color = Color4.White;
        [Configurable]
        public Color4 Color2 = Color4.White;
        [Configurable]
        public float ColorVariance = 0.6f;
        [Configurable]
        public float travelSpeedVal = -2;
        public override void Generate()
        {

            float startTime = 66327;
            float endTime = 84621 - 1143;


            for (int i = 0; i < 45; i++) {
                CreateNewSquareColumn(66327, -110 + i * 20);
            }
            for(int i=1; startTime + i * 1562 < endTime; i++)
            {
                float squareStartTime = startTime + i * 1562;
                CreateNewSquareColumn(squareStartTime, -110 + 44 * 20);
            }


            

            // var beat = 48613 - 48042;

            

            // int travelSpeed = 14;

            // int squareEndTime = squareStartTime;
            // while (GetXCoordinateAt(squareEndTime) > OsuHitObject.WideScreenStoryboardBounds.Left || squareEndTime > 84613) //EndTime would be the endtime of the whole effect
            //     squareEndTime += 100;


            // for (int i = 0; i < 57; i++) {
            //     for (int j = 0; j<24; j++) {

            //         int x = -117 + 20 * i;
            //         int y = 0 + j * 20;
            //         var position = new Vector2(x,y);

            //         var number = GetLayer("SquaresBG").CreateSprite("sb/d.png", OsbOrigin.TopCentre, position);
            //         number.MoveX(66327, 84613, x, x-250);
            //         number.Fade(66327, 1);
            //         number.Fade(83470, 84613, 1, 0);
            //         // number.Scale(66327, 1.5);
            //         if(Random(1,10) > 5) {
            //             number.ScaleVec(66327, 66327 + Random(100, 500), 1.5, 0, 1.5, 1.5);
            //         } else {
            //             number.ScaleVec(66327, 66327 + Random(100, 500), 0, 1.5, 1.5, 1.5);
            //         }
                    
            //         var color = Color;
            //             if (ColorVariance > 0)
            //             {
            //                 ColorVariance = MathHelper.Clamp(ColorVariance, 0, 1);

            //                 var hsba = Color4.ToHsl(color);
            //                 var sMin = Math.Max(0, hsba.Y - ColorVariance * 0.5f);
            //                 var sMax = Math.Min(sMin + ColorVariance, 1);
            //                 var vMin = Math.Max(0, hsba.Z - ColorVariance * 0.5f);
            //                 var vMax = Math.Min(vMin + ColorVariance, 1);

            //                 color = Color4.FromHsl(new Vector4(
            //                     hsba.X,
            //                     (float)Random(sMin, sMax),
            //                     (float)Random(vMin, vMax),
            //                     hsba.W));
            //             }
            //         number.Color(66327, color);

            //         int[] timeStamps = {74899, 75042,  75185, 75327, 75470};

            //         foreach (var stamp in timeStamps)
            //             {
            //                 var color2 = Color2;
            //                 if (ColorVariance > 0)
            //                     {
            //                         var ColorVariance2 = MathHelper.Clamp(0.4f, 0, 1);

            //                         var hsba = Color4.ToHsl(color2);
            //                         var sMin = Math.Max(0, hsba.Y - ColorVariance2 * 0.5f);
            //                         var sMax = Math.Min(sMin + ColorVariance2, 1);
            //                         var vMin = Math.Max(0, hsba.Z - ColorVariance2 * 0.5f);
            //                         var vMax = Math.Min(vMin + ColorVariance2, 1);

            //                         color2 = Color4.FromHsl(new Vector4(
            //                             hsba.X,
            //                             (float)Random(sMin, sMax),
            //                             (float)Random(vMin, vMax),
            //                             hsba.W));
            //                     }
            //                 number.Color(stamp, color2);
            //             }
            //     }
            // }

            var lineBGFlash = GetLayer("flash").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(320, 240));
                lineBGFlash.Scale(75470, 100);
                lineBGFlash.Additive(75470, 76613);
                lineBGFlash.Fade(75470, 76613, 1, 0);

            var heart = GetLayer("squares_heart").CreateSprite("sb/heart.png", OsbOrigin.Centre, new Vector2(500, 240));
            heart.Fade(66327, 66327 + 500, 0, 1);
            heart.Fade(83470, 84613, 1, 0);
            heart.Scale(66327, 0.7);

            var heart2 = GetLayer("squares_heart").CreateSprite("sb/heart.png", OsbOrigin.Centre, new Vector2(500, 240));
            heart2.StartLoopGroup(66899, 7);
            heart2.Scale(0, 1143, 0.7, 0.8);
            heart2.Fade(0, 1143, 1, 0);
            heart2.EndGroup();
            heart2.StartLoopGroup(76042, 7);
            heart2.Scale(0, 1143, 0.7, 0.8);
            heart2.Fade(0, 1143, 1, 0);
            heart2.EndGroup();

        }

        public void OldMethod() {
            for (int i = 0; i < 40; i++) {
                for (int j = 0; j<24; j++) {

                    int x = -117 + 20 * i;
                    int y = 0 + j * 20;
                    var position = new Vector2(x,y);

                    var number = GetLayer("SquaresBG").CreateSprite("sb/d.png", OsbOrigin.TopCentre, position);
                    number.MoveX(66327, 84613, x, x-250);
                    number.Fade(66327, 1);
                    number.Fade(83470, 84613, 1, 0);
                    // number.Scale(66327, 1.5);
                    if(Random(1,10) > 5) {
                        number.ScaleVec(66327, 66327 + Random(100, 500), 1.5, 0, 1.5, 1.5);
                    } else {
                        number.ScaleVec(66327, 66327 + Random(100, 500), 0, 1.5, 1.5, 1.5);
                    }
                    
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
                    number.Color(66327, color);

                    int[] timeStamps = {74899, 75042,  75185, 75327, 75470};

                    foreach (var stamp in timeStamps)
                        {
                            var color2 = Color2;
                            if (ColorVariance > 0)
                                {
                                    var ColorVariance2 = MathHelper.Clamp(0.4f, 0, 1);

                                    var hsba = Color4.ToHsl(color2);
                                    var sMin = Math.Max(0, hsba.Y - ColorVariance2 * 0.5f);
                                    var sMax = Math.Min(sMin + ColorVariance2, 1);
                                    var vMin = Math.Max(0, hsba.Z - ColorVariance2 * 0.5f);
                                    var vMax = Math.Min(vMin + ColorVariance2, 1);

                                    color2 = Color4.FromHsl(new Vector4(
                                        hsba.X,
                                        (float)Random(sMin, sMax),
                                        (float)Random(vMin, vMax),
                                        hsba.W));
                                }
                            number.Color(stamp, color2);
                        }
                }
            }
        }

        public float GetXCoordinateAt(float startPosition, float travelSpeedVector, float time) {
            return startPosition + travelSpeedVector * time / 1000;
        }


        public void CreateNewSquareColumn(float squareStartTime, float initialX) {
            for (int j = 0; j<24; j++) {

                    float startPositionX = initialX;
                    float startPositionY = 0 + j * 20;
                    var position = new Vector2(startPositionX, startPositionY);

                    float squareEndTime = squareStartTime;
                    while (GetXCoordinateAt(startPositionX, travelSpeedVal, squareEndTime - squareStartTime) > -110 && squareEndTime < 84621 - 1143) //EndTime would be the endtime of the whole effect
                        squareEndTime += 100;

                    Log(squareEndTime.ToString());

                    var number = GetLayer("SquaresBG").CreateSprite("sb/d.png", OsbOrigin.TopCentre, position);
                    number.MoveX(squareStartTime, squareEndTime, startPositionX, GetXCoordinateAt(startPositionX, travelSpeedVal, squareEndTime - squareStartTime));
                    number.Fade(squareStartTime, 1);
                    number.Fade(squareEndTime, squareEndTime + 1143, 1, 0);

                    if (squareStartTime == 66327) {
                        if(Random(1,10) > 5) {
                            number.ScaleVec(66327, 66327 + Random(100, 500), 1.5, 0, 1.5, 1.5);
                        } else {
                            number.ScaleVec(66327, 66327 + Random(100, 500), 0, 1.5, 1.5, 1.5);
                        }
                    } else {
                        number.ScaleVec(squareStartTime, 1.5, 1.5);
                    }
                    
                    if (squareStartTime < 74899) {
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
                        number.Color(squareStartTime, color);
                    }

                    int[] timeStamps = {74899, 75042,  75185, 75327, 75470};

                    foreach (var stamp in timeStamps)
                        {
                            if (squareEndTime > stamp && squareStartTime < 74899) {
                            var color2 = Color2;
                            if (ColorVariance > 0)
                                {
                                    var ColorVariance2 = MathHelper.Clamp(0.4f, 0, 1);

                                    var hsba = Color4.ToHsl(color2);
                                    var sMin = Math.Max(0, hsba.Y - ColorVariance2 * 0.5f);
                                    var sMax = Math.Min(sMin + ColorVariance2, 1);
                                    var vMin = Math.Max(0, hsba.Z - ColorVariance2 * 0.5f);
                                    var vMax = Math.Min(vMin + ColorVariance2, 1);

                                    color2 = Color4.FromHsl(new Vector4(
                                        hsba.X,
                                        (float)Random(sMin, sMax),
                                        (float)Random(vMin, vMax),
                                        hsba.W));
                                }
                            number.Color(stamp, color2);
                            }
                            
                        }

                    if (squareStartTime > 75470) {
                        var color2 = Color2;
                        if (ColorVariance > 0)
                            {
                                var ColorVariance2 = MathHelper.Clamp(0.4f, 0, 1);

                                var hsba = Color4.ToHsl(color2);
                                var sMin = Math.Max(0, hsba.Y - ColorVariance2 * 0.5f);
                                var sMax = Math.Min(sMin + ColorVariance2, 1);
                                var vMin = Math.Max(0, hsba.Z - ColorVariance2 * 0.5f);
                                var vMax = Math.Min(vMin + ColorVariance2, 1);

                                color2 = Color4.FromHsl(new Vector4(
                                    hsba.X,
                                    (float)Random(sMin, sMax),
                                    (float)Random(vMin, vMax),
                                    hsba.W));
                            }
                        number.Color(squareStartTime, color2); 
                    }
                    
                }
        }
    }
}
