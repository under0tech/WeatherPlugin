<!-- 1. Launch the AspNetCore Web API application WeatherPlugin -->

# Mastering Weather forecasting Chat GPT Plugin via C#

To create 'Weather forecasting' plugin for Chat GPT from scretch you have to:
1. Open **Visual Studio 2022**

   click '_Create a new Project_' and choose '_ASP.NET Core Web API_' and click '_Next_' button.
   
   in the next window put 'WeatherPluginV1' into textbox 'Project name' and click 'Next'
   
   ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/4c865bfb-e098-4e4e-8326-35c1f8174027)

   in the next window

   ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/0ff911da-8ef0-42cd-a938-966ff6690e62)

   you have to uncheck 'Configure for HTTPS' and choose '.NET 7.0' in the field 'Framework' and click the button 'Create'.

   You will see the initially configured project as shown on the picture below

   ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/ee09f385-8d32-4fb2-86b2-a079e918761b)

   You can remove default endpoint and supportive code as shown on the picture below

   ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/bb1c9f98-2a20-4fc3-93fe-560d4a3ac3fc)

2. Create new class 'GetWeatherResponse'
   ```c#
   ﻿namespace WeatherPlugin
   {
       public class GetWeatherResponse
       {
           public double temperature { get; set; }
           public double windspeed { get; set; }
           public double winddirection { get; set; }
           public GetWeatherResponse() { }
           public GetWeatherResponse(
               double temperature,
               double windspeed,
               double winddirection)
           {
               this.temperature = temperature;
               this.windspeed = windspeed;
               this.winddirection = winddirection;
           }
       }
   }
   ```
   we need this to parse the response from Weather API

3. Add usings in the begining of 'Program.cs'
   ```c#
   using Microsoft.Extensions.FileProviders;
   using System.Text.Json;
   using WeatherPluginV1;
   ```
5. Add the constant with Weater API URL before the builder initialization
   ```c#
   const string WEATHER_API_URL = "https://api.open-meteo.com/v1/forecast?";
   ```
7. Add Cors to services before 'var app = builder.Build();'
   ```c#
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowAll", policy =>
       {
           policy.WithOrigins(
               "https://chat.openai.com", 
               "http://localhost:5139")
               .AllowAnyHeader()
               .AllowAnyMethod();
       });
   });
   ```
9. and using them right after the build
   ```c#
   var app = builder.Build();
   app.UseCors("AllowAll");
   ```
11. ddd

   




