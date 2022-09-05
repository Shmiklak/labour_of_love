/// Author: Darky1#1170
/// Date:   12.01.2019

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

namespace StorybrewScripts {
    public class Phyllotaxis : StoryboardObjectGenerator {
        [Configurable] public string SpritePath = "sb/circle.png";
        [Configurable] public float SpriteScale = 1f;
        [Configurable] public bool SpriteRotate = false; //rotate the sprite to the centre?

        [Configurable] public double StartTime = 0;
        [Configurable] public double EndTime = 1000;

        [Configurable] public double FadeDuration = 200;
        [Configurable] public float FadeDelay = .01f; //we are delaying the fading of sprites by this factor

        [Configurable] public Vector2 Centre = new Vector2(320, 240); //centre of the phyllotaxis
        [Configurable] public float Angle = 137.5f; //want another angle for your phyllotaxis?
        [Configurable] public float Distance = 20f; //how far away should the sprites be?
        
        [Configurable] public int Count = 200; //how many sprites should be generated?
        [Configurable] public int SkipFromCentre = 3; //ignore the first x sprites
        [Configurable] public int DistanceFromBorder = 30; //ignore sprites that are further than x away from the border

        [Configurable]
        public Color4 Color = Color4.White;

        [Configurable]
        public float ColorVariance = 0.6f;


        public override void Generate() {
            var layer = GetLayer("");
            var baseAngle = (float)MathHelper.DegreesToRadians(Angle); //convert from Degrees to Radians

            if(StartTime >= EndTime)
                EndTime = StartTime + FadeDuration * Count * FadeDelay * 2 + FadeDuration; //just fade it out if no EndTime is given
            
            for (int i = SkipFromCentre; i < Count; i++) {
                var angle = i * baseAngle;
                var radius = Distance * (float)Math.Sqrt(i);

                var position = new Vector2(
                    Centre.X + radius * (float)Math.Cos(angle),
                    Centre.Y + radius * (float)Math.Sin(angle)
                );

                if(!(-107 - DistanceFromBorder <= position.X && 857 + DistanceFromBorder >= position.X && -DistanceFromBorder <= position.Y && 480 + DistanceFromBorder >= position.Y)) //check if the sprite is outside or inside the view
                    continue;

                var sTime = StartTime + FadeDuration * i * FadeDelay;
                var eTime = EndTime - FadeDuration * (Count - i) * FadeDelay * 2; //multiplied by 2 to cause them to fade out while others are still fading in if EndTime is close to StartTime

                if(sTime + FadeDuration > eTime) //Make sure we never have unintended command overlaps
                    throw new ArgumentException("Start and Endtime are overlapping with these configurations!");

                var sprite = layer.CreateSprite(SpritePath, OsbOrigin.Centre, position); //create our sprite

                sprite.Scale(OsbEasing.OutBack, sTime, sTime + FadeDuration, 0f, SpriteScale); //fadein
                sprite.Scale(OsbEasing.InSine, eTime, eTime + FadeDuration * 2, SpriteScale, 0f); //fadeout
                
                //sprite.ColorHsb(sTime, r/2 + 200, 1f, 1f); //coloring example
                //sprite.ColorHsb(sTime, Count/i*20, 1f, 1f); //coloring example

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

                sprite.Color(sTime, color);

                // sprite.ColorHsb(sTime, MathHelper.RadiansToDegrees(angle) - radius, 1f, 1f); //coloring example

                if(SpriteRotate)
                    sprite.Rotate(sTime, angle);
            }
        }
    }
}
