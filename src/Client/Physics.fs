module Mario.Physics
open Mario.Model



// If the Up key is pressed (y > 0) and Mario is on the ground,
// then create Mario with the y velocity 'vy' set to 5.0
let jump (_,y) m =
  if y > 0 && m.y = 0. then { m with vy = 5. } else m

// If Mario is in the air, then his "up" velocity is decreasing
let gravity m =
  if m.y > 0. then { m with vy = m.vy - 0.1 } else m

// Apply physics - move Mario according to the current velocities
let physics (w,h) m =
  match (m.x + m.vx) with
  |x when x > w / 2. - 8. ->
        { m with x = -w/2. + 8.; y = max 0. (m.y + m.vy)}
  |x when x < - w / 2. + 8.->
        { m with x = w/2. - 8.; y = max 0. (m.y + m.vy)}
  |_ ->
        { m with x = m.x + m.vx; y = max 0. (m.y + m.vy) }

// When Left/Right keys are pressed, change 'vx' and direction
let walk (x,_)  m =
  let dir = if x < 0 then "left" elif x > 0 then "right" else m.dir
  { m with vx = float x; dir = dir }

let modelStep (w,h) dir (model : Model) =
    let b = model.Beer
    let m = model.Mario
    let beer =
        if (b.dir = "right") then
            match (b.x + 1.7) with
            |x when x > w / 2. - 10. ->
                    { b with x = b.x - 1.7 + 10.; y = 0.; dir="left"}
            |_ ->
                    { b with x = b.x + 1.7; y = 0.}
        else
            match (b.x - 1.7) with
            |x when x < - w / 2. + 10.->
                    { b with x = b.x + 1.7 - 10. ; y = 0.; dir="right"}
            |_ ->
                    { b with x = b.x - 1.7; y = 0.}
    let mario =
      m
      |> physics (w,h)
      |> walk dir
      |> gravity
      |> jump dir
    { model with Mario = mario; Beer = beer; Board = model.Board }