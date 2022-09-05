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
    public class Background : StoryboardObjectGenerator
    {
        [Configurable]
        public string BackgroundPath = "";

        [Configurable]
        public int StartTime = 0;

        [Configurable]
        public int EndTime = 0;

        [Configurable]
        public double Opacity = 0.2;

        public override void Generate()
        {
            if (BackgroundPath == "") BackgroundPath = Beatmap.BackgroundPath ?? string.Empty;
            if (StartTime == EndTime) EndTime = (int)(Beatmap.HitObjects.LastOrDefault()?.EndTime ?? AudioDuration);

            var bitmap = GetMapsetBitmap(BackgroundPath);
            var bg = GetLayer("").CreateSprite(BackgroundPath, OsbOrigin.Centre);
            bg.Fade(StartTime - 500, StartTime, 0, Opacity);

            var sky = GetLayer("bg_sky").CreateSprite("sb/sky.png", OsbOrigin.Centre, new Vector2(320, 120));
            sky.Fade(11470, 1);
            sky.Fade(46327, 0);
            sky.Scale(11470, 0.47);

            var city = GetLayer("bg_city").CreateSprite("sb/buildings.png", OsbOrigin.BottomCentre, new Vector2(320, 255));
            city.Fade(11470, 1);
            city.Fade(46327, 0);
            city.Scale(11470, 0.45);

            var heart = GetLayer("bg_city_heart").CreateSprite("sb/heartBack.png", OsbOrigin.Centre, new Vector2(320, 155));
            heart.Fade(11470, 1);
            heart.Fade(46327, 0);
            heart.Scale(11470, 0.45);

            var heart2 = GetLayer("bg_city_heart").CreateSprite("sb/heartBack.png", OsbOrigin.Centre, new Vector2(320, 155));
            heart2.StartLoopGroup(12042, 14);
            heart2.Scale(0, 1143, 0.45, 0.55);
            heart2.Fade(0, 1143, 1, 0);
            heart2.EndGroup();
            heart2.StartLoopGroup(30327, 14);
            heart2.Scale(0, 1143, 0.45, 0.55);
            heart2.Fade(0, 1143, 1, 0);
            heart2.EndGroup();

            var linesBG = GetLayer("bg_city").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(320, 374));

            linesBG.Fade(11470, 1);
            linesBG.Fade(46327, 0);
            linesBG.Color(11470, new Color4(26, 15, 56, 255));
            linesBG.ScaleVec(11470, 1.4 * 60, 0.36 * 60);

            var linesBG2 = GetLayer("bg_city").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(320, 374));

            linesBG2.Fade(184042, 1);
            linesBG2.Fade(203470, 0);
            linesBG2.Color(184042, new Color4(80, 15, 56, 255));
            linesBG2.ScaleVec(184042, 1.4 * 60, 0.36 * 60);

            var sky2 = GetLayer("bg_sky").CreateSprite("sb/sky.png", OsbOrigin.Centre, new Vector2(320, 120));
            sky2.Fade(184042, 1);
            sky2.Fade(203470, 0);
            sky2.Scale(184042, 0.47);
            sky2.Color(184042, new Color4(255, 0, 140, 255));

            int[] timeStamps = {11470, 19470,  19613, 19899, 20042, 20327, 20470, 20613, 29756, 202327, 202470,202756, 203185};

            var whiteBG = GetLayer("white_bg").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(320, 240));
            whiteBG.Scale(121185, 100);
            whiteBG.Fade(121185, 1);
            whiteBG.Fade(139470, 0);

            foreach (var stamp in timeStamps)
            {
                var lineBGFlash = GetLayer("bg_city").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(320, 374));
                lineBGFlash.ScaleVec(stamp, 1.4 * 60, 0.36 * 60);
                lineBGFlash.Additive(stamp, stamp+500);
                lineBGFlash.Fade(stamp, stamp+500, 0.18, 0);
            }

                    
            

        }
    }
}
