module Mario.Main

open Mario.Canvas
open Mario.Physics
open Fable.Core
open Fable.Core.JS
open Browser

let render (w,h) (mario: MarioBeerModel) (beer: MarioBeerModel)=
    (0., 0., w, h) |> filled (rgb 174 238 238)
    (0., h-50., w, 50.) |> filled (rgb 74 163 41)
    // Select and position Mario
    // (walking is represented as an animated gif)
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

let rec update mario beer () =
    let mario = mario |> Physics.marioStep (Keyboard.arrows()) (w, h)
    let beer = beer |> Physics.beerStep (w,h)
    render (w,h) mario beer
    window.setTimeout(update mario beer, 1000 / 60) |> ignore

let mario = { x=0.; y=0.; vx=0.; vy=0.; dir="right" }
let beer = { x=0.; y=0.; vx=0.; vy=0.; dir="right" }
update mario beer ()