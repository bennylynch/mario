module Mario.Canvas

open Fable.Core
open Fable.Core.JsInterop
open Browser.Types

open Browser.Dom

// Get the canvas context for drawing
let canvas = (document.getElementsByTagName "canvas").[0] :?> HTMLCanvasElement
let context = canvas.getContext_2d()

// Format RGB color as "rgb(r,g,b)"
let ($) s n = s + n.ToString()
let rgb r g b = "rgb(" $ r $ "," $ g $ "," $ b $ ")"

/// Fill rectangle with given color
let filled (color: string) rect =
    let ctx = context
    ctx.fillStyle <- !^ color
    ctx.fillRect rect

let withImg (src : string) =
    let ctx = context
    //let img = HTMLImageElement.Create()
    let img = document.createElement("img") :?> HTMLImageElement
    console.log src
    img.src <- src
    ctx.drawImage(!^ img,0.,0.)

/// Move element to a specified X Y position
let position (x,y) (img : HTMLImageElement) =
    img?style?left <- x.ToString() + "px"
    img?style?top <- (canvas.offsetTop + y).ToString() + "px"

let flip (dir : string ) (img : HTMLImageElement) =
    if (dir = "left") then img?style?transform <- "scaleX(-1)"
    else img?style?transform <- "scaleX(1)"
    img

let getWindowDimensions () =
  canvas.width, canvas.height

/// Get the first <img /> element and set `src` (do
/// nothing if it is the right one to keep animation)
let image (selector : string)  (src:string) =
    let image = document.getElementById( selector ) :?> HTMLImageElement
    if image.src.IndexOf(src) = -1 then image.src <- src
    image