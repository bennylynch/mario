module Mario.Main
open Mario.Model
open Mario.Canvas
open Mario.Physics
open Fable.Core
open Fable.Core.JS
open Browser

let render (w,h) (model : Model )=
    (0., 0., w, h) |> filled (rgb 174 238 238)
    (0., h-50., w, 50.) |> filled (rgb 74 163 41)
    model.Board |> Seq.iteri (fun row cols  ->
        cols.ToCharArray() |> Seq.iteri  (fun col c ->

            let color =
                match c with
                    |'_' ->

                        (rgb 65 42 42)
                    |' ' -> (rgb 174 238 238)
                    |ch  -> Browser.Dom.console.log (row, col, ch);(rgb 0 0 0)
            filled color (float (col * 64) , float (row * 64), 64. , 20.)
        )
    )
    // Select and position Mario
    // (walking is represented as an animated gif)
    let mario = model.Mario
    let beer = model.Beer
    "images/beerRun.gif" |> image "beer"|> flip beer.dir |> position (w/2.-50.+beer.x,  h-50.-70.)
    let verb =
        if mario.y > 0. then "jump"
        elif mario.vx <> 0. then "walk"
        else "stand"
    "images/mario" + verb + mario.dir + ".gif"
    |> image "mario"
    |> position (w/2.-16.+mario.x,  h-50.-31.-mario.y)

Keyboard.initKeyboard()

let w, h = getWindowDimensions()

let rec update model () =
    //let mario = model.Mario |> Physics.marioStep (Keyboard.arrows()) (w, h)
    //let beer = model.Beer |> Physics.beerStep (w,h)
    let newModel = model |> Physics.modelStep (w,h) (Keyboard.arrows())
    render (w,h) newModel
    window.setTimeout(update newModel, 10000 / 60) |> ignore

let model = {
    Mario = { x=0.; y=0.; vx=0.; vy=0.; dir="right" }
    Beer = { x=0.; y=0.; vx=0.; vy=0.; dir="right" }
    Board = Board.board
}
update model ()