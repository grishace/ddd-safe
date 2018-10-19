open Api
open Giraffe
open Giraffe.Serialization
open Thoth.Json.Giraffe
open Microsoft.Extensions.DependencyInjection
open Saturn
open System.IO

let clientPath = Path.Combine("..","Client") |> Path.GetFullPath
let port = 8085us

let browserRouter = router {
    get "/" (htmlFile (Path.Combine(clientPath, "/index.html"))) }

let mainRouter = router {
    forward "/api" apiRouter
    forward "" browserRouter }

let config (services:IServiceCollection) =
    services.AddSingleton<IJsonSerializer>(ThothSerializer())

let app = application {
    use_router mainRouter
    url ("http://0.0.0.0:" + port.ToString() + "/")
    memory_cache 
    use_static clientPath
    service_config config
    use_gzip }

run app