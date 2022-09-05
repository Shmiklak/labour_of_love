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
    public class VhsIntro : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    var bg = GetLayer("intro").CreateSprite("sb/etc/p.png", OsbOrigin.Centre);
            bg.Color(0, new Color4(	0, 0, 255, 255));
            bg.Scale(0, 1000);
            bg.Fade(0, 1);
            bg.Fade(10327, 0);
            var play = GetLayer("intro").CreateSprite("sb/etc/vhs.png", OsbOrigin.Centre);
            play.Fade(0, 1);
            play.Scale(0,1.2);
            play.Fade(10327, 0);
        }
    }
}
