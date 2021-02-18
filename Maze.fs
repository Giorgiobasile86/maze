(*
* LabProg2019 - Progetto di Programmazione a.a. 2019-20
* Maze.fs: maze
* (C) 2019 Alvise Spano' @ Universita' Ca' Foscari di Venezia
*)

module LabProg2019.Maze

open External
open Gfx
open System
open Engine

type state = {
    player : sprite
}
let W = 50
let H = 30
let rnd = Random ()
type CharInfo with
    /// Shortcut for creating a wall pixel.
    static member wall = pixel.create (Config.wall_pixel_char, Color.White)
    /// Shortcut for creating a path pixel.
    static member internal path = pixel.filled Color.Black
    /// Check whether this pixel is a wall.
    member this.isWall = this = pixel.wall


// TODO: implement the maze type, its generation (task 1) and its automatic resolution (task 2)
type maze (W, H) as this =
    let mutable mazeGrid = Array2D.init<CharInfo> H W (fun x y-> if x%2=0 || y%2=0 then CharInfo.wall else CharInfo.path)
    let mutable checkGrid = Array2D.zeroCreate<bool> ((Array2D.length1 mazeGrid / 2)+1) ((Array2D.length2 mazeGrid / 2)+1)
    let firstIndex = (0,rnd.Next(0,Array2D.length2 checkGrid))
    //let my_maze = new maze (50,30)
    // TODO: do not forget to call the generation function in your object initializer
    do this.Generate
    
    member private __.NewBlankMaze =
        Array2D.init<CharInfo> H W (fun x y-> if x%2<>0 || y%2<>0 then CharInfo.wall else CharInfo.path)

    //Convert a given 2D array to a 1D array
    member private __.ConvertToArray (mazeGrid)=
        seq{
            for i in 0 .. (Array2D.length1 mazeGrid - 1) do
                for j in 0 .. (Array2D.length2 mazeGrid - 1)->
                    mazeGrid.[i,j]
        }
            |>Seq.toArray
    member public __.CompleteMaze = __.ConvertToArray mazeGrid
    // TODO: start with implementing the generation
    
//New type R
[< Diagnostics.DebuggerDisplay ("{ToString()}") >]
type cell ()=
   member val topWall = true with get, set
   member val bottomWall = true with get, set
   member val leftWall = true with get, set
   member val rightWall = true with get, set
   member val visited = false with get, set

   override this.ToString ()=
       let sb = new System.Text.StringBuilder ()
       if this.topWall then sb.Append 'T' |> ignore
       if this.bottomWall then sb.Append 'B' |> ignore
       if this.leftWall then sb.Append 'L' |> ignore
       if this.rightWall then sb.Append 'R' |> ignore
       let s = sb.ToString ()
       if this.visited then s.ToUpper () else s.ToLower ()


   (* member private __.GenPath (x,y) (checkGrid:bool[,])=

        checkGrid.[x,y] <- true  // now this location has been visited
        let mutable neighbors = this.CheckNeighbors (x,y) checkGrid
            
        if not (Array.isEmpty neighbors) then
            let nextLocation = neighbors.[rnd.Next(Array.length neighbors )]
            __.DestroyWall (x,y) (nextLocation) mazeGrid
            __.GenPath (nextLocation) checkGrid
        else // non arriva mai qui loop dentro check_neighbors --> non controllo se sono
            for i in 0.. (Array2D.length1 checkGrid - 1 ) do
                for j in 0.. (Array2D.length2 checkGrid - 1) do
                    if checkGrid.[i,j] && not (Array.isEmpty (__.CheckNeighbors (i,j) checkGrid))
                    then __.GenPath (i,j) checkGrid 
    member private __.Generate = 
        __.GenPath firstIndex checkGrid*)
