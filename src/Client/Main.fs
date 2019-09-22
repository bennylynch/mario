module Mario.Main
open Mario.Model
open Mario.Canvas
open Mario.Physics
open Fable.Core
open Fable.Core.JS
open Browser

let w, h = getWindowDimensions()

let render (model : Model )=
    // Select and position Mario
    // (walking is represented as an animated gif)
    let mario = model.Mario
    let beer = model.Beer
    "images/beerRun.gif" |> image "beer" |> flip beer.dir |> position (w/2.-50.+beer.x,  h-50.-70.)
    let verb =
        if mario.vy > 0. then "jump"
        elif mario.vx <> 0. then "walk"
        else "stand"
    "images/mario" + verb + mario.dir + ".gif" |> image "mario" |> position (w/2.-16.+mario.x,  h-50.-31.-mario.y)

Keyboard.initKeyboard()

let rec update model () =
    let newModel = model |> modelStep (Keyboard.arrows())
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

let model = {
    Mario = { x=0.; y=0.; vx=0.; vy=0.; dir="right"; isonplatform=true }
    Beer =  { x=0.; y=0.; vx=0.; vy=0.; dir="left" ; isonplatform=true }
    Board = Board.board
}

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
    update model ()
initRender model
