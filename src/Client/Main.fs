module Mario.Main
open Mario.Model
open Mario.Canvas
open Mario.Physics
open Fable.Core
open Fable.Core.JS
open Browser
open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Thoth.Json

type Msg =
    | GetBoard
    | InitialBoardLoaded of string

let w, h = getWindowDimensions()

let render (m : Model ) =
    // Select and position Mario
    // (walking is represented as an animated gif)
    "images/beerRun.gif" |> image "beer" |> flip m.Beer.dir |> position (w/2.-50.+m.Beer.x,  h-50.-70.)
    let verb =
        if m.Mario.vy > 0. then "jump"
        elif m.Mario.vx <> 0. then "walk"
        else "stand"
    "images/mario" + verb + m.Mario.dir + ".gif" |> image "mario" |> position (w/2.-16.+m.Mario.x,  h-50.-31.-m.Mario.y)

let initRender (model : Model ) =
    (0., 0., w, h)      |> filled (rgb 174 238 238)
    (0., h-50., w, 50.) |> filled (rgb 74 163 41)
    model.Board |> Seq.iteri (fun row cols  ->
        cols |> Seq.iteri  (fun col c ->
            let color =
                match c with
                    |'_' -> (rgb 65 42 42)
                    |' ' -> (rgb 174 238 238)
                    |ch  -> (rgb 174 238 238)
            filled color (float (col * int (w / 16.)) , float (row * int (w / 16.)), (w / 16.) , 20.)
        )
    )
    //update model ()
// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
    Keyboard.initKeyboard()
    let initialModel = {
                Mario = { x=0.; y=0.; vx=0.; vy=0.; dir="right"; isonplatform=true }
                Beer =  { x=0.; y=0.; vx=0.; vy=0.; dir="left" ; isonplatform=true }
                Board = string.Epmty
            }
    let loadBoardCmd =
        Cmd.OfPromise.perform Board.init () InitialBoardLoaded
    InitialBoardLoaded, loadBoardCmd

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | _, InitialBoardLoaded boardstring ->
        let board = boardstring.Split([|',';'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries)
        let nextModel = {
                Mario = { x=0.; y=0.; vx=0.; vy=0.; dir="right"; isonplatform=true }
                Beer =  { x=0.; y=0.; vx=0.; vy=0.; dir="left" ; isonplatform=true }
                Board = board
            }

        nextModel, Cmd.none
    | _ ->
        let newModel = currentModel |> modelStep (Keyboard.arrows())
        render newModel
        if ((int newModel.Mario.x = int newModel.Beer.x) &&
            (newModel.Mario.y = newModel.Beer.y))
            then "images/gameover.gif" |> image "fireworks" |> position (8.,0.)

        if (newModel.Mario.y >= h - 50. && newModel.Mario.x < 50.)
            then
            window.setTimeout(window.location.assign("http://www.laterrazawalton.com"), 50000) |> ignore
            "images/fireworks.gif" |> image "fireworks" |> position (8.,0.)
        else
        window.setTimeout(update newModel, 1000 / 60) |> ignore
        currentModel, Cmd.none

//let rec update model () =

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
