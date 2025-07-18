module Alien

type Alien = { X: int; Y: int }

let initialAliens = [ for x in 3 .. 2 .. 15 -> { X = x; Y = 2 } ] 