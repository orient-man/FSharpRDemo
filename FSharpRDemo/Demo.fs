module Demo

open RProvider
open RProvider.graphics
open FSharp.Data

type Stocks = CsvProvider<"http://ichart.finance.yahoo.com/table.csv?s=MSFT">

/// Returns prices of a given stock for a specified number
/// of days (starting from the most recent)
let getStockPrices stock count =
  let url = "http://ichart.finance.yahoo.com/table.csv?s="
  [| for r in Stocks.Load(url + stock).Take(count).Rows -> float r.Open |]
  |> Array.rev

let run () =
    let tickers = [ "MSFT"; "AAPL" ]
    let data =
      [ for t in tickers ->
          printfn "got one!"
          t, getStockPrices t 255 |> R.log |> R.diff ]

    // Create an R data frame with the data and call 'R.pairs'
    let df = R.data_frame(namedParams data)
    R.pairs(df)