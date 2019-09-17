module Mario.Model

type SpriteModel =
  { x:float; y:float;
    vx:float; vy:float;
    dir:string }

type Model = {
    Mario : SpriteModel
    Beer : SpriteModel
    Board : string []
}