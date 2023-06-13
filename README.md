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

4. Add the constant with Weater API URL before the builder initialization
   ```c#
   const string WEATHER_API_URL = "https://api.open-meteo.com/v1/forecast?";
   ```

5. Add Cors to services before 'var app = builder.Build();'
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

6. and using them right after the build
   ```c#
   var app = builder.Build();
   app.UseCors("AllowAll");
   ```

7. Also we have to add support for Static files cause we will be adding two files for OpenAI into the folder '.well-known'
   ```c#
   app.UseStaticFiles(new StaticFileOptions
   {
       FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), ".well-known")),
       RequestPath = "/.well-known"
   });
   ```
   Do that right after 'if (app.Environment.IsDevelopment())'

8. Next, we will be adding our first and last endpoint in this application
   ```c#
   // Add weather-forecast endpoint.
   app.MapGet("/weather-forecast", async (double latitude, double longitude) =>
   {
       using (var httpClient = new HttpClient())
       {
           var queryParams = $"latitude={latitude}&longitude={longitude}&current_weather=true";
           var url = WEATHER_API_URL + queryParams;
   
           var result = await httpClient.GetStringAsync(url);
           var jsonDocument = JsonDocument.Parse(result);
           var currentWeather = jsonDocument.RootElement.GetProperty("current_weather");
           return JsonSerializer.Deserialize<GetWeatherResponse>(currentWeather);
   
       }
   })
   .WithName("getWeather");
   ```

9. Finally, your 'Program.cs' file should look like this
   ```c#
   using Microsoft.Extensions.FileProviders;
   using System.Text.Json;
   using WeatherPluginV1;
   
   const string WEATHER_API_URL = "https://api.open-meteo.com/v1/forecast?";
   var builder = WebApplication.CreateBuilder(args);
   
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();
   
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
   
   var app = builder.Build();
   
   if (app.Environment.IsDevelopment())
   {
       app.UseSwagger();
       app.UseSwaggerUI();
   }
   
   app.UseStaticFiles(new StaticFileOptions
   {
       FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), ".well-known")),
       RequestPath = "/.well-known"
   });
   
   // Add weather-forecast endpoint.
   app.MapGet("/weather-forecast", async (double latitude, double longitude) =>
   {
       using (var httpClient = new HttpClient())
       {
           var queryParams = $"latitude={latitude}&longitude={longitude}&current_weather=true";
           var url = WEATHER_API_URL + queryParams;
   
           var result = await httpClient.GetStringAsync(url);
           var jsonDocument = JsonDocument.Parse(result);
           var currentWeather = jsonDocument.RootElement.GetProperty("current_weather");
           return JsonSerializer.Deserialize<GetWeatherResponse>(currentWeather);
   
       }
   })
   .WithName("getWeather");
   
   app.Run();
   ```
10. To let OpenAI work with your plugin you have to create the folder '.well-known'

11. Create file 'ai-plugin.json' in that folder with the following content
    ```json
    {
     "schema_version": "v1",
     "name_for_human": "Weather forecast",
     "name_for_model": "weather_forecast",
     "description_for_human": "Plugin for forecasting the weather conditions by latitude and longitude.",
     "description_for_model": "Plugin for forecasting the weather conditions by latitude and longitude.",
     "auth": {
       "type": "none"
     },
     "api": {
       "type": "openapi",
       "url": "http://localhost:5139/swagger/v1/swagger.yaml",
       "is_user_authenticated": false
     },
     "logo_url": "http://localhost:5139/.well-known/logo.png",
     "contact_email": "misterd793@gmail.com",
     "legal_info_url": "https://medium.com/@dmytrosazonov"
    }
    ```
12. And also copy file 'logo.png' into this folder
    
13. You will have something like this
    
    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/e0d150cd-ded7-4491-b914-c8d29e90de73)

14. Just click Run F5 in your Visual Studio and observe the Swagger as shown on the picture bellow

    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/b431fd4f-d8ab-4a34-8a86-605a1cc37ebd)
    
15. As you may see on the picture above, the port for your web API is different from that which we declared 
    in our 'Program.cs'. Adjust it to 'http://localhost:5139' editing the file 'launchSettings.json' as shown bellow
    ```json
    "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5139",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    ```

16. Now you are ready to test your plugin out in the Chat GPT UI.

17. Open the Chat GPT (https://chat.openai.com/) and if you are the Chat GPT Plus user you have the options
    GPT.3.5 or GPT 4.0. You have to choose GPT 4.0

    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/17752c65-28f6-4105-ae27-a485d450b7eb)
    
    and 'Plugins'.

    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/356db2b0-6845-45b3-8fcd-3122549f400f)

    in the 'Plugin store' click the button 'Develop your own plugin'

    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/5e940327-be8b-4d1f-8b0e-77fe413a37d4)

    and put the URL to your local service into the field 'Domain' as shown on the picture above. Then click 'Find manifest file'

    if everything is ok you will see

    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/8123dac9-5752-49df-b52b-b7db24b88f4b)

    Just click 'Install localhost plugin' to let Chat GPT use it in its chatbot UI

    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/305c0e8a-7072-47bd-a1d6-7cd750e4bc3d)

18. Now you can speak with your plugin as shown on the picture bellow

    ![image](https://github.com/under0tech/WeatherPlugin/assets/113665703/3bc45217-e007-469e-8125-a163450cffbd)

If you have questions, you can ask me in **Twitter**: https://twitter.com/dmytro_sazonov

   




