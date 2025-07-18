module Game
open Player
open Alien
open Bullet

let width = 20
let height = 8

type GameState = {
    Player: Player.Player
    Aliens: Alien.Alien list
    Bullets: Bullet.Bullet list
}

let emptyRow () = Array.create width ' '

let drawBoard (state: GameState) =
    let board = Array.init height (fun _ -> emptyRow ())
    for alien in state.Aliens do
        if alien.Y >= 0 && alien.Y < height && alien.X >= 0 && alien.X < width then
            board.[alien.Y].[alien.X] <- 'W'
    for bullet in state.Bullets do
        if bullet.Y >= 0 && bullet.Y < height && bullet.X >= 0 && bullet.X < width then
            board.[bullet.Y].[bullet.X] <- '|'
    let px = state.Player.X
    let py = height - 2
    if px >= 0 && px < width then
        board.[py].[px] <- '^'
    printfn "%s" (String.replicate width "-")
    for y in 1 .. height - 2 do
        printf "|"
        for x in 0 .. width - 1 do
            printf "%c" board.[y].[x]
        printfn "|"
    printfn "%s" (String.replicate width "-")

let start () =
    let initialState = {
        Player = Player.initialPlayer
        Aliens = Alien.initialAliens
        Bullets = []
    }
    drawBoard initialState 