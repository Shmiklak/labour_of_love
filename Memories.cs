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
    public class Memories : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    int startTime = 166899;
            int offset = 400;
            // int oldX = 0;
            for (int i = 1; i<=23; i++) {
                var map = GetLayer("maps").CreateSprite("sb/bgs/" + i + ".png", OsbOrigin.Centre);
                int x = Random(0, 600);
                // if (Math.Abs(oldX - x) < 100) {
                //     x += +100;
                // }
                // oldX = x;
                map.Move(startTime, startTime + 3000, x, 560, x, -80);
                map.Scale(startTime, startTime + 3000, 0.3, 0.4);
                map.Rotate(startTime, startTime + 3000, 0, MathHelper.DegreesToRadians(Random(-60, 60)));
                startTime+=offset;
            }

            startTime = 176042;
            offset = 200;
            // oldX = 0;
            for (int i = 24; i<=61; i++) {
                var map = GetLayer("maps").CreateSprite("sb/bgs/" + i + ".png", OsbOrigin.Centre);
                int x = Random(0, 600);
                // if (Math.Abs(oldX - x) < 100) {
                //     x += +100;
                // }
                // oldX = x;
                map.Move(startTime, startTime + 1200, x, 560, x, -80);
                map.Scale(startTime, startTime + 1200, 0.3, 0.4);
                map.Rotate(startTime, startTime + 1200, 0, MathHelper.DegreesToRadians(Random(-60, 60)));
                startTime+=offset;
                if (offset > 100) {
                    offset-=2;
                }
            }
        }
    }
}
