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
                Vector2[] shakePosition;
                shakePosition = new Vector2[10];
                Vector2[] shakePositionR;
                shakePositionR = new Vector2[10];
                Vector2[] shakePositionB;
                shakePositionB = new Vector2[10];
                
                for(int i = 0; i < 10; i++)
                {
                    shakePosition[i] = new Vector2(
                        (float)Random(0, 2),
                        (float)Random(0, 2)
                    );
                    shakePositionR[i] = new Vector2(
                        (float)Random(0, 5),
                        (float)Random(0, 5)
                    );
                    shakePositionB[i] = new Vector2(
                        (float)Random(0, 5),
                        (float)Random(0, 5)
                    );
                }

                var beat = 509;
                int loopCount = (int)Math.Ceiling((line.EndTime - line.StartTime - 200) / (10 * beat / 4));
                var value = line.Text;

                var lineY = SubtitleY;
                if (line.StartTime > 209149) {
                    lineY = 150;
                }

                var texture = font.GetTexture(line.Text);
                var position = new Vector2(320 - texture.BaseWidth * FontScale * 0.5f, lineY)
                    + texture.OffsetFor(Origin) * FontScale;


                var RCopy = layer.CreateSprite(texture.Path, Origin, position);
                var BCopy = layer.CreateSprite(texture.Path, Origin, position);

                RCopy.ScaleVec(line.StartTime, FontScale, FontScale);
                RCopy.Fade(line.StartTime - 200, line.StartTime, 0, 0.5);
                RCopy.Fade(line.EndTime - 200, line.EndTime, 0.5, 0);
                RCopy.Color(line.StartTime - 200, 255, 0, 0);
                
                RCopy.StartLoopGroup(line.StartTime - 200, loopCount);
                    for(int i = 0; i < 10; i++)
                        RCopy.Move(OsbEasing.OutBounce, beat/4 * i, beat/4 * (i + 1), position + shakePositionR[i], position - shakePositionR[i]/2);
                RCopy.EndGroup();


                // RCopy.ScaleVec(line.EndTime - 200, line.EndTime, FontScale, FontScale, FontScale + 0.5, 0);



                BCopy.ScaleVec(line.StartTime, FontScale, FontScale);
                BCopy.Fade(line.StartTime - 200, line.StartTime, 0, 0.5);
                BCopy.Fade(line.EndTime - 200, line.EndTime, 0.5, 0);
                BCopy.Color(line.StartTime - 200, 0.2, 0.8, 1);
                
                BCopy.StartLoopGroup(line.StartTime - 200, loopCount);
                    for(int i = 0; i < 10; i++)
                        BCopy.Move(OsbEasing.OutBounce, beat/4 * i, beat/4 * (i + 1), position + shakePositionB[i], position - shakePositionB[i]/2);
                BCopy.EndGroup();

                // BCopy.ScaleVec(line.EndTime - 200, line.EndTime, FontScale, FontScale, FontScale + 0.5, 0);

                RCopy.Additive(line.StartTime - 200, line.EndTime);
                BCopy.Additive(line.StartTime - 200, line.EndTime);

                var sprite = layer.CreateSprite(texture.Path, Origin, position);

                sprite.ScaleVec(line.StartTime, FontScale, FontScale);
                sprite.Fade(line.StartTime - 200, line.StartTime, 0, 1);
                sprite.Fade(line.EndTime - 200, line.EndTime, 1, 0);
                
                sprite.StartLoopGroup(line.StartTime - 200, loopCount);
                for(int i = 0; i < 10; i++)
                    sprite.Move(OsbEasing.OutBounce, beat/4 * i, beat/4 * (i + 1), position + shakePosition[i], position - shakePosition[i]/2);
                sprite.EndGroup();


                // sprite.ScaleVec(line.EndTime - 200, line.EndTime, FontScale, FontScale, FontScale + 0.5, 0);


                if (additive) {
                    sprite.Additive(line.StartTime - 200, line.EndTime);
                }
            }
        }

        public void generatePerCharacter(FontGenerator font, SubtitleSet subtitles, StoryboardLayer layer, bool additive)
        {
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
                            var position = new Vector2(letterX, letterY)
                                + texture.OffsetFor(Origin) * FontScale;

                            var sprite = layer.CreateSprite(texture.Path, Origin, position);
                            sprite.Scale(subtitleLine.StartTime, FontScale);
                            sprite.Fade(subtitleLine.StartTime - 200, subtitleLine.StartTime, 0, 1);
                            sprite.Fade(subtitleLine.EndTime - 200, subtitleLine.EndTime, 1, 0);
                            if (additive) sprite.Additive(subtitleLine.StartTime - 200, subtitleLine.EndTime);
                        }
                        letterX += texture.BaseWidth * FontScale;
                    }
                    letterY += lineHeight;
                }
            }
        }
    }
}
