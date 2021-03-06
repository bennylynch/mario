module Mario.Physics
open Mario.Model

let w, h = Canvas.getWindowDimensions()
let numBlocksWdith = Board.board.[0].Length
let numBlocksHiehgt = Board.board.Length
let blockWidth  = w / float numBlocksWdith
let blockHeihgt = h / float numBlocksHiehgt

let board =
        Board.board |> Array.mapi (fun row cols  ->
                cols.ToCharArray() |> Array.mapi  (fun col c ->
                    c = '_' )
                )
//find the board cell mario is in. tranlsate his into 0 based .[Y].[X] indices
let findBoardCell (m : SpriteModel)  =
    let x = int ((float numBlocksWdith * (m.x + w/2.)) / w)  //translate mario.x into board row index
    let y = ((h - m.y + 20.) / blockHeihgt |> int) - 1       //translate mario.y into boar col index
    try board.[y].[x] with |e -> false                       //he can occasionally escape the top.


// If the space bar is pressed (y > 0) and Mario is on the ground,
// then create Mario with the y velocity 'vy' set to 5.0
let jump (_,y) m =
  Canvas.drawText m.isonplatform (100., 100.)
  if (y > 0  && ((m.y = 0.) || m.isonplatform))  then { m with vy = 6. } else m

// If Mario is in the air, then his "up" velocity is decreasing

let gravity m =
  if ((m.y > 0. && (not m.isonplatform )) || (m.y >= h))   then { m with vy = m.vy - 0.1 } else m


// Apply physics - move Mario according to the current velocities
// when he hits a platform, vy <- 0
let physics m =
  let isPlatform,newY =
             match m |> findBoardCell with
             |false -> false, max 0. (m.y + m.vy)
             |_     -> true,  max m.y  (m.y + m.vy)
  match (m.x + m.vx) with
  |x when x > w / 2. - 8. -> //mario's right side is off the right edge
        { m with x = -w/2. + 8.; y = newY; isonplatform = isPlatform }
  |x when x < - w / 2. + 8.-> //left side off the left edge
        { m with x = w/2. - 8.; y = newY; isonplatform = isPlatform }
  |_ -> { m with x = m.x + m.vx; y = newY; isonplatform = isPlatform }

// When Left/Right keys are pressed, change 'vx' and direction
let walk (x,_)  m =
  let dir = if x < 0 then "left" elif x > 0 then "right" else m.dir
  { m with vx = float x; dir = dir }

let beerStep b =
    if (b.dir = "right") then
        match (b.x + 1.7) with
        |x when x > w / 2. - 10. ->
                { b with x = b.x - 1.7 + 10.; y = 0.; dir="left"}
        |_ ->   { b with x = b.x + 1.7; y = 0.}
    else
        match (b.x - 1.7) with
        |x when x < - w / 2. + 10.->
                { b with x = b.x + 1.7 - 10. ; y = 0.; dir="right"}
        |_ ->   { b with x = b.x - 1.7; y = 0.}

let modelStep dir (model : Model) =
    let beer = model.Beer |> beerStep
    let mario =
      model.Mario
      |> physics
      |> walk dir
      |> gravity
      |> jump dir
    { model with Mario = mario; Beer = beer; Board = model.Board }