module Game
open Player
open Alien
open Bullet
open System

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

let clamp x min max =
    if x < min then min
    elif x > max then max
    else x

let movePlayer (player: Player.Player) dx =
    { player with X = clamp (player.X + dx) 0 (width - 1) }

let updateBullets (bullets: Bullet.Bullet list) =
    bullets
    |> List.map (fun b -> { b with Y = b.Y - 1 })
    |> List.filter (fun b -> b.Y >= 0)

let rec gameLoop (state: GameState) =
    Console.Clear()
    drawBoard state
    let mutable newState = state
    if Console.KeyAvailable then
        let key = Console.ReadKey(true)
        match key.Key with
        | ConsoleKey.LeftArrow ->
            newState <- { state with Player = movePlayer state.Player -1 }
        | ConsoleKey.RightArrow ->
            newState <- { state with Player = movePlayer state.Player 1 }
        | ConsoleKey.Spacebar ->
            let px = state.Player.X
            let py = height - 3
            let bulletExists = state.Bullets |> List.exists (fun b -> b.X = px && b.Y = py)
            if not bulletExists then
                newState <- { newState with Bullets = { X = px; Y = py } :: newState.Bullets }
        | _ -> ()
    newState <- { newState with Bullets = updateBullets newState.Bullets }
    System.Threading.Thread.Sleep(80)
    gameLoop newState

let start () =
    let initialState = {
        Player = Player.initialPlayer
        Aliens = Alien.initialAliens
        Bullets = []
    }
    gameLoop initialState 