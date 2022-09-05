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
    public class PanTommyLyrics : StoryboardObjectGenerator
    {
        [Configurable]
        public string SubtitlesPath = "lyrics.srt";

        [Configurable]
        public string TextLine = "Hello world!";

        [Configurable]
        public bool Che = true;

        [Configurable]
        public int TextStartTime = 0;

        [Configurable]
        public int TextEndTime = 0;

        [Configurable]
        public string FontName = "Verdana";

        [Configurable]
        public string SpritesPath = "sb/f";

        [Configurable]
        public int FontSize = 26;

        [Configurable]
        public float FontScale = 0.5f;

        [Configurable]
        public float FontFade = 1;

        [Configurable]
        public int StartAppearTime = 200;

        [Configurable]
        public string ApTo = "ToLeft";

        [Configurable]
        public Vector2 ApShift = new Vector2(0,0);

        [Configurable]
        public float ApScale = 0;

        [Configurable]
        public OsbEasing ApEasing;

        [Configurable]
        public bool WaveAnimation = false;
        
        [Configurable]
        public OsbEasing WaveEasing;

        [Configurable]
        public Vector2 WaveShift = new Vector2(-10, 10);

        [Configurable]
        public int WaveMoves = 4;

        [Configurable]
        public int StartDisappearTime = 200;

        [Configurable]
        public string DisapTo = "ToLeft";

        [Configurable]
        public Vector2 DisapShift = new Vector2(0,0);

        [Configurable]
        public float DisapScale = 0;

        [Configurable]
        public OsbEasing DisapEasing;

        [Configurable]
        public Color4 FontColor = Color4.White;
        [Configurable]
        public Color4 FontColor1 = Color4.White;
        [Configurable]
        public Color4 FontColor2 = Color4.White;


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
        public float X = 140;

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
                Color = new Color4(255,255,255,255),
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


            if(Che)
            {
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
            else 
            {
                string textLine = TextLine;
                generateText(font, textLine, "", false);
            }
        }

        public void generateText(FontGenerator font, string text, string layerName, bool additive)
        {
            var layer = GetLayer(layerName);
            if (PerCharacter) generateTextPerCharacter(font, text, layer, additive);
            //else generatePerLine(font, subtitles, layer, additive);        
        }

        public void generateTextPerCharacter(FontGenerator font, string text, StoryboardLayer layer, bool additive)
        {
            var letterY = SubtitleY;
            var textWidth = 0f;
            var textHeight = 0f;
            int apLetterID = -1;
            int disapLetterID = -1;

            
            float waveGap = (TextEndTime - TextStartTime) / 4;

            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                textWidth += texture.BaseWidth * FontScale;
                textHeight = Math.Max(textHeight, texture.BaseHeight * FontScale);
                switch(ApTo){
                    case "ToRight":
                        apLetterID ++; break;
                    case "ToCentre":
                        apLetterID ++; break;
                    case "ToLeft":
                        apLetterID --; break;
                    default: break;
                }

                switch(DisapTo){
                    case "ToRight":
                        disapLetterID ++; break;
                    case "ToCentre":
                        disapLetterID ++; break;
                    case "ToLeft":
                        disapLetterID --; break;
                    default: break;
                }
            }
            
            if (ApTo == "ToCentre")
                apLetterID = -apLetterID/2 - 1;
            if (DisapTo == "ToCentre")
                disapLetterID = -disapLetterID/2 - 1;

            var letterX = X - textWidth * 0.5f;

            
            foreach (var letter in text)
            {
                switch(ApTo){
                    case "ToRight":
                        apLetterID --; break;
                    case "ToCentre":
                        apLetterID ++; break;
                    case "ToLeft":
                        apLetterID ++; break;
                    default: break;
                }
                switch(DisapTo){
                    case "ToRight":
                        disapLetterID --; break;
                    case "ToCentre":
                        disapLetterID ++; break;
                    case "ToLeft":
                        disapLetterID ++; break;
                    default: break;
                }
                
                //if(DisapToLeft)
                    //disapLetterID ++;
                //else 
                    //disapLetterID --;
                
                var texture = font.GetTexture(letter.ToString());

                if (!texture.IsEmpty)
                {
                    var position = new Vector2(letterX, letterY)
                        + texture.OffsetFor(Origin) * FontScale;
                    
                    var sprite = layer.CreateSprite(texture.Path, Origin, position);

                    sprite.Move(ApEasing, TextStartTime - StartAppearTime, TextStartTime, position.X + (apLetterID * ApShift.X), position.Y + (apLetterID * ApShift.Y), position.X, position.Y);
                    sprite.Move(DisapEasing, TextEndTime - StartDisappearTime, TextEndTime, position.X, position.Y, position.X + (disapLetterID * DisapShift.X), position.Y + (disapLetterID * DisapShift.Y));
                    sprite.Fade(TextStartTime - StartAppearTime, TextStartTime, 0, FontFade);               
                    sprite.Fade(TextEndTime - StartDisappearTime, TextEndTime, FontFade, 0);
                    sprite.Scale(ApEasing, TextStartTime - StartAppearTime, TextStartTime, FontScale + ApScale, FontScale);
                    sprite.Scale(DisapEasing, TextEndTime - StartDisappearTime, TextEndTime, FontScale, FontScale + DisapScale);
                    sprite.Color(TextStartTime, FontColor);

                    
                    if (additive) sprite.Additive(TextStartTime, TextEndTime);
                }
                letterX += texture.BaseWidth * FontScale;
            }
            letterY += textHeight;
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
        
            int lineID = 0;
            foreach (var line in subtitles.Lines)
            {
                
                var texture = font.GetTexture(line.Text);
                var position = new Vector2(X - texture.BaseWidth * FontScale * 0.5f, SubtitleY)
                    + texture.OffsetFor(Origin) * FontScale; 

                
                {
                    lineID ++;    
                    Bitmap bm = new Bitmap(MapsetPath + "/" + texture.Path);
                    double partHeight = bm.Height / 3;
                    Rectangle rect1 = new Rectangle(0, 0, bm.Width, bm.Height/2);
                    Rectangle rect2 = new Rectangle(0, bm.Height/2, bm.Width, bm.Height/2);
                    bm.Clone(rect1, bm.PixelFormat).Save(MapsetPath + "/sb/" + SpritesPath + "/text1_" + lineID + ".png");
                    bm.Clone(rect2, bm.PixelFormat).Save(MapsetPath + "/sb/" + SpritesPath + "/text2_" + lineID + ".png");
                    // bm.Dispose();

                    var sprite1 = layer.CreateSprite("sb/" + SpritesPath + "/text1_" + lineID + ".png", Origin);
                    var sprite2 = layer.CreateSprite("sb/" + SpritesPath + "/text2_" + lineID + ".png", Origin);
                    sprite1.Color(line.StartTime, FontColor);
                    sprite1.Scale(line.StartTime, FontScale);
                    sprite1.Fade(ApEasing,line.StartTime, line.StartTime + StartAppearTime, 0, 0.3);
                    sprite1.Fade(DisapEasing,line.EndTime - StartAppearTime, line.EndTime, 0.3, 0);
                    if (additive) sprite1.Additive(line.StartTime - 200, line.EndTime);

                    sprite2.Color(line.StartTime, FontColor);
                    sprite2.Scale(line.StartTime, FontScale);
                    sprite2.Fade(ApEasing, line.StartTime, line.StartTime + StartAppearTime, 0, 0.3);
                    sprite2.Fade(DisapEasing,line.EndTime - StartAppearTime, line.EndTime, 0.3, 0);
                    if (additive) sprite2.Additive(line.StartTime - 200, line.EndTime);

                    sprite1.Move(ApEasing, line.StartTime, line.StartTime + StartAppearTime, position.X + Random(-40,40), position.Y, position.X, position.Y);
                     sprite1.Move(DisapEasing, line.EndTime - StartDisappearTime, line.EndTime, position.X, position.Y, position.X + Random(-40,40), position.Y);

                    //var spriteBitmap1 = GetMapsetBitmap("/sb/lyrics/text1_" + lineID + ".png");
                    Bitmap bm1 = new Bitmap(MapsetPath + "/sb/ss/text1_" + lineID + ".png");
                    sprite2.Move(ApEasing, line.StartTime, line.StartTime + StartAppearTime, position.X + Random(-40,40), position.Y + bm1.Height * FontScale, position.X, position.Y + bm1.Height * FontScale);
                    sprite2.Move(DisapEasing, line.EndTime - StartDisappearTime, line.EndTime, position.X, position.Y + bm1.Height * FontScale, position.X + Random(-40,40), position.Y + bm1.Height * FontScale);
                
                }
            }
        }
        public void generatePerCharacte(FontGenerator font, SubtitleSet subtitles, StoryboardLayer layer, bool additive)
        {           
            foreach (var subtitleLine in subtitles.Lines)
            {
                var letterY = SubtitleY;
                var lineWidth = 0f;
                var lineHeight = 0f; 
                int apLetterID = -1;
                int disapLetterID = -1;
                int letterID = -1;  
                int waveLetterID = -1;




                int lengthLine = 0;
                foreach (var line in subtitleLine.Text.Split('\n'))
                {
                    lengthLine = line.Length;
                }



                double waveGap = (subtitleLine.EndTime - subtitleLine.StartTime) / lengthLine * 15;
                foreach (var line in subtitleLine.Text.Split('\n'))
                {
                    
                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        lineWidth += texture.BaseWidth * FontScale;
                        lineHeight = Math.Max(lineHeight, texture.BaseHeight * FontScale);
                        switch(ApTo){
                            case "ToRight":
                                apLetterID ++; break;
                            case "ToCentre":
                                apLetterID ++; break;
                            case "ToLeft":
                                apLetterID --; break;
                            default: break;
                        }
                            switch(DisapTo){
                            case "ToRight":
                                disapLetterID ++; break;
                            case "ToCentre":
                                disapLetterID ++; break;
                            case "ToLeft":
                                disapLetterID --; break;
                            default: break;
                        }
                        letterID++;
                    }


                    if (ApTo == "ToCentre")
                        apLetterID = -apLetterID/2 - 1;
                    if (DisapTo == "ToCentre")
                        disapLetterID = -disapLetterID/2 - 1;

                    var letterX = X - lineWidth * 0.5f;
                    foreach (var letter in line)
                    {

                        switch(ApTo){
                            case "ToRight":
                                apLetterID --; break;
                            case "ToCentre":
                                apLetterID ++; break;
                            case "ToLeft":
                                apLetterID ++; break;
                            default: break;
                        }
                        switch(DisapTo){
                            case "ToRight":
                                disapLetterID --; break;
                            case "ToCentre":
                                disapLetterID ++; break;
                            case "ToLeft":
                                disapLetterID ++; break;
                            default: break;
                        }
                        
                        var texture = font.GetTexture(letter.ToString());
                        if (!texture.IsEmpty)
                        {
                            var position = new Vector2(letterX, letterY)
                                + texture.OffsetFor(Origin) * FontScale;
                            
                            var sprite = layer.CreateSprite(texture.Path, Origin, position);
                            waveLetterID ++;
                            

                            if(!WaveAnimation)
                                sprite.Move(ApEasing, subtitleLine.StartTime, subtitleLine.StartTime + StartAppearTime, position.X + Random(-3,3), position.Y  + Random(-3,3), position.X, position.Y);
                            if(WaveAnimation)
                            {
                                sprite.Fade(ApEasing, subtitleLine.StartTime + StartAppearTime + (waveLetterID * 20), subtitleLine.StartTime + (waveLetterID * 10) + 20, 0, FontFade);
                                for(int i = 0; i < lengthLine / 5; i ++)
                                {
                                    /*if(i < WaveMoves)
                                    {*/
                                        if(i == 0)
                                        {
                                            sprite.Rotate(subtitleLine.StartTime, Random(-0.1,0.1));
                                            sprite.Move(WaveEasing, subtitleLine.StartTime - StartAppearTime + (i * waveGap) + waveLetterID * 50, subtitleLine.StartTime - StartAppearTime + ((i + 1) * waveGap) + waveLetterID * 50, position.X, position.Y + (waveLetterID * 0.6), position.X, position.Y + WaveShift.X);
                                        }
                                        else
                                        {
                                            if (i % 2 == 0)
                                                sprite.Move(WaveEasing, subtitleLine.StartTime - StartAppearTime + (i * waveGap) + waveLetterID * 50, subtitleLine.StartTime - StartAppearTime + ((i + 1) * waveGap) + waveLetterID * 50, position.X, position.Y + WaveShift.Y, position.X, position.Y + WaveShift.X);
                                            else sprite.Move(WaveEasing, subtitleLine.StartTime - StartAppearTime + (i * waveGap) + waveLetterID * 50, subtitleLine.StartTime - StartAppearTime + ((i + 1) * waveGap) + waveLetterID * 50, position.X, position.Y + WaveShift.X, position.X, position.Y + WaveShift.Y);
                                        }
                                    /*}
                                    else
                                    {
                                        if (i % 2 == 0)
                                            sprite.Move(WaveEasing, subtitleLine.StartTime - StartAppearTime + (i * waveGap) + waveLetterID * 50, subtitleLine.EndTime, position.X, position.Y + WaveShift.Y, position.X, position.Y);
                                        else sprite.Move(WaveEasing, subtitleLine.StartTime - StartAppearTime + (i * waveGap) + waveLetterID * 50, subtitleLine.EndTime, position.X, position.Y + WaveShift.X, position.X, position.Y);
                                    }*/
                                }
                            }
                            if(!WaveAnimation)
                                sprite.Move(DisapEasing, subtitleLine.EndTime - StartDisappearTime, subtitleLine.EndTime, position.X, position.Y, position.X, position.Y +  + Random(-1,1));
                            sprite.Fade(subtitleLine.StartTime, subtitleLine.StartTime + StartAppearTime, 0, FontFade);
                            sprite.Fade(DisapEasing, subtitleLine.EndTime - StartDisappearTime, subtitleLine.EndTime, FontFade, 0);
                            sprite.Scale(ApEasing, subtitleLine.StartTime - StartAppearTime, subtitleLine.StartTime, FontScale + ApScale, FontScale);
                            sprite.Scale(DisapEasing, subtitleLine.EndTime - StartDisappearTime, subtitleLine.EndTime, FontScale, FontScale + DisapScale);
                            sprite.Color(subtitleLine.StartTime, subtitleLine.EndTime, FontColor, FontColor);
                        }
                        letterX += texture.BaseWidth * FontScale;
                    }
                    letterY += lineHeight;
                }
            }
        }
    }
}
