open System
let width,heght = 1024 , 768 // 16 * 12. each set 64

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
let boardy = board |> Seq.mapi (fun row cols  ->
                cols |> Seq.mapi  (fun col c ->
                    let isPlatform =
                        match c with
                            |'_' -> 1
                            |' ' -> 0
                            |ch  -> 0
                    isPlatform, (float (col * 64) , float (row * 64))//, 64. , 20.)
                )
            )