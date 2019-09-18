open System
let w,h = 1024. , 768. // 16 * 12. each set 64

let board = ("""
                ,
                ,
                ,
  ___           ,
        ____    ,
            ____,
      ___       ,
___             ,
      ___       ,
            ____,
                """).Split([|',';'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries )
let boardy = board |> Array.mapi (fun row cols  ->
                cols.ToCharArray() |> Array.mapi  (fun col c ->
                    let isPlatform =
                        match c with
                            |'_' -> 1
                            |' ' -> 0
                            |ch  -> 0
                    isPlatform, ( float col , float row - h ) //, 64. , 20.)
                ))