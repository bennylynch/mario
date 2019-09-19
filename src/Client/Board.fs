module Mario.Board
open System

let board = ("""
                ,
                ,
                ,
  ___           ,
                ,
_             __,
         ____   ,
                ,
     ___        ,
                ,
                ,
                """).Split([|',';'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries )
