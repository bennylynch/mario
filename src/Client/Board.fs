module Mario.Board

open Fable.Core
open Fable.Core.JsInterop
open Browser.Types

open Browser.Dom

let width,heght = 1024 , 768 // 16 * 12. each set 64
let board = ("\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,\
___   ___   ____,").Split(',')