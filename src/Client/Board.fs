module Mario.Board
open System
open Fable.Core
open Fable.Core.JsInterop
open Thoth.Fetch

//let mutable txt = ""

let init () =
    promise {
        let url = "/api/board"
        let! res = Fetch.fetchAs<string>(url)
        return res
    }
