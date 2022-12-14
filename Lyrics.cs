using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;
using System.Drawing;
using System.IO;

namespace StorybrewScripts
{
    public class Lyrics : StoryboardObjectGenerator
    {
        [Configurable]
        public string SubtitlesPath = "lyrics.srt";

        [Configurable]
        public string FontName = "Verdana";

        [Configurable]
        public string SpritesPath = "sb/f";

        [Configurable]
        public int FontSize = 26;

        [Configurable]
        public float FontScale = 0.5f;

        [Configurable]
        public Color4 FontColor = Color4.White;

        [Configurable]
        public FontStyle FontStyle = FontStyle.Regular;

        [Configurable]
        public int GlowRadius = 0;

        [Configurable]
        public Color4 GlowColor = new Color4(255, 255, 255, 100);

        [Configurable]
        public bool AdditiveGlow = true;

        [Configurable]
        public int OutlineThickness = 3;

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public int ShadowThickness = 0;

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 100);

        [Configurable]
        public Vector2 Padding = Vector2.Zero;

        [Configurable]
        public float SubtitleY = 400;

        [Configurable]
        public bool PerCharacter = true;

        [Configurable]
        public bool TrimTransparency = true;

        [Configurable]
        public bool EffectsOnly = false;

        [Configurable]
        public bool Debug = false;

        [Configurable]
        public OsbOrigin Origin = OsbOrigin.Centre;

        public override void Generate()
        {
            var font = LoadFont(SpritesPath, new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle,
                TrimTransparency = TrimTransparency,
                EffectsOnly = EffectsOnly,
                Debug = Debug,
            },
            new FontGlow()
            {
                Radius = AdditiveGlow ? 0 : GlowRadius,
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            var subtitles = LoadSubtitles(SubtitlesPath);

            if (GlowRadius > 0 && AdditiveGlow)
            {
                var glowFont = LoadFont(Path.Combine(SpritesPath, "glow"), new FontDescription()
                {
                    FontPath = FontName,
                    FontSize = FontSize,
                    Color = FontColor,
                    Padding = Padding,
                    FontStyle = FontStyle,
                    TrimTransparency = TrimTransparency,
                    EffectsOnly = true,
                    Debug = Debug,
                },
                new FontGlow()
                {
                    Radius = GlowRadius,
                    Color = GlowColor,
                });
                generateLyrics(glowFont, subtitles, "glow", true);
            }
            generateLyrics(font, subtitles, "", false);
        }

        public void generateLyrics(FontGenerator font, SubtitleSet subtitles, string layerName, bool additive)
        {
            var layer = GetLayer(layerName);
            if (PerCharacter) generatePerCharacter(font, subtitles, layer, additive);
            else generatePerLine(font, subtitles, layer, additive);
        }

        public void generatePerLine(FontGenerator font, SubtitleSet subtitles, StoryboardLayer layer, bool additive)
        {
            foreach (var line in subtitles.Lines)
            {
                var texture = font.GetTexture(line.Text);
                var position = new Vector2(320 - texture.BaseWidth * FontScale * 0.5f, SubtitleY)
                    + texture.OffsetFor(Origin) * FontScale;

                var sprite = layer.CreateSprite(texture.Path, Origin, position);
                sprite.Scale(line.StartTime, FontScale);
                sprite.Fade(line.StartTime - 200, line.StartTime, 0, 1);
                sprite.Fade(line.EndTime - 200, line.EndTime, 1, 0);
                if (additive) sprite.Additive(line.StartTime - 200, line.EndTime);
            }
        }

        public void generatePerCharacter(FontGenerator font, SubtitleSet subtitles, StoryboardLayer layer, bool additive)
        {
            int partsCount = 4;
            foreach (var subtitleLine in subtitles.Lines)
            {
                var letterY = SubtitleY;
                foreach (var line in subtitleLine.Text.Split('\n'))
                {
                    var lineWidth = 0f;
                    var lineHeight = 0f;
                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        lineWidth += texture.BaseWidth * FontScale;
                        lineHeight = Math.Max(lineHeight, texture.BaseHeight * FontScale);
                    }
                    
                    var letterX = 320 - lineWidth * 0.5f;
                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        if (!texture.IsEmpty)
                        {
                            for(int j = 0; j < partsCount; j++)
                            {
                                Bitmap bm = new Bitmap(MapsetPath + "/" + texture.Path); 
                                Rectangle rect = new Rectangle(0, j * bm.Height / partsCount, bm.Width, bm.Height / partsCount);
                                Bitmap part = bm.Clone(rect, bm.PixelFormat);

                                if(!File.Exists(MapsetPath + "/sb/font/" + letter + "_" + j + ".png"))
                                {
                                    part.Save(MapsetPath + "/sb/font/" + letter + "_" + j + ".png");
                                    bm.Dispose();
                                }
                            }
                            
                            for(int part = 0; part < partsCount; part ++)
                            {
                                Bitmap charPart = GetMapsetBitmap("sb/font/" + letter + "_" + part + ".png");
                                var position = new Vector2(letterX, letterY + (charPart.Height) * FontScale * part)
                                    + texture.OffsetFor(OsbOrigin.TopCentre) * FontScale;

                                var sprite = layer.CreateSprite("sb/font/" + letter + "_" + part + ".png", OsbOrigin.TopCentre, position);
                                if (part%2 == 0) {
                                    sprite.MoveX(subtitleLine.StartTime - 200, subtitleLine.StartTime,position.X - 30,  position.X);
                                } else {
                                    sprite.MoveX(subtitleLine.StartTime - 200, subtitleLine.StartTime, position.X + 30, position.X);
                                }
                                sprite.Scale(subtitleLine.StartTime, FontScale);
                                sprite.Fade(subtitleLine.StartTime - 200, subtitleLine.StartTime, 0, 1);
                                // Log(position.X);
                                if (part%2 == 0) {
                                    sprite.MoveX(subtitleLine.EndTime - 200, subtitleLine.EndTime, position.X, position.X - 30);
                                } else {
                                    sprite.MoveX(subtitleLine.EndTime - 200, subtitleLine.EndTime, position.X, position.X + 30);
                                }
                                sprite.Fade(subtitleLine.EndTime - 200, subtitleLine.EndTime, 1, 0);
                                if (additive) sprite.Additive(subtitleLine.StartTime - 200, subtitleLine.EndTime);
                            }
                        }
                        letterX += texture.BaseWidth * FontScale;
                    }
                    letterY += lineHeight;
                }
            }
            

        }
    }
}
