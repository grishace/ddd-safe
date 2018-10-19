open System.IO
open System.Threading.Tasks

open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Shared

open Giraffe.Serialization
open Thoth.Json.Giraffe

let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

let getInitCounter() : Task<Counter> = task { return 42 }

let webApp = router {
    get "/api/init" (fun next ctx ->
        task {
            let! counter = getInitCounter()
            return! Successful.OK counter next ctx
        })
}

let configureSerialization (services:IServiceCollection) =
    services.AddSingleton<IJsonSerializer>(ThothSerializer())

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router webApp
    memory_cache
    use_static publicPath
    service_config configureSerialization
    use_gzip
}

run app
