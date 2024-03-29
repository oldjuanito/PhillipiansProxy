﻿// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Titanium.Web.Proxy
open Titanium.Web.Proxy.Models
open System.Net
open Titanium.Web.Proxy.EventArguments
open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open System.IO
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open PhillipiansProxy
open Microsoft.Extensions.Logging
open NsfwSpyNS
open Microsoft.Extensions.ML

let _modelPath = Path.Combine(AppContext.BaseDirectory, "NsfwSpyModel.zip") 
let CreateHostBuilder(args)  =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(fun (hostContext:HostBuilderContext) (services) ->
            services.AddLogging(fun configure -> configure.AddConsole() |> ignore ) |> ignore
            services.AddScoped<INsfwSpy, NsfwSpy>() |> ignore

            services.AddPredictionEnginePool<NsfwSpyNS.ModelInput, NsfwSpyNS.ModelOutput>().FromFile( "ImageModel", _modelPath, true) |> ignore
            services.AddHostedService<ProxyService>() |> ignore
        );

[<EntryPoint>]
let main argv =
    CreateHostBuilder(argv).Build().Run()
    0


    //phllipians_proxy_service_acct