using System.IO;
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
using System.Globalization;
using System.Linq;
using System.Diagnostics;

namespace StorybrewScripts
{
    public class ModelImportPulse : StoryboardObjectGenerator
    {
        [Configurable]
        public String FilePath = "model.obj";
		[Configurable]
        public String SpritePath = "sb/pixel.png";

        [Configurable]
        public Color4 Color = Color4.White;

        [Configurable]
        public float ColorVariance = 0.6f;

        [Configurable]
        public double meshScale = 1;
		[Configurable]
        public double SpriteSize = 0.8;
        
        [Configurable]
        public int centerX = 320;
        [Configurable]
        public int centerY = 240;
        [Configurable]
        public double tilt = 10;
        [Configurable]
        public int startTime = 0;
        [Configurable]
        public int EndTime = 1000;
		[Configurable]
        public bool spin = true;
		
		[Configurable]
        public int spinDuration = 1000;
        
        public override void Generate()
        {   
            var ModelLayer = GetLayer("ModelLayer");
            var ModelArray = readModel(FilePath);
		   for (int i = 0; i < ModelArray.Length; i++)
           {   //Translated from Exile-'s work.
               var X = ModelArray[i].X;
               var Y = ModelArray[i].Y;
               var Z = ModelArray[i].Z;
               var Angle = Math.Atan2(Z, X);
			   var Delay = spinDuration * (Angle/(Math.PI * 2));
               var Radius = meshScale * Math.Sqrt((X * X) + (Z * Z));
               var ModelPixel = ModelLayer.CreateSprite(SpritePath,OsbOrigin.Centre,new Vector2(320,(float)(centerY - Y * meshScale)));
               if(spin)
			   {
                   int loopStartTime = startTime - spinDuration - (int)Math.Round(Delay);
                   while (loopStartTime + spinDuration < startTime)
                   {
                       loopStartTime += spinDuration;
                   }
                        

                    // Log(loopStartTime);
				   ModelPixel.StartLoopGroup(loopStartTime, (int)Math.Ceiling((EndTime - (loopStartTime)) * 1.0 / spinDuration ));
				   for(int I = 0; I < 4; I++)
				   {
					   double startAngle = Math.PI * I / 2;
					   double endAngle = Math.PI * (I + 1) / 2;
					   double startTimeSpin = spinDuration * I / 4;
					   double endTimeSpin = spinDuration * (I + 1) / 4;
					   ModelPixel.MoveX((OsbEasing)(I % 2 + 1),startTimeSpin,endTimeSpin,centerX + Radius * Math.Sin(startAngle), centerX + Radius * Math.Sin(endAngle));
					   if (tilt != 0)
                        ModelPixel.MoveY((OsbEasing)((I + 1) % 2 + 1), startTimeSpin,endTimeSpin,centerY - Y * meshScale + tilt * Radius * Math.Cos(startAngle), centerY - Y * meshScale + tilt * Radius * Math.Cos(endAngle));
				   }
				   ModelPixel.EndGroup();
				   ModelPixel.StartLoopGroup(startTime,33);
				   ModelPixel.Scale(0,Beatmap.GetTimingPointAt(startTime).BeatDuration,0.8,0.45);
				   ModelPixel.EndGroup();
			   }else{
                   ModelPixel.Scale(startTime,SpriteSize);
				   double x = centerX + Radius * Math.Sin(Angle);
			   double y = centerY + tilt * Radius * Math.Cos(Angle) - Y * meshScale;
			   ModelPixel.Move(OsbEasing.None, startTime, startTime, x, y, x, y);
			   }
               ModelPixel.Fade(startTime, startTime, 0, 1);
               ModelPixel.Fade(EndTime,0);

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

                ModelPixel.Color(startTime, color);
			   
			   
           } 
		   Log($"Mesh - {(DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds}");
            
        }
        
        public Vector3[] readModel(String FilePath)
        {
			Vector3[] finalModelArray = {};
			List<Vector3> finalModelList = new List<Vector3>();
            using (var stream = OpenProjectFile(FilePath))
            using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {	//Below Code from Damnae
                while (!reader.EndOfStream) {
					var line = reader.ReadLine().Trim();
					String[] values = line.Split(' ');

					if (values[0].Equals("v")) {
						
						// Not sure why y and z are swapped here
                       	var x = float.Parse(values[1], CultureInfo.InvariantCulture);
						var y = float.Parse(values[3], CultureInfo.InvariantCulture);
						var z = float.Parse(values[2], CultureInfo.InvariantCulture);
						finalModelList.Add(new Vector3(x, -y, z));
							
					} 
                    
				}    
				finalModelArray = finalModelList.ToArray();    
            
            return finalModelArray; 
        }
    }
    }
}
