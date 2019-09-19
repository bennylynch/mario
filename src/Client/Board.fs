module Mario.Board
open System

let board = ("""
                ,
                ,
_               ,
  ___           ,
                ,
_             __,
                ,
                ,
_    ___        ,
                ,
_               ,
                """).Split([|',';'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries )
