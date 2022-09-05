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
    public class TransitionFromCenter : StoryboardObjectGenerator
    {
        [Configurable] public string SpritePath = "SB/sq.jpg";
        [Configurable] public float SpriteScale = 20f;

        [Configurable] public double StartTime = 0;
        [Configurable] public double EndTime = 1000;
        [Configurable] public Vector2 Edge = new Vector2(-107, 0);
        [Configurable] public float Distance = 20f;

        [Configurable] public int Count = 2000; 
        [Configurable] public int DistanceFromBorder = 0;
       public override void Generate()
        {
            var layer = GetLayer("");
            for (int j = 0; j < Count; j++)
            {
                for (int i = 0; i < Count; i++)
                {
                    var position = new Vector2(
                        -144 + Edge.X + (i * Distance),
                        Edge.Y + j * Distance
                    );
                    if (!(-107 - DistanceFromBorder <= position.X && 747 + DistanceFromBorder >= position.X && -DistanceFromBorder <= position.Y && 480 + DistanceFromBorder >= position.Y)) 
                        continue;
                    var sprite = layer.CreateSprite(SpritePath, OsbOrigin.Centre, position); 
                    sprite.Scale(OsbEasing.OutBack, StartTime + 5 * Get2dDistance(position,new Vector2(320,240)), StartTime + 5 * Get2dDistance(position,new Vector2(320,240)) + 500, 0f, SpriteScale); 
                    sprite.Scale(StartTime + 25 * Get2dDistance(position,new Vector2(320,240)) + 500,EndTime, SpriteScale,SpriteScale);
                }
            }
        }
         private static float Get2dDistance(Vector2 First, Vector2 Second)
        {
            return (float)Math.Sqrt(Math.Pow((Second.X - First.X), 2) + Math.Pow((Second.Y - First.Y), 2));
        }
    }
}
