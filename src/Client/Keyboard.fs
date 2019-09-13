module Mario.Keyboard

open Browser
open Browser.Dom

let mutable keysPressed = Set.empty

/// Returns 1 if key with given code is pressed
let code x =
  if keysPressed.Contains(x) then 1 else 0

/// Update the state of the set for given key event
let update (e : Types.KeyboardEvent, pressed) =
    let keyCode = int e.keyCode
    let op =  if pressed then Set.add else Set.remove
    keysPressed <- op keyCode keysPressed
    //null

/// Returns pair with -1 for left or down and +1
/// for right or up (0 if no or both keys are pressed)
let arrows () =
  //(code 39 - code 37, code 38 - code 40)
  (code 39 - code 37, code 32)

let initKeyboard () =
  document.addEventListener("keydown", (fun (e : Types.Event) -> update(e :?> Types.KeyboardEvent, true)), true )
  document.addEventListener("keyup", (fun (e : Types.Event) -> update(e :?> Types.KeyboardEvent, false)), true )