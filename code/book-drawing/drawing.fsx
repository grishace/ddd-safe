#r "./packages/MathNet.Numerics/lib/net461/MathNet.Numerics.dll"

open MathNet.Numerics.Distributions

let people = 10
let discrete = DiscreteUniform(1, people)

let winner = discrete.Samples() |> Seq.head
